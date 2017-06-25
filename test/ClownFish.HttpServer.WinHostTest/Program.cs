using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClownFish.HttpTest;

namespace ClownFish.HttpServer.WinHostTest
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Test();
			Application.Run(new Form1());
		}


		static void Test()
		{
			List<RequestTest> list = new List<RequestTest>();

			RequestTest case1 = new RequestTest();
			list.Add(case1);

			case1.Category = "DemoService";
			case1.Title = "xxxxxxxxxxxxxxxxxxx";
			case1.Request = @"
POST http://localhost:50456/api/xmlcommand/page-2-5/GetProductListByCategoryId.aspx HTTP/1.1
X-Gzip-Respond: 1

CategoryID=1";

			case1.Response = new ResponseAssert();
			case1.Response.StatusCode = 200;
			case1.Response.Headers = new List<ResponseHeaderAssert>();
			case1.Response.Headers.Add(new ResponseHeaderAssert { Name = "Content-Type", AssertMode = "=", Value = "application/json; charset=UTF-8" });
			case1.Response.Headers.Add(new ResponseHeaderAssert { Name = "Cache-Control", AssertMode = "include", Value = "max-age=600" });

			case1.Response.Body = new List<ResponseBodyAssert>();
			case1.Response.Body.Add(new ResponseBodyAssert { Name = "Length", AssertMode = "=", Value = "36" });
			case1.Response.Body.Add(new ResponseBodyAssert { Name = "Text", AssertMode = "include", Value = "font-size: 12px;" });

			ClownFish.Base.Xml.XmlHelper.XmlSerializeToFile(list, "testcase111111.xml");
		}
	}
}
