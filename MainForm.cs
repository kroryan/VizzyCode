using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VizzyCode
{
    public partial class MainForm : Form
    {
        private string? _currentFile;
        private CleanViewSidecar? _currentSidecar;
        private bool _isDark = true;

        // Juno integration state
        private int  _junoPartId   = -1;   // part id currently loaded from game
        private string? _junoPartName;     // display name for status

        public MainForm(string? fileToOpen = null)
        {
            InitializeComponent();

            Shown += (_, _) =>
            {
                try { splitMain.SplitterDistance = 230; } catch { }
                try { splitLeft.SplitterDistance = (int)(splitLeft.Height * 0.70); } catch { }
            };

            if (fileToOpen != null && File.Exists(fileToOpen))
                LoadFile(fileToOpen);
            else
                ShowWelcome();
        }

        // ── Welcome ────────────────────────────────────────────────────────────

        private void ShowWelcome()
        {
            codeEditor.Text =
                "// ═══════════════════════════════════════════════════════════\r\n" +
                "//  VizzyCode  –  Open · Convert · Edit Vizzy programs\r\n" +
                "// ═══════════════════════════════════════════════════════════\r\n" +
                "//\r\n" +
                "//  File > Open Craft XML   – open a craft .xml file\r\n" +
                "//  File > Open Vizzy XML   – open a standalone vizzy program\r\n" +
                "//  File > Save as .cs      – save the editable code\r\n" +
                "//  File > Save as Vizzy XML – export back to Vizzy XML\r\n" +
                "//\r\n" +
                "//  Toolbar: Ex: Craft / Ex: Vizzy  – load built-in examples\r\n" +
                "//\r\n";
            Highlight();
        }

        // ── File loading ───────────────────────────────────────────────────────

        private void LoadFile(string path)
        {
            try
            {
                var doc = XDocument.Load(path);
                var conv = new VizzyXmlConverter();
                bool isCraft = doc.Root?.Name.LocalName is "Craft" or "Assembly";
                string exactCode = isCraft
                    ? conv.ConvertCraftToCode(doc)
                    : conv.ConvertProgramToCode(doc);
                var cleanView = CodeCleanView.CreateCleanCode(exactCode);

                _currentFile = path;
                _currentSidecar = cleanView.Sidecar;
                codeEditor.Text = cleanView.CleanCode.Replace("\r\n", "\n").Replace("\n", "\r\n");
                Highlight();
                UpdateTree(doc, conv);
                SetTitle(path);

                string warnTxt = conv.Warnings.Count > 0
                    ? $"{conv.Warnings.Count} warning(s)"
                    : "OK – no warnings";
                statusLabel.Text = $"Loaded: {Path.GetFileName(path)}  |  {warnTxt}";

                warningsBox.Text = conv.Warnings.Count > 0
                    ? "Warnings:\r\n" + string.Join("\r\n", conv.Warnings)
                    : "No warnings.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:\n{ex.Message}", "Load Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Tree view ──────────────────────────────────────────────────────────

        private void UpdateTree(XDocument doc, VizzyXmlConverter conv)
        {
            treeView.Nodes.Clear();
            foreach (var prog in doc.Descendants("Program"))
            {
                string pname = prog.Attribute("name")?.Value ?? "(unnamed)";
                var pn = new TreeNode($"⚡ {pname}");

                var vars = prog.Element("Variables");
                if (vars != null)
                {
                    var vn = new TreeNode($"Variables ({vars.Elements("Variable").Count()})");
                    foreach (var v in vars.Elements("Variable"))
                    {
                        bool isList = v.Element("Items") != null;
                        string nm = v.Attribute("name")?.Value ?? "?";
                        vn.Nodes.Add(new TreeNode((isList ? "📋 " : "📌 ") + nm));
                    }
                    pn.Nodes.Add(vn);
                }

                var instrs = prog.Element("Instructions");
                if (instrs != null)
                {
                    int evCount = instrs.Descendants("Event").Count();
                    int total   = instrs.Descendants().Count();
                    var @in = new TreeNode($"Instructions (~{total} blocks, {evCount} events)");
                    foreach (var ev in instrs.Descendants("Event"))
                        @in.Nodes.Add(new TreeNode("🔔 Event: " + (ev.Attribute("event")?.Value ?? "?")));
                    foreach (var ci in instrs.Elements("CustomInstruction"))
                        @in.Nodes.Add(new TreeNode("🔧 " + (ci.Attribute("name")?.Value ?? "?")));
                    pn.Nodes.Add(@in);
                }
                pn.Expand();
                treeView.Nodes.Add(pn);
            }
            if (treeView.Nodes.Count > 0) treeView.Nodes[0].Expand();
        }

        // ── Syntax highlighting ────────────────────────────────────────────────

        private bool _highlighting;
        private void Highlight()
        {
            if (_highlighting) return;
            _highlighting = true;
            codeEditor.SuspendLayout();
            int ss = codeEditor.SelectionStart, sl = codeEditor.SelectionLength;

            codeEditor.SelectAll();
            codeEditor.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;

            string text = codeEditor.Text;

            // Keywords
            Colorize(text, new[]
            {
                "using","new","var","return","class","public","private","protected",
                "static","void","string","int","float","double","bool","true","false",
                "null","if","else","while","for","foreach","break","continue",
                "namespace","struct","readonly","const","override","virtual","abstract"
            }, _isDark ? Color.FromArgb(86, 156, 214) : Color.Blue);

            // Vz.* calls
            ColorizeRegex(text, @"Vz\.[A-Za-z_.]+", _isDark ? Color.FromArgb(220, 220, 170) : Color.DarkGoldenrod);

            // Numbers
            ColorizeRegex(text, @"\b\d+(\.\d+)?[fFdD]?\b", _isDark ? Color.FromArgb(181, 206, 168) : Color.DarkCyan);

            // Strings (before comments)
            ColorizeStrings(text, _isDark ? Color.FromArgb(214, 157, 133) : Color.DarkRed);

            // Comments (last, overrides everything)
            ColorizeComments(text, _isDark ? Color.FromArgb(87, 166, 74) : Color.DarkGreen);

            codeEditor.Select(ss, sl);
            codeEditor.SelectionColor = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
            codeEditor.ResumeLayout();
            _highlighting = false;
        }

        private void Colorize(string text, string[] words, Color color)
        {
            foreach (string w in words)
            {
                int idx = 0;
                while ((idx = text.IndexOf(w, idx, StringComparison.Ordinal)) >= 0)
                {
                    bool lo = idx == 0 || !char.IsLetterOrDigit(text[idx - 1]) && text[idx - 1] != '_';
                    bool ro = idx + w.Length >= text.Length || !char.IsLetterOrDigit(text[idx + w.Length]) && text[idx + w.Length] != '_';
                    if (lo && ro) { codeEditor.Select(idx, w.Length); codeEditor.SelectionColor = color; }
                    idx += w.Length;
                }
            }
        }

        private void ColorizeRegex(string text, string pattern, Color color)
        {
            foreach (Match m in Regex.Matches(text, pattern))
            {
                if (m.Index > 0 && (char.IsLetter(text[m.Index - 1]) || text[m.Index - 1] == '_')) continue;
                codeEditor.Select(m.Index, m.Length); codeEditor.SelectionColor = color;
            }
        }

        private void ColorizeStrings(string text, Color color)
        {
            int i = 0;
            while (i < text.Length)
            {
                if (text[i] == '"')
                {
                    int s = i++; while (i < text.Length && text[i] != '"') { if (text[i] == '\\') i++; i++; }
                    i++;
                    codeEditor.Select(s, i - s); codeEditor.SelectionColor = color;
                }
                else i++;
            }
        }

        private void ColorizeComments(string text, Color color)
        {
            int i = 0;
            while (i < text.Length)
            {
                if (i + 1 < text.Length && text[i] == '/' && text[i + 1] == '/')
                {
                    int s = i; while (i < text.Length && text[i] != '\n') i++;
                    codeEditor.Select(s, i - s); codeEditor.SelectionColor = color;
                }
                else i++;
            }
        }

        // ── Save / Copy ────────────────────────────────────────────────────────

        private void SaveCode()
        {
            if (string.IsNullOrWhiteSpace(codeEditor.Text)) return;
            string defaultName = _currentFile != null
                ? Path.GetFileNameWithoutExtension(_currentFile) + ".vizzy.cs"
                : "VizzyProgram.vizzy.cs";
            string initDir = Path.Combine(AppBaseDir(), "Scripts");
            if (!Directory.Exists(initDir)) initDir = AppBaseDir();

            using var dlg = new SaveFileDialog
            {
                Title = "Save Vizzy Code", Filter = "Vizzy Code (*.vizzy.cs)|*.vizzy.cs|C# Files (*.cs)|*.cs|All Files (*.*)|*.*",
                DefaultExt = "cs", FileName = defaultName, InitialDirectory = initDir
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;
            File.WriteAllText(dlg.FileName, codeEditor.Text, Encoding.UTF8);
            if (_currentSidecar != null)
                CodeCleanView.SaveSidecar(dlg.FileName, _currentSidecar);
            statusLabel.Text = $"Saved: {Path.GetFileName(dlg.FileName)}";
        }

        private void SaveToVizzyXml()
        {
            if (string.IsNullOrWhiteSpace(codeEditor.Text)) return;

            string programName = _currentFile != null
                ? Path.GetFileNameWithoutExtension(_currentFile)
                : "GeneratedProgram";

            try
            {
                var conv = new VizzyXmlConverter();
                string cleanCode = codeEditor.Text.Replace("\r\n", "\n");
                string exportCode = CodeCleanView.RestoreExactCode(cleanCode, _currentSidecar);
                var xmlDoc = conv.ConvertCodeToXml(exportCode, programName);
                var validationErrors = VizzyExportValidator.Validate(xmlDoc);
                if (validationErrors.Count > 0)
                {
                    string message =
                        "Export validation failed. VizzyCode detected XML patterns that are likely to break Juno loading." +
                        "\n\n" + VizzyExportValidator.Format(validationErrors);
                    MessageBox.Show(message, "Export Validation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusLabel.Text = "Export validation failed.";
                    return;
                }

                string defaultName = programName + ".xml";
                string initDir = Path.Combine(AppBaseDir(), "Programs");
                if (!Directory.Exists(initDir)) initDir = AppBaseDir();

                using var dlg = new SaveFileDialog
                {
                    Title = "Save as Vizzy XML", Filter = "Vizzy XML Files (*.xml)|*.xml|All Files (*.*)|*.*",
                    DefaultExt = "xml", FileName = defaultName, InitialDirectory = initDir
                };

                if (dlg.ShowDialog() != DialogResult.OK) return;

                xmlDoc.Save(dlg.FileName);
                statusLabel.Text = $"Saved Vizzy XML: {Path.GetFileName(dlg.FileName)}";
                MessageBox.Show(
                    $"Vizzy XML program saved successfully!\n\nFile: {dlg.FileName}\n\n" +
                    "You can now import this file into Juno: New Origins and run it with Vizzy.",
                    "Vizzy XML Saved",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error saving Vizzy XML:\n{ex.Message}",
                    "Save Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                statusLabel.Text = $"Error saving XML: {ex.Message}";
            }
        }

        // ── Menu / toolbar handlers ────────────────────────────────────────────

        private void menuOpenCraft_Click(object s, EventArgs e)
        {
            using var d = new OpenFileDialog { Title = "Open Craft XML",
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", InitialDirectory = GetInitDir() };
            if (d.ShowDialog() == DialogResult.OK) LoadFile(d.FileName);
        }
        private void menuOpenVizzy_Click(object s, EventArgs e)
        {
            using var d = new OpenFileDialog { Title = "Open Vizzy Program XML",
                Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", InitialDirectory = GetInitDir() };
            if (d.ShowDialog() == DialogResult.OK) LoadFile(d.FileName);
        }
        private void menuSaveCs_Click(object s, EventArgs e) => SaveCode();
        private void menuSaveXml_Click(object s, EventArgs e) => SaveToVizzyXml();
        private void menuCopy_Click(object s, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(codeEditor.Text))
            { Clipboard.SetText(codeEditor.Text); statusLabel.Text = "Copied to clipboard."; }
        }
        private void menuExit_Click(object s, EventArgs e) => Close();

        private void menuExampleCraft_Click(object s, EventArgs e) =>
            LoadExample("Example", "Craft", "USP-01 Universal Space Probe AI Improved.xml");
        private void menuExampleVizzy_Click(object s, EventArgs e) =>
            LoadExample("Example", "Vizzy", "Universal Vizzy Mission 2 - Enhanced.xml");

        private void LoadExample(params string[] parts)
        {
            string path = Path.Combine(new[] { AppBaseDir() }.Concat(parts).ToArray());
            if (File.Exists(path)) LoadFile(path);
            else MessageBox.Show($"Example not found:\n{path}", "Not Found",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void menuTheme_Click(object s, EventArgs e)
        {
            _isDark = !_isDark;
            ApplyTheme();
            menuTheme.Text = _isDark ? "Light Theme" : "Dark Theme";
        }
        private void menuAbout_Click(object s, EventArgs e) =>
            MessageBox.Show(
                "VizzyCode v1.0\n\n" +
                "Opens Vizzy XML programs (craft files or standalone) and converts\n" +
                "them to editable C# VizzyBuilder code.\n\n" +
                "VizzyBuilder by Rayan Ral  ·  github.com/kroryan/VizzyCode",
                "About VizzyCode", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Toolbar shortcuts
        private void btnOpenCraft_Click(object s, EventArgs e) => menuOpenCraft_Click(s, e);
        private void btnOpenVizzy_Click(object s, EventArgs e) => menuOpenVizzy_Click(s, e);
        private void btnSave_Click(object s, EventArgs e) => SaveToVizzyXml();
        private void btnCopy_Click(object s, EventArgs e) => menuCopy_Click(s, e);
        private void btnExCraft_Click(object s, EventArgs e) => menuExampleCraft_Click(s, e);
        private void btnExVizzy_Click(object s, EventArgs e) => menuExampleVizzy_Click(s, e);

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S)) { SaveToVizzyXml(); return true; }
            if (keyData == (Keys.Control | Keys.O)) { menuOpenCraft_Click(this, EventArgs.Empty); return true; }
            if (keyData == (Keys.F5)) { Highlight(); return true; }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ── Theme ──────────────────────────────────────────────────────────────

        // ── Juno menu handlers ─────────────────────────────────────────────────

        private async void menuJunoConnect_Click(object s, EventArgs e)
        {
            statusLabel.Text = "Connecting to Juno…";
            var info = await JunoClient.GetStatusAsync();
            if (info == null)
            {
                SetJunoStatus(connected: false, label: "Not running");
                MessageBox.Show(
                    $"Cannot reach the VizzyCode mod at {JunoClient.BaseUrl}\n\n" +
                    "Make sure Juno: New Origins is running and the VizzyCode mod is installed.",
                    "Juno: Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string scene = info.Scene ?? "unknown";
            string craft = info.CraftName != null ? $"  ·  Craft: {info.CraftName}" : "";
            SetJunoStatus(connected: true, label: $"Juno {scene}{craft}");
            statusLabel.Text = $"Connected — mod v{info.ModVersion}  ·  Scene: {scene}{craft}";
        }

        private async void menuJunoBrowse_Click(object s, EventArgs e)
        {
            var craft = await JunoClient.GetCraftAsync();
            if (craft == null)
            {
                MessageBox.Show("Could not reach the mod. Is Juno running?",
                    "Juno: Not Connected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Build a simple part picker dialog
            using var dlg = new Form
            {
                Text = $"Craft Parts — {craft.Name}",
                Width = 420, Height = 380,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false
            };
            var list = new ListBox { Dock = DockStyle.Fill, Font = new Font("Consolas", 10f) };
            var btnOk     = new Button { Text = "Import Vizzy", DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom, Height = 34 };
            var lblHint = new Label { Text = "Double-click or select and press Import.",
                Dock = DockStyle.Top, Height = 22, Font = new Font("Segoe UI", 8f),
                ForeColor = Color.Gray };

            var vizzyParts = (craft.Parts ?? Array.Empty<JunoClient.PartInfo>())
                             .Where(p => p.HasVizzy).ToArray();

            foreach (var p in vizzyParts)
                list.Items.Add($"[{p.Id,4}]  {p.Name}");

            if (list.Items.Count == 0)
            {
                MessageBox.Show("No parts with Vizzy programs found in the current craft.",
                    "No Vizzy Parts", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            list.SelectedIndex = 0;
            list.DoubleClick += (_, _) => dlg.DialogResult = DialogResult.OK;
            dlg.Controls.Add(list);
            dlg.Controls.Add(btnOk);
            dlg.Controls.Add(lblHint);

            if (dlg.ShowDialog(this) != DialogResult.OK) return;
            if (list.SelectedIndex < 0) return;

            var selected = vizzyParts[list.SelectedIndex];
            await ImportVizzyFromPart(selected.Id, selected.Name);
        }

        private async void menuJunoImport_Click(object s, EventArgs e)
        {
            // Import from whichever part is currently tracked, or ask user to browse
            if (_junoPartId < 0)
            {
                menuJunoBrowse_Click(s, e);
                return;
            }
            await ImportVizzyFromPart(_junoPartId, _junoPartName);
        }

        private async Task ImportVizzyFromPart(int partId, string? partName)
        {
            statusLabel.Text = $"Importing from part {partId}…";
            var info = await JunoClient.GetVizzyAsync(partId);
            if (info == null || !info.Ok || string.IsNullOrWhiteSpace(info.Xml))
            {
                string err = info?.Error ?? "Connection failed or empty Vizzy XML";
                MessageBox.Show($"Import failed: {err}", "Juno Import",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = $"Import failed: {err}";
                return;
            }

            // Convert XML to code and load into editor
            try
            {
                var doc  = XDocument.Parse(info.Xml);
                var conv = new VizzyXmlConverter();
                string exactCode = conv.ConvertProgramToCode(doc);
                var cleanView    = CodeCleanView.CreateCleanCode(exactCode);

                _currentFile    = null;
                _currentSidecar = cleanView.Sidecar;
                _junoPartId     = partId;
                _junoPartName   = partName ?? info.PartName;

                codeEditor.Text = cleanView.CleanCode.Replace("\r\n", "\n").Replace("\n", "\r\n");
                Highlight();
                UpdateTree(doc, conv);
                Text = $"VizzyCode  –  [Juno] {_junoPartName} (part {partId})";

                SetJunoStatus(connected: true,
                    label: $"Part {partId}: {_junoPartName}");
                statusLabel.Text = $"Imported from Juno — part {partId}: {_junoPartName}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing Vizzy XML:\n{ex.Message}", "Import Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void menuJunoExport_Click(object s, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(codeEditor.Text))
            {
                MessageBox.Show("Nothing to export.", "Juno Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_junoPartId < 0)
            {
                MessageBox.Show(
                    "No part selected. Use Juno > Browse Craft Parts to pick a part first.",
                    "Juno Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Convert code to Vizzy XML
            string programName = _junoPartName ?? "GeneratedProgram";
            string cleanCode   = codeEditor.Text.Replace("\r\n", "\n");
            string exportCode  = CodeCleanView.RestoreExactCode(cleanCode, _currentSidecar);

            string xml;
            try
            {
                var conv   = new VizzyXmlConverter();
                var xmlDoc = conv.ConvertCodeToXml(exportCode, programName);

                var errors = VizzyExportValidator.Validate(xmlDoc);
                if (errors.Count > 0)
                {
                    var res = MessageBox.Show(
                        "Export validation found issues:\n\n" +
                        VizzyExportValidator.Format(errors) +
                        "\n\nExport anyway?",
                        "Validation Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res != DialogResult.Yes) return;
                }

                xml = xmlDoc.Root!.ToString(System.Xml.Linq.SaveOptions.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Code conversion failed:\n{ex.Message}", "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            statusLabel.Text = $"Sending to Juno part {_junoPartId}…";
            var result = await JunoClient.PutVizzyAsync(_junoPartId, xml);

            if (result == null || !result.Ok)
            {
                string err = result?.Error ?? "Connection failed";
                MessageBox.Show($"Export to Juno failed: {err}", "Juno Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                statusLabel.Text = $"Export failed: {err}";
                return;
            }

            SetJunoStatus(connected: true, label: $"Exported → part {_junoPartId}");
            statusLabel.Text = $"Exported to Juno — part {_junoPartId}: {result.PartName}";
            MessageBox.Show(
                $"Vizzy program sent to Juno successfully!\n\nPart: {result.PartName} (id {result.PartId})\n\n" +
                "The game has updated the program. If you're in the designer, you'll see it immediately.",
                "Juno Export OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void menuJunoStages_Click(object s, EventArgs e)
        {
            var stages = await JunoClient.GetStagesAsync();
            if (stages == null || !stages.Ok)
            {
                MessageBox.Show(stages?.Error ?? "Could not reach the mod. Is Juno running?",
                    "Juno: Stages", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Current stage:  {stages.CurrentStage} / {stages.NumStages}");
            sb.AppendLine();
            sb.AppendLine("Activation groups:");
            var names  = stages.ActivationGroupNames  ?? Array.Empty<string>();
            var states = stages.ActivationGroupStates ?? Array.Empty<bool>();
            for (int i = 0; i < names.Length; i++)
            {
                string state = i < states.Length ? (states[i] ? "ON " : "OFF") : "???";
                sb.AppendLine($"  [{state}]  {names[i]}");
            }

            MessageBox.Show(sb.ToString(), "Juno: Stages", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void menuJunoActivateStage_Click(object s, EventArgs e)
        {
            var result = await JunoClient.ActivateStageAsync();
            if (result == null || !result.Ok)
            {
                MessageBox.Show(result?.Error ?? "Could not reach the mod.", "Juno: Stage",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            statusLabel.Text = $"Stage activated — now at stage {result.CurrentStage}/{result.NumStages}";
        }

        private void SetJunoStatus(bool connected, string label)
        {
            if (InvokeRequired) { Invoke(new Action(() => SetJunoStatus(connected, label))); return; }
            statusJuno.Text      = $"Juno: {label}";
            statusJuno.ForeColor = connected ? Color.LimeGreen : Color.Gray;
            menuJunoStatus.Text  = (connected ? "● Connected" : "○ Not connected") + $"  —  {label}";
        }

        private void ApplyTheme()
        {
            var bg  = _isDark ? Color.FromArgb(30, 30, 30)   : Color.White;
            var bg2 = _isDark ? Color.FromArgb(37, 37, 38)   : Color.FromArgb(250, 250, 250);
            var fg  = _isDark ? Color.FromArgb(220, 220, 220) : Color.Black;
            var sbBg = Color.FromArgb(0, 122, 204);

            BackColor = bg2;
            codeEditor.BackColor = bg; codeEditor.ForeColor = fg;
            treeView.BackColor   = bg2; treeView.ForeColor  = fg;
            warningsBox.BackColor = bg2; warningsBox.ForeColor = _isDark ? Color.FromArgb(220,120,120) : Color.DarkRed;
            statusStrip.BackColor = sbBg;
            statusLabel.ForeColor = Color.White;
            Highlight();
        }

        // ── Helpers ────────────────────────────────────────────────────────────

        private void SetTitle(string? path) =>
            Text = path != null ? $"VizzyCode  –  {Path.GetFileName(path)}" : "VizzyCode";

        private string GetInitDir()
        {
            string ex = Path.Combine(AppBaseDir(), "Example");
            return Directory.Exists(ex) ? ex : AppBaseDir();
        }

        private static string AppBaseDir()
        {
            string here = AppDomain.CurrentDomain.BaseDirectory;
            return Directory.GetParent(here)?.FullName ?? here;
        }
    }
}
