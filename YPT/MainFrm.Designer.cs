using YU.Core.YUComponent;

namespace YPT
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.同步StripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.签到ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空CookieToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelTorrent = new System.Windows.Forms.Panel();
            this.dgvTorrent = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.toolSearchPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.innerPanel1 = new System.Windows.Forms.Panel();
            this.lblPromotion = new System.Windows.Forms.Label();
            this.cmbPromotion = new YU.Core.YUComponent.YUComboBox(this.components);
            this.innerPanel2 = new System.Windows.Forms.Panel();
            this.lblAlive = new System.Windows.Forms.Label();
            this.cmbAlive = new YU.Core.YUComponent.YUComboBox(this.components);
            this.innerPanel3 = new System.Windows.Forms.Panel();
            this.lblFav = new System.Windows.Forms.Label();
            this.cmbFav = new YU.Core.YUComponent.YUComboBox(this.components);
            this.innerPanel4 = new System.Windows.Forms.Panel();
            this.cbIsLastSort = new System.Windows.Forms.CheckBox();
            this.cbIsPostSiteOrder = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbIsSearchTiming = new System.Windows.Forms.CheckBox();
            this.cbIsIngoreTop = new System.Windows.Forms.CheckBox();
            this.innerPanel5 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelSite = new System.Windows.Forms.Panel();
            this.tabPersonInfo = new System.Windows.Forms.TabPage();
            this.dgvPersonInfo = new System.Windows.Forms.DataGridView();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.logPanel = new System.Windows.Forms.Panel();
            this.rtbLog = new YU.Core.YUComponent.YURichTextBox(this.components);
            this.btnClearLog = new System.Windows.Forms.Button();
            this.nfyMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.nfyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvTorrentContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpenTorrent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuIDownAndOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvInfoContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpenInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelTorrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTorrent)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.toolSearchPanel.SuspendLayout();
            this.innerPanel1.SuspendLayout();
            this.innerPanel2.SuspendLayout();
            this.innerPanel3.SuspendLayout();
            this.innerPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.innerPanel5.SuspendLayout();
            this.tabPersonInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonInfo)).BeginInit();
            this.tabLog.SuspendLayout();
            this.logPanel.SuspendLayout();
            this.nfyContextMenu.SuspendLayout();
            this.dgvTorrentContextMenu.SuspendLayout();
            this.dgvInfoContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.同步StripMenuItem,
            this.登录ToolStripMenuItem,
            this.签到ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.mainMenuStrip.Size = new System.Drawing.Size(1360, 27);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // 同步StripMenuItem
            // 
            this.同步StripMenuItem.Name = "同步StripMenuItem";
            this.同步StripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.同步StripMenuItem.Text = "同步";
            this.同步StripMenuItem.Click += new System.EventHandler(this.同步StripMenuItem_Click);
            // 
            // 登录ToolStripMenuItem
            // 
            this.登录ToolStripMenuItem.Name = "登录ToolStripMenuItem";
            this.登录ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.登录ToolStripMenuItem.Text = "登录";
            this.登录ToolStripMenuItem.Click += new System.EventHandler(this.登录ToolStripMenuItem_Click);
            // 
            // 签到ToolStripMenuItem
            // 
            this.签到ToolStripMenuItem.Name = "签到ToolStripMenuItem";
            this.签到ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.签到ToolStripMenuItem.Text = "签到";
            this.签到ToolStripMenuItem.Click += new System.EventHandler(this.签到ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于ToolStripMenuItem,
            this.清空CookieToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 清空CookieToolStripMenuItem
            // 
            this.清空CookieToolStripMenuItem.Name = "清空CookieToolStripMenuItem";
            this.清空CookieToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.清空CookieToolStripMenuItem.Text = "清空Cookie";
            this.清空CookieToolStripMenuItem.Click += new System.EventHandler(this.清空CookieToolStripMenuItem_Click);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabSearch);
            this.tabMain.Controls.Add(this.tabPersonInfo);
            this.tabMain.Controls.Add(this.tabLog);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabMain.Location = new System.Drawing.Point(0, 27);
            this.tabMain.Margin = new System.Windows.Forms.Padding(4);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1360, 627);
            this.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMain.TabIndex = 1;
            this.tabMain.TabStop = false;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.panelMain);
            this.tabSearch.Controls.Add(this.panelSite);
            this.tabSearch.Location = new System.Drawing.Point(4, 29);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(1352, 594);
            this.tabSearch.TabIndex = 1;
            this.tabSearch.Text = "种子搜索";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelTorrent);
            this.panelMain.Controls.Add(this.panelSearch);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(164, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1185, 588);
            this.panelMain.TabIndex = 1;
            // 
            // panelTorrent
            // 
            this.panelTorrent.Controls.Add(this.dgvTorrent);
            this.panelTorrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTorrent.Location = new System.Drawing.Point(0, 117);
            this.panelTorrent.Name = "panelTorrent";
            this.panelTorrent.Size = new System.Drawing.Size(1185, 471);
            this.panelTorrent.TabIndex = 1;
            // 
            // dgvTorrent
            // 
            this.dgvTorrent.BackgroundColor = System.Drawing.Color.White;
            this.dgvTorrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTorrent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTorrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTorrent.GridColor = System.Drawing.Color.LightGray;
            this.dgvTorrent.Location = new System.Drawing.Point(0, 0);
            this.dgvTorrent.Name = "dgvTorrent";
            this.dgvTorrent.RowTemplate.Height = 23;
            this.dgvTorrent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTorrent.Size = new System.Drawing.Size(1185, 471);
            this.dgvTorrent.TabIndex = 0;
            this.dgvTorrent.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTorrent_CellMouseDown);
            this.dgvTorrent.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dgvTorrent.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            // 
            // panelSearch
            // 
            this.panelSearch.AutoSize = true;
            this.panelSearch.Controls.Add(this.toolSearchPanel);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1185, 117);
            this.panelSearch.TabIndex = 0;
            this.panelSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSearch_Paint);
            // 
            // toolSearchPanel
            // 
            this.toolSearchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolSearchPanel.AutoSize = true;
            this.toolSearchPanel.Controls.Add(this.innerPanel1);
            this.toolSearchPanel.Controls.Add(this.innerPanel2);
            this.toolSearchPanel.Controls.Add(this.innerPanel3);
            this.toolSearchPanel.Controls.Add(this.innerPanel4);
            this.toolSearchPanel.Controls.Add(this.panel1);
            this.toolSearchPanel.Controls.Add(this.innerPanel5);
            this.toolSearchPanel.Location = new System.Drawing.Point(31, 43);
            this.toolSearchPanel.Name = "toolSearchPanel";
            this.toolSearchPanel.Size = new System.Drawing.Size(1070, 71);
            this.toolSearchPanel.TabIndex = 8;
            // 
            // innerPanel1
            // 
            this.innerPanel1.Controls.Add(this.lblPromotion);
            this.innerPanel1.Controls.Add(this.cmbPromotion);
            this.innerPanel1.Location = new System.Drawing.Point(3, 3);
            this.innerPanel1.Name = "innerPanel1";
            this.innerPanel1.Size = new System.Drawing.Size(160, 65);
            this.innerPanel1.TabIndex = 8;
            // 
            // lblPromotion
            // 
            this.lblPromotion.AutoSize = true;
            this.lblPromotion.Location = new System.Drawing.Point(5, 5);
            this.lblPromotion.Name = "lblPromotion";
            this.lblPromotion.Size = new System.Drawing.Size(65, 20);
            this.lblPromotion.TabIndex = 4;
            this.lblPromotion.Text = "促销类型";
            // 
            // cmbPromotion
            // 
            this.cmbPromotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotion.FormattingEnabled = true;
            this.cmbPromotion.Location = new System.Drawing.Point(9, 31);
            this.cmbPromotion.Name = "cmbPromotion";
            this.cmbPromotion.Size = new System.Drawing.Size(121, 28);
            this.cmbPromotion.TabIndex = 2;
            // 
            // innerPanel2
            // 
            this.innerPanel2.Controls.Add(this.lblAlive);
            this.innerPanel2.Controls.Add(this.cmbAlive);
            this.innerPanel2.Location = new System.Drawing.Point(169, 3);
            this.innerPanel2.Name = "innerPanel2";
            this.innerPanel2.Size = new System.Drawing.Size(160, 65);
            this.innerPanel2.TabIndex = 9;
            // 
            // lblAlive
            // 
            this.lblAlive.AutoSize = true;
            this.lblAlive.Location = new System.Drawing.Point(5, 5);
            this.lblAlive.Name = "lblAlive";
            this.lblAlive.Size = new System.Drawing.Size(71, 20);
            this.lblAlive.TabIndex = 5;
            this.lblAlive.Text = "断种/活种";
            // 
            // cmbAlive
            // 
            this.cmbAlive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlive.FormattingEnabled = true;
            this.cmbAlive.Location = new System.Drawing.Point(9, 31);
            this.cmbAlive.Name = "cmbAlive";
            this.cmbAlive.Size = new System.Drawing.Size(121, 28);
            this.cmbAlive.TabIndex = 3;
            // 
            // innerPanel3
            // 
            this.innerPanel3.Controls.Add(this.lblFav);
            this.innerPanel3.Controls.Add(this.cmbFav);
            this.innerPanel3.Location = new System.Drawing.Point(335, 3);
            this.innerPanel3.Name = "innerPanel3";
            this.innerPanel3.Size = new System.Drawing.Size(160, 65);
            this.innerPanel3.TabIndex = 10;
            // 
            // lblFav
            // 
            this.lblFav.AutoSize = true;
            this.lblFav.Location = new System.Drawing.Point(6, 5);
            this.lblFav.Name = "lblFav";
            this.lblFav.Size = new System.Drawing.Size(65, 20);
            this.lblFav.TabIndex = 7;
            this.lblFav.Text = "显示收藏";
            // 
            // cmbFav
            // 
            this.cmbFav.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFav.FormattingEnabled = true;
            this.cmbFav.Location = new System.Drawing.Point(10, 31);
            this.cmbFav.Name = "cmbFav";
            this.cmbFav.Size = new System.Drawing.Size(121, 28);
            this.cmbFav.TabIndex = 6;
            // 
            // innerPanel4
            // 
            this.innerPanel4.Controls.Add(this.cbIsLastSort);
            this.innerPanel4.Controls.Add(this.cbIsPostSiteOrder);
            this.innerPanel4.Location = new System.Drawing.Point(501, 3);
            this.innerPanel4.Name = "innerPanel4";
            this.innerPanel4.Size = new System.Drawing.Size(119, 65);
            this.innerPanel4.TabIndex = 9;
            // 
            // cbIsLastSort
            // 
            this.cbIsLastSort.AutoSize = true;
            this.cbIsLastSort.Location = new System.Drawing.Point(5, 33);
            this.cbIsLastSort.Name = "cbIsLastSort";
            this.cbIsLastSort.Size = new System.Drawing.Size(112, 24);
            this.cbIsLastSort.TabIndex = 1;
            this.cbIsLastSort.Text = "记住上次排序";
            this.cbIsLastSort.UseVisualStyleBackColor = true;
            // 
            // cbIsPostSiteOrder
            // 
            this.cbIsPostSiteOrder.AutoSize = true;
            this.cbIsPostSiteOrder.Location = new System.Drawing.Point(5, 3);
            this.cbIsPostSiteOrder.Name = "cbIsPostSiteOrder";
            this.cbIsPostSiteOrder.Size = new System.Drawing.Size(112, 24);
            this.cbIsPostSiteOrder.TabIndex = 0;
            this.cbIsPostSiteOrder.Text = "请求站点排序";
            this.cbIsPostSiteOrder.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbIsSearchTiming);
            this.panel1.Controls.Add(this.cbIsIngoreTop);
            this.panel1.Location = new System.Drawing.Point(626, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 65);
            this.panel1.TabIndex = 10;
            // 
            // cbIsSearchTiming
            // 
            this.cbIsSearchTiming.AutoSize = true;
            this.cbIsSearchTiming.Location = new System.Drawing.Point(6, 33);
            this.cbIsSearchTiming.Name = "cbIsSearchTiming";
            this.cbIsSearchTiming.Size = new System.Drawing.Size(84, 24);
            this.cbIsSearchTiming.TabIndex = 1;
            this.cbIsSearchTiming.Text = "定时搜索";
            this.cbIsSearchTiming.UseVisualStyleBackColor = true;
            // 
            // cbIsIngoreTop
            // 
            this.cbIsIngoreTop.AutoSize = true;
            this.cbIsIngoreTop.Location = new System.Drawing.Point(6, 3);
            this.cbIsIngoreTop.Name = "cbIsIngoreTop";
            this.cbIsIngoreTop.Size = new System.Drawing.Size(84, 24);
            this.cbIsIngoreTop.TabIndex = 0;
            this.cbIsIngoreTop.Text = "忽略置顶";
            this.cbIsIngoreTop.UseVisualStyleBackColor = true;
            // 
            // innerPanel5
            // 
            this.innerPanel5.Controls.Add(this.btnSearch);
            this.innerPanel5.Location = new System.Drawing.Point(751, 3);
            this.innerPanel5.Name = "innerPanel5";
            this.innerPanel5.Size = new System.Drawing.Size(119, 65);
            this.innerPanel5.TabIndex = 11;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(8, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(108, 49);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSearch.Location = new System.Drawing.Point(43, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(1058, 26);
            this.txtSearch.TabIndex = 0;
            // 
            // panelSite
            // 
            this.panelSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSite.Location = new System.Drawing.Point(3, 3);
            this.panelSite.Name = "panelSite";
            this.panelSite.Size = new System.Drawing.Size(161, 588);
            this.panelSite.TabIndex = 0;
            this.panelSite.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSite_Paint);
            // 
            // tabPersonInfo
            // 
            this.tabPersonInfo.Controls.Add(this.dgvPersonInfo);
            this.tabPersonInfo.Location = new System.Drawing.Point(4, 29);
            this.tabPersonInfo.Name = "tabPersonInfo";
            this.tabPersonInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPersonInfo.Size = new System.Drawing.Size(1164, 594);
            this.tabPersonInfo.TabIndex = 2;
            this.tabPersonInfo.Text = "个人信息";
            this.tabPersonInfo.UseVisualStyleBackColor = true;
            // 
            // dgvPersonInfo
            // 
            this.dgvPersonInfo.BackgroundColor = System.Drawing.Color.White;
            this.dgvPersonInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPersonInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPersonInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersonInfo.GridColor = System.Drawing.Color.LightGray;
            this.dgvPersonInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvPersonInfo.Name = "dgvPersonInfo";
            this.dgvPersonInfo.RowTemplate.Height = 23;
            this.dgvPersonInfo.Size = new System.Drawing.Size(1158, 588);
            this.dgvPersonInfo.TabIndex = 1;
            this.dgvPersonInfo.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvPersonInfo_CellMouseDown);
            this.dgvPersonInfo.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dgvPersonInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.logPanel);
            this.tabLog.Location = new System.Drawing.Point(4, 29);
            this.tabLog.Margin = new System.Windows.Forms.Padding(4);
            this.tabLog.Name = "tabLog";
            this.tabLog.Padding = new System.Windows.Forms.Padding(4);
            this.tabLog.Size = new System.Drawing.Size(1164, 594);
            this.tabLog.TabIndex = 0;
            this.tabLog.Text = "日志";
            this.tabLog.UseVisualStyleBackColor = true;
            // 
            // logPanel
            // 
            this.logPanel.Controls.Add(this.rtbLog);
            this.logPanel.Controls.Add(this.btnClearLog);
            this.logPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logPanel.Location = new System.Drawing.Point(4, 4);
            this.logPanel.Name = "logPanel";
            this.logPanel.Size = new System.Drawing.Size(1156, 586);
            this.logPanel.TabIndex = 2;
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.Color.White;
            this.rtbLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbLog.IsShowCursor = false;
            this.rtbLog.Location = new System.Drawing.Point(0, 0);
            this.rtbLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.Size = new System.Drawing.Size(1156, 540);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.TabStop = false;
            this.rtbLog.Text = "";
            this.rtbLog.TextChanged += new System.EventHandler(this.rtbLog_TextChanged);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnClearLog.FlatAppearance.BorderSize = 0;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Location = new System.Drawing.Point(0, 540);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(1156, 46);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.TabStop = false;
            this.btnClearLog.Text = "清空";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // nfyMain
            // 
            this.nfyMain.ContextMenuStrip = this.nfyContextMenu;
            this.nfyMain.Icon = ((System.Drawing.Icon)(resources.GetObject("nfyMain.Icon")));
            this.nfyMain.Text = "YPT";
            this.nfyMain.Visible = true;
            this.nfyMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.nfyMain_MouseDoubleClick);
            this.nfyMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.nfyMain_MouseDoubleClick);
            // 
            // nfyContextMenu
            // 
            this.nfyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuQuit});
            this.nfyContextMenu.Name = "nfyContextMenu";
            this.nfyContextMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuQuit
            // 
            this.toolStripMenuQuit.Name = "toolStripMenuQuit";
            this.toolStripMenuQuit.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuQuit.Text = "退出程序";
            this.toolStripMenuQuit.Click += new System.EventHandler(this.toolStripMenuQuit_Click);
            // 
            // dgvTorrentContextMenu
            // 
            this.dgvTorrentContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpenTorrent,
            this.toolStripMenuDown,
            this.toolStripMenuIDownAndOpen,
            this.toolStripMenuItemCopy});
            this.dgvTorrentContextMenu.Name = "dgvContextMenu";
            this.dgvTorrentContextMenu.Size = new System.Drawing.Size(137, 92);
            // 
            // toolStripMenuOpenTorrent
            // 
            this.toolStripMenuOpenTorrent.Name = "toolStripMenuOpenTorrent";
            this.toolStripMenuOpenTorrent.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuOpenTorrent.Text = "打开链接";
            this.toolStripMenuOpenTorrent.Click += new System.EventHandler(this.toolStripMenuOpenTorrent_Click);
            // 
            // toolStripMenuDown
            // 
            this.toolStripMenuDown.Name = "toolStripMenuDown";
            this.toolStripMenuDown.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuDown.Text = "下载种子";
            this.toolStripMenuDown.Click += new System.EventHandler(this.toolStripMenuDown_Click);
            // 
            // toolStripMenuIDownAndOpen
            // 
            this.toolStripMenuIDownAndOpen.Name = "toolStripMenuIDownAndOpen";
            this.toolStripMenuIDownAndOpen.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuIDownAndOpen.Text = "下载并打开";
            this.toolStripMenuIDownAndOpen.Click += new System.EventHandler(this.toolStripMenuIDownAndOpen_Click);
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItemCopy.Text = "复制链接";
            this.toolStripMenuItemCopy.Click += new System.EventHandler(this.toolStripMenuItemCopy_Click);
            // 
            // dgvInfoContextMenu
            // 
            this.dgvInfoContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpenInfo});
            this.dgvInfoContextMenu.Name = "dgvInfoContextMenu";
            this.dgvInfoContextMenu.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuOpenInfo
            // 
            this.toolStripMenuOpenInfo.Name = "toolStripMenuOpenInfo";
            this.toolStripMenuOpenInfo.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuOpenInfo.Text = "打开链接";
            this.toolStripMenuOpenInfo.Click += new System.EventHandler(this.toolStripMenuOpenInfo_Click);
            // 
            // MainFrm
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1360, 654);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.mainMenuStrip);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mainMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(400, 600);
            this.Name = "MainFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "YPT";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.SizeChanged += new System.EventHandler(this.MainFrm_SizeChanged);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabSearch.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelTorrent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTorrent)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.toolSearchPanel.ResumeLayout(false);
            this.innerPanel1.ResumeLayout(false);
            this.innerPanel1.PerformLayout();
            this.innerPanel2.ResumeLayout(false);
            this.innerPanel2.PerformLayout();
            this.innerPanel3.ResumeLayout(false);
            this.innerPanel3.PerformLayout();
            this.innerPanel4.ResumeLayout(false);
            this.innerPanel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.innerPanel5.ResumeLayout(false);
            this.tabPersonInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonInfo)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.logPanel.ResumeLayout(false);
            this.nfyContextMenu.ResumeLayout(false);
            this.dgvTorrentContextMenu.ResumeLayout(false);
            this.dgvInfoContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabLog;
        private YURichTextBox rtbLog;
        private System.Windows.Forms.ToolStripMenuItem 登录ToolStripMenuItem;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.Panel logPanel;
        private System.Windows.Forms.NotifyIcon nfyMain;
        private System.Windows.Forms.ContextMenuStrip nfyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuQuit;
        private System.Windows.Forms.TabPage tabSearch;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelTorrent;
        private System.Windows.Forms.DataGridView dgvTorrent;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Panel panelSite;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ContextMenuStrip dgvTorrentContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpenTorrent;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuDown;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuIDownAndOpen;
        private System.Windows.Forms.TabPage tabPersonInfo;
        private System.Windows.Forms.DataGridView dgvPersonInfo;
        private System.Windows.Forms.ToolStripMenuItem 同步StripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.Label lblAlive;
        private System.Windows.Forms.Label lblPromotion;
        private YUComboBox cmbAlive;
        private YUComboBox cmbPromotion;
        private System.Windows.Forms.Label lblFav;
        private YUComboBox cmbFav;
        private System.Windows.Forms.ContextMenuStrip dgvInfoContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpenInfo;
        private System.Windows.Forms.FlowLayoutPanel toolSearchPanel;
        private System.Windows.Forms.Panel innerPanel1;
        private System.Windows.Forms.Panel innerPanel2;
        private System.Windows.Forms.Panel innerPanel3;
        private System.Windows.Forms.Panel innerPanel4;
        private System.Windows.Forms.CheckBox cbIsSearchTiming;
        private System.Windows.Forms.CheckBox cbIsPostSiteOrder;
        private System.Windows.Forms.Panel innerPanel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox cbIsIngoreTop;
        private System.Windows.Forms.CheckBox cbIsLastSort;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空CookieToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 签到ToolStripMenuItem;
    }
}

