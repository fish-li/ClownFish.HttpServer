using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
    /// <summary>
    /// 用于 跳过 HTTP 执行管线链中的所有事件和筛选并直接执行 EndRequest 事件。
    /// </summary>
    [Serializable]
    internal class HttpApplicationEndRequestException : Exception
	{
	}
}
