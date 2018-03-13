using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YU.Core.DataEntity
{
    [Serializable]
    public class PTUser
    {
        public string Fid { get; set; }

        public PTSite Site { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }

        public string Mail { get; set; }

        public int SecurityQuestionOrder { get; set; }

        public string SecuityAnswer { get; set; }

        public bool isEnableTwo_StepVerification { get; set; }

        public PTUser()
        {
            SecurityQuestionOrder = -1;
            Site = new PTSite();
        }
    }
}
