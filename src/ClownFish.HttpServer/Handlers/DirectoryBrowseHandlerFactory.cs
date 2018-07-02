using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Config;
using ClownFish.HttpServer.Result;
using ClownFish.HttpServer.Utils;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 实现目录浏览功能的HandlerFactory
    /// </summary>
    public sealed class DirectoryBrowseHandlerFactory : IHttpHandlerFactory
    {
        private static string s_template;

        private static void ServerHostInit(ServerOption option)
        {
            if( option.Website?.DirectoryBrowse?.DefaultFile?.Length > 0 ) {
                string[] files = option.Website.DirectoryBrowse.DefaultFile.SplitTrim(';');

                if( files?.Length > 0 && PathHelper.GetExtension(files[0])?.Length > 1 ) {
                    option.InternalOptions.DefaultFiles = files;
                }
            }


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
            string physicalPath = Path.GetFullPath(Path.Combine(rootPath, context.Request.Path.TrimStart('/')));


            // 安全检查，只允许查看指定站点下的文件
            if( physicalPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) == false ) {
                return new Http404Handler();
            }

            if( File.Exists(physicalPath) ) {
                return new StaticFileHandler();
            }
            else if( Directory.Exists(physicalPath) ) {
                return GetDirectoryHandler(context, physicalPath);
            }
            else {
                return null;
            }

        }

        private string UrlEncode(string text)
        {
            if( string.IsNullOrEmpty(text) )
                return text;

            return System.Web.HttpUtility.UrlEncode(text);
        }


        private IHttpHandler GetDirectoryHandler(HttpContext context, string physicalPath)
        {
            string curPath = context.Request.Path.TrimEnd('/');     // 这个路径如果包含特殊字符，得到的结果已被 UrlDecode
            string rawPath = context.Request.RawUrl.TrimEnd('/');   // 这个路径如果包含特殊字符，不会做 UrlDecode

            // 检查目录下是否存在默认文档
            if(context.ServerOption.InternalOptions.DefaultFiles  != null ) {
                foreach(string file in context.ServerOption.InternalOptions.DefaultFiles ) {
                    string testFile = Path.Combine(physicalPath, file);
                    if( File.Exists(testFile) ) {
                        //string redireUrl = rawPath + "/" + UrlEncode(file);
                        //return new RedirectHttpHandler(redireUrl);

                        // 内部URL重写，并将请求交给 StaticFileHandler
                        context.Request.Path = curPath + "/" + file;
                        return new StaticFileHandler();
                    }
                }
            }


            int index = 1;
            
            string rowFormat = "<tr><td>{0}</td><td><a href=\"{1}\" >{2}</a></td><td>{3}</td><td class=\"filesize\">{4}</td></tr>\r\n";
            string rowFormat2 = "<tr><td>{0}</td><td><a href=\"{1}\" target=\"_blank\">{2}</a></td><td>{3}</td><td class=\"filesize\">{4}</td></tr>\r\n";

            StringBuilder html = new StringBuilder();
            DirectoryInfo dir = new DirectoryInfo(physicalPath);

            // 遍历目录下的子目录
            foreach( var d in dir.GetDirectories() ) {
                if( d.Attributes.HasFlag(FileAttributes.Hidden) )
                    continue;

                string link = rawPath + "/" + UrlEncode(d.Name) + "/";
                string time = d.LastWriteTime.ToTimeString();
                html.AppendFormat(rowFormat, index++, link, d.Name, time, "[文件夹]");
            }

            // 遍历目录下的文件
            foreach( var f in dir.GetFiles() ) {
                if( f.Attributes.HasFlag(FileAttributes.Hidden) )
                    continue;

                string link = rawPath + "/" + UrlEncode(f.Name);
                string time = f.LastWriteTime.ToTimeString();
                html.AppendFormat(rowFormat2, index++, link, f.Name, time, f.Length.ToString("N0"));
            }

            //string template = File.ReadAllText(@"D:\my-github\ClownFish.HttpServer\src\ClownFish.HttpServer\Handlers\FileListTemplate.html");
            string navigationLink = GetNavigationLink(curPath);
            string result = s_template
                        .Replace("<!--{current-path}-->", navigationLink)
                        .Replace("<!--{data-row}-->", html.ToString());


            return new HtmlTextHttpHandler(result);
        }


        private string GetNavigationLink(string curPath)
        {
            string rootHtml = "<a href=\"/\">/</a>";

            if( string.IsNullOrEmpty(curPath) )
                return rootHtml;


            string[] names = curPath.Trim('/').Split('/');
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(rootHtml);

            string path = string.Empty;

            for( int i = 0; i < names.Length; i++ ) {

                if( i > 0 )
                    sb.AppendLine("<span>/</span>");

                string x = names[i];
                path = path + "/" + UrlEncode(x);
                sb.AppendLine($"<a href=\"{path}\">{x}</a>");
            }

            return sb.ToString();
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


        //internal class RedirectHttpHandler : IHttpHandler
        //{
        //    private string _url;

        //    public RedirectHttpHandler(string url)
        //    {
        //        _url = url;
        //    }

        //    public void ProcessRequest(HttpContext context)
        //    {
        //        IActionResult result = new RedirectResult(_url);
        //        result.Ouput(context);
        //    }
        //}
    }
}
