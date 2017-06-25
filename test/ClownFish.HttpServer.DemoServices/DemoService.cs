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
	/// 演示使用 [Route] ，注意 {action} 这个占位符是必须的。
	/// </summary>
	[Route("/hello/ClownFish-HttpServer/demo-service/{action}.aspx")]
	public sealed class DemoService
	{

		/// <summary>
		/// 演示使用 [RouteAction] 来实现Action的别名
		/// </summary>
		[RouteAction(Name = "NewGuid")]
		public string GetGuid()
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/NewGuid.aspx
            return Guid.NewGuid().ToString();
		}


		/// <summary>
		/// 如果不指定 [RouteAction]，也是可以的。
		/// </summary>
		public string Now()
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Now.aspx
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		/// <summary>
		/// 演示简单的参数使用
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public string Sha1(string s)
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Sha1.aspx?s=fish-li
            // 测试URL： http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Sha1.aspx?s=%e5%a4%a7%e6%98%8e%e7%8e%8b%e6%9c%9d

            //return s.Sha1();
            string sha1 = s.Sha1();
			return $"Sha1({s}) = {sha1}";
		}


		/// <summary>
		/// 演示使用[FromProperty]，获取 HttpListenerRequest.UserAgent 属性值做为参数
		/// </summary>
		/// <param name="userAgent"></param>
		/// <returns></returns>
		public string ShowAgent([FromRequest]string userAgent)			
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/ShowAgent.aspx
            return userAgent;
		}


		/// <summary>
		/// 演示使用[FromBody]，
		/// 读取整个请求流，注意参数是string，
		/// 如果参数不是 string，将会使用反序列化方式构造参数值
		/// </summary>
		/// <param name="body"></param>
		/// <returns></returns>
		public string ShowBody([FromBody] string body)
		{
            // 匹配URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/ShowBody.aspx
            return body;
		}


		/// <summary>
		/// 这个方法将不会被HTTP请求调用，因为指定了[RouteIgnore]
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		[RouteIgnore]
		public int Add(int a, int b)
		{
            // 测试URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/Add.aspx?a=2&b=3
            return a + b;
		}


		public string ErrorTest()
		{
			// 测试URL:　http://localhost:50456/hello/ClownFish-HttpServer/demo-service/ErrorTest.aspx
			throw new InvalidOperationException("ErrorTest_InvalidOperationException");
		}
                
	}
}

