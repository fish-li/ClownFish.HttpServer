using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClownFish.Base.Xml;
using ClownFish.HttpServer;
using ClownFish.HttpServer.Config;
using ClownFish.KitLib;

namespace ClownFish.StaticFileServer.WinApp
{
    internal static class HttpServerLauncher
    {
        public static ServerHost HostInstance { get; private set; }  // 保留一个静态引用，以免被GC回收


        private static void Init()
        {
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            // 将当前进程添加到防火墙的允许列表中
            FirewallHelper.AllowApplication(exePath);


            // 强制指定【当前目录】，因为程序有可能是从计划任务中启动的，当前目录是Windows系统目录
            Environment.CurrentDirectory = Path.GetDirectoryName(exePath);
        }


        public static void Start()
        {
            string defalutConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerOption.config");
            if( File.Exists(defalutConfigPath) == false )
                throw new ApplicationException("程序启动目录下没有找到配置文件 ServerOption.config");


            Init();

            string[] args = Environment.GetCommandLineArgs();

            // 没有指定启动参数，那么用默认的配置文件启动
            if( args.Length == 1 ) {
                StartByConfig(defalutConfigPath);
                return;
            }

            
            // 为了简单，只支持二种类型的命令行参数：
            // 1，指定一个 ServerOption.config 这样的配置文件，要求以 .config 结尾。
            // 2，指定一个目录，表示要将目录做为站点浏览，端口由程序自动选择。这种方式便于从Windows资源管理器中调用。

            string argsPath = args[1];

            if( Directory.Exists(argsPath) ) {

                // 先检查参数指定的目录下有没有ServerOption.config，如果存在就使用
                string configPath = Path.Combine(argsPath, "ServerOption.config");
                if( File.Exists(configPath) ) {
                    StartByConfig(configPath);
                    return;
                }


                // 使用当前程序目录的ServerOption.config做为基准参数
                ServerOption option = LoadConfig(defalutConfigPath);
                // 修改参数中的【站点目录】
                option.Website.LocalPath = argsPath;
                
                // 以参数方式启动
                StartByConfig(option);
                return;
            }


            if( File.Exists(argsPath) ) {
                if( argsPath.EndsWith(".config", StringComparison.OrdinalIgnoreCase) ) {

                    StartByConfig(argsPath);
                    return;
                }
            }

            // 参数无效
            throw new ApplicationException("无效的启动参数：" + argsPath);
        }

        private static ServerOption LoadConfig(string configPath)
        {
            try {
                return XmlHelper.XmlDeserializeFromFile<ServerOption>(configPath);
            }
            catch( Exception ex ) {
                throw new ApplicationException($"读取配置文件 {configPath} 发生异常，错误原因：{ex.Message}");
            }
        }


        private static void StartByConfig(string configPath)
        {
            if( File.Exists(configPath) == false) 
                throw new ApplicationException("指定的配置文件不存在：" + configPath);
            

            // 启动 HTTP Server
            ServerOption option = LoadConfig(configPath);
            StartByConfig(option);
        }


        private static void StartByConfig(ServerOption option)
        {
            // 启动 HTTP Server
            HostInstance = new ServerHost();
            HostInstance.Run(option);
        }
    }
}
