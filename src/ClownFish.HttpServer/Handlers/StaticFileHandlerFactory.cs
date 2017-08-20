using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Config;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 用于创建StaticFileHandler的工厂类型
    /// </summary>
    public sealed class StaticFileHandlerFactory : IHttpHandlerFactory
    {
		private static string[] s_extNames;


		internal static void Init(ServerOption option)
		{
			if( option.Website.StaticFiles != null && option.Website.StaticFiles.Length > 0 )
				s_extNames = (from x in option.Website.StaticFiles
													 select x.Ext
													 ).ToArray();
		}

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
			if( s_extNames == null || s_extNames.Length == 0 )
				return null;

			string path = context.Request.Path;

			for( int i = 0; i < s_extNames.Length; i++ ) {
				if( path.EndsWith(s_extNames[i], StringComparison.OrdinalIgnoreCase) )
					return new StaticFileHandler();
			}

			return null;
		}




		
    }
}
