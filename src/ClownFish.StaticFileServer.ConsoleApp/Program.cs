using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using HttpServerLauncher=ClownFish.StaticFileServer.WinApp.HttpServerLauncher;

namespace ClownFish.StaticFileServer.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try {
                HttpServerLauncher.Start();
            }
            catch( Exception ex ) {
                Console.WriteLine(ex.ToString());

                System.Threading.Thread.Sleep(60 * 1000);
                return;
            }

            string url = HttpServerLauncher.HostInstance.Option.HttpListenerOptions.First().ToUrl();
            string dir = HttpServerLauncher.HostInstance.Option.Website.LocalPath;

            Console.WriteLine("\r\n ClownFish.HttpServer 启动成功");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(" 站点网址：" + url);
            Console.WriteLine(" 站点目录：" + dir);

            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine(" exit: 退出程序。");
            Console.WriteLine(" open: 打开站点。");
            Console.WriteLine(" dir : 打开目录。");
            Console.WriteLine("----------------------------------------------------------------");


            while( true ) {
                String action = Console.ReadLine();

                if( "open".EqualsIgnoreCase(action) ) {                    
                    System.Diagnostics.Process.Start(url);
                    continue;
                }

                if( "dir".EqualsIgnoreCase(action) ) {
                    System.Diagnostics.Process.Start(dir);
                    continue;
                }

                if( "exit".EqualsIgnoreCase(action) )
                    return;
            }
        }
        

    }
}
