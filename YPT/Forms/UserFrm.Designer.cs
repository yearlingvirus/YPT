using YU.Core.YUComponent;

namespace YPT.Forms
{
    partial class UserFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserFrm));
            this.btnGetCookie = new System.Windows.Forms.Button();
            this.grUserInfo = new System.Windows.Forms.GroupBox();
            this.lblMail = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.lblSite = new System.Windows.Forms.Label();
            this.cmbSite = new YU.Core.YUComponent.YUComboBox(this.components);
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblPassWord = new System.Windows.Forms.Label();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.grOther = new System.Windows.Forms.GroupBox();
            this.lblTip = new System.Windows.Forms.Label();
            this.nudOrder = new System.Windows.Forms.NumericUpDown();
            this.cbTwo_StepVerification = new System.Windows.Forms.CheckBox();
            this.lblSecurityQuestionOrder = new System.Windows.Forms.Label();
            this.txtAnswer = new System.Windows.Forms.TextBox();
            this.lblSecurityQuestion = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.rtbInput = new YU.Core.YUComponent.YURichTextBox(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.grCookie = new System.Windows.Forms.GroupBox();
            this.btnPanel = new System.Windows.Forms.Panel();
            this.cbIsContinue = new System.Windows.Forms.CheckBox();
            this.grUserInfo.SuspendLayout();
            this.grOther.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrder)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.grCookie.SuspendLayout();
            this.btnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetCookie
            // 
            this.btnGetCookie.Location = new System.Drawing.Point(188, 18);
            this.btnGetCookie.Name = "btnGetCookie";
            this.btnGetCookie.Size = new System.Drawing.Size(136, 30);
            this.btnGetCookie.TabIndex = 10;
            this.btnGetCookie.Text = "免密获取Cookie";
            this.btnGetCookie.UseVisualStyleBackColor = true;
            this.btnGetCookie.Click += new System.EventHandler(this.btnGetCookie_Click);
            // 
            // grUserInfo
            // 
            this.grUserInfo.Controls.Add(this.lblMail);
            this.grUserInfo.Controls.Add(this.txtMail);
            this.grUserInfo.Controls.Add(this.lblSite);
            this.grUserInfo.Controls.Add(this.cmbSite);
            this.grUserInfo.Controls.Add(this.lblUserName);
            this.grUserInfo.Controls.Add(this.lblPassWord);
            this.grUserInfo.Controls.Add(this.txtPassWord);
            this.grUserInfo.Controls.Add(this.txtUserName);
            this.grUserInfo.Location = new System.Drawing.Point(13, 13);
            this.grUserInfo.Name = "grUserInfo";
            this.grUserInfo.Size = new System.Drawing.Size(428, 177);
            this.grUserInfo.TabIndex = 12;
            this.grUserInfo.TabStop = false;
            this.grUserInfo.Text = "用户信息";
            // 
            // lblMail
            // 
            this.lblMail.AutoSize = true;
            this.lblMail.Location = new System.Drawing.Point(28, 70);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(37, 20);
            this.lblMail.TabIndex = 6;
            this.lblMail.Text = "邮箱";
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(93, 67);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(318, 26);
            this.txtMail.TabIndex = 3;
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(25, 31);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(40, 20);
            this.lblSite.TabIndex = 5;
            this.lblSite.Text = "PT站";
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Items.AddRange(new object[] {
            "TTG",
            "MTEAM",
            "CHDBITS",
            "OURBITS",
            "KEEPFRDS"});
            this.cmbSite.Location = new System.Drawing.Point(93, 27);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(318, 28);
            this.cmbSite.TabIndex = 0;
            this.cmbSite.SelectedIndexChanged += new System.EventHandler(this.cmbSite_SelectedIndexChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(14, 106);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(51, 20);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "用户名";
            // 
            // lblPassWord
            // 
            this.lblPassWord.AutoSize = true;
            this.lblPassWord.Location = new System.Drawing.Point(28, 144);
            this.lblPassWord.Name = "lblPassWord";
            this.lblPassWord.Size = new System.Drawing.Size(37, 20);
            this.lblPassWord.TabIndex = 2;
            this.lblPassWord.Text = "密码";
            // 
            // txtPassWord
            // 
            this.txtPassWord.Location = new System.Drawing.Point(93, 141);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '●';
            this.txtPassWord.Size = new System.Drawing.Size(318, 26);
            this.txtPassWord.TabIndex = 5;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(93, 103);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(318, 26);
            this.txtUserName.TabIndex = 4;
            // 
            // grOther
            // 
            this.grOther.Controls.Add(this.lblTip);
            this.grOther.Controls.Add(this.nudOrder);
            this.grOther.Controls.Add(this.cbTwo_StepVerification);
            this.grOther.Controls.Add(this.lblSecurityQuestionOrder);
            this.grOther.Controls.Add(this.txtAnswer);
            this.grOther.Controls.Add(this.lblSecurityQuestion);
            this.grOther.Location = new System.Drawing.Point(13, 196);
            this.grOther.Name = "grOther";
            this.grOther.Size = new System.Drawing.Size(428, 184);
            this.grOther.TabIndex = 11;
            this.grOther.TabStop = false;
            this.grOther.Text = "登录信息";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTip.Location = new System.Drawing.Point(266, 52);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(138, 16);
            this.lblTip.TabIndex = 13;
            this.lblTip.Text = "（没有启用安全提问设置-1）";
            // 
            // nudOrder
            // 
            this.nudOrder.Location = new System.Drawing.Point(93, 47);
            this.nudOrder.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.nudOrder.Name = "nudOrder";
            this.nudOrder.Size = new System.Drawing.Size(161, 26);
            this.nudOrder.TabIndex = 7;
            // 
            // cbTwo_StepVerification
            // 
            this.cbTwo_StepVerification.AutoSize = true;
            this.cbTwo_StepVerification.Location = new System.Drawing.Point(103, 142);
            this.cbTwo_StepVerification.Name = "cbTwo_StepVerification";
            this.cbTwo_StepVerification.Size = new System.Drawing.Size(112, 24);
            this.cbTwo_StepVerification.TabIndex = 9;
            this.cbTwo_StepVerification.Text = "启用两步验证";
            this.cbTwo_StepVerification.UseVisualStyleBackColor = true;
            // 
            // lblSecurityQuestionOrder
            // 
            this.lblSecurityQuestionOrder.AutoSize = true;
            this.lblSecurityQuestionOrder.Location = new System.Drawing.Point(10, 50);
            this.lblSecurityQuestionOrder.Name = "lblSecurityQuestionOrder";
            this.lblSecurityQuestionOrder.Size = new System.Drawing.Size(65, 20);
            this.lblSecurityQuestionOrder.TabIndex = 7;
            this.lblSecurityQuestionOrder.Text = "问题序号";
            // 
            // txtAnswer
            // 
            this.txtAnswer.Location = new System.Drawing.Point(93, 97);
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.Size = new System.Drawing.Size(318, 26);
            this.txtAnswer.TabIndex = 8;
            // 
            // lblSecurityQuestion
            // 
            this.lblSecurityQuestion.AutoSize = true;
            this.lblSecurityQuestion.Location = new System.Drawing.Point(38, 100);
            this.lblSecurityQuestion.Name = "lblSecurityQuestion";
            this.lblSecurityQuestion.Size = new System.Drawing.Size(37, 20);
            this.lblSecurityQuestion.TabIndex = 9;
            this.lblSecurityQuestion.Text = "答案";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(94, 18);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(88, 30);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // rtbInput
            // 
            this.rtbInput.BackColor = System.Drawing.Color.White;
            this.rtbInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInput.IsShowCursor = true;
            this.rtbInput.Location = new System.Drawing.Point(3, 22);
            this.rtbInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.rtbInput.Name = "rtbInput";
            this.rtbInput.Size = new System.Drawing.Size(422, 0);
            this.rtbInput.TabIndex = 1;
            this.rtbInput.Text = "";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.grUserInfo);
            this.flowLayoutPanel1.Controls.Add(this.grOther);
            this.flowLayoutPanel1.Controls.Add(this.grCookie);
            this.flowLayoutPanel1.Controls.Add(this.btnPanel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(453, 475);
            this.flowLayoutPanel1.TabIndex = 13;
            // 
            // grCookie
            // 
            this.grCookie.Controls.Add(this.rtbInput);
            this.grCookie.Location = new System.Drawing.Point(13, 386);
            this.grCookie.Name = "grCookie";
            this.grCookie.Size = new System.Drawing.Size(428, 0);
            this.grCookie.TabIndex = 13;
            this.grCookie.TabStop = false;
            this.grCookie.Text = "请在此处输入Cookie";
            // 
            // btnPanel
            // 
            this.btnPanel.Controls.Add(this.btnConfirm);
            this.btnPanel.Controls.Add(this.btnGetCookie);
            this.btnPanel.Controls.Add(this.cbIsContinue);
            this.btnPanel.Location = new System.Drawing.Point(13, 392);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(428, 66);
            this.btnPanel.TabIndex = 14;
            // 
            // cbIsContinue
            // 
            this.cbIsContinue.AutoSize = true;
            this.cbIsContinue.Location = new System.Drawing.Point(341, 22);
            this.cbIsContinue.Name = "cbIsContinue";
            this.cbIsContinue.Size = new System.Drawing.Size(84, 24);
            this.cbIsContinue.TabIndex = 11;
            this.cbIsContinue.Text = "连续新增";
            this.cbIsContinue.UseVisualStyleBackColor = true;
            // 
            // UserFrm
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(453, 475);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户";
            this.TopMost = true;
            this.grUserInfo.ResumeLayout(false);
            this.grUserInfo.PerformLayout();
            this.grOther.ResumeLayout(false);
            this.grOther.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOrder)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.grCookie.ResumeLayout(false);
            this.btnPanel.ResumeLayout(false);
            this.btnPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private YUComboBox cmbSite;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassWord;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox txtAnswer;
        private System.Windows.Forms.Label lblSecurityQuestion;
        private System.Windows.Forms.Label lblSecurityQuestionOrder;
        private System.Windows.Forms.GroupBox grUserInfo;
        private System.Windows.Forms.GroupBox grOther;
        private System.Windows.Forms.CheckBox cbTwo_StepVerification;
        private System.Windows.Forms.NumericUpDown nudOrder;
        private System.Windows.Forms.Label lblTip;
        private System.Windows.Forms.Button btnGetCookie;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.TextBox txtMail;
        private YURichTextBox rtbInput;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox grCookie;
        private System.Windows.Forms.Panel btnPanel;
        private System.Windows.Forms.CheckBox cbIsContinue;
    }
}