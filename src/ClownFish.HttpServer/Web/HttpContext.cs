using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 用于封装请求执行过程中的重要数据对象
	/// </summary>
	public sealed class HttpContext : IDisposable
	{
		internal HttpContext()
		{
			// 单元测试方法
			// 1、继承 HttpRequest，并重写它的虚方法，属性
			// 2、继承 HttpResponse，并重写它的虚方法，属性
			// 3、调用这个无参的构造方法，并直接给Request，Response 赋值
		}

		internal HttpContext(HttpListenerContext conext)
		{
			this.OriginalContext = conext;
			this.Request = new HttpRequest(this);
			this.Response = new HttpResponse(this);
		}


		/// <summary>
		/// HttpListenerContext实例
		/// </summary>
		public HttpListenerContext OriginalContext { get; private set; }

        /// <summary>
        /// HttpApplication实例
        /// </summary>
        internal HttpApplication Application { get; set; }

        /// <summary>
        /// 用于操作Request的对象
        /// </summary>
        public HttpRequest Request { get; private set; }

		/// <summary>
		/// 用于操作Response的对象
		/// </summary>
		public HttpResponse Response { get; private set; }

		/// <summary>
		/// 用于响应当前请求的HttpHandler实例
		/// </summary>
		public IHttpHandler HttpHandler { get; internal set; }

        /// <summary>
        /// 获取用于为客户端获取标识、身份验证信息和安全角色的对象
        /// </summary>
        public IPrincipal User { get; internal set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应跳过对当前请求的授权检查
        /// </summary>
        public bool SkipAuthorization { get; set; }



        /// <summary>
        /// 最近一次产生的异常对象
        /// </summary>
        public Exception LastException { get; internal set; }

		/// <summary>
		/// 清除最近产生的异常对象
		/// </summary>
		public void ClearLastException()
		{
			this.LastException = null;
		}

		/// <summary>
		/// 为当前请求指定一个处理器，调用后将会跳过路由查找过程。
		/// </summary>
		/// <param name="handler"></param>
		public void RemapHandler(IHttpHandler handler)
		{
			this.HttpHandler = handler;
		}



		private IDictionary _items;
		/// <summary>
		/// 存放一些与请求相关的临时数据
		/// </summary>
		public IDictionary Items {
			get {
				if( _items == null )
					_items = new Hashtable();
				return _items;
			}
		}



		#region IDisposable 成员

		/// <summary>
		/// 实现 IDispose 接口
		/// </summary>
		public void Dispose()
		{
			this.Response.Dispose();

			try {
				if( OriginalContext != null ) {
					OriginalContext.Response.Close();
					OriginalContext = null;
				}
			}
			catch { /* 关闭连接再出异常，就不处理了  */}
		}

		#endregion
	}
}
