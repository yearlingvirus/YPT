using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YU.Core.Utils;

namespace YU.Core.Log
{
    public class Logger
    {
        public static void Error(Exception ex)
        {
            //创建日志记录组件实例
            ILog log = log4net.LogManager.GetLogger(YUConst.LOGGER);
            log.Error(ex.GetInnerExceptionMessage(), ex);
        }

        public static void Error(string errMsg, Exception ex)
        {
            //创建日志记录组件实例
            ILog log = log4net.LogManager.GetLogger(YUConst.LOGGER);
            log.Error(errMsg, ex);
        }

        public static void Info(string message)
        {
            //创建日志记录组件实例
            ILog log = log4net.LogManager.GetLogger(YUConst.LOGGER);
            log.Info(message);
        }
    }
}
