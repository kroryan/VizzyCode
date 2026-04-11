using System.Drawing;
using System.Windows.Forms;

namespace VizzyCode
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private MenuStrip    menuStrip;
        private ToolStrip    toolStrip;
        private StatusStrip  statusStrip;
        private ToolStripStatusLabel statusLabel;

        private SplitContainer splitMain;   // left | right
        private SplitContainer splitLeft;   // tree | warnings (vertical in left panel)

        private TreeView    treeView;
        private RichTextBox warningsBox;
        private RichTextBox codeEditor;

        // Menu items
        private ToolStripMenuItem menuFile, menuOpenCraft, menuOpenVizzy, menuSaveCs, menuSaveXml,
            menuCopyItem, menuExamples, menuExCraft, menuExVizzy, menuExit,
            menuView, menuTheme, menuHelp, menuAbout;

        // Toolbar
        private ToolStripButton btnOpenCraft, btnOpenVizzy, btnSave, btnCopy, btnExCraft, btnExVizzy;

        protected override void Dispose(bool d) { if (d && components != null) components.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ── Menu ──────────────────────────────────────────────────────────
            menuStrip = new MenuStrip();
            menuFile      = new ToolStripMenuItem("&File");
            menuOpenCraft = new ToolStripMenuItem("Open &Craft XML...") { ShortcutKeys = Keys.Control | Keys.O };
            menuOpenVizzy = new ToolStripMenuItem("Open &Vizzy XML...");
            menuSaveCs    = new ToolStripMenuItem("&Save as .cs...");
            menuSaveXml   = new ToolStripMenuItem("Save as Vizzy &XML...") { ShortcutKeys = Keys.Control | Keys.S };
            menuCopyItem  = new ToolStripMenuItem("&Copy Code");
            menuExamples  = new ToolStripMenuItem("&Examples");
            menuExCraft   = new ToolStripMenuItem("USP-01 Craft (example)");
            menuExVizzy   = new ToolStripMenuItem("Universal Vizzy Mission 2 (example)");
            menuExit      = new ToolStripMenuItem("E&xit");
            menuView      = new ToolStripMenuItem("&View");
            menuTheme     = new ToolStripMenuItem("Light Theme");
            menuHelp      = new ToolStripMenuItem("&Help");
            menuAbout     = new ToolStripMenuItem("&About...");

            menuFile.DropDownItems.AddRange(new ToolStripItem[]
            {
                menuOpenCraft, menuOpenVizzy, new ToolStripSeparator(),
                menuSaveCs, menuSaveXml, menuCopyItem, new ToolStripSeparator(),
                menuExamples, new ToolStripSeparator(), menuExit
            });
            menuExamples.DropDownItems.AddRange(new ToolStripItem[] { menuExCraft, menuExVizzy });
            menuView.DropDownItems.Add(menuTheme);
            menuHelp.DropDownItems.Add(menuAbout);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile, menuView, menuHelp });

            // ── Toolbar ───────────────────────────────────────────────────────
            toolStrip = new ToolStrip();
            btnOpenCraft = new ToolStripButton { Text = "Open Craft", ToolTipText = "Open Craft XML" };
            btnOpenVizzy = new ToolStripButton { Text = "Open Vizzy", ToolTipText = "Open Vizzy XML" };
            btnSave      = new ToolStripButton { Text = "Save XML",   ToolTipText = "Save Vizzy XML" };
            btnCopy      = new ToolStripButton { Text = "Copy",       ToolTipText = "Copy to clipboard" };
            btnExCraft   = new ToolStripButton { Text = "Ex: Craft",  ToolTipText = "Load example craft" };
            btnExVizzy   = new ToolStripButton { Text = "Ex: Vizzy",  ToolTipText = "Load example vizzy" };
            toolStrip.Items.AddRange(new ToolStripItem[]
            {
                btnOpenCraft, btnOpenVizzy, btnSave, btnCopy,
                new ToolStripSeparator(), btnExCraft, btnExVizzy
            });

            // ── Status bar ────────────────────────────────────────────────────
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Ready") { Spring = true, TextAlign = System.Drawing.ContentAlignment.MiddleLeft };
            statusStrip.Items.Add(statusLabel);

            // ── Tree view ─────────────────────────────────────────────────────
            treeView = new TreeView
            {
                Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f),
                FullRowSelect = true, HideSelection = false, ShowLines = true
            };

            // ── Warnings ──────────────────────────────────────────────────────
            warningsBox = new RichTextBox
            {
                Dock = DockStyle.Fill, ReadOnly = true, BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 8.5f), ScrollBars = RichTextBoxScrollBars.Vertical
            };

            // ── Code editor ───────────────────────────────────────────────────
            codeEditor = new RichTextBox
            {
                Dock = DockStyle.Fill, Font = new Font("Consolas", 10.5f), AcceptsTab = true,
                WordWrap = false, ScrollBars = RichTextBoxScrollBars.Both,
                BorderStyle = BorderStyle.None, DetectUrls = false
            };
            codeEditor.LostFocus += (_, _) => Highlight();

            // ── Split: left (tree+warnings) ───────────────────────────────────
            splitLeft = new SplitContainer
            {
                Dock = DockStyle.Fill, Orientation = Orientation.Horizontal,
                BorderStyle = BorderStyle.None
            };
            splitLeft.Panel1.Controls.Add(treeView);
            splitLeft.Panel2.Controls.Add(warningsBox);

            // ── Main split: left | code ───────────────────────────────────────
            splitMain = new SplitContainer
            {
                Dock = DockStyle.Fill, Orientation = Orientation.Vertical,
                BorderStyle = BorderStyle.None
            };
            splitMain.Panel1.Controls.Add(splitLeft);
            splitMain.Panel2.Controls.Add(codeEditor);

            // ── Form ──────────────────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1400, 800);
            MinimumSize = new Size(900, 550);
            Text = "VizzyCode";
            StartPosition = FormStartPosition.CenterScreen;

            Controls.Add(splitMain);
            Controls.Add(toolStrip);
            Controls.Add(menuStrip);
            Controls.Add(statusStrip);
            MainMenuStrip = menuStrip;

            // ── Wire events ───────────────────────────────────────────────────
            menuOpenCraft.Click += menuOpenCraft_Click;
            menuOpenVizzy.Click += menuOpenVizzy_Click;
            menuSaveCs.Click   += menuSaveCs_Click;
            menuSaveXml.Click  += menuSaveXml_Click;
            menuCopyItem.Click  += menuCopy_Click;
            menuExCraft.Click   += menuExampleCraft_Click;
            menuExVizzy.Click   += menuExampleVizzy_Click;
            menuExit.Click      += menuExit_Click;
            menuTheme.Click     += menuTheme_Click;
            menuAbout.Click     += menuAbout_Click;
            btnOpenCraft.Click  += btnOpenCraft_Click;
            btnOpenVizzy.Click  += btnOpenVizzy_Click;
            btnSave.Click       += btnSave_Click;
            btnCopy.Click       += btnCopy_Click;
            btnExCraft.Click    += btnExCraft_Click;
            btnExVizzy.Click    += btnExVizzy_Click;

            ApplyTheme();
        }
    }
}
