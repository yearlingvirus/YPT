using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YPT.Forms;
using YPT.PT;
using YU.Core;
using YU.Core.DataEntity;
using YU.Core.Event;
using YU.Core.Log;
using YU.Core.Utils;
using YU.Core.YUComponent;

namespace YPT
{
    public partial class MainFrm : Form
    {

        #region 变量

        private System.Timers.Timer SignTimer { get; set; }

        private System.Timers.Timer SyncTimer { get; set; }

        private System.Timers.Timer WriteDBTimer { get; set; }

        private System.Timers.Timer SearchTimer { get; set; }

        /// <summary>
        /// 进度条控件
        /// </summary>
        YUTransparentPanel progressPanel;

        /// <summary>
        /// 上一次排序的列
        /// </summary>
        private KeyValuePair<string, YUEnums.SortOrderType> LastSearchColKvr { get; set; }

        private readonly static object syncFillSearchObject = new object();

        private readonly static object syncInfoObject = new object();

        /// <summary>
        /// 是否正在搜索中
        /// </summary>
        private bool IsSearching { get; set; }


        #endregion

        public MainFrm()
        {
            InitializeComponent();
            this.Text = string.Format("YPT {0}", YUUtils.GetVersion());

            cbIsPostSiteOrder.Checked = Global.Config.IsPostSiteOrder;
            cbIsIngoreTop.Checked = Global.Config.IsIngoreTop;
            cbIsLastSort.Checked = Global.Config.IsLastSort;
            cbIsSearchTiming.Checked = Global.Config.IsSearchTiming;
            FormUtils.InitDataGridView(dgvTorrent);
            FormUtils.CreateDataGridColumns(dgvTorrent, typeof(PTTorrentGridEntity));
            FormUtils.InitDataGridView(dgvPersonInfo);
            FormUtils.CreateDataGridColumns(dgvPersonInfo, typeof(PTInfoGridEntity));
            InitSign(this, null);
            InitSync(this, null);
            InitTorrentSite(this, null);
            InitTorrentCmb();

        }

        #region 签到，登录

        private void InitSign(object sender, EventArgs e)
        {
            if (Global.Config.IsAutoSign)
            {
                if (SignTimer == null)
                    SignTimer = new System.Timers.Timer();
                SignTimer.Enabled = false;
                SignTimer.Elapsed -= SignTimer_Elapsed;
                DateTime now = DateTime.Now;
                TimeSpan span = Global.Config.SignTime.TimeOfDay - now.TimeOfDay;
                if (now.TimeOfDay > Global.Config.SignTime.TimeOfDay)
                    span = span.Add(new TimeSpan(1, 0, 0, 0));
                SignTimer.Interval = span.TotalMilliseconds;
                SignTimer.Elapsed += SignTimer_Elapsed;
                SignTimer.Enabled = true;
                SignTimer.AutoReset = true;
                SignTimer.Start();

            }
            else if (SignTimer != null)
            {
                SignTimer.Enabled = false;
                SignTimer.Elapsed -= SignTimer_Elapsed;
            }
        }

        private void SignTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SignTimer.Interval = 24 * 60 * 60 * 1000;
            Sign();
        }

        private string Pt_VerificationCode(object sender, YU.Core.Event.OnVerificationCodeEventArgs e)
        {
            string imgUrl = e.VerificationCodeUrl;
            var pt = sender as AbstractPT;
            if (!imgUrl.IsNullOrEmptyOrWhiteSpace())
            {
                VerificationCodeFrm frm = new VerificationCodeFrm(e);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    return frm.VerificationCodeKey;
                }
            }
            return string.Empty;
        }


        private string Pt_TwoStepVerification(object sender, OnTwoStepVerificationEventArgs e)
        {
            var pt = sender as AbstractPT;
            TwoStepVerificationFrm frm = new TwoStepVerificationFrm(e);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                return frm.TwoStepVerificationKey;
            }
            return string.Empty;
        }


        private void Login()
        {
            if (Global.Users != null && Global.Users.Count > 0)
            {
                LogMessage(null, "正在启动登录。");
                List<KeyValuePair<string, SQLiteParameter[]>> sqlList = new List<KeyValuePair<string, SQLiteParameter[]>>();

                foreach (var user in Global.Users)
                {
                    bool isNeedUpdate = user.Id <= 0;
                    AbstractPT pt = PTFactory.GetPT(user.Site.Id, user) as AbstractPT;
                    pt.VerificationCode += Pt_VerificationCode;
                    pt.TwoStepVerification += Pt_TwoStepVerification;
                    string msg = pt.Login();

                    if (isNeedUpdate && pt.User != null && pt.User.Id > 0)
                    {
                        user.Id = pt.User.Id;
                        string sql = "UPDATE USER SET USERID = @USERID WHERE PTSITEID = @PTSITEID AND USERNAME = @USERNAME ";
                        SQLiteParameter[] parms = new SQLiteParameter[]
                        {
                            new SQLiteParameter("@USERID", DbType.Int32),
                            new SQLiteParameter("@PTSITEID", DbType.Int32),
                            new SQLiteParameter("@USERNAME", DbType.String),
                        };
                        parms[0].Value = pt.User.Id;
                        parms[1].Value = pt.User.Site.Id;
                        parms[2].Value = pt.User.UserName;
                        sqlList.Add(new KeyValuePair<string, SQLiteParameter[]>(sql, parms));
                    }
                    LogMessage(user.Site, msg);
                }
                if (sqlList.Count > 0)
                    DBUtils.ExecuteNonQueryBatch(sqlList);
                LogMessage(null, "全部登录完毕。");
            }
            else
            {
                LogMessage(null, "请先添加用户。");
            }
        }

        private void Sign()
        {
            if (Global.Users != null && Global.Users.Count > 0)
            {
                LogMessage(null, "正在启动签到。");
                foreach (var user in Global.Users)
                {
                    IPT pt = PTFactory.GetPT(user.Site.Id, user);
                    string msg = pt.Sign();
                    LogMessage(user.Site, msg);
                }
                LogMessage(null, "全部签到完毕。");
            }
            else
            {
                LogMessage(null, "请先添加用户。");
            }
        }

        #endregion

        #region 日志

        private void LogMessage(PTSite site, string msg, bool isOpenLogTab = false)
        {
            StringBuilder sb = new StringBuilder();
            msg = string.Format("{0} {1}", site == null ? "" : site.Name, msg.TrimEnd());
            sb.AppendLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), msg.TrimEnd()));
            this.Invoke(new Action<string>(x =>
            {
                if (rtbLog.Text.Length > 10000)
                    rtbLog.Clear();
                rtbLog.AppendText(x);
                if (isOpenLogTab)
                    tabMain.SelectTab("tabLog");
                Logger.Info(msg);
            }), sb.ToString());
        }


        private void rtbLog_TextChanged(object sender, EventArgs e)
        {
            rtbLog.SelectionStart = rtbLog.Text.Length; //Set the current caret position at the end
            rtbLog.ScrollToCaret(); //Now scroll it automatically
        }


        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLog.Clear();
        }

        #endregion

        #region 窗体事件

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabSearch)
                this.AcceptButton = btnSearch;
            else
                this.AcceptButton = null;
        }


        private void MainFrm_Load(object sender, EventArgs e)
        {
            progressPanel = new YUTransparentPanel(this);
            this.Controls.Add(progressPanel);
        }

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingFrm frm = new SettingFrm(this);
            frm.SignChanged += InitSign;
            frm.UserChanged += InitTorrentSite;
            frm.SyncChanged += InitSync;
            frm.ShowDialog();
        }

        private void 登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabMain.SelectTab("tabLog");
            Task t = Task.Factory.StartNew(() => Login());
        }

        private void 签到ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabMain.SelectTab("tabLog");
            Task t = Task.Factory.StartNew(() => Sign());
        }


        private void 同步StripMenuItem_Click(object sender, EventArgs e)
        {
            SyncPersonInfo();
        }

        private void MainFrm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)  //判断是否最小化
            {
                nfyMain.Visible = true;  //托盘图标可见
            }
        }

        private void toolStripMenuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void nfyMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            else if (e.Button == MouseButtons.Left)
            {
                this.ShowInTaskbar = true;  //显示在系统任务栏
                this.Show();
                this.BringToFront();
                this.WindowState = FormWindowState.Normal;  //还原窗体
            }
        }

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 注意判断关闭事件reason来源于窗体按钮，否则用菜单退出时无法退出!
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (Global.Config.IsFirstOpen)
                {
                    Global.Config.IsFirstOpen = false;
                    if (MessageBox.Show("是否最小化到系统托盘?", "温馨提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Global.Config.IsMiniWhenClose = true;
                    }
                    else
                    {
                        Global.Config.IsMiniWhenClose = false;
                    }
                    Global.SetConfig(YUConst.CONFIG_ISFIRSTOPEN, Global.Config.IsFirstOpen);
                    Global.SetConfig(YUConst.CONFIG_ISMINIWHENCLOSE, Global.Config.IsMiniWhenClose);
                }
                if (Global.Config.IsMiniWhenClose)
                {
                    e.Cancel = true;    //取消"关闭窗口"事件
                    this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
                    this.nfyMain.Visible = true;
                    this.Hide();
                    return;
                }
            }
        }


        #endregion

        #region  种子

        private void InitTorrentCmb()
        {

            BindingSource bs = new BindingSource();
            bs.DataSource = EnumUtils.GetEnumAllKeyDescs<YUEnums.PromotionType>();
            cmbPromotion.ValueMember = "Key";
            cmbPromotion.DisplayMember = "Value";
            cmbPromotion.DataSource = bs;


            bs = new BindingSource();
            bs.DataSource = EnumUtils.GetEnumAllKeyDescs<YUEnums.AliveType>();
            cmbAlive.ValueMember = "Key";
            cmbAlive.DisplayMember = "Value";
            cmbAlive.DataSource = bs;
            cmbAlive.SelectedValue = (int)YUEnums.AliveType.Alive;


            bs = new BindingSource();
            bs.DataSource = EnumUtils.GetEnumAllKeyDescs<YUEnums.FavType>();
            cmbFav.ValueMember = "Key";
            cmbFav.DisplayMember = "Value";
            cmbFav.DataSource = bs;
        }

        /// <summary>
        /// 初始化种子搜索站点
        /// </summary>
        private void InitTorrentSite(object sender, OnUserChangeEventArgs e)
        {
            panelSite.Controls.Clear();
            if (Global.Users != null && Global.Users.Count > 0)
            {
                for (int i = 0; i < Global.Users.Count; i++)
                {
                    var user = Global.Users[i];
                    CheckBox cb = new CheckBox();
                    cb.AutoSize = true;
                    cb.Size = new Size(100, 25);
                    cb.Location = new Point(20, i * cb.Size.Height + 10);
                    cb.Name = "cb" + user.Site.Name;
                    cb.TabIndex = i;
                    cb.Text = user.Site.Name;
                    cb.UseVisualStyleBackColor = true;
                    cb.Tag = user.Site.Id;
                    cb.CheckedChanged += Cb_CheckedChanged;
                    panelSite.Controls.Add(cb);
                }

                //追加全选按钮
                if (panelSite.Controls.Count > 0)
                {
                    CheckBox cb = new CheckBox();
                    cb.AutoSize = true;
                    cb.Size = new Size(100, 25);
                    cb.Location = new Point(20, panelSite.Controls.Count * cb.Size.Height + 10);
                    cb.Name = "cbSelect";
                    cb.TabIndex = panelSite.Controls.Count;
                    cb.Text = "全选";
                    cb.Tag = false;
                    cb.UseVisualStyleBackColor = true;
                    cb.CheckedChanged += CbSelect_CheckedChanged;
                    panelSite.Controls.Add(cb);
                }

                JObject o = new JObject();
                string selectSiteJson = Global.GetConfig<string>(YUConst.CONFIG_SEARCHSITEHISTORY);
                if (!selectSiteJson.IsNullOrEmptyOrWhiteSpace())
                {
                    o = JsonConvert.DeserializeObject<JObject>(selectSiteJson);
                    foreach (var item in panelSite.Controls)
                    {
                        if (item is CheckBox)
                        {
                            var cb = item as CheckBox;
                            if (o.ContainsKey(cb.Name))
                                cb.Checked = o[cb.Name].TryPareValue(false);
                        }
                    }
                }

            }
        }

        private void CbSelect_CheckedChanged(object sender, EventArgs e)
        {
            bool isSelectAll = (sender as CheckBox).Tag.TryPareValue<bool>();
            isSelectAll = !isSelectAll;
            foreach (var item in panelSite.Controls)
            {
                if (item is CheckBox && !item.Equals(sender))
                    (item as CheckBox).Checked = isSelectAll;
            }
            (sender as CheckBox).Tag = isSelectAll;
            if (isSelectAll)
                (sender as CheckBox).Text = "全不选";
            else
                (sender as CheckBox).Text = "全选";
        }

        private void Cb_CheckedChanged(object sender, EventArgs e)
        {
            //这里延迟写入
            if (WriteDBTimer != null)
            {
                WriteDBTimer.Stop();
                WriteDBTimer.Interval = 1000;
                WriteDBTimer.Start();
            }
            else
            {
                WriteDBTimer = new System.Timers.Timer();
                WriteDBTimer.Elapsed += WriteDBTimer_Elapsed;
                WriteDBTimer.Interval = 1000;
                WriteDBTimer.AutoReset = false;
                WriteDBTimer.Start();
            }
        }

        private void WriteDBTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            JObject o = new JObject();
            foreach (var item in panelSite.Controls)
            {
                if (item is CheckBox)
                {
                    var cb = item as CheckBox;
                    if (!cb.Name.EqualIgnoreCase("cbSelect"))
                    {
                        o[cb.Name] = cb.Checked;
                    }
                }
            }
            Global.SetConfig(YUConst.CONFIG_SEARCHSITEHISTORY, JsonConvert.SerializeObject(o));
        }

        private void panelSite_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                                        panelSite.ClientRectangle,
                                        Color.LightGray,
                                        0,
                                        ButtonBorderStyle.None,
                                        Color.LightGray,
                                        0,
                                        ButtonBorderStyle.None,
                                        Color.LightGray,
                                        1,
                                        ButtonBorderStyle.Solid,
                                        Color.LightGray,
                                        0,
                                        ButtonBorderStyle.None);
        }

        private void panelSearch_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics,
                                       panelSearch.ClientRectangle,
                                       Color.LightGray,
                                       0,
                                       ButtonBorderStyle.None,
                                       Color.LightGray,
                                       0,
                                       ButtonBorderStyle.None,
                                       Color.LightGray,
                                       0,
                                       ButtonBorderStyle.None,
                                       Color.LightGray,
                                       1,
                                       ButtonBorderStyle.Solid);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (SearchTimer != null && Global.Config.IsSearchTiming)
                SearchTimer.Interval = Global.Config.SearchTimeSpan * 1000;

            if (IsSearching)
                return;

            List<PTSite> searchSites = new List<PTSite>();
            foreach (var control in panelSite.Controls)
            {
                if (control is CheckBox)
                {
                    var cb = (control as CheckBox);
                    if (cb.Checked && cb.Tag is YUEnums.PTEnum)
                    {
                        YUEnums.PTEnum siteId = (YUEnums.PTEnum)cb.Tag;
                        var site = Global.Sites.Where(x => x.Id == siteId).FirstOrDefault();
                        if (site != null && !searchSites.Contains(site))
                            searchSites.Add(site);
                    }
                }
            }
            if (searchSites.Count > 0)
                this.Invoke(new Action(() =>
                {
                    SearchTorrent(searchSites, txtSearch.Text);
                }));
            else
                LogMessage(null, "请选择站点。", true);

        }

        private void SearchTorrent(List<PTSite> searchSites, string searchKey)
        {
            if (searchSites != null && searchSites.Count > 0)
            {
                IsSearching = true;
                bool isFill = false;
                List<PTTorrent> searchTorrents = new List<PTTorrent>();
                StringBuilder sb = new StringBuilder();
                var cts = new CancellationTokenSource();

                PTSearchArgs args = new PTSearchArgs();
                args.PromotionType = (YUEnums.PromotionType)cmbPromotion.SelectedValue;
                args.AliveType = (YUEnums.AliveType)cmbAlive.SelectedValue;
                args.FavType = (YUEnums.FavType)cmbFav.SelectedValue;
                args.SearchKey = searchKey;
                args.IsPostSiteOrder = Global.Config.IsPostSiteOrder;
                args.SortKvr = LastSearchColKvr;
                args.IsIngoreTop = Global.Config.IsIngoreTop;

                Task task = new Task(() =>
                {
                    Parallel.ForEach(searchSites.Distinct(), (site, state, i) =>
                    {
                        //即便日后支持站点多用户，这里也只会取第一个用户。
                        AbstractPT pt = PTFactory.GetPT(site.Id, Global.Users.Where(x => x.Site.Id == site.Id).FirstOrDefault()) as AbstractPT;
                        //假设4核Cpu，5个任务，因为是并行的原因，如果前4个在并行执行的过程任意一个发生了异常，那么此时前4个中其他3个还会继续执行到结束，但最后一个是不会执行的，所以这里需要做异常捕获处理。
                        try
                        {
                            List<PTTorrent> torrents = pt.SearchTorrent(args);
                            lock (syncFillSearchObject)
                            {
                                if (torrents != null && torrents.Count > 0)
                                    searchTorrents.AddRange(torrents);
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (syncFillSearchObject)
                            {
                                Logger.Error(string.Format("{0} 搜索过程中发生错误。", site.Name), ex);
                                sb.AppendLine(ex.GetInnerExceptionMessage());
                            }
                        }

                    });
                }, cts.Token);
                progressPanel.BeginLoading(20000);
                System.Timers.Timer canelTimer = new System.Timers.Timer(20000);
                canelTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => OnTimedEvent(s, e, cts));
                canelTimer.AutoReset = false;
                canelTimer.Start();
                task.Start();
                cts.Token.Register(new Action(() =>
                {
                    if (!isFill)
                    {
                        FillTorrent(searchTorrents, ref isFill);
                        bool isOpen = searchTorrents.Count <= 0;
                        if (cts.Token.IsCancellationRequested)
                            LogMessage(null, "搜索过程出现错误，错误原因：超时。", isOpen);
                    }
                }));
                task.ContinueWith(result =>
                {
                    if (!cts.Token.IsCancellationRequested && !isFill)
                        FillTorrent(searchTorrents, ref isFill);
                    string errMsg = sb.ToString();
                    if (!errMsg.IsNullOrEmptyOrWhiteSpace())
                    {
                        bool isOpen = searchTorrents.Count <= 0;
                        LogMessage(null, errMsg.TrimEnd(), isOpen);
                    }
                    IsSearching = false;
                });
            }
        }

        private void FillTorrent(List<PTTorrent> searchTorrents, ref bool isFill)
        {
            isFill = true;
            progressPanel.StopLoading();

            if (searchTorrents != null && searchTorrents.Count > 0)
            {
                List<PTTorrentGridEntity> entitys = new List<PTTorrentGridEntity>();
                searchTorrents.ToList().ForEach(x => entitys.Add(x.ToGridEntity()));

                if (Global.Config.IsLastSort && !LastSearchColKvr.Key.IsNullOrEmptyOrWhiteSpace())
                {
                    this.Invoke(new Action(() =>
                    {
                        ReSort(dgvTorrent, LastSearchColKvr.Key, (SortOrder)(int)LastSearchColKvr.Value, entitys);
                        dgvTorrent.Tag = searchTorrents;
                    }));
                }
                else
                {
                    this.Invoke(new Action(() =>
                    {
                        dgvTorrent.DataSource = entitys;
                        //ReSort(dgvTorrent, "UpLoadTime", SortOrder.Descending, entitys);
                        dgvTorrent.Tag = searchTorrents;
                    }));
                }
            }
        }

        private void dgvTorrent_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvTorrent.Rows[e.RowIndex].Selected == false)
                    {
                        dgvTorrent.ClearSelection();
                        dgvTorrent.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvTorrent.SelectedRows.Count == 1)
                    {
                        dgvTorrent.CurrentCell = dgvTorrent.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    dgvTorrentContextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void toolStripMenuOpenTorrent_Click(object sender, EventArgs e)
        {
            if (dgvTorrent.SelectedRows != null && dgvTorrent.SelectedRows.Count > 0)
            {
                string url = dgvTorrent.SelectedRows[0].Cells["LinkUrl"].Value.TryPareValue<string>();
                System.Diagnostics.Process.Start(url);
            }
        }

        private void toolStripMenuDown_Click(object sender, EventArgs e)
        {
            DownLoadFiles();
        }

        private void toolStripMenuIDownAndOpen_Click(object sender, EventArgs e)
        {
            DownLoadFiles(true);
        }

        private void DownLoadFiles(bool isOepn = false)
        {
            if (dgvTorrent.SelectedRows != null && dgvTorrent.SelectedRows.Count > 0)
            {
                var searchTorrents = dgvTorrent.Tag as List<PTTorrent>;
                DataGridViewCellCollection cells = dgvTorrent.SelectedRows[0].Cells;
                string torrentId = cells["Id"].Value.TryPareValue<string>();

                //YUEnums.PTEnum siteId = (YUEnums.PTEnum)EnumUtils.GetKeyByValue<YUEnums.PTEnum>(cells["SiteId"].Value.TryPareValue<string>());
                int siteId = cells["SiteId"].Value.TryPareValue<int>();
                if (searchTorrents != null && searchTorrents.Count > 0)
                {
                    var torrent = searchTorrents.Where(x => x.Id == torrentId && (int)x.SiteId == siteId).FirstOrDefault();
                    if (torrent != null)
                    {
                        Task t = Task.Factory.StartNew(() =>
                        {
                            AbstractPT pt = PTFactory.GetPT((YUEnums.PTEnum)siteId, Global.Users.Where(x => (int)x.Site.Id == siteId).FirstOrDefault()) as AbstractPT;
                            pt.PrepareDownFile += Pt_PrepareDownFile;
                            pt.DownTorrent(torrent, isOepn, Global.Config.IsEnablePostFileName);
                        });
                        TaskCallBack(t, "下载种子过程中出现错误，错误原因：");
                    }
                }

                else
                {
                    LogMessage(Global.Sites.Where(x => (int)x.Id == siteId).FirstOrDefault(), string.Format("下载失败，失败原因：获取种子信息失败。"), true);
                }
            }
        }

        private string Pt_PrepareDownFile(object sender, OnPrepareDownFileEventArgs e)
        {
            string fileName = string.Empty;
            this.Invoke(new Action(() =>
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = e.FileName;
                sfd.Filter = "TORRENT 文件|*.torrent;";
                if (sfd.ShowDialog() == DialogResult.OK)
                    fileName = sfd.FileName;
            }));
            return fileName;
        }

        private void TaskCallBack(Task t, string errTitle)
        {
            t.ContinueWith(result =>
            {
                if (t.Exception != null)
                {
                    LogMessage(null, errTitle + t.Exception.GetInnerExceptionMessage(), true);
                }
            });
        }

        private void cbIsPostSiteOrder_CheckedChanged(object sender, EventArgs e)
        {
            Global.Config.IsPostSiteOrder = (sender as CheckBox).Checked;
            if (Global.Config.IsPostSiteOrder)
                cbIsLastSort.Checked = true;
            Global.SetConfig(YUConst.CONFIG_SEARCH_POSTSITEORDER, Global.Config.IsPostSiteOrder);
        }


        private void cbIsLastSort_CheckedChanged(object sender, EventArgs e)
        {
            Global.Config.IsLastSort = (sender as CheckBox).Checked;
            if (!Global.Config.IsLastSort)
                cbIsPostSiteOrder.Checked = false;
            Global.SetConfig(YUConst.CONFIG_SEARCH_ISLASTSORT, Global.Config.IsLastSort);
        }

        private void cbIsIngoreTop_CheckedChanged(object sender, EventArgs e)
        {
            Global.Config.IsIngoreTop = (sender as CheckBox).Checked;
            Global.SetConfig(YUConst.CONFIG_SEARCH_INGORETOP, Global.Config.IsIngoreTop);
        }

        private void cbIsSearchTiming_CheckedChanged(object sender, EventArgs e)
        {
            Global.Config.IsSearchTiming = (sender as CheckBox).Checked;
            if (Global.Config.IsSearchTiming)
            {
                if (SearchTimer == null)
                    SearchTimer = new System.Timers.Timer();
                SearchTimer.Elapsed -= SearchTimer_Elapsed;
                SearchTimer.Enabled = true;
                SearchTimer.AutoReset = true;
                SearchTimer.Interval = Global.Config.SearchTimeSpan * 1000;
                SearchTimer.Elapsed += SearchTimer_Elapsed;
                SearchTimer.Start();
            }
            else
            {
                if (SearchTimer != null)
                {
                    SearchTimer.Enabled = false;
                    SearchTimer.Stop();
                }
            }
        }

        private void SearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            btnSearch_Click(sender, e);
        }

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            DataGridViewCellCollection cells = dgvTorrent.SelectedRows[0].Cells;
            Clipboard.SetDataObject(cells["LinkUrl"].Value.TryPareValue<string>());
        }


        #endregion

        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var dgv = (sender as DataGridView);
            dgv.TopLeftHeaderCell.Value = "序号";
            for (int i = 1; i <= dgv.Rows.Count; i++)
            {
                dgv.Rows[i - 1].HeaderCell.Value = i.ToString();
            }
        }

        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            //这里重新获取列名，比如Size，获取到的应该是RealSize
            string colName = dgv.Columns[e.ColumnIndex].DataPropertyName;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.DataPropertyName.Contains(colName) && !col.DataPropertyName.EqualIgnoreCase(colName))
                {
                    colName = col.DataPropertyName;
                    break;
                }
            }
            if (dgv.Columns[e.ColumnIndex].SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                switch (dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection)
                {
                    case SortOrder.None:
                    case SortOrder.Ascending:
                        {
                            if (dgv.Name == "dgvTorrent")
                                ReSort(dgv, colName, SortOrder.Descending, dgv.DataSource as IEnumerable<PTTorrentGridEntity>);
                            else
                                ReSort(dgv, colName, SortOrder.Descending, dgv.DataSource as IEnumerable<PTInfoGridEntity>);
                        }
                        dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Descending;
                        break;
                    case SortOrder.Descending:
                        {
                            if (dgv.Name == "dgvTorrent")
                                ReSort(dgv, colName, SortOrder.Ascending, dgv.DataSource as IEnumerable<PTTorrentGridEntity>);
                            else
                                ReSort(dgv, colName, SortOrder.Ascending, dgv.DataSource as IEnumerable<PTInfoGridEntity>);
                        }
                        dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = SortOrder.Ascending;
                        break;
                }
            }
            if (dgv.Name == "dgvTorrent")
                LastSearchColKvr = new KeyValuePair<string, YUEnums.SortOrderType>(dgv.Columns[e.ColumnIndex].DataPropertyName, (YUEnums.SortOrderType)(int)dgv.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection);
        }

        /// <summary>
        /// 重新排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colName"></param>
        /// <param name="sortMode"></param>
        /// <param name="dataSource"></param>
        private void ReSort<T>(DataGridView dgv, string colName, SortOrder sortMode, IEnumerable<T> dataSource)
        {
            if (dataSource == null || dataSource.Count() <= 0)
                return;

            var entityType = typeof(T);
            PropertyInfo propertyInfo = entityType.GetProperty(colName);
            try
            {
                switch (sortMode)
                {
                    case SortOrder.Ascending:
                        dgv.DataSource = dataSource.OrderBy(x => Convert.ChangeType(propertyInfo.GetValue(x, null), propertyInfo.PropertyType)).ToList();
                        break;
                    case SortOrder.Descending:
                        dgv.DataSource = dataSource.OrderByDescending(x => Convert.ChangeType(propertyInfo.GetValue(x, null),
                            propertyInfo.PropertyType)).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                dgv.DataSource = dataSource;
                Logger.Error(colName + "排序失败", ex);
            }
        }

        #region 个人信息

        private void InitSync(object sender, EventArgs e)
        {
            if (Global.Config.IsSyncTiming)
            {
                if (SyncTimer == null)
                    SyncTimer = new System.Timers.Timer();
                SyncTimer.Enabled = false;
                SyncTimer.Elapsed -= SyncTimer_Elapsed;
                DateTime now = DateTime.Now;
                SyncTimer.Interval = (DateTime.Now.AddHours(1).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second) - DateTime.Now).TotalMilliseconds;
                SyncTimer.Elapsed += SyncTimer_Elapsed;
                SyncTimer.Enabled = true;
                SyncTimer.AutoReset = true;
                SyncTimer.Start();

            }
            else if (SyncTimer != null)
            {
                SyncTimer.Enabled = false;
                SyncTimer.Elapsed -= SyncTimer_Elapsed;
            }
        }

        private void SyncTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SyncTimer.Interval = 60 * 60 * 1000;
            SyncPersonInfo(true);
        }

        private void SyncPersonInfo(bool isBack = false)
        {
            if (!isBack)
            {
                this.Invoke(new Action(() =>
                {
                    tabMain.SelectTab("tabPersonInfo");
                }));
            }
            if (Global.Users != null || Global.Users.Count > 0)
            {
                bool isFill = false;
                StringBuilder sb = new StringBuilder();
                var cts = new CancellationTokenSource();

                Task task = new Task(() =>
                {
                    List<PTInfo> infos = new List<PTInfo>();
                    Parallel.ForEach(Global.Users, (user, state, i) =>
                    {
                        IPT pt = PTFactory.GetPT(user.Site.Id, Global.Users.Where(x => x.Site.Id == user.Site.Id).FirstOrDefault());
                        //IPT pt = PTFactory.GetPT(YUEnums.PTEnum.MTEAM, Global.Users.Where(x => x.Site.Id == YUEnums.PTEnum.MTEAM).FirstOrDefault());
                        //假设4核Cpu，5个任务，因为是并行的原因，如果前4个在并行执行的过程任意一个发生了异常，那么此时前4个中其他3个还会继续执行到结束，但最后一个是不会执行的，所以这里需要做异常捕获处理。
                        try
                        {
                            infos.Add(pt.GetPersonInfo());
                            lock (syncInfoObject)
                            {
                                FillPersonInfo(infos, ref isFill);
                            }
                        }
                        catch (Exception ex)
                        {
                            lock (syncInfoObject)
                                sb.AppendLine(ex.GetInnerExceptionMessage());
                        }

                    });
                }, cts.Token);
                if (!isBack)
                    progressPanel.BeginLoading(20000);
                LogMessage(null, "正在同步个人信息。");
                System.Timers.Timer canelTimer = new System.Timers.Timer(20000);
                canelTimer.Elapsed += new System.Timers.ElapsedEventHandler((s, e) => OnTimedEvent(s, e, cts));
                canelTimer.AutoReset = false;
                canelTimer.Start();
                task.Start();
                task.ContinueWith(result =>
                {
                    if (!isBack)
                        this.Invoke(new Action(() =>
                        {
                            progressPanel.StopLoading();
                        }));

                    bool isOpen = !isBack && !isFill;
                    if (cts.Token.IsCancellationRequested)
                        LogMessage(null, "同步过程出现错误，错误原因：超时。", isOpen);
                    else
                    {
                        string errMsg = sb.ToString();
                        if (!errMsg.IsNullOrEmptyOrWhiteSpace())
                        {
                            LogMessage(null, errMsg, isOpen);
                        }
                    }
                    LogMessage(null, "同步个人信息完成。");
                });
            }
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e, CancellationTokenSource cts)
        {
            cts.Cancel();
        }

        /// <summary>
        /// 填充个人信息
        /// </summary>
        /// <param name="infos"></param>
        private void FillPersonInfo(List<PTInfo> infos, ref bool isFill)
        {
            isFill = true;
            if (infos != null && infos.Count > 0)
            {
                List<PTInfoGridEntity> entitys = new List<PTInfoGridEntity>();
                foreach (var info in infos)
                {
                    try
                    {
                        entitys.Add(info.ToGridEntity());
                    }
                    catch (Exception ex)
                    {
                        LogMessage(null, string.Format("{0} {1} 请检查用户信息映射。", info.SiteName, ex.GetInnerExceptionMessage()));
                    }
                }

                entitys = entitys.OrderByDescending(x => x.UpSize).ToList();
                this.Invoke(new Action(() =>
                {
                    dgvPersonInfo.Tag = infos;
                    dgvPersonInfo.DataSource = entitys;
                }));
            }
        }

        private void dgvPersonInfo_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvPersonInfo.Rows[e.RowIndex].Selected == false)
                    {
                        dgvPersonInfo.ClearSelection();
                        dgvPersonInfo.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvPersonInfo.SelectedRows.Count == 1)
                    {
                        dgvPersonInfo.CurrentCell = dgvPersonInfo.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    dgvInfoContextMenu.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void toolStripMenuOpenInfo_Click(object sender, EventArgs e)
        {
            if (dgvPersonInfo.SelectedRows != null && dgvPersonInfo.SelectedRows.Count > 0)
            {
                string url = dgvPersonInfo.SelectedRows[0].Cells["Url"].Value.TryPareValue<string>();
                System.Diagnostics.Process.Start(url);
            }
        }

        #endregion

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutFrm frm = new AboutFrm();
            frm.ShowDialog();
        }

    }
}
