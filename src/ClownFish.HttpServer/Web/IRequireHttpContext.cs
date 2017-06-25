using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 指示某个类型需要包含一个HttpContext实例，
	/// 实现这个接口的HTTP处理类型（Service/Controller），框架将会自动给HttpContext属性赋值。
	/// </summary>
	public interface IRequireHttpContext
	{
		/// <summary>
		/// 一个HttpContext实例。
		/// </summary>
		HttpContext HttpContext { get; set; }


		// 说明：设计这个接口主要是为了避免滥用 HttpContext.Current
	}
}
