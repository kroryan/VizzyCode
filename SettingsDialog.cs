using System;
using System.Drawing;
using System.Windows.Forms;

namespace VizzyCode
{
    public class SettingsDialog : Form
    {
        private TabControl _tabs;
        private Button     _btnOk;
        private Button     _btnCancel;

        // Claude controls
        private ComboBox _cmbClaudeMode;
        private TextBox  _txtClaudeKey;
        private ComboBox _cmbClaudeModel;

        // Gemini controls
        private ComboBox _cmbGeminiMode;
        private TextBox  _txtGeminiKey;
        private ComboBox _cmbGeminiModel;

        // OpenAI controls
        private ComboBox _cmbOpenAiMode;
        private TextBox  _txtOpenAiKey;
        private ComboBox _cmbOpenAiModel;

        // OpenCode controls
        private ComboBox _cmbOpenCodeMode;
        private TextBox  _txtOpenCodeUrl;
        private TextBox  _txtOpenCodeModel;
        private TextBox  _txtOpenCodeKey;

        // Advanced controls
        private NumericUpDown _numTemp;
        private NumericUpDown _numMaxTokens;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public AiSettings Result { get; private set; }

        public SettingsDialog(AiSettings current)
        {
            Result = current;
            BuildUI();
            LoadSettings(current);
        }

        private void BuildUI()
        {
            Text = "VizzyCode AI Settings";
            Size = new Size(540, 450);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = MinimizeBox = false;

            _tabs = new TabControl { Dock = DockStyle.Top, Height = 340, Padding = new Point(12, 4) };
            
            _tabs.TabPages.Add(CreateClaudeTab());
            _tabs.TabPages.Add(CreateGeminiTab());
            _tabs.TabPages.Add(CreateOpenAiTab());
            _tabs.TabPages.Add(CreateOpenCodeTab());
            _tabs.TabPages.Add(CreateAdvancedTab());

            Controls.Add(_tabs);

            _btnOk = new Button { Text = "Save", DialogResult = DialogResult.OK, Location = new Point(330, 360), Width = 85, Height = 28 };
            _btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Location = new Point(422, 360), Width = 85, Height = 28 };
            
            _btnOk.Click += (s, e) => SaveSettings();
            
            Controls.Add(_btnOk);
            Controls.Add(_btnCancel);
            AcceptButton = _btnOk;
            CancelButton = _btnCancel;
        }

        private TabPage CreateClaudeTab() {
            var p = new TabPage("Claude"); int y = 20;
            Label(p, "Access Mode:", 16, y);
            _cmbClaudeMode = Combo(p, 16, y + 22, 430, new[] { "Claude Code CLI (Login via terminal first)", "Anthropic API Key" }, false);
            y += 60; Label(p, "API Key (if using API Mode):", 16, y);
            _txtClaudeKey = Textbox(p, 16, y + 22, 430, true);
            y += 60; Label(p, "Model:", 16, y);
            _cmbClaudeModel = Combo(p, 16, y + 22, 300, new[] { "sonnet", "opus", "claude-sonnet-4-5", "claude-sonnet-4-6" }, true);
            return p;
        }

        private TabPage CreateGeminiTab() {
            var p = new TabPage("Gemini"); int y = 20;
            Label(p, "Access Mode:", 16, y);
            _cmbGeminiMode = Combo(p, 16, y + 22, 430, new[] { "Gemini CLI (Login via terminal first)", "Google AI Studio API Key" }, false);
            y += 60; Label(p, "API Key (if using API Mode):", 16, y);
            _txtGeminiKey = Textbox(p, 16, y + 22, 430, true);
            y += 60; Label(p, "Model:", 16, y);
            _cmbGeminiModel = Combo(p, 16, y + 22, 300, new[] { "gemini-2.5-pro", "gemini-2.5-flash", "gemini-2.0-flash" }, true);
            return p;
        }

        private TabPage CreateOpenAiTab() {
            var p = new TabPage("OpenAI/Codex"); int y = 20;
            Label(p, "Access Mode:", 16, y);
            _cmbOpenAiMode = Combo(p, 16, y + 22, 430, new[] { "Codex CLI (Login via terminal first)", "OpenAI API Key" }, false);
            y += 60; Label(p, "API Key (if using API Mode):", 16, y);
            _txtOpenAiKey = Textbox(p, 16, y + 22, 430, true);
            y += 60; Label(p, "Model:", 16, y);
            _cmbOpenAiModel = Combo(p, 16, y + 22, 300, new[] { "gpt-5-codex", "gpt-5", "gpt-4.1" }, true);
            return p;
        }

        private TabPage CreateOpenCodeTab() {
            var p = new TabPage("OpenCode"); int y = 20;
            Label(p, "Access Mode:", 16, y);
            _cmbOpenCodeMode = Combo(p, 16, y + 22, 430, new[] { "OpenCode CLI (Login via terminal first)", "OpenAI-compatible API" }, false);
            y += 60; Label(p, "Base URL (for API Mode):", 16, y);
            _txtOpenCodeUrl = Textbox(p, 16, y + 22, 430, false);
            y += 60; Label(p, "Model Name (CLI uses provider/model):", 16, y);
            _txtOpenCodeModel = Textbox(p, 16, y + 22, 430, false);
            y += 60; Label(p, "API Key (optional for API mode):", 16, y);
            _txtOpenCodeKey = Textbox(p, 16, y + 22, 430, true);
            return p;
        }

        private TabPage CreateAdvancedTab() {
            var p = new TabPage("Advanced"); int y = 20;
            Label(p, "Temperature (0.0 - 2.0):", 16, y);
            _numTemp = new NumericUpDown { Location = new Point(16, y + 22), Width = 80, DecimalPlaces = 2, Minimum = 0, Maximum = 2, Increment = 0.1m };
            p.Controls.Add(_numTemp);
            y += 60; Label(p, "Max Tokens:", 16, y);
            _numMaxTokens = new NumericUpDown { Location = new Point(16, y + 22), Width = 100, Minimum = 1, Maximum = 1000000, Increment = 1024 };
            p.Controls.Add(_numMaxTokens);
            return p;
        }

        private void LoadSettings(AiSettings s) {
            _cmbClaudeMode.SelectedIndex = (int)s.ClaudeMode; _txtClaudeKey.Text = s.ClaudeApiKey; _cmbClaudeModel.Text = s.ClaudeModel;
            _cmbGeminiMode.SelectedIndex = (int)s.GeminiMode; _txtGeminiKey.Text = s.GeminiApiKey; _cmbGeminiModel.Text = s.GeminiModel;
            _cmbOpenAiMode.SelectedIndex = (int)s.OpenAiMode; _txtOpenAiKey.Text = s.OpenAiApiKey; _cmbOpenAiModel.Text = s.OpenAiModel;
            _cmbOpenCodeMode.SelectedIndex = (int)s.OpenCodeMode; _txtOpenCodeUrl.Text = s.OpenCodeBaseUrl; _txtOpenCodeModel.Text = s.OpenCodeModel; _txtOpenCodeKey.Text = s.OpenCodeApiKey;
            _numTemp.Value = (decimal)s.Temperature; _numMaxTokens.Value = s.MaxTokens;
            _tabs.SelectedIndex = (int)s.Provider;
        }

        private void SaveSettings() {
            Result.ClaudeMode = (AccessMode)_cmbClaudeMode.SelectedIndex; Result.ClaudeApiKey = _txtClaudeKey.Text.Trim(); Result.ClaudeModel = _cmbClaudeModel.Text;
            Result.GeminiMode = (AccessMode)_cmbGeminiMode.SelectedIndex; Result.GeminiApiKey = _txtGeminiKey.Text.Trim(); Result.GeminiModel = _cmbGeminiModel.Text;
            Result.OpenAiMode = (AccessMode)_cmbOpenAiMode.SelectedIndex; Result.OpenAiApiKey = _txtOpenAiKey.Text.Trim(); Result.OpenAiModel = _cmbOpenAiModel.Text;
            Result.OpenCodeMode = (AccessMode)_cmbOpenCodeMode.SelectedIndex; Result.OpenCodeBaseUrl = _txtOpenCodeUrl.Text.Trim(); Result.OpenCodeModel = _txtOpenCodeModel.Text.Trim(); Result.OpenCodeApiKey = _txtOpenCodeKey.Text.Trim();
            Result.Temperature = (double)_numTemp.Value; Result.MaxTokens = (int)_numMaxTokens.Value;
            Result.Provider = (AiProvider)_tabs.SelectedIndex;
        }

        private void Label(TabPage p, string text, int x, int y) { p.Controls.Add(new Label { Text = text, Location = new Point(x, y), AutoSize = true }); }
        private ComboBox Combo(TabPage p, int x, int y, int w, string[] items, bool allowCustom) {
            var c = new ComboBox { Location = new Point(x, y), Width = w, DropDownStyle = allowCustom ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList };
            c.Items.AddRange(items); p.Controls.Add(c); return c;
        }
        private TextBox Textbox(TabPage p, int x, int y, int w, bool password) {
            var t = new TextBox { Location = new Point(x, y), Width = w, UseSystemPasswordChar = password };
            p.Controls.Add(t); return t;
        }
    }
}
