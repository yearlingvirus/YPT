using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core;

namespace YPT.PT
{
    public class PTFactory
    {
        private static Dictionary<YUEnums.PTEnum, string> _mapServer = new Dictionary<YUEnums.PTEnum, string>();

        private static bool notRegistered = true;

        private static object thisLock = new object();

        public static void RegisterPT()
        {

            _mapServer = new Dictionary<YUEnums.PTEnum, string>();

            if (notRegistered)
            {
                _mapServer.Add(YUEnums.PTEnum.CHDBITS, "YPT.PT.CHDBITS,YPT");
                _mapServer.Add(YUEnums.PTEnum.KEEPFRDS, "YPT.PT.KEEPFRDS,YPT");
                _mapServer.Add(YUEnums.PTEnum.MTEAM, "YPT.PT.MTEAM,YPT");
                _mapServer.Add(YUEnums.PTEnum.OURBITS, "YPT.PT.OURBITS,YPT");
                _mapServer.Add(YUEnums.PTEnum.TTG, "YPT.PT.TTG,YPT");
                _mapServer.Add(YUEnums.PTEnum.BTSCHOOL, "YPT.PT.BTSCHOOL,YPT");
                _mapServer.Add(YUEnums.PTEnum.GZTOWN, "YPT.PT.GZTOWN,YPT");
                _mapServer.Add(YUEnums.PTEnum.HDU, "YPT.PT.HDU,YPT");
                notRegistered = false;
            }

        }

        public static IPT GetPT(YUEnums.PTEnum type, params object[] args)
        {
            lock (thisLock)
            {
                if (notRegistered)
                {
                    RegisterPT();
                }
            }
            IPT instance = null;
            if (_mapServer.ContainsKey(type))
            {
                instance = TypesContainer.CreateInstance<IPT>(_mapServer[type], args);
            }
            else
            {
                var argList = args.ToList();
                argList.Add(type);
                instance = TypesContainer.CreateInstance<IPT>("YPT.PT.ExtendPT,YPT", argList.ToArray());
            }
            if (instance == null)
                throw new Exception("instance==null");
            return instance;
        }
    }
}
