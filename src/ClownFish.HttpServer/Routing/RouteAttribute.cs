using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Routing
{
	/// <summary>
	/// 标记可用于响应某个URL的HTTP处理器，
	/// 例如：[Route("/api/ns/class/{action}.aspx")]
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class RouteAttribute : Attribute
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public RouteAttribute() { }

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="url">必须是“占位符”风格的URL</param>
		public RouteAttribute(string url)
		{
			if( string.IsNullOrEmpty(url) )
				throw new ArgumentNullException(nameof(url));

			this.Url = url;
			this.Regex = RouteHelper.CreateRegex(this.Url); // 必须是“占位符”风格的URL
		}

		/// <summary>
		/// 在类型上标记，要匹配的 URL 前缀部分
		/// </summary>
		public string Url { get; private set; }


		/// <summary>
		/// 如果指定的URL属性，就生成一个正则表达式用于匹配
		/// </summary>
		internal Regex Regex { get; set; }
	}



	/// <summary>
	/// 标记可用于响应某个URL的HTTP处理器，与[Route("...")]配套使用
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RouteActionAttribute : Attribute
	{

		/// <summary>
		/// 在类型上标记，要匹配的 URL 前缀部分，
		/// 例如：
		/// 类型上标记： [Route("/api/ns/class/{action}.aspx")]
		/// 方法上标记： [RouteAction(Name = "New-Guid")]
		/// </summary>
		public string Name { get; set; }   // 如果不指定Name的值，将会忽略这个标记。
	}



	/// <summary>
	/// 标记某个方法不用于HTTP请求
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RouteIgnoreAttribute : Attribute
	{
		// 当指定 [Route(Url = "/api/ns/class/{action}.aspx")] 之后，
		// 类型中的所有公开方法【默认】都可以处理HTTP请求，如果不希望跳过某个方法，可以应用此标记
	}


}
