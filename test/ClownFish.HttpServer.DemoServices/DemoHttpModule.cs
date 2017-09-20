using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Web;
using ClownFish.Base;
using ClownFish.HttpServer.Utils;

namespace ClownFish.HttpServer.DemoServices
{
	public class DemoHttpModule : EventSubscriber<HttpModule>
	{
		public class MessageEventArgs : EventArgs
		{
			public string Message { get; set; }
		}
		public static event EventHandler<MessageEventArgs> OnMessage;

		public override void SubscribeEvent(HttpModule instance)
		{
			instance.BeginRequest += Instance_BeginRequest;
			instance.PreMapRequestHandle += Instance_PreMapRequestHandle;
			instance.PreRequestHandlerExecute += Instance_PreRequestHandlerExecute;
			instance.PostRequestHandlerExecute += Instance_PostRequestHandlerExecute;
			instance.EndRequest += Instance_EndRequest;
		}

		private void Instance_EndRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if( app == null )
				throw new ApplicationException("不可能的事情！");
		}

		private void Instance_PostRequestHandlerExecute(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if( app == null )
				throw new ApplicationException("不可能的事情！");
		}

		private void Instance_PreRequestHandlerExecute(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if( app == null )
				throw new ApplicationException("不可能的事情！");
		}

		private void Instance_PreMapRequestHandle(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if( app == null )
				throw new ApplicationException("不可能的事情！");
		}

		private void Instance_BeginRequest(object sender, EventArgs e)
		{
			HttpApplication app = (HttpApplication)sender;
			if( app == null )
				throw new ApplicationException("不可能的事情！");

            //Console.WriteLine($"HttpModuel Event: BeginRequest: url: {app.Request.Path}");
            //ExecuteEvent(DateTime.Now.ToTimeString() + " : " + app.Request.Url.AbsoluteUri);

            NetHttpInfo httpInfo = NetHttpInfo.Create(app.Context);
            ExecuteEvent(httpInfo.RequestText + "\r\n\r\n");
        }


		private void ExecuteEvent(string message)
		{
			EventHandler<MessageEventArgs> handler = OnMessage;
			if (handler != null)
				handler(this, new MessageEventArgs { Message = message });
				
		}
	}
}
