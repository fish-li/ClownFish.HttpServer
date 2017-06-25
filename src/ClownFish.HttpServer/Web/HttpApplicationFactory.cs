using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace ClownFish.HttpServer.Web
{
	internal class HttpApplicationFactory
	{
		public static readonly HttpApplicationFactory Instance = new HttpApplicationFactory();

		private ConcurrentQueue<HttpApplication> _queue = new ConcurrentQueue<HttpApplication>();


		public HttpApplication Get()
		{
			// 先尝试找一个已存在的实例
			HttpApplication app = null;
			if( _queue.TryDequeue(out app) )
				return app;

			else {
				// 找不到就创建一个新实例
				return new HttpApplication();
			}
		}


		/// <summary>
		/// 回收HttpApplication实例
		/// </summary>
		/// <param name="app"></param>
		public void Recycle(HttpApplication app)
		{
			_queue.Enqueue(app);
		}


	}
}
