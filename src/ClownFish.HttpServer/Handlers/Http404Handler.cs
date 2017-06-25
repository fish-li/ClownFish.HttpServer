using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
	internal sealed class Http404Handler : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.StatusCode = 404;
			context.Response.Write("Not Found");
		}
	}
}
