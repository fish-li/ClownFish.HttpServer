using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Result;
using ClownFish.HttpServer.Utils;
using ClownFish.HttpServer.Web;

namespace ClownFish.RequestLogConsole
{
    public class RequestLogHttpModule : EventSubscriber<HttpModule>
    {
        public override void SubscribeEvent(HttpModule instance)
        {
            instance.BeginRequest += Instance_BeginRequest;
        }

        private void Instance_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            NetHttpInfo httpInfo = NetHttpInfo.Create(app.Context);

            Console.WriteLine(httpInfo.RequestText);
            Console.WriteLine("\r\n");


            TextResult result = new TextResult("server is OK. " + DateTime.Now.ToTimeString());
            (result as IActionResult).Ouput(app.Context);

            app.CompleteRequest();
        }
    }
}
