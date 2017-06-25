using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Result
{
	/// <summary>
	/// 表示Action的执行结果为JSON
	/// </summary>
	public sealed class JsonResult : IActionResult
	{
		/// <summary>
		/// 需要以JSON形式输出的数据对象
		/// </summary>
		public object Model { get; private set; }


		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="model">将要序列化的对象</param>
		public JsonResult(object model)
		{
			if( model == null )
				throw new ArgumentNullException("model");

			this.Model = model;
		}


		void IActionResult.Ouput(HttpContext context)
		{
			context.Response.ContentType = ResponseContentType.Json;
			string json = this.Model.ToJson();
			context.Response.Write(json);
		}

	}
}
