using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpTest
{
	public sealed class ResponseResult
	{
		public int StatusCode { get; set; }

		public string ResponseText { get; set; }

		public WebHeaderCollection Headers { get; set; }
	}
}
