using System;
using System.Collections.Generic;
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
            SetFirewall();

            ServerHost host = new ServerHost();
            host.Run(); // 使用默认的配置文件启动监听服务

            Console.WriteLine("HTTP监听设置成功，" + host.Option.HttpListenerOptions.First().ToUrl().TrimEnd('/'));

            while( true ) {
                if( Console.ReadLine().Is("exit") )
                    return;
            }
        }

        private static void SetFirewall()
        {
            System.Reflection.Assembly entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            string exePath = entryAssembly.Location;

            // 将当前进程添加到防火墙的允许列表中
            NetFwTypeLib.FirewallHelper.AllowApplication(exePath);
        }
    }
}
