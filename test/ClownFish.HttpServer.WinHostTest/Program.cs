using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClownFish.HttpServer.Config;
using ClownFish.HttpTest;
using NetFwTypeLib;

namespace ClownFish.HttpServer.WinHostTest
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

            SetFirewall();

            //Create_Testcase_Xml();
            Create_ServerOption_Config();
			Application.Run(new Form1());
		}


		static void Create_Testcase_Xml()
		{
			List<RequestTest> list = new List<RequestTest>();

			RequestTest case1 = new RequestTest();
			list.Add(case1);

			case1.Category = "DemoService";
			case1.Title = "xxxxxxxxxxxxxxxxxxx";
			case1.Request = @"
POST http://localhost:50456/api/xmlcommand/page-2-5/GetProductListByCategoryId.aspx HTTP/1.1
X-Gzip-Respond: 1

CategoryID=1";

			case1.Response = new ResponseAssert();
			case1.Response.StatusCode = 200;
			case1.Response.Headers = new List<ResponseHeaderAssert>();
			case1.Response.Headers.Add(new ResponseHeaderAssert { Name = "Content-Type", AssertMode = "=", Value = "application/json; charset=utf-8" });
			case1.Response.Headers.Add(new ResponseHeaderAssert { Name = "Cache-Control", AssertMode = "include", Value = "max-age=600" });

			case1.Response.Body = new List<ResponseBodyAssert>();
			case1.Response.Body.Add(new ResponseBodyAssert { Name = "Length", AssertMode = "=", Value = "36" });
			case1.Response.Body.Add(new ResponseBodyAssert { Name = "Text", AssertMode = "include", Value = "font-size: 12px;" });

			ClownFish.Base.Xml.XmlHelper.XmlSerializeToFile(list, "testcase111111.xml");
		}


		static void Create_ServerOption_Config()
		{
			if( File.Exists("ServerOption.config") )
				return;

			ServerOption option = new ServerOption();
			option.HttpListenerOptions = new HttpListenerOption[] {
				new HttpListenerOption {
				Protocol = "http",
				//Ip = "*",
				Port = 50456
			}};


			option.Modules = new string[] { "ClownFish.HttpServer.DemoServices.DemoHttpModule, ClownFish.HttpServer.DemoServices" };
			option.Handlers = new string[] { "ClownFish.HttpServer.Handlers.StaticFileHandlerFactory, ClownFish.HttpServer" };

			option.Website = new WebsiteOption();
			option.Website.LocalPath = "..\\Website";
            option.Website.CacheDuration = 3600 * 24 * 365;
            option.Website.StaticFiles = new StaticFileOption[] {
				new StaticFileOption { Ext=".htm", Mine="text/html" },     // 隐式使用默认值
				new StaticFileOption { Ext=".html", Cache=0 },              // 显示使用默认值
				new StaticFileOption { Ext=".js", Cache=3600000 },          // 指定固定值
				new StaticFileOption { Ext=".css", Cache=3600 * 24 * 365 },
				new StaticFileOption { Ext=".png", Cache=3600 * 24 * 365 },
				new StaticFileOption { Ext=".jpg", Cache=3600 * 24 * 365 },
				new StaticFileOption { Ext=".gif", Cache=3600 * 24 * 365 },

				new StaticFileOption { Ext=".eot" },
				new StaticFileOption { Ext=".svg" },
				new StaticFileOption { Ext=".ttf" },
				new StaticFileOption { Ext=".woff" },
				new StaticFileOption { Ext=".woff2" },
				new StaticFileOption { Ext=".map" }
			};

			ClownFish.Base.Xml.XmlHelper.XmlSerializeToFile(option, "ServerOption.config");
			
		}


        static void SetFirewall()
        {
            FirewallHelper.AllowApplication(Application.ExecutablePath);

            FirewallHelper.AllowTcpPort(50457, "50457-测试规则");
            FirewallHelper.AllowUdpPort(50458, "50458-测试规则");
        }

    }
}
