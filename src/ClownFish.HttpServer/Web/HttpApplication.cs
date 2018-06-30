using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Handlers;
using ClownFish.HttpServer.Result;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 表示用于执行HTTP请求的任务过程
	/// </summary>
	public sealed class HttpApplication
	{
		/// <summary>
		/// HttpContext实例
		/// </summary>
		public HttpContext Context { get; private set; }

		private HttpModule _httpModule;


		internal HttpApplication()
		{
			// 允许事件订阅
			_httpModule = ObjectFactory.New<HttpModule>();
			_httpModule.SetHttpApplication(this);

			// 说明：由于事件订阅会影响对象创建的性能，所以HttpApplication的实例会重用
		}


		internal static HttpApplication GetInstance(HttpContext context)
		{
			HttpApplication app = HttpApplicationFactory.Instance.Get();
			app.Context = context;
            context.Application = app;
			return app;
		}

		private void Dispose()
		{
			if( this.Context != null ) {

				// 去掉引用, Application对象还需要回收
				this.Context.Application = null;

				this.Context.Dispose();
				this.Context = null;
			}
		}


		/// <summary>
		/// HttpRequest实例的引用
		/// </summary>
		public HttpRequest Request {
            get {return  this.Context.Request; }
        }

        /// <summary>
        /// HttpResponse实例的引用
        /// </summary>
        public HttpResponse Response {
            get { return this.Context.Response; }
        }



		/// <summary>
		/// 跳过 HTTP 执行管线链中的所有事件和筛选并直接执行 EndRequest 事件。
		/// </summary>
		public void CompleteRequest()
		{
			throw new HttpApplicationEndRequestException();
		}

		/// <summary>
		/// 处理HTTP请求（入口方法）
		/// </summary>
		internal async Task ProcessRequest(/* object xxx */)
		{
			try {
				// 设置基本的响应头
				this.Context.Response.SetBasicHeaders();
				
				_httpModule.Execute_BeginRequest();	// event

                // 验证请求
				_httpModule.Execute_AuthenticateRequest();  // event
				_httpModule.Execute_PostAuthenticateRequest();  // event
                

				// 查找能处理当前请求的HttpHandler
				_httpModule.Execute_PreMapRequestHandle();  // event
				if( this.Context.HttpHandler == null )
					this.Context.HttpHandler = GetHandler();
				_httpModule.Execute_PostMapRequestHandle(); // event

                // 授权验证
                _httpModule.Execute_AuthorizeRequest(); // event
                _httpModule.Execute_PostAuthorizeRequest(); // event

                // 执行 HttpHandler 的初始化
                InitHttpHandler();

                // 执行HttpHandler前的准备工作
                _httpModule.Execute_PreRequestHandlerExecute(); // event
                BeforeProcessRequest();

                //this.Response.AppendHeader("x-DEBUG-BeforeProcessRequest", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

                // 处理请求
                IActionResult result = await ExecuteHandler();
                this.Context.ActionResult = result;

                //this.Response.AppendHeader("x-DEBUG-AfterProcessRequest", System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                
                _httpModule.Execute_PostRequestHandlerExecute();    // event

                // 输出结果
                if( result != null )
                    result.Ouput(this.Context);

                // 后面的事件暂且不实现
            }
			catch( HttpApplicationEndRequestException ) { /* 这里就是一个标记异常，所以直接吃掉 */ }

            //catch( System.Web.HttpException httpException ) {   // 允许代码中抛出异常，中止请求
            //    this.Response.StatusCode = httpException.GetHttpCode();
            //    this.Response.ContentType = ResponseContentType.Text;
            //    this.Response.Write(httpException.Message);
            //}
			catch( Exception ex ) {
				this.Context.LastException = ex;

				// 引发错误事件
				_httpModule.Execute_Error();  // event

				// 如果异常没有被清除，就使用默认的方式处理
				if( this.Context.LastException != null ) {
					ProcessException(ex);
				}
			}
			finally {
				try {
					// 结束整个请求
					_httpModule.Execute_EndRequest();   // event
				}
				catch {	/* 这里吃掉所有异常  */ }

				FinallyReqest();
			}
		}


        private void InitHttpHandler()
        {
            IHttpHandlerInitializer handler = this.Context.HttpHandler as IHttpHandlerInitializer;
            if( handler != null ) {
                handler.Init(this.Context);
            }
        }

        private async Task<IActionResult> ExecuteHandler()
        {
            if( this.Context.HttpHandler is ITaskHttpHandler ) {
                // 只执行Action方法
                ITaskHttpHandler handler2 = this.Context.HttpHandler as ITaskHttpHandler;
                return await handler2.ProcessRequestAsync(this.Context);
            }
            else if( this.Context.HttpHandler is IHttpHandler2 ) {
                // 只执行Action方法
                IHttpHandler2 handler2 = this.Context.HttpHandler as IHttpHandler2;
                return handler2.ProcessRequest2(this.Context);
            }
            else {
                // 老的IHttpHandler，整个过程合并在一起
                this.Context.HttpHandler.ProcessRequest(this.Context);
                return null;
            }
        }

		private void BeforeProcessRequest()
		{
			// 如果客户端期望启用GZIP压缩
			if( this.Context.Request.Headers["X-Gzip-Respond"] == "1" )
				this.Context.Response.EnableGzip();
		}

		private void ProcessException(Exception ex)
		{
			try {
				ErrorHandler errorHandler = new ErrorHandler();
				errorHandler.Exception = ex;
				errorHandler.ProcessRequest(this.Context);
			}
			catch(Exception ex2) {
				string message = ex2.Message;
				// 这里再不处理异常，程序就崩溃了。
			}
		}

		private void FinallyReqest()
		{
            if( this.Context.HttpHandler != null ) {
                IDisposable disposable = this.Context.HttpHandler as IDisposable;

                if( disposable != null ) {
                    try {
                        disposable.Dispose();
                    }
                    catch { // 这里如果出现异常，就直接吃掉
                    }
                }
            }


			// 关闭连接，释放相关资源
			this.Dispose();

			// 回收当前实例，给后续请求使用
			HttpApplicationFactory.Instance.Recycle(this);
		}


		private IHttpHandler GetHandler()
		{
			// 根据请求上下文创建对应的处理器实例
			IHttpHandler handler = HttpHandlerFactory.GetInstance().CreateHandler(this.Context);
			return handler;
		}



		


	}
}
