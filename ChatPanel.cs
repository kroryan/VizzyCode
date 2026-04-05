using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace VizzyCode
{
    /// <summary>
    /// Chat panel that sends Vizzy-context-aware prompts to Claude.
    /// Supports streaming text, tool-use activity display, and both
    /// claude.ai subscription (CLI) and Anthropic API key modes.
    /// </summary>
    public class ChatPanel : Panel
    {
        // ── Public callbacks (set by MainForm) ────────────────────────────────
        public Action<string> OnInsertCode;   // called when user clicks "Insert Code"
        public Func<string>   GetCurrentCode; // returns current editor content

        // ── Controls ──────────────────────────────────────────────────────────
        private RichTextBox _history;
        private TextBox     _input;
        private Button      _btnSend;
        private Button      _btnStop;
        private CheckBox    _chkInclude;
        private Label       _statusLabel;
        private Button      _btnSettings;

        // ── State ─────────────────────────────────────────────────────────────
        private readonly ClaudeClient _claude = new ClaudeClient();
        private CancellationTokenSource? _cts;
        private bool _isDark = true;
        private bool _inTool = false;  // are we currently inside a tool call?

        public ClaudeClient Client => _claude;

        public ChatPanel()
        {
            BuildUI();
            WireEvents();
        }

        // ── UI construction ───────────────────────────────────────────────────

        private void BuildUI()
        {
            Dock = DockStyle.Fill;

            // Title bar
            var title = new Label
            {
                Text      = "Claude AI  ·  Vizzy Assistant",
                Dock      = DockStyle.Top, Height = 28,
                Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(8, 0, 0, 0)
            };

            // Settings button
            _btnSettings = new Button
            {
                Text      = "⚙", Width = 28, Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10f),
                Anchor    = AnchorStyles.Top | AnchorStyles.Right,
                TabStop   = false
            };
            _btnSettings.FlatAppearance.BorderSize = 0;

            // Chat history
            _history = new RichTextBox
            {
                Dock        = DockStyle.Fill,
                ReadOnly    = true,
                WordWrap    = true,
                ScrollBars  = RichTextBoxScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                Font        = new Font("Consolas", 9.5f),
                DetectUrls  = false
            };

            // Status bar
            _statusLabel = new Label
            {
                Dock      = DockStyle.Top, Height = 18,
                Font      = new Font("Segoe UI", 7.5f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(4, 0, 0, 0),
                Text      = "Ready  ·  Using claude CLI (subscription)"
            };

            // Include code checkbox
            _chkInclude = new CheckBox
            {
                Text     = "Include current code as context",
                AutoSize = true,
                Checked  = true,
                Font     = new Font("Segoe UI", 8.5f)
            };

            // Input textbox
            _input = new TextBox
            {
                Dock          = DockStyle.Fill,
                Multiline     = true,
                ScrollBars    = ScrollBars.Vertical,
                Font          = new Font("Consolas", 9.5f),
                BorderStyle   = BorderStyle.None,
                Height        = 70,
                AcceptsReturn = false,
                AcceptsTab    = false
            };

            // Send / Stop buttons
            _btnSend = new Button
            {
                Text      = "Send  ↵", Width = 80, Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9f, FontStyle.Bold),
                Anchor    = AnchorStyles.Bottom | AnchorStyles.Right
            };
            _btnStop = new Button
            {
                Text      = "■ Stop", Width = 60, Height = 28,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 9f),
                Anchor    = AnchorStyles.Bottom | AnchorStyles.Right,
                Visible   = false
            };

            // Input area panel
            var inputPanel = new Panel { Dock = DockStyle.Bottom, Height = 105 };
            var btnRow = new FlowLayoutPanel
            {
                Dock          = DockStyle.Bottom, Height = 32,
                FlowDirection = FlowDirection.RightToLeft,
                Padding       = new Padding(2)
            };
            btnRow.Controls.AddRange(new Control[] { _btnSend, _btnStop });

            var chkRow = new FlowLayoutPanel
            {
                Dock    = DockStyle.Bottom, Height = 22,
                Padding = new Padding(4, 2, 2, 0)
            };
            chkRow.Controls.Add(_chkInclude);

            inputPanel.Controls.Add(_input);
            inputPanel.Controls.Add(chkRow);
            inputPanel.Controls.Add(btnRow);

            // Title row
            var titlePanel = new Panel { Dock = DockStyle.Top, Height = 28 };
            titlePanel.Controls.Add(title);
            titlePanel.Controls.Add(_btnSettings);
            _btnSettings.Location = new Point(titlePanel.Width - 30, 0);
            titlePanel.SizeChanged += (_, _) =>
                _btnSettings.Location = new Point(titlePanel.Width - 30, 0);

            Controls.Add(_history);
            Controls.Add(_statusLabel);
            Controls.Add(inputPanel);
            Controls.Add(titlePanel);

            ApplyTheme(true);
        }

        private void WireEvents()
        {
            _btnSend.Click     += (_, _) => SendMessage();
            _btnStop.Click     += (_, _) => _cts?.Cancel();
            _btnSettings.Click += (_, _) => ShowSettings();

            _input.KeyDown += (_, e) =>
            {
                if (e.KeyCode == Keys.Return && !e.Shift)
                { e.SuppressKeyPress = true; SendMessage(); }
            };

            _claude.OnChunk        += chunk => AppendStreamChunk(chunk);
            _claude.OnDone         += full  => FinishMessage(full);
            _claude.OnError        += err   => ShowError(err);
            _claude.OnToolActivity += msg   => AppendToolActivity(msg);
        }

        // ── Sending ───────────────────────────────────────────────────────────

        private async void SendMessage()
        {
            string msg = _input.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;

            _input.Clear();
            AppendUserMessage(msg);

            // Build system prompt (replace default to focus purely on Vizzy)
            string sys = VizzySystemPrompt.Text;
            if (_chkInclude.Checked && GetCurrentCode != null)
            {
                string code = GetCurrentCode.Invoke();
                if (!string.IsNullOrWhiteSpace(code))
                    sys += $"\n\n## Current Code Being Edited\n```csharp\n{code}\n```" +
                           "\nThe user may reference or ask to modify the above code.";
            }

            _btnSend.Visible = false;
            _btnStop.Visible = true;
            _statusLabel.Text = "Claude is thinking...";
            _inTool = false;
            AppendAssistantStart();

            _cts = new CancellationTokenSource();
            await _claude.SendAsync(msg, sys, _cts.Token);
        }

        // ── History display ───────────────────────────────────────────────────

        private void AppendUserMessage(string msg)
        {
            SafeInvoke(() =>
            {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(100, 160, 255) : Color.DarkBlue;
                _history.SelectionFont  = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                _history.AppendText("\n▶ You\n");
                _history.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
                _history.SelectionFont  = new Font("Consolas", 9.5f);
                _history.AppendText(msg + "\n\n");
            });
        }

        private void AppendAssistantStart()
        {
            SafeInvoke(() =>
            {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(100, 220, 130) : Color.DarkGreen;
                _history.SelectionFont  = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                _history.AppendText("◆ Claude\n");
                _history.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
                _history.SelectionFont  = new Font("Consolas", 9.5f);
            });
        }

        private void AppendStreamChunk(string chunk)
        {
            SafeInvoke(() =>
            {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
                _history.SelectionFont  = new Font("Consolas", 9.5f);
                _history.AppendText(chunk);
                _history.ScrollToCaret();
            });
        }

        private void AppendToolActivity(string msg)
        {
            SafeInvoke(() =>
            {
                bool isStart = msg.StartsWith("🔧");
                bool isDone  = msg.StartsWith("✓");

                _history.SelectionStart = _history.TextLength;
                _history.SelectionFont  = new Font("Segoe UI", 8f, FontStyle.Italic);

                if (isStart)
                {
                    _history.SelectionColor = _isDark
                        ? Color.FromArgb(180, 140, 255)
                        : Color.DarkViolet;
                    _history.AppendText("\n" + msg + " ");
                    _inTool = true;
                }
                else if (isDone && _inTool)
                {
                    _history.SelectionColor = _isDark
                        ? Color.FromArgb(100, 200, 100)
                        : Color.DarkGreen;
                    _history.AppendText(msg + "\n");
                    _inTool = false;
                }
                else
                {
                    _history.SelectionColor = _isDark
                        ? Color.FromArgb(150, 150, 200)
                        : Color.SlateBlue;
                    _history.AppendText(msg + "\n");
                }

                _history.SelectionFont  = new Font("Consolas", 9.5f);
                _history.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
                _history.ScrollToCaret();
            });
        }

        private void FinishMessage(string full)
        {
            SafeInvoke(() =>
            {
                // Append Insert Code button if response has a code block
                if (full.Contains("```"))
                    AddInsertCodeButton(full);

                _history.AppendText("\n\n");
                _history.ScrollToCaret();
                _btnSend.Visible  = true;
                _btnStop.Visible  = false;
                _statusLabel.Text = $"Done  ·  {GetModeLabel()}";
                _inTool = false;
            });
        }

        // ── Insert Code button ────────────────────────────────────────────────

        private void AddInsertCodeButton(string full)
        {
            // Extract first code block
            int start = full.IndexOf("```csharp");
            if (start < 0) start = full.IndexOf("```");
            if (start < 0) return;
            int codeStart = full.IndexOf('\n', start) + 1;
            int codeEnd   = full.IndexOf("```", codeStart);
            if (codeStart <= 0 || codeEnd < 0) return;
            string code = full.Substring(codeStart, codeEnd - codeStart).Trim();

            _history.AppendText("\n");
            var btn = new Button
            {
                Text      = "⬆ Insert Code into Editor",
                Font      = new Font("Segoe UI", 8.5f),
                FlatStyle = FlatStyle.Flat,
                Height    = 24, Width = 190,
                BackColor = _isDark ? Color.FromArgb(0, 122, 204) : Color.SteelBlue,
                ForeColor = Color.White,
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (_, _) => { OnInsertCode?.Invoke(code); btn.Text = "✓ Inserted"; };
            ShowFloatingButton(btn);
        }

        private Button _floatingBtn;
        private void ShowFloatingButton(Button btn)
        {
            _floatingBtn?.Dispose();
            _floatingBtn = btn;
            btn.Parent = this;
            btn.BringToFront();
            btn.Location = new Point(
                _history.Right  - btn.Width  - 8,
                _history.Bottom - btn.Height - 8);
            btn.Visible = true;
            _input.Click += (_, _) => { btn.Visible = false; };
        }

        private void ShowError(string err)
        {
            SafeInvoke(() =>
            {
                _history.SelectionStart = _history.TextLength;
                _history.SelectionColor = _isDark ? Color.FromArgb(255, 100, 100) : Color.DarkRed;
                _history.SelectionFont  = new Font("Consolas", 9.5f);
                _history.AppendText($"\n⚠ {err}\n\n");
                _history.ScrollToCaret();
                _btnSend.Visible  = true;
                _btnStop.Visible  = false;
                _statusLabel.Text = "Error — check Settings (⚙) or verify claude is installed";
                _inTool = false;
            });
        }

        // ── Settings dialog ───────────────────────────────────────────────────

        private void ShowSettings()
        {
            using var dlg = new SettingsDialog(_claude.Settings);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _claude.Settings  = dlg.Result;
                _statusLabel.Text = $"Settings saved  ·  {GetModeLabel()}";
            }
        }

        private string GetModeLabel()
        {
            if (_claude.Settings.Mode == ClaudeMode.ApiKey &&
                !string.IsNullOrWhiteSpace(_claude.Settings.ApiKey))
                return $"API key  ·  {_claude.Settings.Model}";
            return $"claude CLI  ·  {_claude.Settings.Model}";
        }

        // ── Theme ─────────────────────────────────────────────────────────────

        public void ApplyTheme(bool dark)
        {
            _isDark = dark;
            var bg     = dark ? Color.FromArgb(30, 30, 30)    : Color.White;
            var bg2    = dark ? Color.FromArgb(37, 37, 38)    : Color.FromArgb(245, 245, 245);
            var fg     = dark ? Color.FromArgb(220, 220, 220) : Color.Black;
            var titleBg = dark ? Color.FromArgb(45, 45, 48)  : Color.FromArgb(0, 122, 204);
            var btnBg  = Color.FromArgb(0, 122, 204);

            BackColor = bg2;
            _history.BackColor     = bg;
            _history.ForeColor     = fg;
            _input.BackColor       = bg2;
            _input.ForeColor       = fg;
            _statusLabel.BackColor = bg2;
            _statusLabel.ForeColor = dark ? Color.FromArgb(150, 150, 150) : Color.Gray;
            _chkInclude.BackColor  = bg2;
            _chkInclude.ForeColor  = fg;
            _btnSend.BackColor     = btnBg;
            _btnSend.ForeColor     = Color.White;
            _btnSend.FlatAppearance.BorderColor = btnBg;
            _btnStop.BackColor     = dark ? Color.FromArgb(200, 70, 70) : Color.Firebrick;
            _btnStop.ForeColor     = Color.White;
            _btnSettings.BackColor = titleBg;
            _btnSettings.ForeColor = Color.White;

            foreach (Control c in Controls)
                if (c is Panel p) p.BackColor = bg2;
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void SafeInvoke(Action a)
        {
            if (InvokeRequired) BeginInvoke(a);
            else a();
        }
    }
}
