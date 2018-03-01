using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YU.Core.YUComponent
{
    public partial class YUComboBox : ComboBox
    {
        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        public YUComboBox()
        {
            InitializeComponent();
        }

        public YUComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
