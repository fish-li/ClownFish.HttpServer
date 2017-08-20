using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ClownFish.HttpServer.Config
{
	/// <summary>
	/// 表示整个服务实例的运行参数
	/// </summary>
	[Serializable]
	public sealed class ServerOption
	{
		/// <summary>
		/// HTTP监听参数
		/// </summary>
		[XmlArray("httpListener")]
		[XmlArrayItem("option")]
		public HttpListenerOption[] HttpListenerOptions { get; set; }
		/// <summary>
		/// 需要加载的模块（类型字符串）
		/// </summary>
		[XmlArray("modules")]
		[XmlArrayItem("type")]
		public string[] Modules { get; set; }


		internal Type[] ModuleTypes { get; set; }
		internal Type[] HandlerTypes { get; set; }

		/// <summary>
		/// 需要加载的处理器工厂（类型字符串）
		/// </summary>
		[XmlArray("handlers")]
		[XmlArrayItem("type")]
		public string[] Handlers { get; set; }
		/// <summary>
		/// 站点参数
		/// </summary>
		[XmlElement("website")]
		public WebsiteOption Website { get; set; }
	}

	/// <summary>
	/// HTTP监听参数
	/// </summary>
	[Serializable]
	public sealed class HttpListenerOption
	{
		/// <summary>
		/// 协议，可选范围：http, https
		/// </summary>
		[XmlAttribute("protocol")]
		public string Protocol { get; set; }
		/// <summary>
		/// 需要监听的IP地址
		/// </summary>
		[XmlAttribute("ip")]
		[DefaultValue("*")]
		public string Ip { get; set; }
		/// <summary>
		/// 需要监听的TCP端口
		/// </summary>
		[XmlAttribute("port")]
		public int Port { get; set; }

		/// <summary>
		/// 重写ToString方法
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"{Protocol}://{Ip}:{Port}/";
		}
	}

	/// <summary>
	/// 站点参数
	/// </summary>
	[Serializable]
	public sealed class WebsiteOption
	{
		/// <summary>
		/// 站点的本地路径
		/// </summary>
		[XmlElement("localPath")]
		public string LocalPath { get; set; }
		/// <summary>
		/// 静态文件参数
		/// </summary>
		[XmlArray("staticFile")]
		[XmlArrayItem("option")]
		public StaticFileOption[] StaticFiles { get; set; }
	}

	/// <summary>
	/// 静态文件参数
	/// </summary>
	[Serializable]
	public sealed class StaticFileOption
	{
		/// <summary>
		/// 扩展名
		/// </summary>
		[XmlAttribute("ext")]
		public string Ext { get; set; }
		/// <summary>
		/// 需要缓存的秒数， 
		/// 小于 0  表示不缓存，
		/// 等于 0  表示取默认值（当前版本为 1年）
		/// 大于 0  表示缓存秒数
		/// </summary>
		[XmlAttribute("cache")]
		[DefaultValue(0)]
		public int Cache { get; set; }
		/// <summary>
		/// 扩展名对应的MimeType
		/// </summary>
		[XmlAttribute("mime")]
		public string Mine { get; set; }
	}

	
}
