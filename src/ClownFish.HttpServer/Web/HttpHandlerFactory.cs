using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Handlers;
using ClownFish.HttpServer.Routing;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 创建 HttpHandler 的工厂类型
	/// </summary>
	public class HttpHandlerFactory
	{
		/// <summary>
		/// 静态单例对象
		/// </summary>
		private static readonly LazyObject<HttpHandlerFactory> s_instance = new LazyObject<HttpHandlerFactory>(true);
		// 说明：上面采用 LazyObject<> 的写法，允许在运行时获取到HttpHandlerFactory的继承类型实例。


		private List<IHttpHandlerFactory> _factoryList = new List<IHttpHandlerFactory>(16);

		internal static HttpHandlerFactory GetInstance()
		{
			return s_instance.Instance;
		}


		internal void RegisterFactory(IHttpHandlerFactory factory)
		{
			_factoryList.Add(factory);
		}


		/// <summary>
		/// 尝试创建一个可直接处理 OPTIONS 请求的IHttpHandler实例，
		/// 如果请求方法不是 OPTIONS，则返回 null
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual IHttpHandler CreateOptionsHandler(HttpContext context)
		{
			if( context.Request.HttpMethod == "OPTIONS" ) {
				return new OptionsHandler();     // 提前结束
			}
			return null;
		}

		/// <summary>
		/// 尝试从注册的IHttpHandlerFactory实例中返回一个有效的IHttpHandler实例，
		/// 如果所有的IHttpHandlerFactory实例都不能返回，则返回 null
		/// </summary>
		/// <param name="context"></param>
		/// <param name="beforeRouting">是否在路由匹配之前执行</param>
		/// <returns></returns>
		protected virtual IHttpHandler CreateHandlerByFactory(HttpContext context, bool beforeRouting)
		{
			foreach( var factory in _factoryList ) {
				if( beforeRouting != factory.IsBeforeRouting )
					continue;

				IHttpHandler handler = factory.CreateHandler(context);
				if( handler != null )
					return handler;
			}


			return null;
		}

		/// <summary>
		/// 尝试根据路由规则来获取一个匹配的IHttpHandler实例，
		/// 如果没有找到任何匹配，则返回 null
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual IHttpHandler CreateHandlerByRouting(HttpContext context)
		{
			return RoutingManager.Instance.CreateHandler(context);
		}


		/// <summary>
		/// 默认返回 Http404Handler 来处理没有任何匹配的IHttpHandler
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual IHttpHandler CreateHandler404(HttpContext context)
		{
			return new Http404Handler();
		}


		/// <summary>
		/// 创建一个可处理HttpContext请求的HttpHandler实例
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual IHttpHandler CreateHandler(HttpContext context)
		{
			IHttpHandler handler = CreateOptionsHandler(context)

									?? CreateHandlerByFactory(context, true)

									?? CreateHandlerByRouting(context)

									?? CreateHandlerByFactory(context, false)

									?? CreateHandler404(context);


			// 为处理器类型设置 HttpContext
			IRequireHttpContext xx = handler as IRequireHttpContext;
			if( xx != null )
				xx.HttpContext = context;

			return handler;

		}



	}
}
