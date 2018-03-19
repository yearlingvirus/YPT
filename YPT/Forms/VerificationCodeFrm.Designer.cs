
namespace YPT.Forms
{
    partial class VerificationCodeFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerificationCodeFrm));
            this.picCode = new System.Windows.Forms.PictureBox();
            this.panelInput = new System.Windows.Forms.Panel();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.picPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picCode)).BeginInit();
            this.panelInput.SuspendLayout();
            this.picPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // picCode
            // 
            this.picCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCode.Location = new System.Drawing.Point(0, 0);
            this.picCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picCode.Name = "picCode";
            this.picCode.Size = new System.Drawing.Size(213, 85);
            this.picCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCode.TabIndex = 0;
            this.picCode.TabStop = false;
            // 
            // panelInput
            // 
            this.panelInput.Controls.Add(this.btnConfirm);
            this.panelInput.Controls.Add(this.txtCode);
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInput.Location = new System.Drawing.Point(0, 85);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(213, 83);
            this.panelInput.TabIndex = 2;
            // 
            // btnConfirm
            // 
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Location = new System.Drawing.Point(12, 43);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(189, 32);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "确定";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(12, 11);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(189, 26);
            this.txtCode.TabIndex = 2;
            // 
            // picPanel
            // 
            this.picPanel.Controls.Add(this.picCode);
            this.picPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPanel.Location = new System.Drawing.Point(0, 0);
            this.picPanel.Name = "picPanel";
            this.picPanel.Size = new System.Drawing.Size(213, 85);
            this.picPanel.TabIndex = 3;
            // 
            // VerificationCodeFrm
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(213, 168);
            this.Controls.Add(this.picPanel);
            this.Controls.Add(this.panelInput);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "VerificationCodeFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "验证码";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picCode)).EndInit();
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.picPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picCode;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel picPanel;
    }
}