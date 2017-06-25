using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Attributes
{
	/// <summary>
	/// 从HttpRequest的属性中获取某个值
	/// </summary>
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public sealed class FromRequestAttribute : System.Attribute
	{
	}

}
