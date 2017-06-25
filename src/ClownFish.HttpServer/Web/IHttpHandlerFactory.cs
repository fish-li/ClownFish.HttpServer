using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 定义一个根据HttpContext获取IHttpHandler实例的接口
	/// </summary>
	public interface IHttpHandlerFactory
	{
		/// <summary>
		/// 根据HttpContext获取IHttpHandler实例
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		IHttpHandler CreateHandler(HttpContext context);

		/// <summary>
		/// 是否在路由匹配之前执行，返回false指示将在路由匹配之后再调用CreateHandler方法
		/// </summary>
		/// <returns></returns>
		bool IsBeforeRouting { get; }
	}
}
