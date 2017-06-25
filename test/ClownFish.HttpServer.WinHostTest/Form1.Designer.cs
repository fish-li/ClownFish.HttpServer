namespace ClownFish.HttpServer.WinHostTest
{
	partial class Form1
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.chkLogRequest = new System.Windows.Forms.CheckBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.txtRootPath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtMessage = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.listboxUrlLog = new System.Windows.Forms.ListBox();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(786, 746);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.chkLogRequest);
			this.tabPage1.Controls.Add(this.btnStop);
			this.tabPage1.Controls.Add(this.btnStart);
			this.tabPage1.Controls.Add(this.txtRootPath);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.txtPort);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Location = new System.Drawing.Point(4, 4);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(778, 720);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "启动参数";
			// 
			// chkLogRequest
			// 
			this.chkLogRequest.AutoSize = true;
			this.chkLogRequest.Location = new System.Drawing.Point(102, 90);
			this.chkLogRequest.Name = "chkLogRequest";
			this.chkLogRequest.Size = new System.Drawing.Size(252, 16);
			this.chkLogRequest.TabIndex = 6;
			this.chkLogRequest.Text = "记录请求日志（性能压力测试时不要勾选）";
			this.chkLogRequest.UseVisualStyleBackColor = true;
			// 
			// btnStop
			// 
			this.btnStop.Enabled = false;
			this.btnStop.Location = new System.Drawing.Point(304, 132);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(108, 23);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "停止运行（&T）";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(102, 132);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(108, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "启动网站（&R）";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// txtRootPath
			// 
			this.txtRootPath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRootPath.Location = new System.Drawing.Point(102, 56);
			this.txtRootPath.Name = "txtRootPath";
			this.txtRootPath.ReadOnly = true;
			this.txtRootPath.Size = new System.Drawing.Size(310, 22);
			this.txtRootPath.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(26, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "网站目录";
			// 
			// txtPort
			// 
			this.txtPort.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPort.Location = new System.Drawing.Point(102, 21);
			this.txtPort.Name = "txtPort";
			this.txtPort.ReadOnly = true;
			this.txtPort.Size = new System.Drawing.Size(310, 22);
			this.txtPort.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(26, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "监听端口";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage2.Controls.Add(this.linkLabel1);
			this.tabPage2.Controls.Add(this.txtMessage);
			this.tabPage2.Controls.Add(this.splitter1);
			this.tabPage2.Controls.Add(this.treeView1);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Location = new System.Drawing.Point(4, 4);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(778, 720);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "测试用例";
			// 
			// txtMessage
			// 
			this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMessage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtMessage.HideSelection = false;
			this.txtMessage.Location = new System.Drawing.Point(3, 508);
			this.txtMessage.Multiline = true;
			this.txtMessage.Name = "txtMessage";
			this.txtMessage.ReadOnly = true;
			this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtMessage.Size = new System.Drawing.Size(772, 209);
			this.txtMessage.TabIndex = 3;
			this.txtMessage.WordWrap = false;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(3, 501);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(772, 7);
			this.splitter1.TabIndex = 2;
			this.splitter1.TabStop = false;
			// 
			// treeView1
			// 
			this.treeView1.CheckBoxes = true;
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Top;
			this.treeView1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeView1.FullRowSelect = true;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(3, 26);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(772, 475);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			this.treeView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyDown);
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(3, 3);
			this.label3.Name = "label3";
			this.label3.Padding = new System.Windows.Forms.Padding(5);
			this.label3.Size = new System.Drawing.Size(772, 23);
			this.label3.TabIndex = 0;
			this.label3.Text = "选择要运行的用例，按F5执行";
			// 
			// tabPage3
			// 
			this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage3.Controls.Add(this.listboxUrlLog);
			this.tabPage3.Location = new System.Drawing.Point(4, 4);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(778, 720);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "URL日志";
			// 
			// listboxUrlLog
			// 
			this.listboxUrlLog.BackColor = System.Drawing.SystemColors.Control;
			this.listboxUrlLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listboxUrlLog.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listboxUrlLog.FormattingEnabled = true;
			this.listboxUrlLog.HorizontalScrollbar = true;
			this.listboxUrlLog.ItemHeight = 14;
			this.listboxUrlLog.Location = new System.Drawing.Point(3, 3);
			this.listboxUrlLog.Name = "listboxUrlLog";
			this.listboxUrlLog.ScrollAlwaysVisible = true;
			this.listboxUrlLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.listboxUrlLog.Size = new System.Drawing.Size(772, 714);
			this.listboxUrlLog.TabIndex = 0;
			this.listboxUrlLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listboxUrlLog_KeyDown);
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.linkLabel1.Location = new System.Drawing.Point(213, 8);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(77, 12);
			this.linkLabel1.TabIndex = 8;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "运行测试用例";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(786, 746);
			this.Controls.Add(this.tabControl1);
			this.KeyPreview = true;
			this.Name = "Form1";
			this.Text = "ClownFish.HttpServer.WinHostTest";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.TextBox txtRootPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPort;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox listboxUrlLog;
		private System.Windows.Forms.TextBox txtMessage;
		private System.Windows.Forms.CheckBox chkLogRequest;
		private System.Windows.Forms.LinkLabel linkLabel1;
	}
}

