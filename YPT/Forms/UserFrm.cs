using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YU.PT;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class UserFrm : Form
    {
        public PTUser User { get; set; }

        /// <summary>
        /// 用户发生变更
        /// </summary>
        public event EventHandler<OnUserChangeEventArgs> UserChanged;

        public UserFrm(PTUser user)
        {
            if (user == null)
                User = new PTUser();
            else
                User = ObjectUtils.CreateCopy<PTUser>(user);
            InitializeComponent();

            YUEnums.PTEnum selectSiteId = User.Site != null ? User.Site.Id : 0;

            //绑定数据源，必须是属性，不能为字段
            BindingSource bs = new BindingSource();
            bs.DataSource = ObjectUtils.CreateCopy<Dictionary<YUEnums.PTEnum, string>>(Global.Sites.ToDictionary(x => x.Id, x => x.Name));
            cmbSite.ValueMember = "Key";
            cmbSite.DisplayMember = "Value";
            cmbSite.DataSource = bs;

            if (selectSiteId != 0)
                cmbSite.SelectedValue = selectSiteId;

            ReBindControl();

            var pt = PTFactory.GetPT(User.Site.Id, User) as AbstractPT;
            if (File.Exists(pt.GetCookieFilePath()))
                rtbInput.Text = File.ReadAllText(pt.GetCookieFilePath());
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            //存在Cookie时则不要求输入任何信息
            if (Validation(rtbInput.Text.IsNullOrEmptyOrWhiteSpace()))
            {
                var result = Save();
                if (result)
                {
                    if (cbIsContinue.Checked)
                        FormUtils.ShowInfoMessage("保存成功，你可以选择其他站点继续添加。");
                    else
                        this.DialogResult = DialogResult.OK;
                    OnUserChangeEventArgs el = new OnUserChangeEventArgs();
                    el.User = User;
                    OnUserChanged(el);
                }
            }
        }

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            var user = Global.Users.Where(x => (int)x.Site.Id == (int)cmbSite.SelectedValue).FirstOrDefault();
            if (user != null)
            {
                User = ObjectUtils.CreateCopy<PTUser>(user);
                ReBindControl();
            }
            else
            {
                var site = Global.Sites.Where(x => (int)x.Id == (int)cmbSite.SelectedValue).FirstOrDefault();
                if (site != null)
                    User.Site = ObjectUtils.CreateCopy<PTSite>(site);
            }

            var pt = PTFactory.GetPT(User.Site.Id, User) as AbstractPT;
            if (File.Exists(pt.GetCookieFilePath()))
                rtbInput.Text = File.ReadAllText(pt.GetCookieFilePath());
            else
                rtbInput.Text = string.Empty;
        }

        private void ReBindControl()
        {
            txtUserName.DataBindings.Clear();
            txtPassWord.DataBindings.Clear();
            txtMail.DataBindings.Clear();
            txtAnswer.DataBindings.Clear();
            nudOrder.DataBindings.Clear();
            cbTwo_StepVerification.DataBindings.Clear();
            txtUserName.DataBindings.Add("Text", User, "UserName");
            txtPassWord.DataBindings.Add("Text", User, "PassWord");
            txtMail.DataBindings.Add("Text", User, "Mail");
            txtAnswer.DataBindings.Add("Text", User, "SecuityAnswer");
            nudOrder.DataBindings.Add("Value", User, "SecurityQuestionOrder");
            cbTwo_StepVerification.DataBindings.Add("Checked", User, "isEnableTwo_StepVerification");
        }

        private void btnGetCookie_Click(object sender, EventArgs e)
        {
            if (grCookie.Height <= 0)
            {
                grCookie.Visible = true;
                grCookie.Height = 180;
                this.Location = new Point(this.Location.X, this.Location.Y - 90);
            }

            System.Diagnostics.Process.Start("explorer.exe", User.Site.Url);
            System.Threading.Thread.Sleep(500);
            SendKeys.SendWait("{F12}");
            System.Threading.Thread.Sleep(500);
            SendKeys.SendWait("{F5}");
        }

        private bool Save()
        {
            var pt = PTFactory.GetPT(User.Site.Id, User) as AbstractPT;
            try
            {
                if (AppService.UpdateOrInsertUser(User) <= 0)
                {
                    FormUtils.ShowErrMessage("很抱歉，由于未知原因保存失败。");
                    return false;
                }
                else
                {
                    if (!rtbInput.Text.IsNullOrEmptyOrWhiteSpace())
                        YUUtils.WriteCookiesToDisk(pt.GetCookieFilePath(), rtbInput.Text);
                    return true;
                }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                string errMsg = ex.GetInnerExceptionMessage();
                FormUtils.ShowErrMessage(string.Format("保存失败，失败原因：{0}", errMsg));
                Logger.Error(string.Format("用户[{0}]保存失败。", User.UserName), ex);
                return false;
            }
        }

        private bool Validation(bool isNeed)
        {
            List<string> fields = new List<string>();
            if (cmbSite.SelectedValue.TryPareValue(0) <= 0)
            {
                fields.Add("[站点]");
            }
            if (isNeed)
            {
                if (User.Site != null && User.Site.IsLoginByMail && txtMail.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    fields.Add("[邮箱]");
                }
                if (txtUserName.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    fields.Add("[用户名]");
                }
                if (txtPassWord.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    fields.Add("[密码]");
                }
            }
            if (fields.Count > 0)
            {
                FormUtils.ShowErrMessage(string.Join(",", fields) + "为必录项。");
                return false;
            }
            return true;
        }

        private void OnUserChanged(OnUserChangeEventArgs e)
        {
            if (e.User != null)
            {
                if (UserChanged != null)
                    UserChanged.Invoke(this, e);
            }
        }
    }
}
