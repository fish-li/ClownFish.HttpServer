using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer;

namespace ClownFish.RequestLogConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            Init();

            ServerHost httpHost = new ServerHost();
            httpHost.Run();

            Console.WriteLine("HTTP监听设置成功。");
            Console.WriteLine("站点网址：" + httpHost.Option.HttpListenerOptions.First().ToUrl());
            Console.WriteLine("EXIT: 退出模拟服务，cls: 清屏。");

            while( true ) {
                string input = Console.ReadLine();
                if( input.Is("exit") ) {
                    return;
                }
                if( input.Is("cls") ) {
                    Console.Clear();
                }
            }
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
