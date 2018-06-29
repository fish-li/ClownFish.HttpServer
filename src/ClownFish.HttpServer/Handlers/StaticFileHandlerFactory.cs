using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Config;
using ClownFish.HttpServer.Utils;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 用于创建StaticFileHandler的工厂类型
    /// </summary>
    public sealed class StaticFileHandlerFactory : IHttpHandlerFactory
    {
		private static void ServerHostInit(ServerOption option)
		{
            if( option.Website?.StaticFiles?.Length > 0 )
                option.InternalOptions.StaticFileExtNames 
                                = (from x in option.Website.StaticFiles
                                    select x.Ext
                                    ).ToDictionary(x => x, x => 1, StringComparer.OrdinalIgnoreCase);
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
            if( context.ServerOption.InternalOptions.StaticFileExtNames?.Count == 0 )
                return null;

            string path = context.Request.Path;
            string extName = PathHelper.GetExtension(path);

            if( context.ServerOption.InternalOptions.StaticFileExtNames.ContainsKey(extName) )
                return new StaticFileHandler();

            return null;
        }


        

    }
}
