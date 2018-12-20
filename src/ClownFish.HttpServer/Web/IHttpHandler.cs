using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Result;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 定义用于处理HTTP语法的接口
	/// </summary>
	public interface IHttpHandler
	{
		/// <summary>
		/// 处理HTTP请求
		/// </summary>
		/// <param name="context"></param>
		void ProcessRequest(HttpContext context);
	}


    /// <summary>
    /// 异步版本的IHttpHandler
    /// </summary>
    public interface ITaskHttpHandler : IHttpHandler
    {
        /// <summary>
        /// 处理HTTP请求，
        /// 仅获取Action的执行结果，不包含输出过程。
        /// </summary>
        /// <param name="context"></param>
        Task ProcessRequestAsync(HttpContext context);
    }


    /// <summary>
    /// 增强版本的IHttpHandler，更适合MVC的设计思路
    /// </summary>
    public interface IMvcHttpHandler : IHttpHandler
    {
        /// <summary>
        /// 处理HTTP请求，
        /// 仅获取Action的执行结果，不包含输出过程。
        /// </summary>
        /// <param name="context"></param>
        IActionResult ProcessRequest2(HttpContext context);
    }

    /// <summary>
    /// 增强版本的IHttpHandler，更适合MVC的设计思路
    /// </summary>
    public interface ITaskMvcHttpHandler : IHttpHandler
	{
		/// <summary>
		/// 处理HTTP请求，
		/// 仅获取Action的执行结果，不包含输出过程。
		/// </summary>
		/// <param name="context"></param>
		Task<IActionResult> ProcessRequestAsync(HttpContext context);
	}
}
