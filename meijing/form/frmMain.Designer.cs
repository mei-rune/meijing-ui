using System.Windows.Forms;
using meijing.ui.module;

namespace meijing.ui
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("设备", 1, 1);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("线路", 15, 16);
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusSelectedObj = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUserInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAction = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.addDevice_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteDevice_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addLinktoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteLinktoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.addTriggertoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteTriggertoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.ManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.addDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OperationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ThanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addDevice_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDevice_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLink_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLink_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTrigger_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTrigger_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trvsrvlst = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.rPanel = new System.Windows.Forms.Panel();
            this.listView = new System.Windows.Forms.ListView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.triggerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.editTrigger_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.linkContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripMain.SuspendLayout();
            this.toolStripMain.SuspendLayout();
            this.menuStripMain.SuspendLayout();
            this.contextMenuStripMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.rPanel.SuspendLayout();
            this.triggerContextMenuStrip.SuspendLayout();
            this.linkContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripMain
            // 
            this.statusStripMain.BackColor = System.Drawing.Color.Transparent;
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusSelectedObj,
            this.lblUserInfo,
            this.lblAction});
            this.statusStripMain.Location = new System.Drawing.Point(0, 680);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Padding = new System.Windows.Forms.Padding(1, 0, 18, 0);
            this.statusStripMain.Size = new System.Drawing.Size(1223, 22);
            this.statusStripMain.TabIndex = 8;
            // 
            // toolStripStatusSelectedObj
            // 
            this.toolStripStatusSelectedObj.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusSelectedObj.Name = "toolStripStatusSelectedObj";
            this.toolStripStatusSelectedObj.Size = new System.Drawing.Size(44, 17);
            this.toolStripStatusSelectedObj.Text = "Ready";
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(35, 17);
            this.lblUserInfo.Text = "User";
            // 
            // lblAction
            // 
            this.lblAction.Name = "lblAction";
            this.lblAction.Size = new System.Drawing.Size(44, 17);
            this.lblAction.Text = "Action";
            // 
            // toolStripMain
            // 
            this.toolStripMain.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDevice_toolStripButton,
            this.deleteDevice_toolStripButton,
            this.addLinktoolStripButton,
            this.deleteLinktoolStripButton,
            this.addTriggertoolStripButton,
            this.deleteTriggertoolStripButton});
            this.toolStripMain.Location = new System.Drawing.Point(0, 25);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1223, 25);
            this.toolStripMain.TabIndex = 7;
            this.toolStripMain.Text = "工具栏";
            // 
            // addDevice_toolStripButton
            // 
            this.addDevice_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addDevice_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addDevice_toolStripButton.Image")));
            this.addDevice_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addDevice_toolStripButton.Name = "addDevice_toolStripButton";
            this.addDevice_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.addDevice_toolStripButton.Text = "增加设备";
            this.addDevice_toolStripButton.Click += new System.EventHandler(this.addDeviceToolStripMenuItem_Click);
            // 
            // deleteDevice_toolStripButton
            // 
            this.deleteDevice_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteDevice_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteDevice_toolStripButton.Image")));
            this.deleteDevice_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteDevice_toolStripButton.Name = "deleteDevice_toolStripButton";
            this.deleteDevice_toolStripButton.Size = new System.Drawing.Size(23, 22);
            this.deleteDevice_toolStripButton.Text = "删除设备";
            this.deleteDevice_toolStripButton.Click += new System.EventHandler(this.deleteDeviceToolStripMenuItem_Click);
            // 
            // addLinktoolStripButton
            // 
            this.addLinktoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addLinktoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addLinktoolStripButton.Image")));
            this.addLinktoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addLinktoolStripButton.Name = "addLinktoolStripButton";
            this.addLinktoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.addLinktoolStripButton.Text = "添加线路";
            this.addLinktoolStripButton.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // deleteLinktoolStripButton
            // 
            this.deleteLinktoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteLinktoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteLinktoolStripButton.Image")));
            this.deleteLinktoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteLinktoolStripButton.Name = "deleteLinktoolStripButton";
            this.deleteLinktoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.deleteLinktoolStripButton.Text = "删除线路";
            this.deleteLinktoolStripButton.Click += new System.EventHandler(this.deleteLinkToolStripMenuItem_Click);
            // 
            // addTriggertoolStripButton
            // 
            this.addTriggertoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addTriggertoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addTriggertoolStripButton.Image")));
            this.addTriggertoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addTriggertoolStripButton.Name = "addTriggertoolStripButton";
            this.addTriggertoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.addTriggertoolStripButton.Text = "添加任务";
            this.addTriggertoolStripButton.Click += new System.EventHandler(this.addTriggerToolStripMenuItem_Click);
            // 
            // deleteTriggertoolStripButton
            // 
            this.deleteTriggertoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteTriggertoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteTriggertoolStripButton.Image")));
            this.deleteTriggertoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteTriggertoolStripButton.Name = "deleteTriggertoolStripButton";
            this.deleteTriggertoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.deleteTriggertoolStripButton.Text = "删除任务";
            this.deleteTriggertoolStripButton.ToolTipText = "删除任务";
            this.deleteTriggertoolStripButton.Click += new System.EventHandler(this.deleteTriggerToolStripMenuItem_Click);
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ManagerToolStripMenuItem,
            this.OperationToolStripMenuItem,
            this.HelpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStripMain.Size = new System.Drawing.Size(1223, 25);
            this.menuStripMain.TabIndex = 6;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // ManagerToolStripMenuItem
            // 
            this.ManagerToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.ManagerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshToolStripMenuItem,
            this.toolStripMenuItem1,
            this.addDeviceToolStripMenuItem,
            this.deleteDeviceToolStripMenuItem,
            this.addLinkToolStripMenuItem,
            this.deleteLinkToolStripMenuItem,
            this.toolStripSeparator1,
            this.ExitToolStripMenuItem});
            this.ManagerToolStripMenuItem.Name = "ManagerToolStripMenuItem";
            this.ManagerToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.ManagerToolStripMenuItem.Text = "管理";
            // 
            // RefreshToolStripMenuItem
            // 
            this.RefreshToolStripMenuItem.Image = global::meijing.ui.Properties.Resources.Refresh;
            this.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem";
            this.RefreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.RefreshToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.RefreshToolStripMenuItem.Text = "刷新";
            this.RefreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(142, 6);
            // 
            // addDeviceToolStripMenuItem
            // 
            this.addDeviceToolStripMenuItem.Name = "addDeviceToolStripMenuItem";
            this.addDeviceToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.addDeviceToolStripMenuItem.Text = "增加设备";
            this.addDeviceToolStripMenuItem.Click += new System.EventHandler(this.addDeviceToolStripMenuItem_Click);
            // 
            // deleteDeviceToolStripMenuItem
            // 
            this.deleteDeviceToolStripMenuItem.Name = "deleteDeviceToolStripMenuItem";
            this.deleteDeviceToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.deleteDeviceToolStripMenuItem.Text = "删除设备";
            this.deleteDeviceToolStripMenuItem.Click += new System.EventHandler(this.deleteDeviceToolStripMenuItem_Click);
            // 
            // addLinkToolStripMenuItem
            // 
            this.addLinkToolStripMenuItem.Name = "addLinkToolStripMenuItem";
            this.addLinkToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.addLinkToolStripMenuItem.Text = "增加线路";
            this.addLinkToolStripMenuItem.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // deleteLinkToolStripMenuItem
            // 
            this.deleteLinkToolStripMenuItem.Name = "deleteLinkToolStripMenuItem";
            this.deleteLinkToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.deleteLinkToolStripMenuItem.Text = "删除线路";
            this.deleteLinkToolStripMenuItem.Click += new System.EventHandler(this.deleteLinkToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(142, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.ExitToolStripMenuItem.Text = "退出";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // OperationToolStripMenuItem
            // 
            this.OperationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTriggerToolStripMenuItem,
            this.deleteTriggerToolStripMenuItem});
            this.OperationToolStripMenuItem.Name = "OperationToolStripMenuItem";
            this.OperationToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.OperationToolStripMenuItem.Text = "任务";
            // 
            // addTriggerToolStripMenuItem
            // 
            this.addTriggerToolStripMenuItem.Name = "addTriggerToolStripMenuItem";
            this.addTriggerToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addTriggerToolStripMenuItem.Text = "添加任务";
            this.addTriggerToolStripMenuItem.Click += new System.EventHandler(this.addTriggerToolStripMenuItem_Click);
            // 
            // deleteTriggerToolStripMenuItem
            // 
            this.deleteTriggerToolStripMenuItem.Name = "deleteTriggerToolStripMenuItem";
            this.deleteTriggerToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteTriggerToolStripMenuItem.Text = "删除任务";
            this.deleteTriggerToolStripMenuItem.Click += new System.EventHandler(this.deleteTriggerToolStripMenuItem_Click);
            // 
            // HelpToolStripMenuItem
            // 
            this.HelpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ThanksToolStripMenuItem,
            this.toolStripSeparator2,
            this.AboutToolStripMenuItem,
            this.toolStripMenuItem12});
            this.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem";
            this.HelpToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.HelpToolStripMenuItem.Text = "帮助";
            // 
            // ThanksToolStripMenuItem
            // 
            this.ThanksToolStripMenuItem.Name = "ThanksToolStripMenuItem";
            this.ThanksToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.ThanksToolStripMenuItem.Text = "感谢";
            this.ThanksToolStripMenuItem.Click += new System.EventHandler(this.ThanksToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(97, 6);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.AboutToolStripMenuItem.Text = "关于";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(97, 6);
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addDevice_ToolStripMenuItem,
            this.deleteDevice_ToolStripMenuItem,
            this.addLink_ToolStripMenuItem,
            this.deleteLink_ToolStripMenuItem,
            this.addTrigger_ToolStripMenuItem,
            this.deleteTrigger_ToolStripMenuItem});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.Size = new System.Drawing.Size(125, 136);
            // 
            // addDevice_ToolStripMenuItem
            // 
            this.addDevice_ToolStripMenuItem.Name = "addDevice_ToolStripMenuItem";
            this.addDevice_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addDevice_ToolStripMenuItem.Text = "添加设备";
            this.addDevice_ToolStripMenuItem.Click += new System.EventHandler(this.addDeviceToolStripMenuItem_Click);
            // 
            // deleteDevice_ToolStripMenuItem
            // 
            this.deleteDevice_ToolStripMenuItem.Name = "deleteDevice_ToolStripMenuItem";
            this.deleteDevice_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteDevice_ToolStripMenuItem.Text = "删除设备";
            this.deleteDevice_ToolStripMenuItem.Click += new System.EventHandler(this.deleteDeviceToolStripMenuItem_Click);
            // 
            // addLink_ToolStripMenuItem
            // 
            this.addLink_ToolStripMenuItem.Name = "addLink_ToolStripMenuItem";
            this.addLink_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addLink_ToolStripMenuItem.Text = "添加线路";
            this.addLink_ToolStripMenuItem.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // deleteLink_ToolStripMenuItem
            // 
            this.deleteLink_ToolStripMenuItem.Name = "deleteLink_ToolStripMenuItem";
            this.deleteLink_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteLink_ToolStripMenuItem.Text = "删除线路";
            this.deleteLink_ToolStripMenuItem.Click += new System.EventHandler(this.deleteLinkToolStripMenuItem_Click);
            // 
            // addTrigger_ToolStripMenuItem
            // 
            this.addTrigger_ToolStripMenuItem.Name = "addTrigger_ToolStripMenuItem";
            this.addTrigger_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.addTrigger_ToolStripMenuItem.Text = "添加任务";
            this.addTrigger_ToolStripMenuItem.Click += new System.EventHandler(this.addTriggerToolStripMenuItem_Click);
            // 
            // deleteTrigger_ToolStripMenuItem
            // 
            this.deleteTrigger_ToolStripMenuItem.Name = "deleteTrigger_ToolStripMenuItem";
            this.deleteTrigger_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.deleteTrigger_ToolStripMenuItem.Text = "删除任务";
            this.deleteTrigger_ToolStripMenuItem.Click += new System.EventHandler(this.deleteTriggerToolStripMenuItem_Click);
            // 
            // trvsrvlst
            // 
            this.trvsrvlst.BackColor = System.Drawing.Color.White;
            this.trvsrvlst.ContextMenuStrip = this.contextMenuStripMain;
            this.trvsrvlst.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvsrvlst.ImageIndex = 0;
            this.trvsrvlst.ImageList = this.imageList1;
            this.trvsrvlst.Location = new System.Drawing.Point(0, 0);
            this.trvsrvlst.Name = "trvsrvlst";
            treeNode1.ImageIndex = 1;
            treeNode1.Name = "devices";
            treeNode1.SelectedImageIndex = 1;
            treeNode1.StateImageKey = "(无)";
            treeNode1.Text = "设备";
            treeNode2.ImageIndex = 15;
            treeNode2.Name = "links";
            treeNode2.SelectedImageIndex = 16;
            treeNode2.StateImageKey = "(无)";
            treeNode2.Text = "线路";
            this.trvsrvlst.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.trvsrvlst.SelectedImageIndex = 0;
            this.trvsrvlst.ShowNodeToolTips = true;
            this.trvsrvlst.Size = new System.Drawing.Size(410, 630);
            this.trvsrvlst.TabIndex = 0;
            this.trvsrvlst.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvsrvlst_AfterSelect);
            this.trvsrvlst.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trvsrvlst_MouseClick);
            this.trvsrvlst.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trvsrvlst_MouseDoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "(00,48).png");
            this.imageList1.Images.SetKeyName(1, "(00,48).png");
            this.imageList1.Images.SetKeyName(2, "1005Icon.gif");
            this.imageList1.Images.SetKeyName(3, "1005Icon.gif");
            this.imageList1.Images.SetKeyName(4, "(00,21).png");
            this.imageList1.Images.SetKeyName(5, "(00,21).png");
            this.imageList1.Images.SetKeyName(6, "100114Icon.gif");
            this.imageList1.Images.SetKeyName(7, "100114Icon.gif");
            this.imageList1.Images.SetKeyName(8, "(05,07).png");
            this.imageList1.Images.SetKeyName(9, "(05,07).png");
            this.imageList1.Images.SetKeyName(10, "(31,20).png");
            this.imageList1.Images.SetKeyName(11, "(31,20).png");
            this.imageList1.Images.SetKeyName(12, "(13,12).png");
            this.imageList1.Images.SetKeyName(13, "(13,12).png");
            this.imageList1.Images.SetKeyName(14, "(07,42).png");
            this.imageList1.Images.SetKeyName(15, "(18,27).png");
            this.imageList1.Images.SetKeyName(16, "(26,29).png");
            this.imageList1.Images.SetKeyName(17, "link_001.png");
            this.imageList1.Images.SetKeyName(18, "link_001.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rPanel);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.trvsrvlst);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1223, 630);
            this.panel1.TabIndex = 9;
            // 
            // rPanel
            // 
            this.rPanel.Controls.Add(this.listView);
            this.rPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rPanel.Location = new System.Drawing.Point(413, 0);
            this.rPanel.Name = "rPanel";
            this.rPanel.Size = new System.Drawing.Size(810, 630);
            this.rPanel.TabIndex = 2;
            // 
            // listView
            // 
            this.listView.ContextMenuStrip = this.contextMenuStripMain;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(810, 630);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(410, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 630);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // triggerContextMenuStrip
            // 
            this.triggerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.editTrigger_ToolStripMenuItem,
            this.toolStripMenuItem7});
            this.triggerContextMenuStrip.Name = "contextMenuStripMain";
            this.triggerContextMenuStrip.Size = new System.Drawing.Size(153, 180);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "添加设备";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.addDeviceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "删除设备";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.deleteDeviceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem4.Text = "添加线路";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem5.Text = "删除线路";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.deleteLinkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem6.Text = "添加任务";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.addTriggerToolStripMenuItem_Click);
            // 
            // editTrigger_ToolStripMenuItem
            // 
            this.editTrigger_ToolStripMenuItem.Name = "editTrigger_ToolStripMenuItem";
            this.editTrigger_ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.editTrigger_ToolStripMenuItem.Text = "修改任务";
            this.editTrigger_ToolStripMenuItem.Click += new System.EventHandler(this.editTrigger_ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem7.Text = "删除任务";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.deleteTriggerToolStripMenuItem_Click);
            // 
            // linkContextMenuStrip
            // 
            this.linkContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem13,
            this.toolStripMenuItem14,
            this.toolStripMenuItem16});
            this.linkContextMenuStrip.Name = "contextMenuStripMain";
            this.linkContextMenuStrip.Size = new System.Drawing.Size(125, 158);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem8.Text = "添加设备";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.addDeviceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem9.Text = "删除设备";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.deleteDeviceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem10.Text = "添加线路";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.addLinkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem11.Text = "修改线路";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.editLinkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem13
            // 
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem13.Text = "删除线路";
            this.toolStripMenuItem13.Click += new System.EventHandler(this.deleteLinkToolStripMenuItem_Click);
            // 
            // toolStripMenuItem14
            // 
            this.toolStripMenuItem14.Name = "toolStripMenuItem14";
            this.toolStripMenuItem14.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem14.Text = "添加任务";
            this.toolStripMenuItem14.Click += new System.EventHandler(this.addTriggerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem16
            // 
            this.toolStripMenuItem16.Name = "toolStripMenuItem16";
            this.toolStripMenuItem16.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem16.Text = "删除任务";
            this.toolStripMenuItem16.Click += new System.EventHandler(this.deleteTriggerToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1223, 702);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "meijing-ui";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.contextMenuStripMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.rPanel.ResumeLayout(false);
            this.triggerContextMenuStrip.ResumeLayout(false);
            this.linkContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
         #endregion

        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSelectedObj;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem ManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OperationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem HelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ThanksToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripStatusLabel lblUserInfo;
        private TreeView trvsrvlst;
        private Panel panel1;
        private Panel rPanel;
        private Splitter splitter1;
        private ToolStripStatusLabel lblAction;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripMenuItem12;
        private ImageList imageList1;
        private ListView listView;
        private ToolStripMenuItem addDeviceToolStripMenuItem;
        private ToolStripMenuItem deleteDeviceToolStripMenuItem;
        private ToolStripMenuItem addLinkToolStripMenuItem;
        private ToolStripMenuItem deleteLinkToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton addDevice_toolStripButton;
        private ToolStripButton deleteDevice_toolStripButton;
        private ToolStripButton addLinktoolStripButton;
        private ToolStripButton deleteLinktoolStripButton;
        private ToolStripButton addTriggertoolStripButton;
        private ToolStripButton deleteTriggertoolStripButton;
        private ToolStripMenuItem addTriggerToolStripMenuItem;
        private ToolStripMenuItem deleteTriggerToolStripMenuItem;
        private ToolStripMenuItem addDevice_ToolStripMenuItem;
        private ToolStripMenuItem deleteDevice_ToolStripMenuItem;
        private ToolStripMenuItem addLink_ToolStripMenuItem;
        private ToolStripMenuItem deleteLink_ToolStripMenuItem;
        private ToolStripMenuItem addTrigger_ToolStripMenuItem;
        private ToolStripMenuItem deleteTrigger_ToolStripMenuItem;
        private ContextMenuStrip triggerContextMenuStrip;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5;
        private ToolStripMenuItem toolStripMenuItem6;
        private ToolStripMenuItem editTrigger_ToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem7;
        private ContextMenuStrip linkContextMenuStrip;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem13;
        private ToolStripMenuItem toolStripMenuItem14;
        private ToolStripMenuItem toolStripMenuItem16;
    }
}