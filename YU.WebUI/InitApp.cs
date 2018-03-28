using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.StaticFiles;
using Microsoft.Owin.FileSystems;

[assembly: OwinStartup(typeof(YU.WebUI.InitApp))]

namespace YU.WebUI
{
    public class InitApp
    {
        public void Configuration(IAppBuilder app)
        {
            //// 有关如何配置应用程序的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=316888

            //HttpConfiguration config = new HttpConfiguration();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //app.UseWebApi(config);

            //var physicalFileSystem = new PhysicalFileSystem(@"./Website/aaa");
            //var options = new FileServerOptions
            //{
            //    EnableDefaultFiles = true,
            //    FileSystem = physicalFileSystem
            //};
            //options.StaticFileOptions.FileSystem = physicalFileSystem;
            //options.StaticFileOptions.ServeUnknownFileTypes = false;


            //app.UseFileServer(options);

        }
    }
}
