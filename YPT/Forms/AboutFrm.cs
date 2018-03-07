using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using YU.Core.Utils;

namespace YPT.Forms
{
    public partial class AboutFrm : Form
    {
        public AboutFrm()
        {
            InitializeComponent();

            DateTime now = YUUtils.GetWebSiteDateTime("https://www.baidu.com");
            string licYear = string.Empty;
            if (now.Year > 2018)
                licYear = string.Format("2018-{0}", now.Year);
            else
                licYear = "2018";
            lblVersion.Text = string.Format("版本 {0}", YUUtils.GetVersion());

            lblLicence.Text = string.Format("yearlingvirus 版权所有 {0}。保留所有权利。", licYear);
        }

        private void lblGit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/yearlingvirus/ypt/wiki/YPT%E4%BD%BF%E7%94%A8%E8%AF%B4%E6%98%8E");
        }
    }
}
