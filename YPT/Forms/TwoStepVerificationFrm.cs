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
    public partial class TwoStepVerificationFrm : Form
    {

        public OnTwoStepVerificationEventArgs El;

        /// <summary>
        /// 输入的二级验证字符
        /// </summary>
        public string TwoStepVerificationKey { get; set; }

        public TwoStepVerificationFrm(OnTwoStepVerificationEventArgs e)
        {
            El = e;
            InitializeComponent();
            if (El != null)
            {
                if (El.Site != null)
                    this.Text = El.Site.Name + "二级验证";
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
                TwoStepVerificationKey = txtCode.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
