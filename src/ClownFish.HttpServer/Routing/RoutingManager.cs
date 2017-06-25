using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ClownFish.Base.Reflection;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Web;
using ClownFish.HttpServer.Attributes;

namespace ClownFish.HttpServer.Routing
{
	internal sealed class RoutingManager
	{
		public static readonly RoutingManager Instance = new RoutingManager();

		private readonly object _lock = new object();
		private bool _inited = false;

		/// <summary>
		/// 直接可以做URL映射查找的字典表
		/// </summary>
		private Dictionary<string, RoutingObject> _urlDict = new Dictionary<string, RoutingObject>(1024, StringComparer.OrdinalIgnoreCase);

		/// <summary>
		/// 需要做正则比较的路由规则表
		/// </summary>
		private List<RoutingObject> _regexList = new List<RoutingObject>(1024);



		public void LoadRoutes()
		{
			if( _inited == false ) {
				lock( _lock ) {
					if( _inited == false ) {
						Init();
						_inited = true;
					}
				}
			}
		}


		private void Init()
		{
            List< Assembly> assemblies = ReflectionExtensions.GetAssemblyList<ControllerAssemblyAttribute>();

            foreach(Assembly asm in assemblies ) {

                foreach(Type t in asm.GetPublicTypes() ) {

					if( LoadHandlerRoute(t) )   // 第一种路由方式
						continue;


					RouteAttribute a1 = t.GetMyAttribute<RouteAttribute>();
					if( a1 != null ) {
						if( a1.Url != null ) {
							LoadServiceRoute(t, a1);   // 第二种路由方式
						}
						else {
							LoadActionRoute(t, a1);   // 第三种路由方式
						}
					}
				}
			}
		}


		private bool LoadHandlerRoute(Type t)
		{
			RouteUrlAttribute a1 = t.GetMyAttribute<RouteUrlAttribute>();
			if( a1 != null ) {
				if( t.IsCompatible(typeof(IHttpHandler)) == false )
					throw new InvalidCodeException("标记[RouteUrl]的类型必须实现IHttpHandler接口。");

				if( string.IsNullOrEmpty(a1.Url) )
					throw new InvalidCodeException("[RouteUrl]的Url属性不能为空。");

				RoutingHandler obj = new RoutingHandler {
					Attr = a1,
					HandlerType = t
				};

				if( a1.Regex != null ) 
					_regexList.Add(obj);
				
				else 
					_urlDict.AddValue(a1.Url, obj);

				return true;
			}

			// 返回 false 表示针对某个类型，没有找到路由标记
			return false;
		}


		private void LoadServiceRoute(Type t, RouteAttribute a1)
		{
			RoutingService obj = new RoutingService {
				Attr = a1,
				ServiceType = t
			};
			_regexList.Add(obj);
		}


		private void LoadActionRoute(Type t, RouteAttribute a1)
		{
			// 只查找公开的实例方法
			var methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

			foreach(MethodInfo m in methods ) {				

				RouteUrlAttribute a2 = m.GetMyAttribute<RouteUrlAttribute>();
				if( a2 != null ) {
					RoutingAction obj = new RoutingAction {
						Attr = a2,
						Method = m,
						ServiceType = t
					};

					if( a2.Regex != null )
						_regexList.Add(obj);
					else
						_urlDict.AddValue(a2.Url, obj);
				}
			}
		}


		public IHttpHandler CreateHandler(HttpContext context)
		{
			// 先尝试简单的字典表查找，用于快速定位【固定的URL】路由
			RoutingObject obj = null;
			if( _urlDict.TryGetValue(context.Request.Path, out obj) ) {
				return obj.CreateHandler(context);
			}


			// 遍历所有正则表达式，寻找合适的匹配
			try {
				foreach( RoutingObject b2 in _regexList ) {
					if( b2.IsMatch(context) )
						return b2.CreateHandler(context);
				}
			}
			catch( RouteMatchExistException ) { 

				// 这种场景发生在【第二种路由方式】上，类型的模式是匹配的，但是找不到匹配的方法
				// 为了避免再去尝试其它的正则匹配，所以直接以异常的形式结束整个查找过程。
				// 实现实现代码可参考 RouteHelper.IsMatchRequest(HttpContext context, RoutingService route)
				return null;
			}
			
			// 没有匹配的结果，所有路由查找失败。
			return null;
		}

	}
}
