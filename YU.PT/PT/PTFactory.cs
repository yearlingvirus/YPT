using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core;

namespace YU.PT
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
                _mapServer.Add(YUEnums.PTEnum.CMCT, "YU.PT.CMCT,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.CHDBits, "YU.PT.CHDBITS,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.KeepFrds, "YU.PT.KEEPFRDS,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.MTeam, "YU.PT.MTEAM,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.OurBits, "YU.PT.OURBITS,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.TTG, "YU.PT.TTG,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.BTSchool, "YU.PT.BTSCHOOL,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.GZTown, "YU.PT.GZTOWN,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.HDU, "YU.PT.HDU,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.NYPT, "YU.PT.NY,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.HDHome, "YU.PT.HDHOME,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.HDSky, "YU.PT.HDSKY,YU.PT");
                _mapServer.Add(YUEnums.PTEnum.U2, "YU.PT.U2,YU.PT");
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
                instance = TypesContainer.CreateInstance<IPT>("YU.PT.ExtendPT,YU.PT", argList.ToArray());
            }
            if (instance == null)
                throw new Exception("instance==null");
            return instance;
        }
    }
}
