using ClownFish.Base;
using ClownFish.Data;
using ClownFish.Data.SqlClient;
using ClownFish.Data.Xml;
using ClownFish.HttpServer.Attributes;
using ClownFish.HttpServer.Common;
using ClownFish.HttpServer.Result;
using ClownFish.HttpServer.Routing;
using ClownFish.HttpServer.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.DemoServices
{
	[Route]
	public class XmlCommandService
	{
		private static bool s_inited = false;
		private static readonly object s_lock = new object();

		private LazyObject<StringConverter> _stringConverter = new LazyObject<StringConverter>();


		public static void Init()
		{
			if( s_inited == false) {
				lock (s_lock) {
					if( s_inited == false) {
						InternalInit();
						s_inited = true;
					}
				}
			}
		}

		private static void InternalInit()
		{
			string directory = ConfigurationManager.AppSettings["XmlCommand-Directory"];
			if (string.IsNullOrEmpty(directory) == false)
				directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);

			ClownFish.Data.Initializer.Instance
					.LoadXmlCommandFromDirectory(directory)
					.InitConnection()
					.AllowCreateOneOffDbContext();
		}



		//[RouteUrl("/api/xmlcommand/show/{x_name}.aspx", UrlType.Pattern)]
		//public TextResult Show(string x_name)
		//{
		//	XmlCommandItem command = GetXmlCommand(x_name);
		//	string xml = command.ToXml();
		//	return new TextResult(xml, ResponseContentType.Xml);
		//}

		/// <summary>
		/// 运行XmlCommand，主要用于包含 insert, delete, update 语句的场景
		/// </summary>
		/// <param name="x_name">XmlCommand名称</param>
		/// <param name="form">表单数据集合</param>
		/// <returns>影响的记录行数</returns>
		[RouteUrl("/api/xmlcommand/execute/{x_name}.aspx", UrlType.Pattern)]
		public object Execute(string x_name, [FromRequest]NameValueCollection form)
		{
			// name 这个参数名太常见，所以就改成 x_name

			XmlCommandItem command = GetXmlCommand(x_name);

			Dictionary<string, object> args = null;
			if (command.Parameters != null || command.Parameters.Count > 0)
				args = GetCommandParameters(command, form);

			XmlCommand xmlCommand = XmlCommand.Create(x_name, args);
			int result = xmlCommand.ExecuteNonQuery();

			// 处理输出参数，回填到参数字典中返回给前端
			BackFillOutputArgs(xmlCommand, args);

			return new { args = args, data = result };
		}


		/// <summary>
		/// 运行XmlCommand，主要用于包含 select 语句的场景
		/// </summary>
		/// <param name="x_name">XmlCommand名称</param>
		/// <param name="form">表单数据集合</param>
		/// <returns>查询到的数据结果</returns>
		[RouteUrl("/api/xmlcommand/query/{x_name}.aspx", UrlType.Pattern)]
		public object Query(string x_name, [FromRequest]NameValueCollection form)
		{
			XmlCommandItem command = GetXmlCommand(x_name);

			Dictionary<string, object> args = null;
			if (command.Parameters != null || command.Parameters.Count > 0)
				args = GetCommandParameters(command, form);

			XmlCommand xmlCommand = XmlCommand.Create(x_name, args);
			DataTable table = xmlCommand.ToDataTable();

			// 处理输出参数，回填到参数字典中返回给前端
			BackFillOutputArgs(xmlCommand, args);

			return new { args = args, data = table };
		}


		private void BackFillOutputArgs(XmlCommand xmlCommand, Dictionary<string, object> args)
		{
			foreach( DbParameter p in xmlCommand.Command.Parameters ) {
				if( p.Direction == ParameterDirection.InputOutput )
					// 输出参数回写到参数对象上
					args[p.ParameterName.TrimStart('@')] = p.Value;
			}
		}



		/// <summary>
		/// 执行一个分页查询，XmlCommand中只需要包含一个查询，由ClownFish.Data组装成二条语句来查询，分别获取分页的数据和总行数
		/// </summary>
		/// <param name="x_name"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <param name="form"></param>
		/// <returns></returns>
		[RouteUrl("/api/xmlcommand/page-{pageIndex}-{pageSize}/{x_name}.aspx", UrlType.Pattern)]
		public object Paging(string x_name, int pageIndex, int pageSize, [FromRequest]NameValueCollection form)
		{
			XmlCommandItem command = GetXmlCommand(x_name);

			Dictionary<string, object> args = null;
			if (command.Parameters != null || command.Parameters.Count > 0)
				args = GetCommandParameters(command, form);

			PagingInfo info = new PagingInfo { PageIndex = pageIndex, PageSize = pageSize };
			XmlCommand xmlCommand = XmlCommand.Create(x_name, args);
			DataTable table = xmlCommand.ToPageTable(info);

			// 处理输出参数，回填到参数字典中返回给前端
			BackFillOutputArgs(xmlCommand, args);

			return new { args = args, paging = info, data = table };
		}



		private XmlCommandItem GetXmlCommand(string name)
		{
			Init();

			XmlCommandItem command = XmlCommandManager.GetCommand(name);
			if (command == null)
				throw new ArgumentOutOfRangeException("name", "不能根据指定的名称找到匹配的XmlCommand，name: " + name);

			return command;
		}


		private Dictionary<string, object> GetCommandParameters(XmlCommandItem command, NameValueCollection form)
		{
			Dictionary<string, object> table = new Dictionary<string, object>();

			foreach(var p in command.Parameters) {
				string paraName = p.Name.TrimStart('@');
				string text = form[paraName];
				Type t = ConvertType(p.Type);
				object value = _stringConverter.Instance.ToObject(text, t) ?? DBNull.Value;

				table[paraName] = value;
			}
			return table;
		}

		private Type ConvertType(DbType t)
		{
			if (t == DbType.AnsiString
				|| t == DbType.AnsiStringFixedLength
				|| t == DbType.String
				|| t == DbType.StringFixedLength
				|| t == DbType.Xml
				)
				return typeof(string);

			if (t == DbType.Int16)
				return typeof(short);
			if (t == DbType.Int32)
				return typeof(int);
			if (t == DbType.Int64)
				return typeof(long);

			if (t == DbType.Date || t == DbType.DateTime)
				return typeof(DateTime);

			if (t == DbType.Currency || t == DbType.Decimal)
				return typeof(decimal);

			if (t == DbType.Double || t == DbType.Single)
				return typeof(double);

			if (t == DbType.Guid)
				return typeof(Guid);

			if (t == DbType.Binary)
				return typeof(byte[]);

			if (t == DbType.Boolean)
				return typeof(bool);


			// 默认就按字符串来处理
			return typeof(string);
		}

	}
}
