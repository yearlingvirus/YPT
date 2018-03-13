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
                _mapServer.Add(YUEnums.PTEnum.CHDBits, "YPT.PT.CHDBITS,YPT");
                _mapServer.Add(YUEnums.PTEnum.KeepFrds, "YPT.PT.KEEPFRDS,YPT");
                _mapServer.Add(YUEnums.PTEnum.MTeam, "YPT.PT.MTEAM,YPT");
                _mapServer.Add(YUEnums.PTEnum.OurBits, "YPT.PT.OURBITS,YPT");
                _mapServer.Add(YUEnums.PTEnum.TTG, "YPT.PT.TTG,YPT");
                _mapServer.Add(YUEnums.PTEnum.BTSchool, "YPT.PT.BTSCHOOL,YPT");
                _mapServer.Add(YUEnums.PTEnum.GZTown, "YPT.PT.GZTOWN,YPT");
                _mapServer.Add(YUEnums.PTEnum.HDU, "YPT.PT.HDU,YPT");
                _mapServer.Add(YUEnums.PTEnum.NYPT, "YPT.PT.NY,YPT");
                _mapServer.Add(YUEnums.PTEnum.HDHome, "YPT.PT.HDHOME,YPT");
                _mapServer.Add(YUEnums.PTEnum.HDSky, "YPT.PT.HDSKY,YPT");
                _mapServer.Add(YUEnums.PTEnum.U2, "YPT.PT.U2,YPT");
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
