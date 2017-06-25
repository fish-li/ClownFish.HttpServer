using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Reflection;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Routing
{
	/// <summary>
	/// 表示每个路由描述信息，可用于在匹配成功后获取IHttpHandler实例
	/// </summary>
	internal abstract class RoutingObject
	{
		public abstract IHttpHandler CreateHandler(HttpContext context);
		public abstract bool IsMatch(HttpContext context);
	}


	/// <summary>
	/// 对应[RouteUrl("......")]标记HttpHandler类型
	/// </summary>
	internal sealed class RoutingHandler : RoutingObject
	{
		public RouteUrlAttribute Attr { get; set; }
		public Type HandlerType { get; set; }

		public override IHttpHandler CreateHandler(HttpContext context)
		{
			return (IHttpHandler)this.HandlerType.FastNew();
		}

		public override bool IsMatch(HttpContext context)
		{
			return RouteHelper.IsMatchRequest(context, Attr.Regex);
		}
	}


	/// <summary>
	/// 对应[RouteUrl("...固定URL...")]标记方法
	/// </summary>
	internal sealed class RoutingAction : RoutingObject
	{
		public RouteUrlAttribute Attr { get; set; }
		public Type ServiceType { get; set; }
		public MethodInfo Method { get; set; }

		public override IHttpHandler CreateHandler(HttpContext context)
		{
			return new ServiceHandler(this.ServiceType, this.Method);
		}
		public override bool IsMatch(HttpContext context)
		{
			return RouteHelper.IsMatchRequest(context, Attr.Regex);
		}
	}
	
	

	/// <summary>
	/// 对应[Route(".....")]标记的类型
	/// </summary>
	internal sealed class RoutingService : RoutingObject
	{
		public RouteAttribute Attr { get; set; }
		public Type ServiceType { get; set; }

		/// <summary>
		/// 注意：此属性仅在匹配成功后，克隆对象并赋值
		/// </summary>
		internal MethodInfo Method { get; set; }


		public RoutingService Clone(MethodInfo m)
		{
			return new RoutingService {
				Attr = this.Attr,
				ServiceType = this.ServiceType,
				Method = m
			};
		}

		internal static readonly string Key = "9adda0cf-5802-4920-ac38-112b0809d10d";

		public void SetHandler(HttpContext context)
		{
			IHttpHandler handler = new ServiceHandler(this.ServiceType, this.Method);
			context.Items[Key] = handler;
		}

		public override IHttpHandler CreateHandler(HttpContext context)
		{
			//return new ServiceHandler(this.ServiceType, this.Method);
			return (IHttpHandler)context.Items[Key];
		}


		public override bool IsMatch(HttpContext context)
		{
			return RouteHelper.IsMatchRequest(context, this);
		}
	}



}
