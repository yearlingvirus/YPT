using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class InputFrm : Form
    {
        /// <summary>
        /// 默认文本
        /// </summary>
        public string DefaultText { get; set; }

        /// <summary>
        /// 返回文本
        /// </summary>
        public string ReturnText { get; set; }

        public InputFrm()
        {
            InitializeComponent();
        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {
            ReturnText = rtbInput.Text;
            this.DialogResult = DialogResult.OK;
        }


    }
}
