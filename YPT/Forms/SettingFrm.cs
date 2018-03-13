using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private Dictionary<Control, KeyValuePair<string, string>> SettingDict = new Dictionary<Control, KeyValuePair<string, string>>();


        public SettingFrm(Form owner)
        {
            this.Owner = owner;
            InitializeComponent();
            InitUser();

            SettingDict.Add(nudSearchTimeSpan, new KeyValuePair<string, string>(YUConst.CONFIG_SEARCH_TIMESPAN, "SearchTimeSpan"));
            SettingDict.Add(cbIsMiniWhenClose, new KeyValuePair<string, string>(YUConst.CONFIG_ISMINIWHENCLOSE, "IsMiniWhenClose"));
            SettingDict.Add(cbIsSyncTiming, new KeyValuePair<string, string>(YUConst.CONFIG_SYNC_AUTO, "IsSyncTiming"));
            SettingDict.Add(cbIsEnablePostFileName, new KeyValuePair<string, string>(YUConst.CONFIG_ENABLEPOSTFILENAME, "IsEnablePostFileName"));
            SettingDict.Add(cbAutoSign, new KeyValuePair<string, string>(YUConst.CONFIG_SIGN_AUTO, "IsAutoSign"));
            SettingDict.Add(dtpSignTime, new KeyValuePair<string, string>(YUConst.CONFIG_SIGN_TIME, "SignTime"));
            FormUtils.BindControlValue(Global.Config, SettingDict.Select(x => new KeyValuePair<Control, string>(x.Key, x.Value.Value)));
            FormUtils.BindControlDataChanged(SettingDict.Select(x => x.Key), Control_DataChanged);
            dtpSignTime.ValueChanged += OnSignChanged;
            cbAutoSign.CheckedChanged += OnSignChanged;
            cbIsSyncTiming.CheckedChanged += OnSyncChanged;
        }

        private void Control_DataChanged(object sender, EventArgs e)
        {
            object value = null;
            if (sender is CheckBox)
                value = (sender as CheckBox).Checked;
            else if (sender is DateTimePicker)
                value = (sender as DateTimePicker).Value;
            else if (sender is NumericUpDown)
                value = (sender as NumericUpDown).Value;
            else if (sender is TextBox)
                value = (sender as TextBox).Text;
            var control = sender as Control;
            if (SettingDict.ContainsKey(control))
            {
                if (!SettingDict[control].Key.IsNullOrEmptyOrWhiteSpace())
                    AppService.SetConfig(SettingDict[control].Key, value);
                Type t = Global.Config.GetType();
                var info = t.GetProperty(SettingDict[control].Value);
                info.SetValue(Global.Config, value, null);
            }
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
                if (AppService.DeleteUser(user.Site.Id) > 0)
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
                    panel.Size = new Size(mainPanel.Size.Width, 50);
                    panel.Left = 0;
                    panel.Top = i * (panel.Size.Height + 5);
                    panel.Name = "panel" + user.Site.Name;
                    panel.TabIndex = 0;

                    Label lblSite = new Label();
                    lblSite.AutoSize = true;
                    lblSite.Font = new Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lblSite.Location = new Point(30, 5);
                    lblSite.Name = "lblSite" + user.Site.Name;
                    lblSite.TabIndex = 0;
                    lblSite.Text = user.Site.Name;
                    lblSite.Size = new Size(126, 19);

                    Label lblUser = new Label();
                    lblUser.AutoSize = true;
                    lblUser.Font = new Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    lblUser.Location = new Point(30, 24);
                    lblUser.Name = "lblUser" + user.Site.Name;
                    lblUser.Text = user.UserName;
                    lblUser.TabIndex = 1;
                    lblUser.Size = new Size(126, 19);

                    Button btnDel = new Button();
                    btnDel.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
                    btnDel.Name = "btnDel" + user.Site.Name;
                    btnDel.Size = new Size(60, 40);
                    btnDel.Font = new Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btnDel.Location = new Point(panel.Width - btnDel.Width - 30, 5);
                    btnDel.TabIndex = 3;
                    btnDel.Text = "删除";
                    btnDel.UseVisualStyleBackColor = true;
                    btnDel.TabStop = false;
                    btnDel.Tag = user;
                    btnDel.Click += BtnDel_Click;

                    Button btnEdit = new Button();
                    btnEdit.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
                    btnEdit.Name = "btnEdit" + user.Site.Name;
                    btnEdit.Size = new Size(60, 40);
                    btnEdit.Font = new Font("微软雅黑", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    btnEdit.Location = new Point(panel.Width - btnEdit.Width - btnDel.Width - 40, 5);
                    btnEdit.TabIndex = 2;
                    btnEdit.Text = "编辑";
                    btnEdit.UseVisualStyleBackColor = true;
                    btnEdit.TabStop = false;
                    btnEdit.Tag = user;
                    btnEdit.Click += BtnEdit_Click;

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

        private void OnUserChanged(OnUserChangeEventArgs e)
        {
            if (e.User != null)
            {
                Global.Users = AppService.GetAllUsers(Global.Sites);
                InitUser();
                if (UserChanged != null)
                    UserChanged.Invoke(this, e);
            }
        }

        private void OnSignChanged(object sender, EventArgs e)
        {
            if (SignChanged != null)
                SignChanged.Invoke(this, e);
        }

        private void OnSyncChanged(object sender, EventArgs e)
        {
            if (SyncChanged != null)
                SyncChanged.Invoke(this, e);
        }

    }
}
