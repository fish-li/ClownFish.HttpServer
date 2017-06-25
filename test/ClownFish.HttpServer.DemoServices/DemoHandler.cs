using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Attributes;
using ClownFish.HttpServer.Routing;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.DemoServices
{
    // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-url.aspx


    /// <summary>
    /// 用于测试的处理器
    /// </summary>
    [RouteUrl("/hello/ClownFish-HttpServer/demo-url.aspx")]
	public sealed class DemoHandler : IHttpHandler
	{
		private HttpContext _context;

		/// <summary>
		/// 实现ProcessRequest方法
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
		{
			_context = context;

			// 设置基本的响应头
			context.Response.SetBasicHeaders();

			string action = context.Request.QueryString["action"];
			if( string.IsNullOrEmpty(action) == false ) {
				MethodInfo method = this.GetType().GetMethod(action, BindingFlags.Instance | BindingFlags.Public);
				if( method != null && method.GetParameters().Length == 0 ) {
					method.Invoke(this, null);
					return;
				}
			}

			context.Response.Write("NotFound");
		}


		/// <summary>
		/// 获取一个新的GUID，可用于给一个任务命名
		/// </summary>
		public void NewGuid()
		{
			_context.Response.Write(Guid.NewGuid().ToString());
		}

		
		/// <summary>
		/// 获取当前时间
		/// </summary>
		public void GetTime()
		{
			_context.Response.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
		}
		
	}
}
