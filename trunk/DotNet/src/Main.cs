using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using mshtml;
using SHDocVw;
using TD.SandBar;
using TD.SandDock;
using ToolBar=TD.SandBar.ToolBar;

namespace WatirRecorder
{
	/// <summary>
	/// Code and concept by Scott Hanselman http://www.computerzen.com
	/// UI by Rutger Smit http://www.RutgerSmit.com+
    /// 
    /// Enhancements: Alex Elder http://www.alexelder.co.uk/blog
	/// </summary>
	public class frmMain : Form
	{
		private const string fileDialogFilter = "Ruby files (*.rb)|*.rb";

		private FontDialog fontDialog1;
		private ContextMenu contextMenu1;
		private MenuItem setFontMenuItem;
		private MenuItem saveMenuItem;
		private SaveFileDialog saveFileDialog1;
		private MenuItem openMenuItem;
		private OpenFileDialog openFileDialog1;
		private StatusBar statusBar1;
		private StatusBarPanel statusBarPanel1;
		private SandDockManager sandDockManager1;
		private DockContainer leftSandDock;
		private DockContainer rightSandDock;
		private DockContainer bottomSandDock;
		private DockContainer topSandDock;
		private TD.SandBar.ToolBar toolBar1;
		private ButtonItem buttonOpen;
		private ButtonItem saveButton;
		private ButtonItem aboutButton;
		private ButtonItem recordButton;
		private ButtonItem playbackButton;

		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private DocumentContainer documentContainer1;
		private DockControl dockControl1;
		public RichTextBox textScript;
		private Splitter splitter1;
		private GroupBox groupBox2;
		public ListBox lstEvents;
		private Panel panel1;
		private GroupBox groupBox1;


		public string starterProject = frmMain.configSetup();
 
		public string endProject = frmMain.configTearDown();

		private int m_dwCookie = 0;  
		private UCOMIConnectionPoint pConPt = null;  
		private IEHTMLDocumentEvents d;
		private DropDownMenuItem dropDownMenuItem1;
		private MenuButtonItem menuButtonItem1;
		private MenuButtonItem menuButtonItem3;
		private MenuButtonItem menuButtonItem4;
        private ButtonItem assertButton;
        private MenuButtonItem menuButtonItem2;
        private MenuButtonItem menuButtonItem5;
        private MenuButtonItem menuButtonItem6;
        private DockControl dockControlOutput;
        private RichTextBox rtbStdOutLog;
        private Button btnClear;
        private Button btnSaveLog;
        private GroupBox groupBox3;


		private enum IconType
		{
			About,
			App,
            Assert,
			Exit,
			New,
			Open,
			Playback,
			Save,
			Start,
			Stop
		}
		
		private Size iconSize = new	Size(32,32);
		public Size IconSize
		{
			get { return iconSize; }
			set { iconSize = value; }
		}



		public frmMain(string[] args)
		{
			
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			SetIcons();
			
			if(args.Length == 1)
			{
				ReadWatirFile(args[0]);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.setFontMenuItem = new System.Windows.Forms.MenuItem();
			this.openMenuItem = new System.Windows.Forms.MenuItem();
			this.saveMenuItem = new System.Windows.Forms.MenuItem();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
			this.sandDockManager1 = new TD.SandDock.SandDockManager();
			this.leftSandDock = new TD.SandDock.DockContainer();
			this.rightSandDock = new TD.SandDock.DockContainer();
			this.bottomSandDock = new TD.SandDock.DockContainer();
			this.topSandDock = new TD.SandDock.DockContainer();
			this.toolBar1 = new TD.SandBar.ToolBar();
			this.buttonOpen = new TD.SandBar.ButtonItem();
			this.saveButton = new TD.SandBar.ButtonItem();
			this.recordButton = new TD.SandBar.ButtonItem();
			this.assertButton = new TD.SandBar.ButtonItem();
			this.playbackButton = new TD.SandBar.ButtonItem();
			this.dropDownMenuItem1 = new TD.SandBar.DropDownMenuItem();
			this.menuButtonItem2 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem5 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem6 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem3 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem4 = new TD.SandBar.MenuButtonItem();
			this.menuButtonItem1 = new TD.SandBar.MenuButtonItem();
			this.aboutButton = new TD.SandBar.ButtonItem();
			this.documentContainer1 = new TD.SandDock.DocumentContainer();
			this.dockControl1 = new TD.SandDock.DockControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textScript = new System.Windows.Forms.RichTextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lstEvents = new System.Windows.Forms.ListBox();
			this.dockControlOutput = new TD.SandDock.DockControl();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rtbStdOutLog = new System.Windows.Forms.RichTextBox();
			this.btnSaveLog = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
			this.documentContainer1.SuspendLayout();
			this.dockControl1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.dockControlOutput.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									this.setFontMenuItem,
									this.openMenuItem,
									this.saveMenuItem});
			// 
			// setFontMenuItem
			// 
			this.setFontMenuItem.Index = 0;
			this.setFontMenuItem.Text = "Set Font...";
			this.setFontMenuItem.Click += new System.EventHandler(this.setFontMenuItem_Click);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Index = 1;
			this.openMenuItem.Text = "Open...";
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Index = 2;
			this.saveMenuItem.Text = "Save...";
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 583);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
									this.statusBarPanel1});
			this.statusBar1.ShowPanels = true;
			this.statusBar1.Size = new System.Drawing.Size(608, 22);
			this.statusBar1.TabIndex = 8;
			// 
			// statusBarPanel1
			// 
			this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statusBarPanel1.Name = "statusBarPanel1";
			this.statusBarPanel1.Width = 592;
			// 
			// sandDockManager1
			// 
			this.sandDockManager1.OwnerForm = this;
			// 
			// leftSandDock
			// 
			this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandDock.Guid = new System.Guid("25ec745e-de38-4c1a-a783-53b829ea6734");
			this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.leftSandDock.Location = new System.Drawing.Point(0, 0);
			this.leftSandDock.Manager = this.sandDockManager1;
			this.leftSandDock.Name = "leftSandDock";
			this.leftSandDock.Size = new System.Drawing.Size(0, 605);
			this.leftSandDock.TabIndex = 10;
			// 
			// rightSandDock
			// 
			this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandDock.Guid = new System.Guid("c4aec7ed-9055-44f4-af3f-2ea35df51099");
			this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.rightSandDock.Location = new System.Drawing.Point(608, 0);
			this.rightSandDock.Manager = this.sandDockManager1;
			this.rightSandDock.Name = "rightSandDock";
			this.rightSandDock.Size = new System.Drawing.Size(0, 605);
			this.rightSandDock.TabIndex = 11;
			// 
			// bottomSandDock
			// 
			this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandDock.Guid = new System.Guid("5196e983-c717-40e9-b223-368ca5f449f3");
			this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.bottomSandDock.Location = new System.Drawing.Point(0, 605);
			this.bottomSandDock.Manager = this.sandDockManager1;
			this.bottomSandDock.Name = "bottomSandDock";
			this.bottomSandDock.Size = new System.Drawing.Size(608, 0);
			this.bottomSandDock.TabIndex = 12;
			// 
			// topSandDock
			// 
			this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandDock.Guid = new System.Guid("c11b7c3d-b652-47b2-bf7d-0a72097ec98e");
			this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400);
			this.topSandDock.Location = new System.Drawing.Point(0, 0);
			this.topSandDock.Manager = this.sandDockManager1;
			this.topSandDock.Name = "topSandDock";
			this.topSandDock.Size = new System.Drawing.Size(608, 0);
			this.topSandDock.TabIndex = 13;
			// 
			// toolBar1
			// 
			this.toolBar1.ContextMenu = this.contextMenu1;
			this.toolBar1.FlipLastItem = true;
			this.toolBar1.Guid = new System.Guid("a57ee27e-f5fa-4a3f-986a-cbe4264539d5");
			this.toolBar1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
									this.buttonOpen,
									this.saveButton,
									this.recordButton,
									this.assertButton,
									this.playbackButton,
									this.dropDownMenuItem1,
									this.aboutButton});
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowShortcutsInToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(608, 22);
			this.toolBar1.TabIndex = 14;
			this.toolBar1.Text = "";
			this.toolBar1.TextAlign = TD.SandBar.ToolBarTextAlign.Underneath;
			// 
			// buttonOpen
			// 
			this.buttonOpen.IconSize = new System.Drawing.Size(32, 32);
			this.buttonOpen.Text = "Open";
			this.buttonOpen.ToolTipText = "Open";
			this.buttonOpen.Activate += new System.EventHandler(this.buttonOpen_Activate);
			// 
			// saveButton
			// 
			this.saveButton.IconSize = new System.Drawing.Size(32, 32);
			this.saveButton.Text = "Save";
			this.saveButton.ToolTipText = "Save";
			this.saveButton.Activate += new System.EventHandler(this.saveButton_Activate);
			// 
			// recordButton
			// 
			this.recordButton.BeginGroup = true;
			this.recordButton.Text = "Start";
			this.recordButton.ToolTipText = "Start";
			this.recordButton.Activate += new System.EventHandler(this.recordButton_Activate);
			// 
			// assertButton
			// 
			this.assertButton.IconSize = new System.Drawing.Size(128, 128);
			this.assertButton.Text = "Assert";
			this.assertButton.ToolTipText = "Add assertion";
			this.assertButton.Activate += new System.EventHandler(this.assertButton_Activate);
			// 
			// playbackButton
			// 
			this.playbackButton.Text = "Playback";
			this.playbackButton.ToolTipText = "Playback";
			this.playbackButton.Activate += new System.EventHandler(this.playbackButton_Activate);
			// 
			// dropDownMenuItem1
			// 
			this.dropDownMenuItem1.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
									this.menuButtonItem2,
									this.menuButtonItem3,
									this.menuButtonItem4,
									this.menuButtonItem1});
			this.dropDownMenuItem1.Text = "Options";
			this.dropDownMenuItem1.Visible = false;
			// 
			// menuButtonItem2
			// 
			this.menuButtonItem2.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
									this.menuButtonItem5,
									this.menuButtonItem6});
			this.menuButtonItem2.Text = "Browser Selection";
			// 
			// menuButtonItem5
			// 
			this.menuButtonItem5.Text = "Firewatir: Mozilla Firefox";
			// 
			// menuButtonItem6
			// 
			this.menuButtonItem6.Text = "Watir: Internet Explorer";
			// 
			// menuButtonItem3
			// 
			this.menuButtonItem3.Text = "Watir Project Page";
			// 
			// menuButtonItem4
			// 
			this.menuButtonItem4.Text = "Ruby Project Page";
			// 
			// menuButtonItem1
			// 
			this.menuButtonItem1.BeginGroup = true;
			this.menuButtonItem1.Text = "About...";
			this.menuButtonItem1.Activate += new System.EventHandler(this.menuButtonItem1_Activate);
			// 
			// aboutButton
			// 
			this.aboutButton.BuddyMenu = this.menuButtonItem1;
			this.aboutButton.IconSize = new System.Drawing.Size(32, 32);
			this.aboutButton.Text = "About";
			this.aboutButton.ToolTipText = "About";
			this.aboutButton.Activate += new System.EventHandler(this.aboutButton_Activate);
			// 
			// documentContainer1
			// 
			this.documentContainer1.Controls.Add(this.dockControl1);
			this.documentContainer1.Controls.Add(this.dockControlOutput);
			this.documentContainer1.Guid = new System.Guid("a1f0db60-29b8-4f8c-b3ca-0613232ae52d");
			this.documentContainer1.LayoutSystem = new TD.SandDock.SplitLayoutSystem(250, 400, System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
									((TD.SandDock.LayoutSystemBase)(new TD.SandDock.DocumentLayoutSystem(606, 559, new TD.SandDock.DockControl[] {
																		this.dockControl1,
																		this.dockControlOutput}, this.dockControl1)))});
			this.documentContainer1.Location = new System.Drawing.Point(0, 22);
			this.documentContainer1.Manager = null;
			this.documentContainer1.Name = "documentContainer1";
			this.documentContainer1.Renderer = new TD.SandDock.Rendering.Office2003Renderer();
			this.documentContainer1.Size = new System.Drawing.Size(608, 561);
			this.documentContainer1.TabIndex = 15;
			this.documentContainer1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textScript_DragDrop);
			this.documentContainer1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textScript_DragEnter);
			// 
			// dockControl1
			// 
			this.dockControl1.Controls.Add(this.panel1);
			this.dockControl1.Guid = new System.Guid("804e5184-274f-4191-a3ab-346cf996384a");
			this.dockControl1.Location = new System.Drawing.Point(5, 33);
			this.dockControl1.Name = "dockControl1";
			this.dockControl1.Size = new System.Drawing.Size(598, 523);
			this.dockControl1.TabIndex = 0;
			this.dockControl1.Text = "Recording";
			this.dockControl1.Closing += new System.ComponentModel.CancelEventHandler(this.dockControl1_Closing);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.splitter1);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(598, 523);
			this.panel1.TabIndex = 8;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textScript);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(598, 384);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Watir Test Code";
			this.groupBox1.DragOver += new System.Windows.Forms.DragEventHandler(this.textScript_DragEnter);
			this.groupBox1.DragDrop += new System.Windows.Forms.DragEventHandler(this.textScript_DragDrop);
			this.groupBox1.DragEnter += new System.Windows.Forms.DragEventHandler(this.textScript_DragEnter);
			// 
			// textScript
			// 
			this.textScript.AllowDrop = true;
			this.textScript.ContextMenu = this.contextMenu1;
			this.textScript.DetectUrls = false;
			this.textScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textScript.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textScript.Location = new System.Drawing.Point(3, 16);
			this.textScript.Name = "textScript";
			this.textScript.Size = new System.Drawing.Size(592, 365);
			this.textScript.TabIndex = 1;
			this.textScript.Text = "";
			this.textScript.DragDrop += new System.Windows.Forms.DragEventHandler(this.textScript_DragDrop);
			this.textScript.DragEnter += new System.Windows.Forms.DragEventHandler(this.textScript_DragEnter);
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.splitter1.Location = new System.Drawing.Point(0, 384);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(598, 3);
			this.splitter1.TabIndex = 6;
			this.splitter1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lstEvents);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox2.Location = new System.Drawing.Point(0, 387);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(598, 136);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Events";
			// 
			// lstEvents
			// 
			this.lstEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstEvents.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lstEvents.IntegralHeight = false;
			this.lstEvents.ItemHeight = 16;
			this.lstEvents.Location = new System.Drawing.Point(3, 16);
			this.lstEvents.Name = "lstEvents";
			this.lstEvents.Size = new System.Drawing.Size(592, 117);
			this.lstEvents.TabIndex = 2;
			// 
			// dockControlOutput
			// 
			this.dockControlOutput.Controls.Add(this.groupBox3);
			this.dockControlOutput.Controls.Add(this.btnSaveLog);
			this.dockControlOutput.Controls.Add(this.btnClear);
			this.dockControlOutput.Guid = new System.Guid("e4b3c490-2cd9-4436-a969-ac697467fb4a");
			this.dockControlOutput.Location = new System.Drawing.Point(5, 33);
			this.dockControlOutput.Name = "dockControlOutput";
			this.dockControlOutput.Size = new System.Drawing.Size(598, 523);
			this.dockControlOutput.TabIndex = 1;
			this.dockControlOutput.Text = "Standard Output Log";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.rtbStdOutLog);
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(603, 460);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Standard Output";
			// 
			// rtbStdOutLog
			// 
			this.rtbStdOutLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbStdOutLog.Enabled = false;
			this.rtbStdOutLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.rtbStdOutLog.ForeColor = System.Drawing.SystemColors.Desktop;
			this.rtbStdOutLog.Location = new System.Drawing.Point(3, 16);
			this.rtbStdOutLog.Name = "rtbStdOutLog";
			this.rtbStdOutLog.ReadOnly = true;
			this.rtbStdOutLog.Size = new System.Drawing.Size(597, 441);
			this.rtbStdOutLog.TabIndex = 0;
			this.rtbStdOutLog.Text = "";
			// 
			// btnSaveLog
			// 
			this.btnSaveLog.Location = new System.Drawing.Point(435, 466);
			this.btnSaveLog.Name = "btnSaveLog";
			this.btnSaveLog.Size = new System.Drawing.Size(75, 23);
			this.btnSaveLog.TabIndex = 2;
			this.btnSaveLog.Text = "Save Log";
			this.btnSaveLog.UseVisualStyleBackColor = true;
			this.btnSaveLog.Click += new System.EventHandler(this.btnSaveLog_Click);
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(516, 466);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// frmMain
			// 
			this.AccessibleName = "WatirRecorder#";
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 605);
			this.Controls.Add(this.documentContainer1);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.leftSandDock);
			this.Controls.Add(this.rightSandDock);
			this.Controls.Add(this.bottomSandDock);
			this.Controls.Add(this.topSandDock);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMain";
			this.Text = "WatirRecorder#";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
			this.documentContainer1.ResumeLayout(false);
			this.dockControl1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.dockControlOutput.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			Application.Run(new frmMain(args));
		}

		InternetExplorerClass ie = null;
		HTMLDocument doc = null;

        string[] keywords = {
                "link", 
				"require", 
				"class", 
				"def",
				"radio",
				"include", 
				"text_field", 
				"goto", 
				"button", 
				"contains_text", 
				"new", 
				"ie",
				"end"};

		void ParseLine(string line, bool indent) 
		{

			Regex r = new Regex("([ \\t{}():;.])");
			String [] tokens = r.Split(line); 
			if (indent) textScript.SelectedText = "\t";
			foreach (string token in tokens) 
			{ 
				// Set the tokens default color and font.
				textScript.SelectionColor = Color.Black;

				// Check whether the token is a keyword. 

				for (int i = 0; i < keywords.Length; i++) 
				{
					if (keywords[i] == token) 
					{
						// Apply alternative color and font to highlight keyword.
						textScript.SelectionColor = Color.Blue;
						break;
					}
				}
				textScript.SelectedText = token;
				
			} 
			textScript.SelectedText = "\r\n";
			Application.DoEvents();
		} 

		public void AppendText(string text)
		{
			this.Focus();
			AppendText("\t" + text, true);
		}

		public void AppendText(string text, bool indent)
		{
			this.textScript.ReadOnly = true;
			Regex r = new Regex("\\r\\n");
			String [] lines = r.Split(text);
			foreach (string l in lines) 
			{
				ParseLine(l, indent);
			} 
			this.textScript.ReadOnly = false;
			this.textScript.ScrollToCaret();
		}

		public void LogEvent(string msg)
		{
			int i = lstEvents.Items.Add(msg);
			lstEvents.SelectedIndex = i;
		}

        public void LogOutput(string stdout)
        {
            rtbStdOutLog.AppendText(stdout);
            rtbStdOutLog.AppendText("----------------------------------------\n");
        }

        void textScript_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        void textScript_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            this.ReadWatirFile(files[0]);
        }

		/// <summary>
		/// Ie_s the document complete.
		/// </summary>
		/// <param name="pDisp">P disp.</param>
		/// <param name="URL">URL.</param>
		private void ie_DocumentComplete(object pDisp, ref object URL)
		{
			if(m_dwCookie!=0)  
			{  
				pConPt.Unadvise(m_dwCookie);  
			}  

			//Funky, yes, with a reason: http://support.microsoft.com/?id=811645
			// Help from http://www.eggheadcafe.com/ng/microsoft.public.dotnet.framework.sdk/post21853543.asp
			// We are going to sink the event here by using COM connection point.  
			doc = (HTMLDocument)ie.Document;  
			// I am going to QueryInterface UCOMConnectionPointContainer of the WebBrowser Control  
			UCOMIConnectionPointContainer pConPtCon = (UCOMIConnectionPointContainer)doc;  

			// Get access to the IConnectionPoint pointer.  
			// UCOMIConnectionPoint pConPt;  
			// Get the GUID of the HTMLDocumentEvents2  
			Guid guid = typeof(HTMLDocumentEvents2).GUID;  
			pConPtCon.FindConnectionPoint(ref guid, out pConPt);  
			// define your event handler class IEHTMLDocumentEvents  
			d = new IEHTMLDocumentEvents(this);    
			pConPt.Advise(d,out m_dwCookie);
		}

		private void ie_NavigateComplete2(object pDisp, ref object URL)
		{
			LogEvent("Navigate Complete");
		}

		private bool suppress = false;
		private bool firstTime = true;
		public void SuppressOneGoto()
		{
			suppress = true;
		}
		// TODO: Pull to Mono
		private void ie_BeforeNavigate2(object pDisp, ref object URL, ref object Flags, ref object TargetFrameName, ref object PostData, ref object Headers, ref bool Cancel)
		{
			LogEvent("Before Navigate");
			if (suppress == true)
			{
				suppress = false;
				return;
			}
			//Ignore iframe navigations...
			if(URL.ToString().StartsWith("about:") == false && TargetFrameName == null && firstTime == true)
			{
				AppendText(string.Format("@browser.goto('{0}')",URL.ToString().Replace("'",@"\'")));
				firstTime = false;
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
		}
		

		#region Context menu Events
		private void setFontMenuItem_Click(object sender, EventArgs e)
		{
			fontDialog1.Font = textScript.Font;

			if(fontDialog1.ShowDialog() != DialogResult.Cancel )
			{
				string tempText = textScript.Text;
				textScript.Font = fontDialog1.Font;
				textScript.Clear();
				foreach(string s in tempText.Split('\r'))
				{
					AppendText(s,false);
				}

			}
		}

		private void saveMenuItem_Click(object sender, EventArgs e)
		{
			SaveWatirFile();

		}

		private void openMenuItem_Click(object sender, EventArgs e)
		{
			OpenWatirFile();
		}
		#endregion

		#region Main menu Events
		private void aboutMenuItem_Click(object sender, EventArgs e)
		{
			string aboutText = @"
Watir Recorder
--------------
This Watir Recorder is based on Scott Hanselman's Watir Recorder.
For more information visit his blog at http://www.ComputerZen.com
";
			MessageBox.Show(this, aboutText, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void openMenuItem2_Click(object sender, EventArgs e)
		{
			OpenWatirFile();
		}

		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			this.Dispose();
		}

		private void saveMenuItem2_Click(object sender, EventArgs e)
		{
			SaveWatirFile();
		}

		#endregion


		#region Shared Methods

		/// <summary>
		/// Opens the watir file.
		/// </summary>
		private void OpenWatirFile()
		{
			openFileDialog1.Filter = fileDialogFilter;
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				ReadWatirFile(openFileDialog1.FileName);
			}
		}

		/// <summary>
		/// Reads the watir file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		private void ReadWatirFile(string filename)
		{
			StreamReader re = File.OpenText( filename );
			string input = null;
			textScript.Clear();
			while ((input = re.ReadLine()) != null)
			{
				AppendText(input,false);
			}
			re.Close();
		}

		/// <summary>
		/// Saves the watir file.
		/// </summary>
		private void SaveWatirFile()
		{
			Stream myStream ;
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
   
			saveFileDialog1.Filter = fileDialogFilter;
   
			saveFileDialog1.RestoreDirectory = true ;
   
			if(saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				if((myStream = saveFileDialog1.OpenFile()) != null)
				{
					StreamWriter writer =new StreamWriter(myStream);
					writer.Write(textScript.Text);
					writer.Close();
					myStream.Close();
				}
			}
		}
		#endregion 

        private void SaveStandardOutput()
        {
            Stream outputStream;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text Documents (*.txt)|*.txt|All files (*.*)|*.*";
            saveDialog.RestoreDirectory = true;
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if ((outputStream = saveDialog.OpenFile()) != null)
                {
                    StreamWriter writer = new StreamWriter(outputStream);
                    writer.Write(rtbStdOutLog.Text);
                    writer.Close();
                    outputStream.Close();
                }
            }
        }


		/// <summary>
		/// Gets the icon.
		/// </summary>
		/// <param name="iconType">Type of the icon.</param>
		/// <returns></returns>
		private Icon GetIcon(IconType iconType)
		{
			return new Icon(GetType(), "icons."+iconType.ToString().ToLower()+".ico");
		}


		private void SetIcons()
		{
			this.Icon = GetIcon(IconType.App);
			buttonOpen.Icon = GetIcon(IconType.Open);
			saveButton.Icon = GetIcon(IconType.Save);
			aboutButton.Icon = GetIcon(IconType.About);
			recordButton.Icon = GetIcon(IconType.Start);
			playbackButton.Icon = GetIcon(IconType.Playback);
            /* 
             * The assert icon was created by Bruno Maia.
             * IconTexto - http://www.icontexto.com
             */
            assertButton.Icon = GetIcon(IconType.Assert);
			
			for(int i=0; i<toolBar1.Items.Count; i++)
			{
				object item = toolBar1.Items[i];
				if(item.GetType().Equals( new ButtonItem().GetType() ))
				{
					((ButtonItem)item).IconSize = IconSize;
				}
			}
		}

		private void saveButton_Activate(object sender, EventArgs e)
		{
			SaveWatirFile();
		}

		private void buttonOpen_Activate(object sender, EventArgs e)
		{
			OpenWatirFile();
		}

		private void aboutButton_Activate(object sender, EventArgs e)
		{
			string aboutText = @"
Watir Recorder
--------------
This Watir Recorder is based on Scott Hanselman's Watir Recorder.
For more information visit his blog at http://www.ComputerZen.com
";
			MessageBox.Show(this, aboutText, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void recordButton_Activate(object sender, EventArgs e)
		{
			StartNewRecording();
		}
		// TODO: Pull this for Mono
		private void StartNewRecording()
		{
			if (this.recordButton.Text == "Start")
			{
				recordButton.Icon = GetIcon(IconType.Stop);
				this.recordButton.Text = "Stop";

				if(documentContainer1.Documents.Length == 0)
				{
					documentContainer1.AddDocument(dockControl1);
					Application.DoEvents();
				}

				this.textScript.Clear();
				AppendText(starterProject, false);
				if (ie != null)
				{
					ie.Quit();
					ie = null;
				}
				ie = new InternetExplorerClass();
				ie.Visible = true;
				object url = "about:blank";
				object nullObj = String.Empty;
				ie.Navigate2(ref url,ref nullObj,ref nullObj,ref nullObj,ref nullObj);
				ie.DocumentComplete += new DWebBrowserEvents2_DocumentCompleteEventHandler(ie_DocumentComplete);
				ie.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(ie_NavigateComplete2);
				ie.BeforeNavigate2 += new DWebBrowserEvents2_BeforeNavigate2EventHandler(ie_BeforeNavigate2);
			}
			else
			{
				recordButton.Icon = GetIcon(IconType.Start);
				this.recordButton.Text = "Start";
				AppendText(endProject, false);
				if (ie != null)
				{
					try
					{
						ie.Quit();
					}
					catch(COMException)
					{

					}
					finally
					{
						ie = null;
					}
				}
			}
		}

		private void PlaybackRecording()
		{
			this.textScript.SaveFile(AppDomain.CurrentDomain.BaseDirectory+"temp.rb",RichTextBoxStreamType.PlainText);
			using(Process p = new Process())
			{
				p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = "ruby.exe";
                p.StartInfo.Arguments = "temp.rb";
				p.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
				p.Start();
                //string output = ;
                LogOutput(p.StandardOutput.ReadToEnd());
                p.WaitForExit();

			}
		}

		private void playbackButton_Activate(object sender, EventArgs e)
		{
			if(this.textScript.TextLength>0)
				PlaybackRecording();
			else
				MessageBox.Show(this, "Nothing to playback", "Oops...", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}

		private void menuButtonItem1_Activate(object sender, EventArgs e)
		{
			string aboutText = @"
Watir Recorder#
----------------
This Watir Recorder is based on Scott Hanselman's Watir Recorder.
For more information visit his great blog at http://www.ComputerZen.com
";
			MessageBox.Show(this, aboutText, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

        private void assertButton_Activate(object sender, EventArgs e)
        {
            AddAssertion();
        }

        private void AddAssertion()
        {
            if (this.d != null)
            {
                if (d.GetAssertion().Length > 0)
                {
                    this.AppendText(d.GetAssertion());
                    d.ResetAssertion();
                }
            }
        }

        public void enableAssertionButton()
        {
            this.assertButton.Enabled = true;
        }

        private void dockControl1_Closing(object sender, CancelEventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.rtbStdOutLog.Clear();
        }

        private void btnSaveLog_Click(object sender, EventArgs e)
        {
            this.SaveStandardOutput();
        }
        public static string configSetup()
        {
        	string fileName = "config.xml";
			XmlDocument doc = new XmlDocument();
			XmlCDataSection cdataSection = null;
			doc.Load(fileName);
			XmlElement root = doc.DocumentElement;
			XmlNode node = doc.DocumentElement.SelectSingleNode(
			@"/options/start");
			XmlNode childNode = node.ChildNodes[0];
			if (childNode is XmlCDataSection)
			{
				cdataSection = childNode as XmlCDataSection;
			}
			string tmp = cdataSection.Value;
			return cdataSection.Value;
        }
        public static string configTearDown()
        {
        	string fileName = "config.xml";
			XmlDocument doc = new XmlDocument();
			XmlCDataSection cdataSection = null;
			doc.Load(fileName);
			XmlElement root = doc.DocumentElement;
			XmlNode node = doc.DocumentElement.SelectSingleNode(
			@"/options/end");
			XmlNode childNode = node.ChildNodes[0];
			if (childNode is XmlCDataSection)
			{
				cdataSection = childNode as XmlCDataSection;
			}
			string tmp = cdataSection.Value;
			return cdataSection.Value;
        }
	}
}
