using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YU.Core.Event;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class VerificationCodeFrm : Form
    {

        public OnVerificationCodeEventArgs El;

        /// <summary>
        /// 输入的验证码字符
        /// </summary>
        public string VerificationCodeKey { get; set; }

        public VerificationCodeFrm(OnVerificationCodeEventArgs e)
        {
            El = e;
                 InitializeComponent();
            if (El != null)
            {
                if (El.Image != null)
                    picCode.Image = El.Image;
                else if (!El.VerificationCodeUrl.IsNullOrEmptyOrWhiteSpace())
                    picCode.Image = ImageUtils.ImageFromWebTest(El.VerificationCodeUrl, El.Cookie);
                if (El.Site != null)
                    this.Text = El.Site.Name + "验证码";
                if (!El.Code.IsNullOrEmptyOrWhiteSpace())
                    this.txtCode.Text = El.Code;
            }

        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtCode.Text.IsNullOrEmptyOrWhiteSpace())
            {
                FormUtils.ShowErrMessage("请输入验证码。");
            }
            else
            {
                VerificationCodeKey = txtCode.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

    }
}
