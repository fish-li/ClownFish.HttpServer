using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer
{
	/// <summary>
	/// 表示错误的代码异常
	/// </summary>
	public sealed class InvalidCodeException : SystemException
	{

		/// <summary>
		/// 使用指定的错误信息初始化 InvalidCodeException 类的新实例。
		/// </summary>
		/// <param name="message">解释异常原因的错误信息。</param>
		public InvalidCodeException(string message) : base(message)
		{

		}
	
	}
}
