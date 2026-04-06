using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VizzyCode
{
    public class ChatPanel : Panel
    {
        public Action<string> OnInsertCode;
        public Action<string> OnReplaceCode;
        public Func<string>   GetCurrentCode;
        public Func<string>   GetWorkingDirectory;
        public Func<string>   GetDocumentKey;

        private RichTextBox _history;
        private TextBox     _input;
        private Button      _btnSend;
        private Button      _btnStop;
        private Button      _btnOpenCli;
        private CheckBox    _chkInclude;
        private Label       _statusLabel;
        private Button      _btnSettings;
        private ComboBox    _cmbProvider;

        private AiSettings _settings = AiSettings.Load();
        private IAiClient _activeClient;
        private AgentWorkspace _workspace;
        private CancellationTokenSource? _cts;
        private FileSystemWatcher? _workspaceWatcher;
        private System.Windows.Forms.Timer? _workspaceSyncTimer;
        private bool _workspaceDirty;
        private bool _suppressWorkspaceWatcher;
        private bool _isDark = true;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public AiSettings Settings { get => _settings; set { _settings = value; RefreshClient(); } }

        public ChatPanel()
        {
            _workspace = AgentWorkspace.Create(null, null);
            BuildUI();
            RefreshClient();
            WireEvents();
            InitializeWorkspaceSync();
        }

        private void RefreshClient()
        {
            if (_activeClient != null)
            {
                _activeClient.OnChunk -= AppendStreamChunk;
                _activeClient.OnDone  -= FinishMessage;
                _activeClient.OnError -= ShowError;
                _activeClient.OnToolActivity -= AppendToolActivity;
                _activeClient.OnApprovalRequired -= HandleApprovalRequired;
                _activeClient.OnToolCall -= HandleToolCall;
            }

            switch (_settings.Provider)
            {
                case AiProvider.Gemini: _activeClient = new GeminiClient { Settings = _settings }; break;
                case AiProvider.OpenAI: _activeClient = new OpenAiClient { Settings = _settings, IsOpenCode = false }; break;
                case AiProvider.OpenCode: _activeClient = new OpenCodeClient { Settings = _settings }; break;
                default: _activeClient = new ClaudeClient { Settings = _settings }; break;
            }

            _activeClient.OnChunk += AppendStreamChunk;
            _activeClient.OnDone  += FinishMessage;
            _activeClient.OnError += ShowError;
            _activeClient.OnToolActivity += AppendToolActivity;
            _activeClient.OnApprovalRequired += HandleApprovalRequired;
            _activeClient.OnToolCall += HandleToolCall;
            
            if (_cmbProvider != null) _cmbProvider.Text = _settings.Provider.ToString();
            UpdateStatusLabel();
        }

        private void BuildUI()
        {
            Dock = DockStyle.Fill;
            var titlePanel = new Panel { Dock = DockStyle.Top, Height = 35, BackColor = Color.FromArgb(45, 45, 48) };
            var title = new Label { Text = "VIZZY AI", Location = new Point(8, 8), AutoSize = true, Font = new Font("Segoe UI", 10f, FontStyle.Bold), ForeColor = Color.White };
            _cmbProvider = new ComboBox { Width = 100, Height = 25, Location = new Point(130, 5), DropDownStyle = ComboBoxStyle.DropDownList, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5f), BackColor = Color.FromArgb(60, 60, 60), ForeColor = Color.White };
            _cmbProvider.Items.AddRange(new object[] { "Claude", "Gemini", "OpenAI", "OpenCode" });
            _cmbProvider.SelectedItem = _settings.Provider.ToString();
            _cmbProvider.SelectedIndexChanged += (s, e) => { if (Enum.TryParse<AiProvider>(_cmbProvider.Text, out var p)) { _settings.Provider = p; _settings.Save(); RefreshClient(); } };
            _btnSettings = new Button { Text = "⚙ Settings", Width = 85, Height = 25, Location = new Point(240, 5), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 8.5f), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, TabStop = false };
            _btnSettings.FlatAppearance.BorderSize = 0;
            titlePanel.Controls.Add(title); titlePanel.Controls.Add(_cmbProvider); titlePanel.Controls.Add(_btnSettings);
            titlePanel.SizeChanged += (s, e) => { _btnSettings.Left = titlePanel.Width - _btnSettings.Width - 5; _cmbProvider.Left = _btnSettings.Left - _cmbProvider.Width - 5; };
            _history = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, WordWrap = true, ScrollBars = RichTextBoxScrollBars.Vertical, BorderStyle = BorderStyle.None, Font = new Font("Consolas", 10f), DetectUrls = false };
            _statusLabel = new Label { Dock = DockStyle.Top, Height = 20, Font = new Font("Segoe UI", 8f), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(5, 0, 0, 0), Text = "Ready" };
            var inputArea = new Panel { Dock = DockStyle.Bottom, Height = 120, Padding = new Padding(5) };
            _chkInclude = new CheckBox { Text = "Context: Current Code", Dock = DockStyle.Top, Height = 20, Checked = true, Font = new Font("Segoe UI", 8.5f) };
            _input = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Vertical, Font = new Font("Consolas", 10f), BorderStyle = BorderStyle.FixedSingle, AcceptsReturn = false };
            var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 35 };
            _btnSend = new Button { Text = "Send Message", Dock = DockStyle.Right, Width = 110, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f, FontStyle.Bold), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White };
            _btnStop = new Button { Text = "Stop AI", Dock = DockStyle.Right, Width = 80, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f), BackColor = Color.FromArgb(200, 70, 70), ForeColor = Color.White, Visible = false };
            _btnOpenCli = new Button { Text = "Open CLI", Dock = DockStyle.Left, Width = 90, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9f), BackColor = Color.FromArgb(65, 65, 70), ForeColor = Color.White };
            btnPanel.Controls.Add(_btnSend); btnPanel.Controls.Add(_btnStop); btnPanel.Controls.Add(_btnOpenCli);
            inputArea.Controls.Add(_input); inputArea.Controls.Add(_chkInclude); inputArea.Controls.Add(btnPanel);
            Controls.Add(_history); Controls.Add(_statusLabel); Controls.Add(inputArea); Controls.Add(titlePanel);
            ApplyTheme(true);
        }

        private void WireEvents() {
            _btnSend.Click += (_, _) => SendMessage();
            _btnStop.Click += (_, _) => _cts?.Cancel();
            _btnOpenCli.Click += (_, _) => OpenNativeCli();
            _btnSettings.Click += (_, _) => ShowSettings();
            _input.KeyDown += (_, e) => { if (e.KeyCode == Keys.Return && !e.Shift) { e.SuppressKeyPress = true; SendMessage(); } };
        }

        private void UpdateStatusLabel() {
            string mode = "";
            switch (_settings.Provider) {
                case AiProvider.Claude: mode = _settings.ClaudeMode.ToString(); break;
                case AiProvider.Gemini: mode = _settings.GeminiMode.ToString(); break;
                case AiProvider.OpenAI: mode = _settings.OpenAiMode.ToString(); break;
                case AiProvider.OpenCode: mode = _settings.OpenCodeMode.ToString(); break;
            }
            _statusLabel.Text = $"Agent: {_settings.Provider} | Mode: {mode} | Model: {GetCurrentModelName()} | Open CLI = native approvals";
        }

        private string GetCurrentModelName() {
            switch (_settings.Provider) {
                case AiProvider.Claude: return _settings.ClaudeModel;
                case AiProvider.Gemini: return _settings.GeminiModel;
                case AiProvider.OpenAI: return _settings.OpenAiModel;
                case AiProvider.OpenCode: return _settings.OpenCodeModel;
                default: return "Unknown";
            }
        }

        private async void SendMessage() {
            string msg = _input.Text.Trim(); if (string.IsNullOrEmpty(msg)) return;
            _input.Clear(); AppendUserMessage(msg);
            string sys = BuildSystemPrompt();
            PrepareWorkspace(msg, sys);
            _btnSend.Visible = false; _btnStop.Visible = true;
            _statusLabel.Text = $"{_settings.Provider} running in {_workspace.RootDirectory}...";
            AppendAssistantStart();
            _cts = new CancellationTokenSource();
            _activeClient.WorkingDirectory = GetEffectiveWorkingDirectory();
            _activeClient.WorkspaceDirectory = _workspace.RootDirectory;
            try { await _activeClient.SendAsync(msg, sys, _cts.Token); }
            catch (Exception ex) { ShowError(ex.Message); }
            finally {
                SyncWorkspaceBackToEditor();
                _btnSend.Visible = true; _btnStop.Visible = false; UpdateStatusLabel();
            }
        }

        private string BuildSystemPrompt()
        {
            string sys = VizzySystemPrompt.Text;
            if (_chkInclude.Checked && GetCurrentCode != null)
            {
                sys += "\n\n## Workspace\nThe current editable C# script is available as `CurrentProgram.cs` in the working directory. Edit that file directly when you need to change code.";
            }
            return sys;
        }

        private void PrepareWorkspace(string userMessage, string systemPrompt)
        {
            _workspace = AgentWorkspace.Create(GetEffectiveWorkingDirectory(), GetDocumentKeySafe());
            EnsureWorkspaceWatcher();
            string currentCode = GetCurrentCode != null ? GetCurrentCode.Invoke() : string.Empty;
            _suppressWorkspaceWatcher = true;
            _workspace.WriteInputs(currentCode, userMessage, systemPrompt);
            _suppressWorkspaceWatcher = false;
        }

        private void SyncWorkspaceBackToEditor()
        {
            if (OnReplaceCode == null)
                return;

            string updated = _workspace.ReadCurrentProgram();
            if (string.IsNullOrWhiteSpace(updated))
                return;

            string current = GetCurrentCode != null ? GetCurrentCode.Invoke() : string.Empty;
            if (!string.Equals(updated, current, StringComparison.Ordinal))
            {
                OnReplaceCode(updated);
                SafeInvoke(() =>
                {
                    _history.SelectionStart = _history.TextLength;
                    _history.SelectionColor = _isDark ? Color.FromArgb(120, 220, 120) : Color.DarkGreen;
                    _history.AppendText("\nWorkspace changes applied to the editor.\n");
                    _history.ScrollToCaret();
                });
            }
        }

        private void InitializeWorkspaceSync()
        {
            _workspaceSyncTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _workspaceSyncTimer.Tick += (_, _) =>
            {
                if (!_workspaceDirty)
                    return;

                _workspaceDirty = false;
                SyncWorkspaceBackToEditor();
            };
            _workspaceSyncTimer.Start();
        }

        private void EnsureWorkspaceWatcher()
        {
            _workspaceWatcher?.Dispose();
            _workspaceWatcher = new FileSystemWatcher(_workspace.RootDirectory, "CurrentProgram.cs")
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
            };
            _workspaceWatcher.Changed += OnWorkspaceFileChanged;
            _workspaceWatcher.Created += OnWorkspaceFileChanged;
            _workspaceWatcher.Renamed += OnWorkspaceFileChanged;
            _workspaceWatcher.EnableRaisingEvents = true;
        }

        private void OnWorkspaceFileChanged(object? sender, FileSystemEventArgs e)
        {
            if (_suppressWorkspaceWatcher)
                return;

            _workspaceDirty = true;
        }

        private string? GetDocumentKeySafe()
        {
            try
            {
                if (GetDocumentKey != null)
                {
                    string key = GetDocumentKey.Invoke();
                    if (!string.IsNullOrWhiteSpace(key))
                        return key;
                }
            }
            catch
            {
            }

            return null;
        }

        private string GetEffectiveWorkingDirectory()
        {
            try
            {
                if (GetWorkingDirectory != null)
                {
                    string dir = GetWorkingDirectory.Invoke();
                    if (!string.IsNullOrWhiteSpace(dir) && System.IO.Directory.Exists(dir))
                        return dir;
                }
            }
            catch { }

            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private void OpenNativeCli()
        {
            string prompt = _input.Text.Trim();
            string systemPrompt = BuildSystemPrompt();
            PrepareWorkspace(prompt, systemPrompt);
            string workingDirectory = _workspace.RootDirectory;

            if (!CliIntegration.TryLaunchInteractive(_settings, workingDirectory, systemPrompt, prompt, out var error))
            {
                ShowError(error);
                return;
            }

            SafeInvoke(() =>
            {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(255, 220, 120) : Color.DarkGoldenrod;
                _history.SelectionFont = new Font("Segoe UI", 9f, FontStyle.Italic);
                _history.AppendText($"\nOpened {_settings.Provider} CLI in workspace: {workingDirectory}\n");
                _history.AppendText($"Current editable file: {_workspace.CurrentProgramPath}\n");
                _history.SelectionFont = new Font("Consolas", 10f);
                _history.ScrollToCaret();
            });
        }

        private void AppendUserMessage(string msg) {
            SafeInvoke(() => {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(100, 160, 255) : Color.DarkBlue;
                _history.SelectionFont = new Font("Segoe UI", 10f, FontStyle.Bold);
                _history.AppendText("\n▶ USER\n");
                _history.SelectionColor = _isDark ? Color.FromArgb(230, 230, 230) : Color.Black;
                _history.SelectionFont = new Font("Consolas", 10f);
                _history.AppendText(msg + "\n\n");
            });
        }

        private void AppendAssistantStart() {
            SafeInvoke(() => {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(100, 220, 130) : Color.DarkGreen;
                _history.SelectionFont = new Font("Segoe UI", 10f, FontStyle.Bold);
                _history.AppendText($"◆ {_settings.Provider}\n");
                _history.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
                _history.SelectionFont = new Font("Consolas", 10f);
            });
        }

        private void AppendStreamChunk(string chunk) { SafeInvoke(() => { _history.AppendText(chunk); _history.ScrollToCaret(); }); }
        private void AppendToolActivity(string msg) { SafeInvoke(() => { _history.SelectionStart = _history.TextLength; _history.SelectionFont = new Font("Segoe UI", 9f, FontStyle.Italic); _history.SelectionColor = Color.Gray; _history.AppendText("\n" + msg + "\n"); _history.SelectionFont = new Font("Consolas", 10f); _history.ScrollToCaret(); }); }
        private void FinishMessage(string full) { SafeInvoke(() => { if (full.Contains("```")) AddInsertCodeButton(full); _history.AppendText("\n\n"); _history.ScrollToCaret(); _btnSend.Visible = true; _btnStop.Visible = false; UpdateStatusLabel(); }); }

        private void AddInsertCodeButton(string full) {
            int start = full.IndexOf("```csharp"); if (start < 0) start = full.IndexOf("```");
            if (start < 0) return;
            int codeStart = full.IndexOf('\n', start) + 1;
            int codeEnd = full.IndexOf("```", codeStart);
            if (codeStart <= 0 || codeEnd < 0) return;
            string code = full.Substring(codeStart, codeEnd - codeStart).Trim();
            var btn = new Button { Text = "⬆ Insert Code", Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Height = 30, Width = 120, BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (_, _) => { OnInsertCode?.Invoke(code); btn.Text = "✓ Done"; };
            ShowFloatingButton(btn);
        }

        private Button _floatingBtn;
        private void ShowFloatingButton(Button btn) { _floatingBtn?.Dispose(); _floatingBtn = btn; btn.Parent = this; btn.BringToFront(); btn.Location = new Point(_history.Right - btn.Width - 15, _history.Bottom - btn.Height - 15); btn.Visible = true; _input.Click += (_, _) => { btn.Visible = false; }; }
        private void ShowError(string err) { SafeInvoke(() => { _history.SelectionStart = _history.TextLength; _history.SelectionColor = Color.Tomato; _history.AppendText($"\n⚠ ERROR: {err}\n\n"); _history.ScrollToCaret(); }); }
        private void ShowSettings() {
            using var dlg = new SettingsDialog(_settings);
            if (dlg.ShowDialog() == DialogResult.OK) {
                Settings = dlg.Result;
                _settings.Save();
                RefreshClient();
            }
        }

        public void ApplyTheme(bool dark) {
            _isDark = dark; var bg = dark ? Color.FromArgb(30, 30, 30) : Color.White; var fg = dark ? Color.FromArgb(230, 230, 230) : Color.Black;
            BackColor = dark ? Color.FromArgb(37, 37, 38) : Color.WhiteSmoke; _history.BackColor = bg; _history.ForeColor = fg;
            _input.BackColor = dark ? Color.FromArgb(45, 45, 48) : Color.White; _input.ForeColor = fg;
            _statusLabel.ForeColor = dark ? Color.Gray : Color.DarkGray; _chkInclude.ForeColor = fg;
        }

        private void SafeInvoke(Action a) { if (InvokeRequired) BeginInvoke(a); else a(); }

        private void HandleApprovalRequired(ToolCallRequest request)
        {
            SafeInvoke(() => {
                var approvalPanel = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = _isDark ? Color.FromArgb(45, 55, 65) : Color.FromArgb(240, 240, 245), Padding = new Padding(10) };
                
                var label = new Label 
                { 
                    Text = $"🔧 Tool Execution Request: {request.ToolName}",
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    ForeColor = _isDark ? Color.FromArgb(255, 200, 100) : Color.DarkOrange,
                    Dock = DockStyle.Top
                };
                
                var details = new Label
                {
                    Text = $"Parameters: {string.Join(", ", request.Parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}",
                    Font = new Font("Consolas", 8.5f),
                    ForeColor = _isDark ? Color.FromArgb(200, 200, 200) : Color.Gray,
                    Dock = DockStyle.Top
                };

                var btnPanel = new Panel { Dock = DockStyle.Bottom, Height = 40 };
                
                var btnApprove = new Button
                {
                    Text = "✓ Approve",
                    BackColor = Color.FromArgb(0, 150, 80),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    Width = 100,
                    Height = 35,
                    Left = 0
                };
                btnApprove.FlatAppearance.BorderSize = 0;
                btnApprove.Click += (_, _) => {
                    _activeClient.ApproveTool(request.CallId);
                    approvalPanel.Dispose();
                };

                var btnReject = new Button
                {
                    Text = "✗ Reject",
                    BackColor = Color.FromArgb(200, 60, 60),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                    Width = 100,
                    Height = 35,
                    Left = 110
                };
                btnReject.FlatAppearance.BorderSize = 0;
                btnReject.Click += (_, _) => {
                    _activeClient.RejectTool(request.CallId);
                    approvalPanel.Dispose();
                };

                btnPanel.Controls.Add(btnApprove);
                btnPanel.Controls.Add(btnReject);
                approvalPanel.Controls.Add(label);
                approvalPanel.Controls.Add(details);
                approvalPanel.Controls.Add(btnPanel);

                _history.Parent?.Controls.Add(approvalPanel);
                approvalPanel.BringToFront();
            });
        }

        private void HandleToolCall(ToolCallResponse response)
        {
            SafeInvoke(() => {
                if (response.Success)
                {
                    _history.SelectionStart = _history.TextLength;
                    _history.SelectionColor = Color.FromArgb(100, 200, 100);
                    _history.SelectionFont = new Font("Segoe UI", 9f, FontStyle.Italic);
                    _history.AppendText($"\n✓ Tool executed successfully\n");
                }
                else
                {
                    _history.SelectionStart = _history.TextLength;
                    _history.SelectionColor = Color.FromArgb(200, 100, 100);
                    _history.SelectionFont = new Font("Segoe UI", 9f, FontStyle.Italic);
                    _history.AppendText($"\n✗ Tool execution failed\n");
                }
                _history.SelectionFont = new Font("Consolas", 10f);
                _history.ScrollToCaret();
            });
        }
    }
}
