using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClownFish.Base.Xml;

namespace ClownFish.HttpTest
{
	public sealed class RequestTest
	{
		[XmlAttribute]
		public string Category { get; set; }

		[XmlAttribute]
		public string Title { get; set; }

		public XmlCdata Request { get; set; }

		public ResponseAssert Response { get; set; }
	}


	public sealed class ResponseAssert
	{
		[XmlAttribute]
		public int StatusCode { get; set; }

		[XmlArrayItem(ElementName = "check")]
		public List<ResponseHeaderAssert> Headers { get; set; }

		[XmlArrayItem(ElementName = "check")]
		public List<ResponseBodyAssert> Body { get; set; }
	}

	public sealed class ResponseHeaderAssert : AssertItem
	{
		// 断言范围
		// header = value        某个响应头的值 等于 什么
		// header include value  某个响应头的值 包含 什么
	}

	public sealed class ResponseBodyAssert : AssertItem
	{
		// 断言范围
		// Length = value        响应文本的长度 等于 多少
		// Text = value          响应文本 等于 什么
		// Text include value    响应文本 包含 什么
		// Text regex value      响应文本 正则表达式 匹配检查
	}

	public class AssertItem
	{
		[XmlAttribute("name")]
		public string Name { get; set; }

		[XmlAttribute("check")]
		public string AssertMode { get; set; }

		[XmlAttribute("value")]
		public string Value { get; set; }
	}


	
}
