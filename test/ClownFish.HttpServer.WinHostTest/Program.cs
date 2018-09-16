using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClownFish.HttpServer.Config;
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
                new StaticFileOption { Ext=".htm", Mine="text/html" },
                new StaticFileOption { Ext=".html", Mine="text/html"},

                new StaticFileOption { Ext=".xml", Mine="application/xml" },
                new StaticFileOption { Ext=".js", Mine="application/javascript" },                
                new StaticFileOption { Ext=".css", Mine="text/css", Cache=3600 * 24 * 365 },

                new StaticFileOption { Ext=".png", Mine="image/png", Cache=3600 * 24 * 365 },
                new StaticFileOption { Ext=".jpg", Mine="image/jpeg", Cache=3600 * 24 * 365 },
                new StaticFileOption { Ext=".gif", Mine="image/gif", Cache=3600 * 24 * 365 },

                new StaticFileOption { Ext=".txt", Mine="text/plain" },
                new StaticFileOption { Ext=".log", Mine="text/plain" },
                

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
