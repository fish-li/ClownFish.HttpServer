using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClownFish.HttpTest;

namespace ClownFish.PerformanceTest
{
    internal class ThreadWorker
    {
        private List<string> _message;


        public void Execute(object obj)
        {
            ThreadParam param = obj as ThreadParam;

			try {
				for( ;;) {
					// 按照参数，运行一轮测试
					RunOneLoop(param);

					// 判断是否要继续运行测试
					if( param.InfiniteLoop == false )
						break;
				}
			}
			catch( StopTestException ) { /* 停止测试  */ }
		}


        private void RunOneLoop(ThreadParam param)
        {
            _message = new List<string>();
            Stopwatch watch = Stopwatch.StartNew();

			try {
				for( int i = 0; i < param.RunCount; i++ ) {
					foreach( var test in param.TestList ) {

						if( param.MainForm.NeedStop() )
							throw new StopTestException();

						this.ExecuteTest(test);
					}
				}
			}
			catch( StopTestException ) { /* 停止测试  */
				throw;
			}
			catch( Exception ex ) {
				_message.Add("### Exception: " + ex.Message);
			}
			finally {
				watch.Stop();
			}

            //_message.Add("============================");
            _message.Add($"Thread {Thread.CurrentThread.ManagedThreadId} 运行结束，消耗时间：{watch.Elapsed.ToString()}");
            //_message.Add("============================");

            param.SyncContext.Post(param.MainForm.ShowMessage, _message);
        }

        public void ExecuteTest(RequestTest test)
        {
            RequestExecutor executor = new RequestExecutor(test);
            bool isPassed = executor.Execute();

            if( isPassed == false ) {
                _message.Add($"### Response ERROR ###  {test.Title}");
                _message.Add($"### {executor.ErrorMessage} ###");
            }
        }

    }
}
