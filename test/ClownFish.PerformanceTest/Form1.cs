using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClownFish.Base.WebClient;
using ClownFish.Base.Xml;
using ClownFish.HttpTest;

namespace ClownFish.PerformanceTest
{
    public partial class Form1 : Form
    {
        private SynchronizationContext _syncContext;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _syncContext = SynchronizationContext.Current;

            // 这个不可能超过100核吧
            this.numericUpDown2.Value = System.Environment.ProcessorCount;

            LoadTestCases();
        }


        private void LoadTestCases()
        {
            string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testcase.xml");
            if( File.Exists(file) == false )
                return;

            string fileContent = File.ReadAllText(file, Encoding.UTF8);
            List<RequestTest> list = XmlHelper.XmlDeserialize<List<RequestTest>>(fileContent);

            List<string> groups = (from x in list
                                   group x by x.Category into g
                                   select g.Key
                                   ).ToList();

            foreach( string g in groups ) {
                TreeNode root = new TreeNode(g);

                List<RequestTest> cases = (from x in list
                                           where x.Category == g
                                           select x
                                           ).ToList();

                foreach( RequestTest c in cases ) {
                    TreeNode node = new TreeNode();
                    node.Tag = c;
                    HttpOption option = HttpOption.FromRawText(c.Request);
                    node.Text = option.Url;

                    root.Nodes.Add(node);
                }

                treeView1.Nodes.Add(root);
            }
        }

                
        private void button1_Click(object sender, EventArgs e)
        {
            if( this.button1.Tag == null )
                this.RunTest();
            else
                this.StopTest();
        }


        private void RunTest()
        {
            List<RequestTest> testList = new List<RequestTest>();

            foreach( TreeNode root in treeView1.Nodes )
                GetSelectTest(root, testList);

			if( testList.Count == 0 ) {
				MessageBox.Show("没有选择测试的用例。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

            ThreadParam param = new ThreadParam {
                TestList = testList,
                ThreadCount = (int)numericUpDown2.Value,
                RunCount = (int)numericUpDown1.Value,
                InfiniteLoop = checkBox1.Checked,
                SyncContext = _syncContext,
                MainForm = this
            };

			listBox1.Items.Clear();
            //listBox1.Items.Add(string.Empty);
            //listBox1.Items.Add("============================");

			this.backgroundWorker1.RunWorkerAsync(param);
            this.button1.Tag = this.button1.Text;
            this.button1.Text = "停止测试";
        }

        private void StopTest()
        {
            if( this.backgroundWorker1.IsBusy ) {
                this.backgroundWorker1.CancelAsync();
                this.button1.Enabled = false;
            }
        }


        private void GetSelectTest (TreeNode root, List<RequestTest> testList)
        {
            if( root.Checked && root.Tag != null )
                testList.Add(root.Tag as RequestTest);

            foreach( TreeNode node in root.Nodes )
                GetSelectTest(node, testList);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.button1.Text = (string)this.button1.Tag;
            this.button1.Tag = null;
            this.button1.Enabled = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ThreadParam param = e.Argument as ThreadParam;

            Thread[] threads = new Thread[param.ThreadCount];

            for( int i = 0; i < threads.Length; i++ ) {
                ThreadWorker worker = new ThreadWorker();
                Thread thread = new Thread(worker.Execute);
                thread.IsBackground = true;

                threads[i] = thread;
                thread.Start(param);
            }


            for( int i = 0; i < threads.Length; i++ ) {
                threads[i].Join();
            }
        }



        public void ShowMessage(object state)
        {
            List<string> list = (List<string>)state;

            foreach( string message in list )
                listBox1.Items.Add(message);
        }

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if( e.Node.Nodes.Count == 0 )
				return;

			foreach( TreeNode node in e.Node.Nodes )
				node.Checked = e.Node.Checked;
		}


		public bool NeedStop()
		{
			return this.backgroundWorker1.CancellationPending;
		}
	}
}
