using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClownFish.Base.WebClient;

namespace ClownFish.HttpServer.PerformanceTestConsoleApplication1
{
	class Program
	{
		/// <summary>
		/// 每轮测试中的循环次数
		/// </summary>
		static readonly int TestCount = 1000*10;

		static void Main(string[] args)
		{
			Console.WriteLine("请先启动 ClownFish.HttpServer.WinHostTest，按回车键开始执行测试");
			Console.ReadLine();

			// 先预热一次
			SendRequest();


			// 长时间运行，用于发现有没有内存泄露问题
			while( true ) {
				Console.WriteLine("每线程测试执行次数：" + TestCount.ToString());
				Console.WriteLine("\r\n");

				// 单线程测试
				Console.WriteLine("开始 单线程测试：");
				RunTest(null);
				Console.WriteLine("\r\n");


				// 多线程测试
				//int threadCount = 32;
				int threadCount = System.Environment.ProcessorCount;
				Console.WriteLine("开始 多线程测试，线程数量：" + threadCount.ToString());
				MultiThreadTest(threadCount);
				Console.WriteLine("\r\n");


				Console.Write("输入 X 退出测试，其它则继续运行测试：");
				//string input = Console.ReadLine();
				//if( string.Equals("X", input, StringComparison.OrdinalIgnoreCase) )
				//	return;
				//else
				Console.WriteLine("\r\n");
			}

		}


		static void RunTest(object xx)
		{
			Stopwatch watch = Stopwatch.StartNew();

			for( int i = 0; i < TestCount; i++ )
				SendRequest();

			watch.Stop();			
			Console.WriteLine($"ThreadId：{Thread.CurrentThread.ManagedThreadId}, " + watch.Elapsed.ToString());
		}


		static void MultiThreadTest(int threadCount)
		{			
			Thread[] threads = new Thread[threadCount];

			
			Stopwatch watch = Stopwatch.StartNew();

			// 创建线程
			for(int i=0;i<threadCount;i++ ) {
				Thread thread = new Thread(RunTest);
				thread.IsBackground = true;
				threads[i] = thread;
			}

			// 启动线程
			for( int i = 0; i < threadCount; i++ ) {
				threads[i].Start();
			}

			// 等待所有线程结束运行
			for( int i = 0; i < threadCount; i++ ) {
				threads[i].Join();
			}

			watch.Stop();
			Console.WriteLine($"{threadCount}线程，总时间：" + watch.Elapsed.ToString());
		}

		

		static void SendRequest()
		{
			try {
				HttpOption option = new HttpOption {
					Url = "http://localhost:50456/hello/ClownFish-HttpServer/demo-ccc/Now.aspx"
					//Url = "http://localhost:50456/hello/ClownFish-HttpServer/demo-ccc/Now.aspx?sleepMillisecondsTimeout=90"
				};
				string response = option.GetResult();
			}
			catch( RemoteWebException webException ) {
				Console.WriteLine(webException.ResponseText);
			}
			catch( Exception ex ) {
				Console.WriteLine(ex.Message);
			}
		}

	}
}
