using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClownFish.HttpTest;

namespace ClownFish.PerformanceTest
{
    public sealed class ThreadParam
    {
        public List<RequestTest> TestList { get; set; }

        public int ThreadCount { get; set; }

        public int RunCount { get; set; }

        public bool InfiniteLoop { get; set; }

        public SynchronizationContext SyncContext { get; set; }

        public Form1 MainForm { get; set; }

    }
}
