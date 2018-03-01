using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YU.Core.Utils;

namespace YU.Core.YUComponent
{

    public partial class YURichTextBox : RichTextBox
    {
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }
    
        private bool isShowCursor = true;

        /// <summary>
        /// 是否显示光标
        /// </summary>
        public bool IsShowCursor
        {
            get
            {
                return isShowCursor;
            }
            set
            {
                isShowCursor = value;
            }
        }


        public YURichTextBox()
        {
            InitializeComponent();
            this.MouseMove += new MouseEventHandler(MouseHideCaret);
            this.MouseUp += new MouseEventHandler(MouseHideCaret);
            this.MouseDown += new MouseEventHandler(MouseHideCaret);
            this.MouseClick += new MouseEventHandler(MouseHideCaret);
            this.MouseDoubleClick += new MouseEventHandler(MouseHideCaret);
            this.Enter += new System.EventHandler(MouseEnterHideCaret);
        }

        public YURichTextBox(IContainer container)
             : this()
        {
            container.Add(this);
        }


        private void MouseEnterHideCaret(object sender, EventArgs e)
        {
            SetCursor(sender as Control);
        }

        private void MouseHideCaret(object sender, MouseEventArgs e)
        {
            SetCursor(sender as Control);
        }



        private void SetCursor(Control ctrl)
        {
            if (isShowCursor)
            {
                WindowsApiUtils.ShowCaret(ctrl.Handle);
            }
            else
            {
                WindowsApiUtils.HideCaret(ctrl.Handle);
            }
        }
    }
}
