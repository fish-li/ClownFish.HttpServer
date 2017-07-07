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
        private ServerOptions _options;
        private Thread _workThread;


        /// <summary>
        /// 获取一个值，指示 HttpListener 是否已启动。
        /// </summary>
        public bool IsListening {
			get {
				if( _listener == null )
                    return false;

                return _listener.IsListening;
			}
		}


		/// <summary>
		/// 启动HTTP请求监听
		/// </summary>
		public void Run(ServerOptions options)
		{
			if( _options != null )
				throw new InvalidOperationException("不要重复调用Run方法。");

			// 检查参数是否完整
			options.CheckMe();
            _options = options.Clone();


			// 注册HttpModule
			foreach(Type moduleType in options.ModuleTypeList)
				ExtenderManager.RegisterSubscriber(moduleType);

			// 注册HttpHandlerFactory
			foreach( var factory in options.HttpHandlerFactoryList )
				HttpHandlerFactory.GetInstance().RegisterFactory(factory);

			// 加载所有路由规则
			RoutingManager.Instance.LoadRoutes();



            // 启动后台线程响应所有请求
            _workThread = new Thread(ServerProc);
            _workThread.IsBackground = true;
            _workThread.Start();

            // 创建HttpListener实例并初始化
            this.Start();
        }


		private HttpListener CreaetetHttpListener()
		{
			if( HttpListener.IsSupported == false )
				throw new NotSupportedException("当前操作系统版本太低，不支持HttpListener");

            HttpListener listener = new HttpListener();

            // 允许匿名访问
            listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;

			// 监听端口
			foreach( var t in _options.HostAddress )
                listener.Prefixes.Add(t.ToString());

            listener.Start();
            return listener;
        }


		/// <summary>
		/// 继续监听请求（仅在Stop后调用）
		/// </summary>
		public void Start()
		{
            if( _listener != null && _listener.IsListening )
                throw new InvalidOperationException("HttpListener的实例已经启动了，不要重复调用Start方法。");


            // 不重用 HttpListener 的实例，
            // 如果启动时失败，状态会标记为关闭，后面根本就不能再开启监听
            _listener = CreaetetHttpListener();
		}

		/// <summary>
		/// 暂停监听请求（然后调用Start可继续监听）
		/// </summary>
		public void Stop()
		{
            if( _listener == null )
                return;

            // 不重用 HttpListener 的实例，
            // 如果启动时失败，状态会标记为关闭，后面根本就不能再开启监听
            this.Dispose();
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
                if( _listener == null || _listener.IsListening == false ) {
                    // 如果没有监听，就休眠一下
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                if( _listener == null || _listener.IsListening == false ) {
                    continue;
                }

                try {
					HttpListenerContext context = _listener.GetContext();
					ProcessRequest(context);
				}
                catch( ObjectDisposedException ) { // 此对象已关闭。 
                }
                catch( InvalidOperationException ) { // 此对象尚未启动或当前已停止。
                }
                catch( HttpListenerException ) {
                    // TODO: 暂时先不处理这个异常，以后再来优化
                }
            }
		}


		private void ProcessRequest(HttpListenerContext context)
		{
			HttpContext httpContext = new HttpContext(context);
			httpContext.Request.WebsitePath = _options.WebsitePath;


			HttpApplication app = HttpApplication.GetInstance(httpContext);


			// 进入线程池排除队
			ThreadPool.QueueUserWorkItem(app.ProcessRequest, null);

			// 模拟后台线程崩溃
			//throw new NotImplementedException("xxxxxxxxxxx模拟后台线程崩溃xxxxxxxxxx");
		}

		
	}
}
