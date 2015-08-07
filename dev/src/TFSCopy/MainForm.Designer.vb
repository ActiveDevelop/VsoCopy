<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.SourceTFSTextBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ChoosePathButton = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.MaintenanceFolderTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.ServerPathLabel = New System.Windows.Forms.Label()
        Me.ServerPathTreeView = New System.Windows.Forms.TreeView()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LocalPathLinkLabel = New System.Windows.Forms.LinkLabel()
        Me.ServerItemContentListBox = New System.Windows.Forms.ListBox()
        Me.WorkspacesComboBox = New System.Windows.Forms.ComboBox()
        Me.QueryWorkspacesButton = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.ListDestinationTFSProjectsButton = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SourceTFSTreeView = New System.Windows.Forms.TreeView()
        Me.SourceTFSLabel = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DestinationTFSTextBox = New System.Windows.Forms.TextBox()
        Me.QueryCollectionsButton = New System.Windows.Forms.Button()
        Me.SourceTfsCollectionNodesComboBox = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitTFSManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VersionSourceControlToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackupSelectedTreeNodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TeamCollectionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WorkItemsAbgleichenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.CurrentTaskItemInfoToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TestabgleichToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Get4CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'SourceTFSTextBox
        '
        Me.SourceTFSTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SourceTFSTextBox.Location = New System.Drawing.Point(9, 58)
        Me.SourceTFSTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.SourceTFSTextBox.Name = "SourceTFSTextBox"
        Me.SourceTFSTextBox.Size = New System.Drawing.Size(1237, 20)
        Me.SourceTFSTextBox.TabIndex = 0
        Me.SourceTFSTextBox.Text = "https://activedevelop.visualstudio.com/"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 42)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Source TFS:"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(9, 145)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1361, 462)
        Me.TabControl1.TabIndex = 7
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ChoosePathButton)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.MaintenanceFolderTextBox)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.SplitContainer1)
        Me.TabPage1.Controls.Add(Me.WorkspacesComboBox)
        Me.TabPage1.Controls.Add(Me.QueryWorkspacesButton)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1353, 436)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Source Control Server"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ChoosePathButton
        '
        Me.ChoosePathButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChoosePathButton.Location = New System.Drawing.Point(1797, 65)
        Me.ChoosePathButton.Margin = New System.Windows.Forms.Padding(2)
        Me.ChoosePathButton.Name = "ChoosePathButton"
        Me.ChoosePathButton.Size = New System.Drawing.Size(145, 26)
        Me.ChoosePathButton.TabIndex = 14
        Me.ChoosePathButton.Text = "Choose Path"
        Me.ChoosePathButton.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(14, 54)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(318, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Path for maintenance purposes (different than local mapped path!)"
        '
        'MaintenanceFolderTextBox
        '
        Me.MaintenanceFolderTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaintenanceFolderTextBox.Location = New System.Drawing.Point(16, 69)
        Me.MaintenanceFolderTextBox.Name = "MaintenanceFolderTextBox"
        Me.MaintenanceFolderTextBox.Size = New System.Drawing.Size(444, 20)
        Me.MaintenanceFolderTextBox.TabIndex = 12
        Me.MaintenanceFolderTextBox.Text = "C:\Temp\TempSrcDownload"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Workspaces:"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(15, 118)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.ServerPathLabel)
        Me.SplitContainer1.Panel1.Controls.Add(Me.ServerPathTreeView)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.Label4)
        Me.SplitContainer1.Panel2.Controls.Add(Me.LocalPathLinkLabel)
        Me.SplitContainer1.Panel2.Controls.Add(Me.ServerItemContentListBox)
        Me.SplitContainer1.Size = New System.Drawing.Size(1927, 299)
        Me.SplitContainer1.SplitterDistance = 640
        Me.SplitContainer1.TabIndex = 10
        '
        'ServerPathLabel
        '
        Me.ServerPathLabel.AutoSize = True
        Me.ServerPathLabel.Location = New System.Drawing.Point(7, 10)
        Me.ServerPathLabel.Name = "ServerPathLabel"
        Me.ServerPathLabel.Size = New System.Drawing.Size(66, 13)
        Me.ServerPathLabel.TabIndex = 13
        Me.ServerPathLabel.Text = "Server Path:"
        '
        'ServerPathTreeView
        '
        Me.ServerPathTreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ServerPathTreeView.Location = New System.Drawing.Point(10, 34)
        Me.ServerPathTreeView.Name = "ServerPathTreeView"
        Me.ServerPathTreeView.Size = New System.Drawing.Size(615, 247)
        Me.ServerPathTreeView.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(9, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Mapped Path:"
        '
        'LocalPathLinkLabel
        '
        Me.LocalPathLinkLabel.AutoSize = True
        Me.LocalPathLinkLabel.Location = New System.Drawing.Point(83, 10)
        Me.LocalPathLinkLabel.Name = "LocalPathLinkLabel"
        Me.LocalPathLinkLabel.Size = New System.Drawing.Size(45, 13)
        Me.LocalPathLinkLabel.TabIndex = 10
        Me.LocalPathLinkLabel.TabStop = True
        Me.LocalPathLinkLabel.Text = "x:/yyyyy"
        '
        'ServerItemContentListBox
        '
        Me.ServerItemContentListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ServerItemContentListBox.FormattingEnabled = True
        Me.ServerItemContentListBox.Location = New System.Drawing.Point(12, 34)
        Me.ServerItemContentListBox.Name = "ServerItemContentListBox"
        Me.ServerItemContentListBox.Size = New System.Drawing.Size(1256, 238)
        Me.ServerItemContentListBox.TabIndex = 9
        '
        'WorkspacesComboBox
        '
        Me.WorkspacesComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WorkspacesComboBox.FormattingEnabled = True
        Me.WorkspacesComboBox.Location = New System.Drawing.Point(15, 23)
        Me.WorkspacesComboBox.Name = "WorkspacesComboBox"
        Me.WorkspacesComboBox.Size = New System.Drawing.Size(853, 21)
        Me.WorkspacesComboBox.TabIndex = 9
        '
        'QueryWorkspacesButton
        '
        Me.QueryWorkspacesButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.QueryWorkspacesButton.Location = New System.Drawing.Point(873, 19)
        Me.QueryWorkspacesButton.Margin = New System.Windows.Forms.Padding(2)
        Me.QueryWorkspacesButton.Name = "QueryWorkspacesButton"
        Me.QueryWorkspacesButton.Size = New System.Drawing.Size(145, 26)
        Me.QueryWorkspacesButton.TabIndex = 7
        Me.QueryWorkspacesButton.Text = "Query Workspaces:"
        Me.QueryWorkspacesButton.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.ListDestinationTFSProjectsButton)
        Me.TabPage2.Controls.Add(Me.TableLayoutPanel1)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.DestinationTFSTextBox)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1353, 436)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Collection Manager"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'ListDestinationTFSProjectsButton
        '
        Me.ListDestinationTFSProjectsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListDestinationTFSProjectsButton.Location = New System.Drawing.Point(594, 26)
        Me.ListDestinationTFSProjectsButton.Margin = New System.Windows.Forms.Padding(2)
        Me.ListDestinationTFSProjectsButton.Name = "ListDestinationTFSProjectsButton"
        Me.ListDestinationTFSProjectsButton.Size = New System.Drawing.Size(125, 26)
        Me.ListDestinationTFSProjectsButton.TabIndex = 13
        Me.ListDestinationTFSProjectsButton.Text = "Query Team Projects"
        Me.ListDestinationTFSProjectsButton.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel2, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(41, 65)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(680, 378)
        Me.TableLayoutPanel1.TabIndex = 12
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.SourceTFSTreeView)
        Me.Panel1.Controls.Add(Me.SourceTFSLabel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(2, 2)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(336, 374)
        Me.Panel1.TabIndex = 0
        '
        'SourceTFSTreeView
        '
        Me.SourceTFSTreeView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SourceTFSTreeView.Location = New System.Drawing.Point(4, 24)
        Me.SourceTFSTreeView.Margin = New System.Windows.Forms.Padding(2)
        Me.SourceTFSTreeView.Name = "SourceTFSTreeView"
        Me.SourceTFSTreeView.Size = New System.Drawing.Size(330, 351)
        Me.SourceTFSTreeView.TabIndex = 1
        '
        'SourceTFSLabel
        '
        Me.SourceTFSLabel.AutoSize = True
        Me.SourceTFSLabel.Location = New System.Drawing.Point(2, 7)
        Me.SourceTFSLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.SourceTFSLabel.Name = "SourceTFSLabel"
        Me.SourceTFSLabel.Size = New System.Drawing.Size(121, 13)
        Me.SourceTFSLabel.TabIndex = 0
        Me.SourceTFSLabel.Text = "Source TFS Collections:"
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(342, 2)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(336, 374)
        Me.Panel2.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(2, 7)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(140, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Destination TFS Collections:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(39, 13)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Destination TFS:"
        '
        'DestinationTFSTextBox
        '
        Me.DestinationTFSTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DestinationTFSTextBox.Location = New System.Drawing.Point(41, 30)
        Me.DestinationTFSTextBox.Margin = New System.Windows.Forms.Padding(2)
        Me.DestinationTFSTextBox.Name = "DestinationTFSTextBox"
        Me.DestinationTFSTextBox.Size = New System.Drawing.Size(538, 20)
        Me.DestinationTFSTextBox.TabIndex = 10
        Me.DestinationTFSTextBox.Text = "http://192.168.1.114:8080/tfs"
        '
        'QueryCollectionsButton
        '
        Me.QueryCollectionsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.QueryCollectionsButton.Location = New System.Drawing.Point(1251, 51)
        Me.QueryCollectionsButton.Name = "QueryCollectionsButton"
        Me.QueryCollectionsButton.Size = New System.Drawing.Size(115, 33)
        Me.QueryCollectionsButton.TabIndex = 8
        Me.QueryCollectionsButton.Text = "Query Collections"
        Me.QueryCollectionsButton.UseVisualStyleBackColor = True
        '
        'SourceTfsCollectionNodesComboBox
        '
        Me.SourceTfsCollectionNodesComboBox.FormattingEnabled = True
        Me.SourceTfsCollectionNodesComboBox.Location = New System.Drawing.Point(9, 104)
        Me.SourceTfsCollectionNodesComboBox.Name = "SourceTfsCollectionNodesComboBox"
        Me.SourceTfsCollectionNodesComboBox.Size = New System.Drawing.Size(429, 21)
        Me.SourceTfsCollectionNodesComboBox.TabIndex = 10
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 88)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(61, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Collections:"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.VersionSourceControlToolStripMenuItem, Me.TeamCollectionsToolStripMenuItem, Me.Get4CopyToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1387, 24)
        Me.MenuStrip1.TabIndex = 12
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitTFSManagerToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitTFSManagerToolStripMenuItem
        '
        Me.ExitTFSManagerToolStripMenuItem.Name = "ExitTFSManagerToolStripMenuItem"
        Me.ExitTFSManagerToolStripMenuItem.Size = New System.Drawing.Size(164, 22)
        Me.ExitTFSManagerToolStripMenuItem.Text = "Exit TFS Manager"
        '
        'VersionSourceControlToolStripMenuItem
        '
        Me.VersionSourceControlToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BackupSelectedTreeNodeToolStripMenuItem})
        Me.VersionSourceControlToolStripMenuItem.Name = "VersionSourceControlToolStripMenuItem"
        Me.VersionSourceControlToolStripMenuItem.Size = New System.Drawing.Size(139, 20)
        Me.VersionSourceControlToolStripMenuItem.Text = "Version Source Control"
        '
        'BackupSelectedTreeNodeToolStripMenuItem
        '
        Me.BackupSelectedTreeNodeToolStripMenuItem.Name = "BackupSelectedTreeNodeToolStripMenuItem"
        Me.BackupSelectedTreeNodeToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.BackupSelectedTreeNodeToolStripMenuItem.Text = "Backup Selected TreeNode..."
        '
        'TeamCollectionsToolStripMenuItem
        '
        Me.TeamCollectionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WorkItemsAbgleichenToolStripMenuItem, Me.TestabgleichToolStripMenuItem})
        Me.TeamCollectionsToolStripMenuItem.Name = "TeamCollectionsToolStripMenuItem"
        Me.TeamCollectionsToolStripMenuItem.Size = New System.Drawing.Size(110, 20)
        Me.TeamCollectionsToolStripMenuItem.Text = "Team Collections"
        '
        'WorkItemsAbgleichenToolStripMenuItem
        '
        Me.WorkItemsAbgleichenToolStripMenuItem.Name = "WorkItemsAbgleichenToolStripMenuItem"
        Me.WorkItemsAbgleichenToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.WorkItemsAbgleichenToolStripMenuItem.Text = "WorkItems abgleichen"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CurrentTaskItemInfoToolStripStatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 614)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1387, 22)
        Me.StatusStrip1.TabIndex = 13
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'CurrentTaskItemInfoToolStripStatusLabel
        '
        Me.CurrentTaskItemInfoToolStripStatusLabel.Name = "CurrentTaskItemInfoToolStripStatusLabel"
        Me.CurrentTaskItemInfoToolStripStatusLabel.Size = New System.Drawing.Size(222, 17)
        Me.CurrentTaskItemInfoToolStripStatusLabel.Text = "CurrentTaskItemInfoToolStripStatusLabel"
        '
        'TestabgleichToolStripMenuItem
        '
        Me.TestabgleichToolStripMenuItem.Name = "TestabgleichToolStripMenuItem"
        Me.TestabgleichToolStripMenuItem.Size = New System.Drawing.Size(192, 22)
        Me.TestabgleichToolStripMenuItem.Text = "Testabgleich"
        '
        'Get4CopyToolStripMenuItem
        '
        Me.Get4CopyToolStripMenuItem.Name = "Get4CopyToolStripMenuItem"
        Me.Get4CopyToolStripMenuItem.Size = New System.Drawing.Size(74, 20)
        Me.Get4CopyToolStripMenuItem.Text = "Get 4Copy"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1387, 636)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.SourceTfsCollectionNodesComboBox)
        Me.Controls.Add(Me.QueryCollectionsButton)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SourceTFSTextBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.Text = "Spoony's TFS Tool by ActiveDevelop - Historic Code Backup, Backlog Backup, Report"& _ 
    "s and more!"
        Me.TabControl1.ResumeLayout(false)
        Me.TabPage1.ResumeLayout(false)
        Me.TabPage1.PerformLayout
        Me.SplitContainer1.Panel1.ResumeLayout(false)
        Me.SplitContainer1.Panel1.PerformLayout
        Me.SplitContainer1.Panel2.ResumeLayout(false)
        Me.SplitContainer1.Panel2.PerformLayout
        CType(Me.SplitContainer1,System.ComponentModel.ISupportInitialize).EndInit
        Me.SplitContainer1.ResumeLayout(false)
        Me.TabPage2.ResumeLayout(false)
        Me.TabPage2.PerformLayout
        Me.TableLayoutPanel1.ResumeLayout(false)
        Me.Panel1.ResumeLayout(false)
        Me.Panel1.PerformLayout
        Me.Panel2.ResumeLayout(false)
        Me.Panel2.PerformLayout
        Me.MenuStrip1.ResumeLayout(false)
        Me.MenuStrip1.PerformLayout
        Me.StatusStrip1.ResumeLayout(false)
        Me.StatusStrip1.PerformLayout
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents SourceTFSTextBox As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents WorkspacesComboBox As ComboBox
    Friend WithEvents QueryWorkspacesButton As Button
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents ListDestinationTFSProjectsButton As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents SourceTFSTreeView As TreeView
    Friend WithEvents SourceTFSLabel As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents DestinationTFSTextBox As TextBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ServerPathTreeView As TreeView
    Friend WithEvents ServerItemContentListBox As ListBox
    Friend WithEvents QueryCollectionsButton As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents LocalPathLinkLabel As LinkLabel
    Friend WithEvents SourceTfsCollectionNodesComboBox As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents ServerPathLabel As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitTFSManagerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VersionSourceControlToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TeamCollectionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackupSelectedTreeNodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChoosePathButton As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents MaintenanceFolderTextBox As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents CurrentTaskItemInfoToolStripStatusLabel As ToolStripStatusLabel
    Friend WithEvents WorkItemsAbgleichenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TestabgleichToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Get4CopyToolStripMenuItem As ToolStripMenuItem
End Class
