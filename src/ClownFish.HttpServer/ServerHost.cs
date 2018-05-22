using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Config;
using ClownFish.HttpServer.Handlers;
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
        private ServerOption _option;
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
		/// 根据指定的参数对象启动监听服务
		/// </summary>
		/// <param name="option"></param>
		public void Run(ServerOption option)
		{
			if( option == null )
				throw new ArgumentNullException(nameof(option));

			// 先将参数克隆一份，避免调用代码后期修改参数对象
			ServerOption option2 = option.CloneObject();
			InternalInit(option2);
		}

		/// <summary>
		/// 根据指定的配置文件启动监听服务
		/// </summary>
		/// <param name="configPath"></param>
		public void Run(string configPath)
		{
			if( string.IsNullOrEmpty(configPath) )
				throw new ArgumentNullException(nameof(configPath));

			if( File.Exists(configPath) == false )
				throw new FileNotFoundException("指定的配置文件不存在：" + configPath);


			ServerOption option = ClownFish.Base.Xml.XmlHelper.XmlDeserializeFromFile<ServerOption>(configPath);
			InternalInit(option);
		}


		/// <summary>
		/// 使用默认的配置文件启动监听服务，
		/// 默认配置文件：当前目录下的ServerOption.config
		/// </summary>
		public void Run()
		{
			string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerOption.config");
			Run(configPath);
		}


		private void InternalInit(ServerOption option)
		{
			if( _option != null )
				throw new InvalidOperationException("不要重复调用Run方法。");

			ServerOptionValidator validator = new ServerOptionValidator();
			validator.Validate(option);
			validator.SetDefaultValues(option);


			// 注册HttpHandlerFactory
			foreach( Type handlerType in option.HandlerTypes ) {
				var factory = Activator.CreateInstance(handlerType) as IHttpHandlerFactory;
				HttpHandlerFactory.GetInstance().RegisterFactory(factory);
			}


			// 注册HttpModule
			foreach( Type moduleType in option.ModuleTypes )
				ExtenderManager.RegisterSubscriber(moduleType);

			if( option.Website != null ) {
				StaticFileHandlerFactory.Init(option);
				StaticFileHandler.Init(option);
			}

			

			// 加载当前目录下的所有程序集
			AssemblyLoader.LoadAssemblies(AppDomain.CurrentDomain.BaseDirectory);

			// 加载所有路由规则
			RoutingManager.Instance.LoadRoutes();

			_option = option;


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
			foreach( var t in _option.HttpListenerOptions )
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

			if( _option.Website != null )
				httpContext.Request.WebsitePath = _option.Website.LocalPath;


			HttpApplication app = HttpApplication.GetInstance(httpContext);


			// 进入线程池排除队
			ThreadPool.QueueUserWorkItem(app.ProcessRequest, null);

			// 模拟后台线程崩溃
			//throw new NotImplementedException("xxxxxxxxxxx模拟后台线程崩溃xxxxxxxxxx");
		}

		
	}
}
