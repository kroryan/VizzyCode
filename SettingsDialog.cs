using System;
using System.Drawing;
using System.Windows.Forms;

namespace VizzyCode
{
    public class SettingsDialog : Form
    {
        private ComboBox _cmbMode;
        private TextBox  _txtApiKey;
        private ComboBox _cmbModel;
        private Label    _lblApiKey;
        private Label    _lblInfo;
        private Button   _btnOk;
        private Button   _btnCancel;

        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public ClaudeSettings Result { get; private set; }

        public SettingsDialog(ClaudeSettings current)
        {
            Result = new ClaudeSettings
            {
                Mode   = current.Mode,
                ApiKey = current.ApiKey,
                Model  = current.Model
            };
            BuildUI(current);
        }

        private void BuildUI(ClaudeSettings current)
        {
            Text = "VizzyCode Settings";
            Size = new Size(480, 300);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = MinimizeBox = false;

            int y = 18;

            // Mode
            Add(new Label { Text = "Claude Access Mode:", Location = new Point(16, y), AutoSize = true });
            y += 22;
            _cmbMode = new ComboBox
            {
                Location = new Point(16, y), Width = 430,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbMode.Items.AddRange(new object[]
            {
                "claude CLI  (uses your claude.ai subscription — no API key needed)",
                "Anthropic API Key  (direct API access)"
            });
            _cmbMode.SelectedIndex = current.Mode == ClaudeMode.ApiKey ? 1 : 0;
            Controls.Add(_cmbMode); y += 34;

            // Info label
            _lblInfo = new Label
            {
                Location = new Point(16, y), Size = new Size(440, 42),
                Text = "Requires Claude Code to be installed and logged in.\n" +
                       "Install: https://claude.ai/code  then run: claude login",
                Font = new Font("Segoe UI", 8f),
                ForeColor = Color.DimGray
            };
            Controls.Add(_lblInfo); y += 48;

            // API key
            _lblApiKey = new Label { Text = "Anthropic API Key:", Location = new Point(16, y), AutoSize = true };
            Controls.Add(_lblApiKey); y += 22;
            _txtApiKey = new TextBox
            {
                Location = new Point(16, y), Width = 430,
                Text = current.ApiKey,
                UseSystemPasswordChar = true
            };
            Controls.Add(_txtApiKey); y += 30;

            // Model
            Add(new Label { Text = "Model:", Location = new Point(16, y), AutoSize = true });
            y += 22;
            _cmbModel = new ComboBox
            {
                Location = new Point(16, y), Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _cmbModel.Items.AddRange(new object[]
            {
                "claude-sonnet-4-6",
                "claude-opus-4-6",
                "claude-haiku-4-5-20251001"
            });
            _cmbModel.Text = current.Model;
            if (_cmbModel.SelectedIndex < 0) _cmbModel.SelectedIndex = 0;
            Controls.Add(_cmbModel); y += 38;

            // Buttons
            _btnOk = new Button { Text = "Save", DialogResult = DialogResult.OK,
                Location = new Point(270, y), Width = 85, Height = 28 };
            _btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel,
                Location = new Point(362, y), Width = 85, Height = 28 };
            Controls.Add(_btnOk); Controls.Add(_btnCancel);
            AcceptButton = _btnOk; CancelButton = _btnCancel;

            _cmbMode.SelectedIndexChanged += (_, _) => UpdateVisibility();
            _btnOk.Click += (_, _) =>
            {
                Result = new ClaudeSettings
                {
                    Mode   = _cmbMode.SelectedIndex == 1 ? ClaudeMode.ApiKey : ClaudeMode.ClaudeCli,
                    ApiKey = _txtApiKey.Text.Trim(),
                    Model  = _cmbModel.Text.Trim()
                };
            };

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            bool apiMode = _cmbMode.SelectedIndex == 1;
            _lblInfo.Visible    = !apiMode;
            _lblApiKey.Visible  = apiMode;
            _txtApiKey.Visible  = apiMode;
            _txtApiKey.UseSystemPasswordChar = true;
        }

        private void Add(Control c) => Controls.Add(c);
    }
}
