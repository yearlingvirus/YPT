namespace YPT.Forms
{
    partial class SettingFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingFrm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUser = new System.Windows.Forms.TabPage();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tabSign = new System.Windows.Forms.TabPage();
            this.cbAutoSign = new System.Windows.Forms.CheckBox();
            this.lblSignTime = new System.Windows.Forms.Label();
            this.dtpSignTime = new System.Windows.Forms.DateTimePicker();
            this.tabOther = new System.Windows.Forms.TabPage();
            this.cbIsSyncTiming = new System.Windows.Forms.CheckBox();
            this.cbIsEnablePostFileName = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabUser.SuspendLayout();
            this.tabSign.SuspendLayout();
            this.tabOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabUser);
            this.tabControl.Controls.Add(this.tabSign);
            this.tabControl.Controls.Add(this.tabOther);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(434, 574);
            this.tabControl.TabIndex = 0;
            this.tabControl.TabStop = false;
            // 
            // tabUser
            // 
            this.tabUser.BackColor = System.Drawing.Color.White;
            this.tabUser.Controls.Add(this.mainPanel);
            this.tabUser.Controls.Add(this.btnAdd);
            this.tabUser.Location = new System.Drawing.Point(4, 29);
            this.tabUser.Name = "tabUser";
            this.tabUser.Padding = new System.Windows.Forms.Padding(3);
            this.tabUser.Size = new System.Drawing.Size(426, 541);
            this.tabUser.TabIndex = 0;
            this.tabUser.Text = "用户";
            // 
            // mainPanel
            // 
            this.mainPanel.AutoScroll = true;
            this.mainPanel.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(3, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(420, 489);
            this.mainPanel.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Location = new System.Drawing.Point(3, 492);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(420, 46);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.TabStop = false;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tabSign
            // 
            this.tabSign.Controls.Add(this.cbAutoSign);
            this.tabSign.Controls.Add(this.lblSignTime);
            this.tabSign.Controls.Add(this.dtpSignTime);
            this.tabSign.Location = new System.Drawing.Point(4, 29);
            this.tabSign.Name = "tabSign";
            this.tabSign.Padding = new System.Windows.Forms.Padding(3);
            this.tabSign.Size = new System.Drawing.Size(426, 541);
            this.tabSign.TabIndex = 1;
            this.tabSign.Text = "签到";
            this.tabSign.UseVisualStyleBackColor = true;
            // 
            // cbAutoSign
            // 
            this.cbAutoSign.AutoSize = true;
            this.cbAutoSign.Location = new System.Drawing.Point(25, 69);
            this.cbAutoSign.Name = "cbAutoSign";
            this.cbAutoSign.Size = new System.Drawing.Size(112, 24);
            this.cbAutoSign.TabIndex = 2;
            this.cbAutoSign.Text = "启用自动签到";
            this.cbAutoSign.UseVisualStyleBackColor = true;
            this.cbAutoSign.CheckedChanged += new System.EventHandler(this.cbAutoSign_CheckedChanged);
            // 
            // lblSignTime
            // 
            this.lblSignTime.AutoSize = true;
            this.lblSignTime.Location = new System.Drawing.Point(25, 30);
            this.lblSignTime.Name = "lblSignTime";
            this.lblSignTime.Size = new System.Drawing.Size(93, 20);
            this.lblSignTime.TabIndex = 1;
            this.lblSignTime.Text = "每日签到时间";
            // 
            // dtpSignTime
            // 
            this.dtpSignTime.CustomFormat = "HH:mm";
            this.dtpSignTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSignTime.Location = new System.Drawing.Point(134, 27);
            this.dtpSignTime.Name = "dtpSignTime";
            this.dtpSignTime.ShowUpDown = true;
            this.dtpSignTime.Size = new System.Drawing.Size(161, 26);
            this.dtpSignTime.TabIndex = 0;
            this.dtpSignTime.Value = new System.DateTime(2018, 2, 20, 0, 0, 0, 0);
            this.dtpSignTime.ValueChanged += new System.EventHandler(this.dtpSignTime_ValueChanged);
            // 
            // tabOther
            // 
            this.tabOther.Controls.Add(this.cbIsSyncTiming);
            this.tabOther.Controls.Add(this.cbIsEnablePostFileName);
            this.tabOther.Location = new System.Drawing.Point(4, 29);
            this.tabOther.Name = "tabOther";
            this.tabOther.Padding = new System.Windows.Forms.Padding(3);
            this.tabOther.Size = new System.Drawing.Size(426, 541);
            this.tabOther.TabIndex = 2;
            this.tabOther.Text = "其他";
            this.tabOther.UseVisualStyleBackColor = true;
            // 
            // cbIsSyncTiming
            // 
            this.cbIsSyncTiming.AutoSize = true;
            this.cbIsSyncTiming.Location = new System.Drawing.Point(36, 63);
            this.cbIsSyncTiming.Name = "cbIsSyncTiming";
            this.cbIsSyncTiming.Size = new System.Drawing.Size(168, 24);
            this.cbIsSyncTiming.TabIndex = 4;
            this.cbIsSyncTiming.Text = "准点自动同步个人信息";
            this.cbIsSyncTiming.UseVisualStyleBackColor = true;
            this.cbIsSyncTiming.CheckedChanged += new System.EventHandler(this.cbIsSyncTiming_CheckedChanged);
            // 
            // cbIsEnablePostFileName
            // 
            this.cbIsEnablePostFileName.AutoSize = true;
            this.cbIsEnablePostFileName.Location = new System.Drawing.Point(36, 26);
            this.cbIsEnablePostFileName.Name = "cbIsEnablePostFileName";
            this.cbIsEnablePostFileName.Size = new System.Drawing.Size(196, 24);
            this.cbIsEnablePostFileName.TabIndex = 0;
            this.cbIsEnablePostFileName.Text = "启用请求服务器下载文件名";
            this.cbIsEnablePostFileName.UseVisualStyleBackColor = true;
            this.cbIsEnablePostFileName.CheckedChanged += new System.EventHandler(this.cbIsEnablePostFileName_CheckedChanged);
            // 
            // SettingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 574);
            this.Controls.Add(this.tabControl);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 540);
            this.Name = "SettingFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.tabControl.ResumeLayout(false);
            this.tabUser.ResumeLayout(false);
            this.tabSign.ResumeLayout(false);
            this.tabSign.PerformLayout();
            this.tabOther.ResumeLayout(false);
            this.tabOther.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUser;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.TabPage tabSign;
        private System.Windows.Forms.DateTimePicker dtpSignTime;
        private System.Windows.Forms.Label lblSignTime;
        private System.Windows.Forms.CheckBox cbAutoSign;
        private System.Windows.Forms.TabPage tabOther;
        private System.Windows.Forms.CheckBox cbIsEnablePostFileName;
        private System.Windows.Forms.CheckBox cbIsSyncTiming;
    }
}