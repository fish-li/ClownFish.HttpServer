using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.Base.Xml;
using ClownFish.HttpServer;
using ClownFish.HttpServer.Config;

namespace ClownFish.StaticFileServer.ConsoleApp
{
    class Program
    {
        public static ServerHost HostInstance { get; private set; }

        static void Main(string[] args)
        {
            if( Start() == false )
                return;

            Console.WriteLine("HTTP监听设置成功。");
            Console.WriteLine("站点网址：" + HostInstance.Option.HttpListenerOptions.First().ToUrl());
            Console.WriteLine("站点目录：" + HostInstance.Option.Website.LocalPath);

            while( true ) {
                if( Console.ReadLine().Is("exit") )
                    return;
            }
        }


        private static bool Start()
        {
            // 检查默认的配置文件是否存在：ServerOption.config
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerOption.config");
            if( File.Exists(configPath) == false ) {
                Console.WriteLine("在前目录下没有配置文件 ServerOption.config");
                return false;
            }


            Init();            


            string[] args = System.Environment.GetCommandLineArgs();

            HostInstance = new ServerHost();

            // 没有指定启动参数，就用 ServerOption.config 做为启动设置
            if( args.Length == 1 ) {
                HostInstance.Run();
                return true;
            }


            // 为了简单，只支持二种命令行参数：
            // 1，指定一个 ServerOption.config 这样的配置文件，要求以 .config 结尾。
            // 2，指定一个目录，表示要将目录做为站点浏览，端口由程序自动选择。这种方式便于从Windows资源管理器中调用。

            string argsPath = args[1];

            if( Directory.Exists(argsPath) ) {

                // 使用当前程序目录的ServerOption.config做为基准参数
                ServerOption option = XmlHelper.XmlDeserializeFromFile<ServerOption>(configPath);
                option.Website.LocalPath = argsPath;

                // 启动 HTTP Server
                HostInstance.Run(option);
                return true;
            }


            if( File.Exists(argsPath) ) {
                if( argsPath.EndsWith(".config", StringComparison.OrdinalIgnoreCase) ) {

                    // 用指定的配置文件 启动 HTTP Server
                    HostInstance.Run(argsPath);
                    return true;
                }
            }

            Console.WriteLine("命令行参数无效");
            return false;
        }


        private static void Init()
        {
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            // 将当前进程添加到防火墙的允许列表中
            NetFwTypeLib.FirewallHelper.AllowApplication(exePath);


            // 强制指定【当前目录】，因为程序有可能是从计划任务中启动的，当前目录是Windows系统目录
            Environment.CurrentDirectory = Path.GetDirectoryName(exePath);
        }

    }
}
