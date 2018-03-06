using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YPT.PT;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class SettingFrm : Form
    {
        /// <summary>
        /// 自动签到变更
        /// </summary>
        public event EventHandler<EventArgs> SignChanged;

        /// <summary>
        /// 用户发生变更
        /// </summary>
        public event EventHandler<OnUserChangeEventArgs> UserChanged;

        /// <summary>
        /// 准点同步发生变更
        /// </summary>
        public event EventHandler<EventArgs> SyncChanged;

        public SettingFrm(Form owner)
        {
            this.Owner = owner;
            InitializeComponent();
            InitUser();
            dtpSignTime.Value = Global.Config.SignTime;
            cbAutoSign.Checked = Global.Config.IsAutoSign;
            cbIsSyncTiming.Checked = Global.Config.IsSyncTiming;
            cbIsEnablePostFileName.Checked = Global.Config.IsEnablePostFileName;
            cbIsMiniWhenClose.Checked = Global.Config.IsMiniWhenClose;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserFrm frm = new UserFrm(null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                OnUserChangeEventArgs el = new OnUserChangeEventArgs();
                el.User = frm.User;
                OnUserChanged(el);
            }
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            PTUser user = ((sender as Control).Tag as PTUser);
            if (user == null)
                throw new Exception("获取用户数据失败。");
            else
            {
                YUEnums.PTEnum siteId = user.Site.Id;
                string delSql = "DELETE FROM USER WHERE PTSITEID = @PTSITEID";
                SQLiteParameter parm = new SQLiteParameter("@PTSITEID", DbType.Int32);
                parm.Value = (int)siteId;
                if (DBUtils.ExecuteNonQuery(delSql, parm) > 0)
                {
                    OnUserChangeEventArgs el = new OnUserChangeEventArgs();
                    el.User = user;
                    OnUserChanged(el);
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            PTUser user = ((sender as Control).Tag as PTUser);
            if (user == null)
                throw new Exception("获取用户数据失败。");
            else
            {
                UserFrm frm = new UserFrm(user);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    OnUserChangeEventArgs el = new OnUserChangeEventArgs();
                    el.User = user;
                    OnUserChanged(el);
                }
            }
        }

        private void InitUser()
        {
            mainPanel.Controls.Clear();
            if (Global.Users != null && Global.Users.Count > 0)
            {
                for (int i = 0; i < Global.Users.Count; i++)
                {
                    var user = Global.Users[i];
                    if (user.Site == null)
                        continue;

                    Panel panel = new Panel();
                    panel.BackColor = Color.LightGray;
                    panel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left)));
                    panel.Size = new Size(mainPanel.Size.Width, 100);
                    panel.Left = 0;
                    panel.Top = i * (panel.Size.Height + 10);
                    panel.Name = "panel" + user.Site.Name;
                    panel.TabIndex = 0;

                    Label lblSite = new Label();
                    lblSite.AutoSize = true;
                    lblSite.Font = new Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lblSite.Location = new Point(30, 11);
                    lblSite.Name = "lblSite" + user.Site.Name;
                    lblSite.TabIndex = 0;
                    lblSite.Text = user.Site.Name;
                    lblSite.Size = new Size(126, 38);

                    Label lblUser = new Label();
                    lblUser.AutoSize = true;
                    lblUser.Font = new Font("微软雅黑", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lblUser.Location = new Point(30, 48);
                    lblUser.Name = "lblUser" + user.Site.Name;
                    lblUser.Text = user.UserName;
                    lblUser.TabIndex = 1;
                    lblUser.Size = new Size(126, 38);

                    Button btnEdit = new Button();
                    btnEdit.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
                    btnEdit.Name = "btnEdit" + user.Site.Name;
                    btnEdit.Size = new Size(116, 39);
                    btnEdit.Location = new Point(panel.Width - btnEdit.Width - 30, 10);
                    btnEdit.TabIndex = 2;
                    btnEdit.Text = "编辑";
                    btnEdit.UseVisualStyleBackColor = true;
                    btnEdit.TabStop = false;
                    btnEdit.Tag = user;
                    btnEdit.Click += BtnEdit_Click;

                    Button btnDel = new Button();
                    btnDel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
                    btnDel.Name = "btnDel" + user.Site.Name;
                    btnDel.Size = new Size(116, 39);
                    btnDel.Location = new Point(panel.Width - btnDel.Width - 30, 55);
                    btnDel.TabIndex = 3;
                    btnDel.Text = "删除";
                    btnDel.UseVisualStyleBackColor = true;
                    btnDel.TabStop = false;
                    btnDel.Tag = user;
                    btnDel.Click += BtnDel_Click;

                    panel.Controls.Add(lblSite);
                    panel.Controls.Add(lblUser);
                    panel.Controls.Add(btnEdit);
                    panel.Controls.Add(btnDel);

                    mainPanel.Controls.Add(panel);
                }

                int defaultHeight = mainPanel.Controls.Count * 150;

                this.Size = new Size(this.Size.Width, defaultHeight > this.Owner.Size.Height ? this.Owner.Size.Height : defaultHeight);
            }
        }

        private void dtpSignTime_ValueChanged(object sender, EventArgs e)
        {
            Global.SetConfig(YUConst.CONFIG_SIGN_TIME, dtpSignTime.Value);
            Global.Config.SignTime = dtpSignTime.Value;
            OnSignChanged(e);
        }

        private void cbAutoSign_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetConfig(YUConst.CONFIG_SIGN_AUTO, cbAutoSign.Checked);
            Global.Config.IsAutoSign = cbAutoSign.Checked;
            OnSignChanged(e);
        }

        private void OnUserChanged(OnUserChangeEventArgs e)
        {
            if (e.User != null)
            {
                Global.InitUser();
                InitUser();
                if (UserChanged != null)
                    UserChanged.Invoke(this, e);
            }
        }

        private void OnSignChanged(EventArgs e)
        {
            if (SignChanged != null)
                SignChanged.Invoke(this, e);
        }

        private void cbIsEnablePostFileName_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetConfig(YUConst.CONFIG_ENABLEPOSTFILENAME, cbIsEnablePostFileName.Checked);
            Global.Config.IsEnablePostFileName = cbIsEnablePostFileName.Checked;
        }

        private void cbIsSyncTiming_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetConfig(YUConst.CONFIG_SYNC_AUTO, cbIsSyncTiming.Checked);
            Global.Config.IsSyncTiming = cbIsSyncTiming.Checked;
            OnSyncChanged(e);
        }

        private void OnSyncChanged(EventArgs e)
        {
            if (SyncChanged != null)
                SyncChanged.Invoke(this, e);
        }

        private void cbIsMiniWhenClose_CheckedChanged(object sender, EventArgs e)
        {
            Global.SetConfig(YUConst.CONFIG_ISMINIWHENCLOSE, cbIsMiniWhenClose.Checked);
            Global.Config.IsMiniWhenClose = cbIsMiniWhenClose.Checked;
        }
    }
}
