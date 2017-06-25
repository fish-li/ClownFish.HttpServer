using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Attributes;
using ClownFish.HttpServer.Routing;

namespace ClownFish.HttpServer.DemoServices
{
	/// <summary>
	/// 演示使用 [Route] 和 [RouteUrl]
	/// </summary>
	[Route]
	public class DemoController
	{
		/// <summary>
		/// 演示使用 [RouteUrl]
		/// </summary>
		[RouteUrl("/hello/ClownFish-HttpServer/demo-ccc/NewGuid.aspx")]
		public string GetGuid()
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/New-Guid.aspx
            return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// 演示使用 [RouteUrl]
		/// </summary>
		/// <param name="sleepMillisecondsTimeout"></param>
		/// <returns></returns>
		[RouteUrl("/hello/ClownFish-HttpServer/demo-ccc/Now.aspx")]
		public string Now(int? sleepMillisecondsTimeout)
		{
			// 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Now.aspx

			if( sleepMillisecondsTimeout.HasValue && sleepMillisecondsTimeout.Value > 0)
				System.Threading.Thread.Sleep(sleepMillisecondsTimeout.Value);

			return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// 演示使用 [RouteRegexUrl]
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		[RouteUrl("/hello/ClownFish-HttpServer/demo-ccc/Sha1/{s}.aspx", UrlType.Pattern)]
		public string Sha1(string s)
		{
            // 注意：参数 s 来自于URL中的占位符匹配结果。

            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Sha1/fishli.aspx
            return s.Sha1();
		}



		/// <summary>
		/// 这个方法将不会被HTTP请求调用，因为没有指定[RouteUrl]
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public int Add(int a, int b)
		{
			return a + b;
		}


        /// <summary>
        /// 演示复杂的URL模式，及接收数据实体对象
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [RouteUrl("/hello/ClownFish-HttpServer/demo-ccc/model/{a}/{b}/ok.aspx", UrlType.Pattern)]
        public string TestInputModel(int a, string b, [FromBody]DataModel1 model)
        {
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-ccc/model/53/fishli/ok.aspx

            return $"a={a}, b={b}, id={model.Id}, name={model.Name}";
        }

    }
}
