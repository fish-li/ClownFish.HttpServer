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
using System.Management;

namespace ClownFish.HttpServer.WinHostTest
{
	public partial class Form1 : Form
	{
		private ServerHost _host;

		private StringBuilder _message = new StringBuilder();

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

			_syncContext = SynchronizationContext.Current;
			DemoHttpModule.OnMessage += DemoHttpModule_OnMessage;
		}

		private void DemoHttpModule_OnMessage(object sender, DemoHttpModule.MessageEventArgs e)
		{
			_syncContext.Post(x => LogReqeust((string)x), e.Message);
		}

        private void LogReqeust(string message)
        {
            if( chkLogRequest.Checked == false )
                return;


            if( this.listboxUrlLog.Items.Count > 1000 ) {
                listboxUrlLog.BeginUpdate();
                for( int i = 0; i < 800; i++ )
                    listboxUrlLog.Items.RemoveAt(0);
                listboxUrlLog.EndUpdate();
            }

            string[] lines = message.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            listboxUrlLog.BeginUpdate();
            foreach( string line in lines )
                listboxUrlLog.Items.Add(line);
            listboxUrlLog.EndUpdate();
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

		




		
	}
}
