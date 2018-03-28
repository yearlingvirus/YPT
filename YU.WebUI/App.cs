using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YU.WebUI
{

    public static class App
    {
        private static IDisposable Instance { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="port"></param>
        public static void Start(int port)
        {
            if (Instance != null)
                Instance.Dispose();
            StartOptions options = new StartOptions();
            Instance = WebApp.Start<InitApp>(new StartOptions(string.Format("http://*:{0}", port)));
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            if (Instance != null)
                Instance.Dispose();
        }


    }
}
