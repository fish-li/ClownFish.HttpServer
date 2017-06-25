using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Routing;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer
{
	/// <summary>
	/// 用于监听HTTP请求的服务器类型
	/// </summary>
	public sealed class ServerHost : IDisposable
	{
		private HttpListener _listener;

		private string _websitePath;

		/// <summary>
		/// 获取一个值，指示 HttpListener 是否已启动。
		/// </summary>
		public bool IsListening {
			get {
				if( _listener == null )
					throw new InvalidOperationException("需要先调用 Run 方法。");
				return _listener.IsListening;
			}
		}


		/// <summary>
		/// 启动HTTP请求监听
		/// </summary>
		public void Run(ServerOptions options)
		{
			if( _listener != null )
				throw new InvalidOperationException("不能多次调用Run方法。");

			// 检查参数是否完整
			options.CheckMe();

			this._websitePath = options.WebsitePath;


			// 注册HttpModule
			foreach(Type moduleType in options.ModuleTypeList)
				ExtenderManager.RegisterSubscriber(moduleType);

			// 注册HttpHandlerFactory
			foreach( var factory in options.HttpHandlerFactoryList )
				HttpHandlerFactory.GetInstance().RegisterFactory(factory);

			// 加载所有路由规则
			RoutingManager.Instance.LoadRoutes();


			// 创建HttpListener实例并初始化
			InitHttpListener(options);
			this.Start();


			// 启动后台线程响应所有请求
			Thread thread = new Thread(ServerProc);
			thread.IsBackground = true;
			thread.Start();
		}


		private void InitHttpListener(ServerOptions options)
		{
			if( HttpListener.IsSupported == false )
				throw new NotSupportedException("当前操作系统版本太低，不支持HttpListener");

			_listener = new HttpListener();

			// 允许匿名访问
			_listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

			// 监听端口
			foreach( var t in options.HostAddress ) 
				_listener.Prefixes.Add(t.ToString());
		}


		/// <summary>
		/// 继续监听请求（仅在Stop后调用）
		/// </summary>
		public void Start()
		{
			if( _listener == null )
				throw new InvalidOperationException("不能直接调用Start方法，请先调用Run方法。");

			if( _listener.IsListening )
				throw new InvalidOperationException("HttpListener的实例已经启动了，不要重复调用Start方法。");

			_listener.Start();
		}

		/// <summary>
		/// 暂停监听请求（然后调用Start可继续监听）
		/// </summary>
		public void Stop()
		{
			if( _listener == null )
				throw new InvalidOperationException("不能直接调用Start方法，请先调用Run方法。");

			_listener.Stop();
		}

		/// <summary>
		/// 关闭 HttpListener
		/// </summary>
		public void Dispose()
		{
			if( _listener != null ) {
				_listener.Close();
				_listener = null;
			}
		}


		private void ServerProc(object runOptions)
		{
			while( true ) {
				if( _listener.IsListening == false ) {
					// 如果没有监听，就休眠一下
					System.Threading.Thread.Sleep(1000);
					continue;
				}

				try {
					HttpListenerContext context = _listener.GetContext();
					ProcessRequest(context);
				}
				catch( HttpListenerException ) {
					// TODO: 暂时先不处理这个异常，以后再来优化
				}
			}
		}


		private void ProcessRequest(HttpListenerContext context)
		{
			HttpContext httpContext = new HttpContext(context);
			httpContext.Request.WebsitePath = _websitePath;


			HttpApplication app = HttpApplication.GetInstance(httpContext);


			// 进入线程池排除队
			ThreadPool.QueueUserWorkItem(app.ProcessRequest, null);

			// 模拟后台线程崩溃
			//throw new NotImplementedException("xxxxxxxxxxx模拟后台线程崩溃xxxxxxxxxx");
		}

		
	}
}
