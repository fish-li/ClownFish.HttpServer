using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClownFish.StaticFileServer.WinApp
{
    public partial class MainForm1 : Form
    {
        public MainForm1()
        {
            InitializeComponent();
        }

        private void MainForm1_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Icon = Properties.Resources.mwn;
            this.notifyIcon1.Icon = Properties.Resources.mwn;


            string url = HttpServerLauncher.HostInstance.Option.HttpListenerOptions.First().ToUrl();
            string path = HttpServerLauncher.HostInstance.Option.Website.LocalPath;

            this.MenuItemLocalPath.Text += path;
            this.MenuItemSiteUrl.Text += url;

            this.notifyIcon1.BalloonTipTitle = "ClownFish.StaticFileServer";
            //this.notifyIcon1.Text = $"站点网址：{url}\r\n站点目录：{path}";     // 有可能太长，超过限制长度
            this.notifyIcon1.Text = url;
            this.notifyIcon1.BalloonTipText = $"站点网址：{url}\r\n站点目录：{path}";
            this.notifyIcon1.ShowBalloonTip(3000);

            this.MenuItemOpen_Click(null, null);
        }

        private void MenuItemOpen_Click(object sender, EventArgs e)
        {
            string url = HttpServerLauncher.HostInstance.Option.HttpListenerOptions.First().ToUrl();
            System.Diagnostics.Process.Start(url);
        }

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            //this.MenuItemOpen_Click(null, null);
        }
    }
}
