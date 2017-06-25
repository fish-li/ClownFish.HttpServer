using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 请求体中的数据格式
	/// </summary>
	internal enum RequestDataType
	{
        /// <summary>
        /// 未指定
        /// </summary>
        NoSet,
		/// <summary>
		/// 表单形式的数据
		/// </summary>
		Form,
		/// <summary>
		/// JSON格式数据
		/// </summary>
		Json,
		/// <summary>
		/// XML格式数据
		/// </summary>
		Xml,
        /// <summary>
        /// 未知的数据格式
        /// </summary>
        Unknown
	}
}
