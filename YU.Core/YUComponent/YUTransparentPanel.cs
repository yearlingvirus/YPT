using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YU.Core.YUComponent
{
    public partial class YUTransparentPanel : Panel
    {

        private ProgressBar progressBar;

        private System.Timers.Timer timer;

        private static object syncObject = new object();

        private bool isLoading = false;


        public YUTransparentPanel(Form frm)
        {
            this.Size = frm.ClientSize;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.Opaque, true);
            this.BackColor =  Color.FromArgb(80, 255, 255, 255) ;
            this.Dock = DockStyle.Fill;
            //frm.SizeChanged += Frm_SizeChanged;
            timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Elapsed += _timer_Elapsed;
            InitializeComponent();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopLoading();
        }

        private void Frm_SizeChanged(object sender, EventArgs e)
        {
            if (progressBar != null)
            {
                int x = (int)(0.5 * (this.Width - progressBar.Width));
                int y = (int)(0.5 * (this.Height - progressBar.Height));
                progressBar.Location = new Point(x, y);
            }
        }

        public void BeginLoading(double timeOut = 0)
        {
            lock (syncObject)
            {
                if (!isLoading)
                {
                    this.Invoke(new Action(() =>
                    {
                        if (progressBar == null)
                        {
                            progressBar = new ProgressBar();
                            progressBar.MarqueeAnimationSpeed = 10;
                            progressBar.MinimumSize = new Size(150, 23);
                            progressBar.Size = new Size(150, 23);
                            //int x = (int)(0.5 * (this.Width - progressBar.Width));
                            //int y = (int)(0.5 * (this.Height - progressBar.Height));
                            progressBar.Dock = DockStyle.Bottom;
                            //progressBar.Location = new Point(x, y);
                            progressBar.Style = ProgressBarStyle.Marquee;
                            //progressBar.Anchor = AnchorStyles.Top;
                            this.Controls.Add(progressBar);
                        }
 
                        if (timeOut != 0)
                        {
                            timer.Interval = timeOut;
                            timer.Start();
                        }
                        this.Cursor = Cursors.WaitCursor;
                        progressBar.BringToFront();
                        this.BringToFront();
                        isLoading = true;
                    }));
                }
            }
        }

        public void StopLoading()
        {
            lock (syncObject)
            {
                if (isLoading)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Cursor = Cursors.Default;
                        this.SendToBack();
                        if (progressBar != null)
                        {
                            progressBar.Dispose();
                            progressBar = null;
                            this.Controls.Remove(progressBar);
                        }
                        isLoading = false;
                    }));
                }
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x20;
                return cp;
            }
        }
    }
}
