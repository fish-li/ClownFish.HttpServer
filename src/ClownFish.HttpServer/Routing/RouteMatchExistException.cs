using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Routing
{
	/// <summary>
	/// 用于在匹配过程中快速结束整个查找过程
	/// </summary>
	internal sealed class RouteMatchExistException : Exception
	{
	}
}
