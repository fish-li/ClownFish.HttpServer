using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.TypeExtend;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Web;
using ClownFish.HttpServer.Handlers;
using System.IO;

namespace ClownFish.HttpServer
{
	/// <summary>
	/// 表示当前监听服务运行的参数信息
	/// </summary>
	public sealed class ServerOptions
	{
		internal List<HttpListenerOption> HostAddress { get; } = new List<HttpListenerOption>();

		internal List<Type> ModuleTypeList { get; } = new List<Type>();

		internal List<IHttpHandlerFactory> HttpHandlerFactoryList {get;} = new List<IHttpHandlerFactory>();

		internal string WebsitePath { get; set; }


		private ServerOptions() { }

		/// <summary>
		/// 创建一个ServerOptions实例
		/// </summary>
		/// <returns></returns>
		public static ServerOptions Create()
		{
			return new ServerOptions();
		}

		/// <summary>
		/// 增加一个监听地址
		/// </summary>
		/// <param name="protocol">协议名称， http or https</param>
		/// <param name="ip">要监听的IP地址，要监听本机所有IP，可以指定星号 * </param>
		/// <param name="port">要监听的TCP端口</param>
		/// <returns></returns>
		public ServerOptions AddListenerAddress(string protocol, string ip, int port)
		{
			if( string.IsNullOrEmpty(protocol) )
				throw new ArgumentNullException(nameof(protocol));

			if( string.IsNullOrEmpty(ip) )
				throw new ArgumentNullException(nameof(ip));


			if( port <= 0 )
				throw new ArgumentOutOfRangeException(nameof(port), "监听端口无效。");
			

			this.HostAddress.Add(new HttpListenerOption {
				Protocol = protocol,
				Ip = ip,
				Port = port
			});

			return this;
		}


        /// <summary>
        /// 自动绑定HTTPS端口的SSL证书
        /// </summary>
        /// <returns></returns>
        public ServerOptions AutoSetSslCertificate()
        {
            //TODO: 后续完善
            return this;
        }


		internal void CheckMe()
		{
			if( this.HostAddress.Count == 0 )
				throw new InvalidOperationException("没有设置要监听的端口信息。");

            // 加载当前目录下的所有程序集
             AssemblyLoader.GetBinAssemblyReferences(AppDomain.CurrentDomain.BaseDirectory);
        }


		/// <summary>
		/// 注册HttpModule
		/// </summary>
		/// <param name="moduleType"></param>
		public ServerOptions RegisterModule(Type moduleType)
		{
			if( moduleType == null )
				throw new ArgumentNullException(nameof(moduleType));

			if( moduleType.IsCompatible(typeof(EventSubscriber<HttpModule>)) == false )
				throw new ArgumentException("模块类型必须继承于 EventSubscriber<HttpModule>");

			this.ModuleTypeList.Add(moduleType);

			return this;
		}

		/// <summary>
		/// 注册HttpHandlerFactory
		/// </summary>
		/// <param name="factory"></param>
		public ServerOptions RegisterHttpHandlerFactory(IHttpHandlerFactory factory)
		{
			if( factory == null )
				throw new ArgumentNullException(nameof(factory));


			this.HttpHandlerFactoryList.Add(factory);
			return this;
		}


		/// <summary>
		/// 设置网站根目录
		/// </summary>
		/// <param name="rootPath"></param>
		/// <returns></returns>
		public ServerOptions SetWebsitePath(string rootPath)
		{
			if (string.IsNullOrEmpty(rootPath))
				throw new ArgumentNullException(rootPath);

			if (Directory.Exists(rootPath) == false)
				throw new FileNotFoundException("目录不存在：" + rootPath);

			if (this.WebsitePath != null)
				throw new InvalidOperationException("不允许重复调用 SetWebsiteRootPath() 方法。");

			this.WebsitePath = rootPath;


			this.RegisterHttpHandlerFactory(new StaticFileHandlerFactory());
			return this;
		}
	}


	internal class HttpListenerOption
	{
		public string Protocol;
		public string Ip;
		public int Port;

		public override string ToString()
		{
			return $"{Protocol}://{Ip}:{Port}/";
		}
	}
}
