using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
    /// <summary>
    /// 为HttpHandler定义一个执行前的初始化接口
    /// </summary>
    public interface IHttpHandlerInitializer
    {
        /// <summary>
        /// 处理HTTP请求
        /// </summary>
        /// <param name="context"></param>
        void Init(HttpContext context);
    }
}
