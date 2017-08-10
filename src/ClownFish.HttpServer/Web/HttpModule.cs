using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Reflection;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Authentication;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 类似 ASP.NET 中的 IHttpModule，允许以事件的方式进入请求的处理过程
	/// </summary>
	public sealed class HttpModule : BaseEventObject
	{
		private HttpApplication _app;

		internal void SetHttpApplication(HttpApplication app)
		{
			_app = app;
		}


		/// <summary>
		/// 响应请求时作为 HTTP 执行管线链中的第一个事件发生。
		/// </summary>
		public event EventHandler BeginRequest;

		internal void Execute_BeginRequest()
		{
			// 这种写法虽然可能会有点小问题，但是这种场景可以不予考虑。
			this.BeginRequest?.Invoke(_app, null);
		}



		/// <summary>
		/// 当安全模块已建立用户标识时发生。
		/// </summary>
		public event EventHandler AuthenticateRequest;
		internal void Execute_AuthenticateRequest()
		{
			this.AuthenticateRequest?.Invoke(_app, null);
		}


		/// <summary>
		/// 当安全模块已建立用户标识时发生。
		/// </summary>
		public event EventHandler PostAuthenticateRequest;

		internal void Execute_PostAuthenticateRequest()
		{
			this.PostAuthenticateRequest?.Invoke(_app, null);
		}



		/// <summary>
		/// 当安全模块已验证用户授权时发生。
		/// </summary>
		public event EventHandler AuthorizeRequest;
		internal void Execute_AuthorizeRequest()
		{
			this.AuthorizeRequest?.Invoke(_app, null);
		}



		/// <summary>
		/// 当安全模块已验证用户授权时发生。
		/// </summary>
		public event EventHandler PostAuthorizeRequest;
        internal void Execute_PostAuthorizeRequest()
        {
            this.PostAuthorizeRequest?.Invoke(_app, null);


            ServiceHandler serviceHandler = _app.Context.HttpHandler as ServiceHandler;
            if( serviceHandler != null )
                serviceHandler.CheckAuthorization(_app.Context);

            else {
                AuthorizeAttribute attr = _app.Context.HttpHandler.GetType().GetMyAttribute<AuthorizeAttribute>();
                if( attr != null ) {

                    if( attr.AuthenticateRequest(_app.Context) == false )
                        throw new System.Web.HttpException(403,
                                    "很抱歉，您没有合适的权限访问该资源：" + _app.Context.Request.RawUrl);
                }
            }
        }




		/// <summary>
		/// 将当前请求映射到相应的事件处理程序时发生。
		/// </summary>
		public event EventHandler PreMapRequestHandle;
		internal void Execute_PreMapRequestHandle()
		{
			this.PreMapRequestHandle?.Invoke(_app, null);
		}





		/// <summary>
		/// 将当前请求映射到相应的事件处理程序时发生。
		/// </summary>
		public event EventHandler PostMapRequestHandle;
		internal void Execute_PostMapRequestHandle()
		{
			this.PostMapRequestHandle?.Invoke(_app, null);
		}



		/// <summary>
		/// 开始执行事件处理程序前发生。
		/// </summary>
		public event EventHandler PreRequestHandlerExecute;
		internal void Execute_PreRequestHandlerExecute()
		{
			this.PreRequestHandlerExecute?.Invoke(_app, null);
		}




		/// <summary>
		/// 在事件处理程序执行完毕时发生。
		/// </summary>
		public event EventHandler PostRequestHandlerExecute;
		internal void Execute_PostRequestHandlerExecute()
		{
			this.PostRequestHandlerExecute?.Invoke(_app, null);
		}

        

		/// <summary>
		/// 请求时作为 HTTP 执行管线链中的最后一个事件发生。
		/// </summary>
		public event EventHandler EndRequest;
		internal void Execute_EndRequest()
		{
			this.EndRequest?.Invoke(_app, null);
		}



		/// <summary>
		/// 当引发未处理的异常时发生。
		/// </summary>
		public event EventHandler Error;
		internal void Execute_Error()
		{
			this.Error?.Invoke(_app, null);
		}

	}
}
