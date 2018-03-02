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

            List<string> fields = new List<string>();
            if (cmbSite.SelectedValue.TryPareValue(0) <= 0)
            {
                fields.Add("[站点]");
            }
            if (txtUserName.Text.IsNullOrEmptyOrWhiteSpace())
            {
                fields.Add("[用户名]");
            }
            if (txtPassWord.Text.IsNullOrEmptyOrWhiteSpace() && !isWriteCookie && !isExistCookie)
            {
                fields.Add("[密码]");
            }
            if (fields.Count > 0)
            {
                FormUtils.ShowErrMessage(string.Join(",", fields) + "为必录项。");
            }
            else
            {
                try
                {
                    string selectSql = " SELECT PTSITEID FROM USER WHERE PTSITEID = @PTSITEID ";
                    SQLiteParameter param = new SQLiteParameter("@PTSITEID", DbType.Int32);
                    param.Value = User.Site.Id;
                    string sql = string.Empty;
                    if (DBUtils.ExecuteScalar<int>(selectSql, -1, param) != (int)User.Site.Id)
                        sql = @" INSERT INTO USER(PTSITEID,USERNAME,PASSWORD,SECURITYQUESTIONORDER,SECUITYANSWER,ISENABLETWO_STEPVERIFICATION) VALUES(@PTSITEID,@USERNAME,@PASSWORD,@SECURITYQUESTIONORDER,@SECUITYANSWER,@ISENABLETWO_STEPVERIFICATION) ";
                    else
                        sql = @" UPDATE USER SET USERNAME = @USERNAME, PASSWORD = @PASSWORD, SECURITYQUESTIONORDER = @SECURITYQUESTIONORDER, SECUITYANSWER = @SECUITYANSWER , ISENABLETWO_STEPVERIFICATION = @ISENABLETWO_STEPVERIFICATION WHERE PTSITEID = @PTSITEID";


                    SQLiteParameter[] parms = new SQLiteParameter[]
                    {
                        new SQLiteParameter("@PTSITEID", DbType.Int32),
                        new SQLiteParameter("@USERNAME", DbType.String),
                        new SQLiteParameter("@PASSWORD", DbType.String),
                        new SQLiteParameter("@SECURITYQUESTIONORDER", DbType.Int32),
                        new SQLiteParameter("@SECUITYANSWER", DbType.String),
                        new SQLiteParameter("@ISENABLETWO_STEPVERIFICATION", DbType.Boolean),
                    };
                    parms[0].Value = User.Site.Id;
                    parms[1].Value = User.UserName;
                    parms[2].Value = User.PassWord;
                    parms[3].Value = User.SecurityQuestionOrder;
                    parms[4].Value = User.SecuityAnswer;
                    parms[5].Value = User.isEnableTwo_StepVerification;

                    if (DBUtils.ExecuteNonQuery(sql, parms) <= 0)
                    {
                        FormUtils.ShowErrMessage("很抱歉，由于未知原因保存失败。");
                    }
                    else
                    {
                        if (isWriteCookie)
                        {
                            HttpUtils.WriteCookiesToDisk(pt.GetCookieFilePath(), cookie);
                        }
                        else
                        {
                            if (isExistCookie)
                            {
                                if (MessageBox.Show(string.Format("账户发生变更，是否删除[{0}][{1}]对应的Cookie?", User.Site.Name, User.UserName), "温馨提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        (pt as AbstractPT).DelLocalCookie();
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Error(string.Format("{0} 用户名：{1} 删除Cookie出错。", User.Site.Name, User.UserName), ex);
                                    }
                                }
                            }
                        }
                        this.DialogResult = DialogResult.OK;
                    }
                }
                catch (SQLiteException ex)
                {
                    string errMsg = ex.GetInnerExceptionMessage();
                    FormUtils.ShowErrMessage(string.Format("保存失败，失败原因：{0}", errMsg));
                    Logger.Error(string.Format("用户[{0}]保存失败。", User.UserName), ex);
                }

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
            this.ImeMode = ImeMode.Off;
            System.Diagnostics.Process.Start(User.Site.Url);
            System.Threading.Thread.Sleep(2000);
            SendKeys.SendWait("{F12}");
            System.Threading.Thread.Sleep(1000);
            SendKeys.SendWait("document.cookie");
            SendKeys.SendWait("{ENTER}");
            SendKeys.SendWait("{ENTER}");
            System.Threading.Thread.Sleep(1000);


            InputFrm frm = new InputFrm();
            frm.Text = "在此窗口输入浏览器中返回的Cookie";
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (!frm.ReturnText.IsNullOrEmptyOrWhiteSpace())
                {
                    cookie = frm.ReturnText;
                    isWriteCookie = true;
                }
            }

        }
    }
}
