using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core.DataEntity;

namespace YU.Core.Event
{
    [Serializable]
    public  class OnUserChangeEventArgs
    {
        public PTUser User { get; set; }
    }
}
