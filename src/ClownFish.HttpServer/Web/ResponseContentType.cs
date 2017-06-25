using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 返回内容的 MIME 类型
	/// </summary>
	public static class ResponseContentType
	{
		/// <summary>
		/// 表示以普通文本形式响应
		/// </summary>
		public static readonly string Text = "text/plain";


		/// <summary>
		/// 表示以JSON形式响应
		/// </summary>
		public static readonly string Json = "application/json";

		/// <summary>
		/// 表示以XML形式响应
		/// </summary>
		public static readonly string Xml = "application/xml";


		/// <summary>
		/// 表示以HTML形式响应
		/// </summary>
		public static readonly string Html = "text/html";


	}
}
