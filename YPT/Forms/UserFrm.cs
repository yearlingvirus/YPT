﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YPT.PT;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Log;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class UserFrm : Form
    {
        public PTUser User { get; set; }

        private bool isWriteCookie = false;

        private string cookie = string.Empty;

        public UserFrm(PTUser user)
        {
            if (user == null)
                User = new PTUser();
            else
                User = user;
            InitializeComponent();

            YUEnums.PTEnum selectSiteId = User.Site != null ? User.Site.Id : 0;

            //绑定数据源，必须是属性，不能为字段
            BindingSource bs = new BindingSource();
            bs.DataSource = Global.Sites.ToDictionary(x => x.Id, x => x.Name);
            cmbSite.ValueMember = "Key";
            cmbSite.DisplayMember = "Value";
            cmbSite.DataSource = bs;

            txtUserName.DataBindings.Add("Text", User, "UserName");
            txtPassWord.DataBindings.Add("Text", User, "PassWord");
            txtMail.DataBindings.Add("Text", User, "Mail");
            txtAnswer.DataBindings.Add("Text", User, "SecuityAnswer");
            nudOrder.DataBindings.Add("Value", User, "SecurityQuestionOrder");
            cbTwo_StepVerification.DataBindings.Add("Checked", User, "isEnableTwo_StepVerification");

            if (selectSiteId != 0)
                cmbSite.SelectedValue = selectSiteId;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var pt = PTFactory.GetPT(User.Site.Id, User) as AbstractPT;
            bool isExistCookie = File.Exists(pt.GetCookieFilePath());
            //存在Cookie时则不要求输入密码
            if (Validation(!isExistCookie))
            {
                Save();
            }
        }

        private void cmbSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            var site = Global.Sites.Where(x => (int)x.Id == (int)cmbSite.SelectedValue).FirstOrDefault();
            if (site != null)
                User.Site = site;
        }

        private void btnGetCookie_Click(object sender, EventArgs e)
        {
            if (Validation(false))
            {
                System.Diagnostics.Process.Start(User.Site.Url);
                System.Threading.Thread.Sleep(2000);
                SendKeys.SendWait("{F12}");
                System.Threading.Thread.Sleep(1000);
                SendKeys.SendWait("{F5}");
                System.Threading.Thread.Sleep(1000);

                InputFrm frm = new InputFrm();
                frm.Text = "在此窗口输入浏览器中返回的Cookie";
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (!frm.ReturnText.IsNullOrEmptyOrWhiteSpace())
                    {
                        cookie = frm.ReturnText;
                        isWriteCookie = true;
                        Save();
                    }
                }
            }
        }

        private void Save()
        {
            var pt = PTFactory.GetPT(User.Site.Id, User) as AbstractPT;
            try
            {
                if (AppService.UpdateOrInsertUser(User) <= 0)
                {
                    FormUtils.ShowErrMessage("很抱歉，由于未知原因保存失败。");
                }
                else
                {
                    if (isWriteCookie)
                        YUUtils.WriteCookiesToDisk(pt.GetCookieFilePath(), cookie);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                string errMsg = ex.GetInnerExceptionMessage();
                FormUtils.ShowErrMessage(string.Format("保存失败，失败原因：{0}", errMsg));
                Logger.Error(string.Format("用户[{0}]保存失败。", User.UserName), ex);
            }
        }

        private bool Validation(bool isNeedPwd)
        {
            List<string> fields = new List<string>();
            if (cmbSite.SelectedValue.TryPareValue(0) <= 0)
            {
                fields.Add("[站点]");
            }
            if (User.Site != null && User.Site.IsLoginByMail && txtMail.Text.IsNullOrEmptyOrWhiteSpace())
            {
                fields.Add("[邮箱]");
            }
            if (txtUserName.Text.IsNullOrEmptyOrWhiteSpace())
            {
                fields.Add("[用户名]");
            }
            if (txtPassWord.Text.IsNullOrEmptyOrWhiteSpace() && isNeedPwd)
            {
                fields.Add("[密码]");
            }
            if (fields.Count > 0)
            {
                FormUtils.ShowErrMessage(string.Join(",", fields) + "为必录项。");
                return false;
            }
            return true;
        }
    }
}
