using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace VizzyCode
{
    public partial class MainForm : Form
    {
        private string? _currentFile;
        private CleanViewSidecar? _currentSidecar;
        private bool _isDark = true;

        public MainForm(string? fileToOpen = null)
        {
            InitializeComponent();

            Shown += (_, _) =>
            {
                try { splitMain.SplitterDistance  = 230; } catch { }
                try { splitRight.SplitterDistance = (int)(splitRight.Width * 0.60); } catch { }
                try { splitLeft.SplitterDistance  = (int)(splitLeft.Height * 0.70); } catch { }
            };

            chatPanel.GetCurrentCode  = () => codeEditor.Text;
            chatPanel.GetWorkingDirectory = GetAgentWorkingDirectory;
            chatPanel.GetDocumentKey = GetAgentDocumentKey;
            chatPanel.OnInsertCode    = InsertCodeFromChat;
            chatPanel.OnReplaceCode   = ReplaceCodeFromChat;

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
                "//  VizzyCode  –  Open · Convert · AI-edit Vizzy programs\r\n" +
                "// ═══════════════════════════════════════════════════════════\r\n" +
                "//\r\n" +
                "//  File > Open Craft XML   – open a craft .xml file\r\n" +
                "//  File > Open Vizzy XML   – open a standalone vizzy program\r\n" +
                "//  File > Save as .cs      – save for VizzyBuilder\r\n" +
                "//\r\n" +
                "//  Toolbar: Ex: Craft / Ex: Vizzy  – load built-in examples\r\n" +
                "//\r\n" +
                "//  Chat panel (right): ask AI to write or modify Vizzy code\r\n" +
                "//  Supports Claude, Gemini, OpenAI and Local LLMs.\r\n" +
                "//  Use ⚙ in the chat panel to configure API keys.\r\n" +
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

        // ── Chat integration ───────────────────────────────────────────────────

        private void InsertCodeFromChat(string code)
        {
            // If editor has selection, replace it. Otherwise append.
            if (codeEditor.SelectionLength > 0)
                codeEditor.SelectedText = code;
            else
            {
                codeEditor.SelectionStart = codeEditor.TextLength;
                codeEditor.AppendText("\r\n" + code);
            }
            Highlight();
            statusLabel.Text = "Code inserted from AI Assistant.";
        }

        private void ReplaceCodeFromChat(string code)
        {
            codeEditor.Text = (code ?? string.Empty).Replace("\r\n", "\n").Replace("\n", "\r\n");
            Highlight();
            statusLabel.Text = "Code updated from AI workspace.";
        }

        private string GetAgentWorkingDirectory()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_currentFile))
                {
                    string? dir = Path.GetDirectoryName(_currentFile);
                    if (!string.IsNullOrWhiteSpace(dir) && Directory.Exists(dir))
                        return dir;
                }
            }
            catch
            {
            }

            return AppBaseDir();
        }

        private string GetAgentDocumentKey()
        {
            if (!string.IsNullOrWhiteSpace(_currentFile))
                return _currentFile;

            return "unsaved-editor-buffer";
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
                    "You can now import this file into SimpleRockets 2 and run it with Vizzy.",
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
                "Includes AI chat powered by Claude (subscription or API key).\n\n" +
                "VizzyBuilder by Rayan Ral  ·  github.com/kroryan/VizzyCode",
                "About VizzyCode", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Toolbar shortcut
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
            chatPanel.ApplyTheme(_isDark);
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
