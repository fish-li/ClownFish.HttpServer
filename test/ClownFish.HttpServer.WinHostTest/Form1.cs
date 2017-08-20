using ClownFish.Base.WebClient;
using ClownFish.HttpServer.DemoServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClownFish.Base.Xml;
using ClownFish.HttpTest;
using System.Management;

namespace ClownFish.HttpServer.WinHostTest
{
	public partial class Form1 : Form
	{
		private ServerHost _host;

		private StringBuilder _message = new StringBuilder();
		private bool _testIsPassed = true;

		private SynchronizationContext _syncContext;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// 为了方便演示，固定二个参数
			this.txtPort.Text = "50456";
			this.txtRootPath.Text = "..\\Website";

			this.LoadTestCases();
			this.SelectAllTestNodes();

			_syncContext = SynchronizationContext.Current;
			DemoHttpModule.OnMessage += DemoHttpModule_OnMessage;
		}

		private void DemoHttpModule_OnMessage(object sender, DemoHttpModule.MessageEventArgs e)
		{
			_syncContext.Post(x => LogReqeust((string)x), e.Message);
		}

		private void LogReqeust(string url)
		{
			if( chkLogRequest.Checked == false )
				return;


			if( this.listboxUrlLog.Items.Count > 1000 ) {
				listboxUrlLog.BeginUpdate();
				for( int i = 0; i < 800; i++ )
					listboxUrlLog.Items.RemoveAt(0);
				listboxUrlLog.EndUpdate();
			}

			listboxUrlLog.Items.Add(url);
		}


		private static string GetComputerName()
		{
			// 取计算机名
			SelectQuery query = new SelectQuery("Win32_ComputerSystem");
			using( ManagementObjectSearcher searcher = new ManagementObjectSearcher(query) ) {
				foreach( ManagementObject mo in searcher.Get() ) {
					if( (bool)mo["partofdomain"] )
						return mo["DNSHostName"].ToString() + "." + mo["domain"].ToString();
				}
			}

			return System.Environment.MachineName;
		}

		private void LoadTestCases()
		{
			string file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testcase.xml");
			if( File.Exists(file) == false )
				return;

			string fileContent = File.ReadAllText(file, Encoding.UTF8)
						.Replace("http://localhost:",
								$"http://{GetComputerName()}:");


			List <RequestTest> list = XmlHelper.XmlDeserialize<List<RequestTest>>(fileContent);

			List<string> groups = (from x in list
								   group x by x.Category into g
								   select g.Key
								   ).ToList();

			foreach(string g in groups ) {
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


		private void btnStart_Click(object sender, EventArgs e)
		{
			if( _host == null) {
				_host = new ServerHost();
				_host.Run();  // 使用默认的配置文件启动监听服务
			}
			else {
				_host.Start();
			}

			btnStart.Enabled = false;
			btnStop.Enabled = true;

			tabControl1.SelectedIndex = 1;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			_host.Stop();

			btnStop.Enabled = false;
			btnStart.Enabled = true;
		}

		private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Nodes.Count == 0)
				return;

			foreach (TreeNode node in e.Node.Nodes)
				node.Checked = e.Node.Checked;
		}

		private void treeView1_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.Control && e.KeyCode == Keys.A) {
				foreach( TreeNode root in treeView1.Nodes ) {
					root.Checked = root.Checked == false;
				}
			}
		}


		private void SelectAllTestNodes()
		{
			foreach( TreeNode root in treeView1.Nodes ) {
				root.Checked = true;
				root.Expand();
			}
		}

		private void listboxUrlLog_KeyDown(object sender, KeyEventArgs e)
		{
			if( e.Control && e.KeyCode == Keys.X ) {
				listboxUrlLog.Items.Clear();
			}

			if( e.KeyCode == Keys.Delete ) {
				listboxUrlLog.BeginUpdate();
				while( listboxUrlLog.SelectedIndices.Count > 0 )
					listboxUrlLog.Items.RemoveAt(listboxUrlLog.SelectedIndex);
				listboxUrlLog.EndUpdate();
			}
		}

		

		private void SendRequest(string request)
		{
			try {
				_message.AppendLine("=========================================================");
				_message.AppendLine(request);
				_message.AppendLine();

				HttpOption option = HttpOption.FromRawText(request);
				string response = option.GetResult();


				_message.AppendLine("### Response ###");
				_message.AppendLine(response);
				_message.AppendLine("\r\n\r\n");

			}
			catch (ClownFish.Base.WebClient.RemoteWebException webException) {
				_message.AppendLine(webException.ResponseText);
			}
			catch (Exception ex) {
				_message.AppendLine(ex.Message);
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			RunSelectedTest();
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F5) {
				RunSelectedTest();
			}
		}

		private void RunSelectedTest()
		{
			if( _host == null ) {
				MessageBox.Show("网站还没有启动。", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			_testIsPassed = true;
			txtMessage.Text = string.Empty;
			Application.DoEvents();


			foreach( TreeNode root in treeView1.Nodes )
				ExecuteNodeRequest(root);

			txtMessage.Text = _message.ToString();
			_message.Clear();

			if( _testIsPassed == false )
				this.txtMessage.BackColor = SystemColors.Info;
			else
				this.txtMessage.BackColor = SystemColors.Control;
		}


		private void ExecuteNodeRequest(TreeNode root)
		{
			if( root.Checked && root.Tag != null )
				//SendRequest((root.Tag as RequestTest).Request);
				ExecuteTest(root.Tag as RequestTest);

			foreach( TreeNode node in root.Nodes )
				ExecuteNodeRequest(node);
		}


		private void ExecuteTest(RequestTest test)
		{
			RequestExecutor executor = new RequestExecutor(test);
			bool isPassed = executor.Execute();

			if( isPassed == false )
				_testIsPassed = false;

			_message.AppendLine("=========================================================");
			_message.AppendLine(test.Request);
			_message.AppendLine();

			if( isPassed ) {
				_message.AppendLine("### Response OK ###");
				_message.AppendLine(executor.Result.ResponseText);
			}
			else {
				_message.AppendLine("### Response ERROR ###");
				_message.AppendLine($"### {executor.ErrorMessage} ###");

				if( executor.Result.ResponseText != null )
					_message.AppendLine(executor.Result.ResponseText);
			}


			_message.AppendLine("\r\n\r\n");

		}

		
	}
}
