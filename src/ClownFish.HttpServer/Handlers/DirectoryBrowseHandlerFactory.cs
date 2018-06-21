using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 实现目录浏览功能的HandlerFactory
    /// </summary>
    public sealed class DirectoryBrowseHandlerFactory : IHttpHandlerFactory
    {
        private static readonly string s_template;

        static DirectoryBrowseHandlerFactory()
        {
            Stream stream = typeof(DirectoryBrowseHandlerFactory).Assembly.GetManifestResourceStream("ClownFish.HttpServer.Handlers.FileListTemplate.html");
            using( StreamReader reader = new StreamReader(stream, Encoding.UTF8) ) {
                s_template = reader.ReadToEnd();
            }
        }

        /// <summary>
		/// 是否在路由匹配之前执行，当前类型固定返回false
		/// </summary>
		public bool IsBeforeRouting {
            get {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IHttpHandler CreateHandler(HttpContext context)
        {
            string rootPath = context.Request.WebsitePath;
            string current = Path.GetFullPath(Path.Combine(rootPath, context.Request.Path.TrimStart('/')));


            // 安全检查，只允许查看指定站点下的文件
            if( current.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) == false ) {
                return new Http404Handler();
            }

            context.Response.AppendHeader("x-AppHandler", "ClownFish.StaticFileServer");

            if( File.Exists(current) ) {
                return new StaticFileHandler();
            }
            else if( Directory.Exists(current) ) {
                return GetDirectoryHandler(context, current);
            }
            else {
                return null;
            }

        }



        private IHttpHandler GetDirectoryHandler(HttpContext context, string path)
        {
            string subPath = context.Request.Path.TrimEnd('/');


            int index = 1;
            string rowFormat = "<tr><td>{0}</td><td><a href=\"{1}\" >{2}</a></td><td>{3}</td><td class=\"filesize\">{4}</td></tr>\r\n";
            string rowFormat2 = "<tr><td>{0}</td><td><a href=\"{1}\" target=\"_blank\">{2}</a></td><td>{3}</td><td class=\"filesize\">{4}</td></tr>\r\n";

            StringBuilder html = new StringBuilder();
            DirectoryInfo dir = new DirectoryInfo(path);

            // 遍历目录下的子目录
            foreach( var d in dir.GetDirectories() ) {
                string link = subPath + "/" + System.Web.HttpUtility.UrlEncode(d.Name);
                string time = d.LastWriteTime.ToTimeString();
                html.AppendFormat(rowFormat, index++, link, d.Name, time, "[文件夹]");
            }

            // 遍历目录下的文件
            foreach( var f in dir.GetFiles() ) {
                string link = subPath + "/" + System.Web.HttpUtility.UrlEncode(f.Name);
                string time = f.LastWriteTime.ToTimeString();
                html.AppendFormat(rowFormat2, index++, link, f.Name, time, f.Length.ToString());
            }


            string result = s_template
                        .Replace("<!--{current-path}-->", (subPath.Length > 0 ? subPath : "/"))
                        .Replace("<!--{data-row}-->", html.ToString());


            return new HtmlTextHttpHandler(result);
        }

        internal class HtmlTextHttpHandler : IHttpHandler
        {
            private string _html;

            public HtmlTextHttpHandler(string html)
            {
                _html = html;
            }

            public void ProcessRequest(HttpContext context)
            {
                context.Response.ContentType = "text/html";
                context.Response.Write(_html);
            }
        }
    }
}
