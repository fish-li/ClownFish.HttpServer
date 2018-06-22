using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer;

namespace ClownFish.StaticFileServer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;

            // 将当前进程添加到防火墙的允许列表中
            NetFwTypeLib.FirewallHelper.AllowApplication(exePath);


            // 强制指定【当前目录】，因为程序有可能是从计划任务中启动的，当前目录是Windows系统目录
            Environment.CurrentDirectory = Path.GetDirectoryName(exePath);

            // 启动 HTTP Server
            ServerHost host = new ServerHost();
            host.Run(); // 使用默认的配置文件启动监听服务



            Console.WriteLine("HTTP监听设置成功。");
            Console.WriteLine("站点网址：" + host.Option.HttpListenerOptions.First().ToUrl().TrimEnd('/'));
            Console.WriteLine("站点目录：" + host.Option.Website.LocalPath);

            while( true ) {
                if( Console.ReadLine().Is("exit") )
                    return;
            }
        }


    }
}
