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
	/// 标记可用于响应某个URL的HTTP处理器
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class RouteUrlAttribute : Attribute
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="url"></param>
		public RouteUrlAttribute(string url) : this(url, UrlType.FixUrl)
		{
		}

		/// <summary>
		/// 构造方法
		/// </summary>
		/// <param name="url"></param>
		/// <param name="type"></param>
		public RouteUrlAttribute(string url, UrlType type)
		{
			if( string.IsNullOrEmpty(url) )
				throw new ArgumentNullException(nameof(url));

			this.Url = url;
			this.UrlType = type;


			if( type == UrlType.Pattern )
				this.Regex = RouteHelper.CreateRegex(this.Url);

			else if( type == UrlType.Regex)
				this.Regex = new Regex(this.Url, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		/// <summary>
		/// 标记某个方法可以处理哪个URL，
		/// 例如：
		/// 类型上标记： [Route]
		/// 方法上标记： [RouteUrl("/inner-test.aspx")]
		/// 方法上标记： [RouteUrl(@"/page/{id}/{year}-{month}-{day}.aspx")]
		/// </summary>
		public string Url { get; private set; }

		/// <summary>
		/// Url属性值的类别
		/// </summary>
		public UrlType UrlType { get; private set; }

		/// <summary>
		/// 如果不是固定URL，就生成一个正则表达式用于匹配
		/// </summary>
		internal Regex Regex { get; set; }

	}


	/// <summary>
	/// 路由描述中的URL类别
	/// </summary>
	public enum UrlType
	{
		/// <summary>
		/// 固定的URL
		/// </summary>
		FixUrl,
		/// <summary>
		/// 带有占位符的URL模式
		/// </summary>
		Pattern,
		/// <summary>
		/// 正则表达式的URL模式
		/// </summary>
		Regex
	}


	
}
