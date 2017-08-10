using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 用于创建StaticFileHandler的工厂类型
    /// </summary>
    public sealed class StaticFileHandlerFactory : IHttpHandlerFactory
    {

		/// <summary>
		/// 是否在路由匹配之前执行，当前类型固定返回false
		/// </summary>
		public bool IsBeforeRouting {
			get {
				return false;
			}
		}

		/// <summary>
		/// 判断当前请求是不是静态文件，如果是，则创建一个StaticFileHandler的实例，否则返回null
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public IHttpHandler CreateHandler(HttpContext context)
		{
			string path = context.Request.Path;
			if (path.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".css", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
				|| path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)

                || path.EndsWith(".eot", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".ttf", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".woff", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".woff2", StringComparison.OrdinalIgnoreCase)
                || path.EndsWith(".map", StringComparison.OrdinalIgnoreCase)
                ) {
				return new StaticFileHandler();
			}

			return null;
		}




		
    }
}
