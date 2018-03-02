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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelTorrent = new System.Windows.Forms.Panel();
            this.dgvTorrent = new System.Windows.Forms.DataGridView();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblFav = new System.Windows.Forms.Label();
            this.cmbFav = new YU.Core.YUComponent.YUComboBox(this.components);
            this.lblAlive = new System.Windows.Forms.Label();
            this.lblPromotion = new System.Windows.Forms.Label();
            this.cmbAlive = new YU.Core.YUComponent.YUComboBox(this.components);
            this.cmbPromotion = new YU.Core.YUComponent.YUComboBox(this.components);
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
            this.dgvContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuIDownAndOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenuStrip.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelTorrent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTorrent)).BeginInit();
            this.panelSearch.SuspendLayout();
            this.tabPersonInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonInfo)).BeginInit();
            this.tabLog.SuspendLayout();
            this.logPanel.SuspendLayout();
            this.nfyContextMenu.SuspendLayout();
            this.dgvContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.同步StripMenuItem,
            this.登录ToolStripMenuItem,
            this.签到ToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(7, 3, 0, 3);
            this.mainMenuStrip.Size = new System.Drawing.Size(1092, 27);
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
            this.tabMain.Size = new System.Drawing.Size(1092, 631);
            this.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMain.TabIndex = 1;
            this.tabMain.TabStop = false;
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.panelMain);
            this.tabSearch.Controls.Add(this.panelSite);
            this.tabSearch.Location = new System.Drawing.Point(4, 29);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Padding = new System.Windows.Forms.Padding(3);
            this.tabSearch.Size = new System.Drawing.Size(1084, 598);
            this.tabSearch.TabIndex = 1;
            this.tabSearch.Text = "种子搜索";
            this.tabSearch.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelTorrent);
            this.panelMain.Controls.Add(this.panelSearch);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(139, 3);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(942, 592);
            this.panelMain.TabIndex = 1;
            // 
            // panelTorrent
            // 
            this.panelTorrent.Controls.Add(this.dgvTorrent);
            this.panelTorrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTorrent.Location = new System.Drawing.Point(0, 92);
            this.panelTorrent.Name = "panelTorrent";
            this.panelTorrent.Size = new System.Drawing.Size(942, 500);
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
            this.dgvTorrent.Size = new System.Drawing.Size(942, 500);
            this.dgvTorrent.TabIndex = 0;
            this.dgvTorrent.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTorrent_CellMouseDown);
            this.dgvTorrent.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_ColumnHeaderMouseClick);
            this.dgvTorrent.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.lblFav);
            this.panelSearch.Controls.Add(this.cmbFav);
            this.panelSearch.Controls.Add(this.lblAlive);
            this.panelSearch.Controls.Add(this.lblPromotion);
            this.panelSearch.Controls.Add(this.cmbAlive);
            this.panelSearch.Controls.Add(this.cmbPromotion);
            this.panelSearch.Controls.Add(this.btnSearch);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(942, 92);
            this.panelSearch.TabIndex = 0;
            this.panelSearch.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSearch_Paint);
            // 
            // lblFav
            // 
            this.lblFav.AutoSize = true;
            this.lblFav.Location = new System.Drawing.Point(557, 55);
            this.lblFav.Name = "lblFav";
            this.lblFav.Size = new System.Drawing.Size(65, 20);
            this.lblFav.TabIndex = 7;
            this.lblFav.Text = "显示收藏";
            // 
            // cmbFav
            // 
            this.cmbFav.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFav.FormattingEnabled = true;
            this.cmbFav.Location = new System.Drawing.Point(639, 51);
            this.cmbFav.Name = "cmbFav";
            this.cmbFav.Size = new System.Drawing.Size(121, 28);
            this.cmbFav.TabIndex = 6;
            // 
            // lblAlive
            // 
            this.lblAlive.AutoSize = true;
            this.lblAlive.Location = new System.Drawing.Point(300, 55);
            this.lblAlive.Name = "lblAlive";
            this.lblAlive.Size = new System.Drawing.Size(71, 20);
            this.lblAlive.TabIndex = 5;
            this.lblAlive.Text = "断种/活种";
            // 
            // lblPromotion
            // 
            this.lblPromotion.AutoSize = true;
            this.lblPromotion.Location = new System.Drawing.Point(46, 55);
            this.lblPromotion.Name = "lblPromotion";
            this.lblPromotion.Size = new System.Drawing.Size(65, 20);
            this.lblPromotion.TabIndex = 4;
            this.lblPromotion.Text = "促销类型";
            // 
            // cmbAlive
            // 
            this.cmbAlive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlive.FormattingEnabled = true;
            this.cmbAlive.Location = new System.Drawing.Point(385, 51);
            this.cmbAlive.Name = "cmbAlive";
            this.cmbAlive.Size = new System.Drawing.Size(121, 28);
            this.cmbAlive.TabIndex = 3;
            // 
            // cmbPromotion
            // 
            this.cmbPromotion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPromotion.FormattingEnabled = true;
            this.cmbPromotion.Location = new System.Drawing.Point(128, 51);
            this.cmbPromotion.Name = "cmbPromotion";
            this.cmbPromotion.Size = new System.Drawing.Size(121, 28);
            this.cmbPromotion.TabIndex = 2;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(795, 23);
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
            this.txtSearch.Location = new System.Drawing.Point(43, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(718, 26);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // panelSite
            // 
            this.panelSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSite.Location = new System.Drawing.Point(3, 3);
            this.panelSite.Name = "panelSite";
            this.panelSite.Size = new System.Drawing.Size(136, 592);
            this.panelSite.TabIndex = 0;
            this.panelSite.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSite_Paint);
            // 
            // tabPersonInfo
            // 
            this.tabPersonInfo.Controls.Add(this.dgvPersonInfo);
            this.tabPersonInfo.Location = new System.Drawing.Point(4, 29);
            this.tabPersonInfo.Name = "tabPersonInfo";
            this.tabPersonInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPersonInfo.Size = new System.Drawing.Size(1084, 598);
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
            this.dgvPersonInfo.Size = new System.Drawing.Size(1078, 592);
            this.dgvPersonInfo.TabIndex = 1;
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
            this.tabLog.Size = new System.Drawing.Size(1084, 598);
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
            this.logPanel.Size = new System.Drawing.Size(1076, 590);
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
            this.rtbLog.Size = new System.Drawing.Size(1076, 544);
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
            this.btnClearLog.Location = new System.Drawing.Point(0, 544);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(1076, 46);
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
            // dgvContextMenu
            // 
            this.dgvContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuOpen,
            this.toolStripMenuDown,
            this.toolStripMenuIDownAndOpen,
            this.toolStripMenuItemCopy});
            this.dgvContextMenu.Name = "dgvContextMenu";
            this.dgvContextMenu.Size = new System.Drawing.Size(137, 92);
            // 
            // toolStripMenuOpen
            // 
            this.toolStripMenuOpen.Name = "toolStripMenuOpen";
            this.toolStripMenuOpen.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuOpen.Text = "打开链接";
            this.toolStripMenuOpen.Click += new System.EventHandler(this.toolStripMenuOpen_Click);
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
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 658);
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
            this.panelTorrent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTorrent)).EndInit();
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.tabPersonInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersonInfo)).EndInit();
            this.tabLog.ResumeLayout(false);
            this.logPanel.ResumeLayout(false);
            this.nfyContextMenu.ResumeLayout(false);
            this.dgvContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripMenuItem 签到ToolStripMenuItem;
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
        private System.Windows.Forms.ContextMenuStrip dgvContextMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuOpen;
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
    }
}

