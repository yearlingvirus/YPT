using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YU.Core.Event
{
    [Serializable]
    public  class OnPrepareDownFileEventArgs : EventArgs
    {
        public string FileName { get; set; }
    }
}
