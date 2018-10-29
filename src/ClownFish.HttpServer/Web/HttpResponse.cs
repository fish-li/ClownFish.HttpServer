using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Web
{
    /// <summary>
    /// 用于封装HttpListenerResponse对象的写操作，提供与HttpResponse类似的API
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class HttpResponse : IDisposable
	{
		private HttpContext _context;
		private HttpListenerResponse _response;

		private GZipStream _gzipStream;
		private BinaryWriter _writer;

		private bool _bodyIsSend = false;

		/// <summary>
		/// 构造方法
		/// </summary>
		protected HttpResponse() { }

		internal HttpResponse(HttpContext context)
		{
			_context = context;
			_response = context.OriginalContext.Response;
		}

		/// <summary>
		/// 获取或设置返回给客户端的 HTTP 状态代码。
		/// </summary>
		public virtual int StatusCode {
			get { return _response.StatusCode; }
			set { _response.StatusCode = value; }
		}

		/// <summary>
		/// 获取或设置返回内容的 MIME 类型。 
		/// </summary>
		public virtual string ContentType {
			get { return _response.ContentType; }
			set {
				// 固定以UTF-8编码方式返回结果
				if( value.IndexOf("charset=") < 0 )
					_response.ContentType = value + "; charset=utf-8";
				else
					_response.ContentType = value;
			}
		}

        /// <summary>
        /// 获取服务器返回的标头名称/值对集合。
        /// </summary>
        public virtual WebHeaderCollection Headers {
            get { return _response.Headers; }
        }

        /// <summary>
        /// 获取随响应返回的 Cookie 集合。
        /// </summary>
        public virtual CookieCollection Cookies {
            get { return _response.Cookies; }
        }


		/// <summary>
		/// 将指定的文本写入HTTP响应流
		/// </summary>
		/// <param name="text"></param>
		public virtual void Write(string text)
		{
			if( string.IsNullOrEmpty(text) )
				return;

			byte[] bb = Encoding.UTF8.GetBytes(text);
			Write(bb);
		}


		/// <summary>
		/// 将字节数组写入HTTP响应流
		/// </summary>
		/// <param name="buffer"></param>
		public virtual void Write(byte[] buffer)
		{
			if( buffer == null || buffer.Length == 0 )
				return;

			if( _writer== null ) {
				if( _gzipStream == null )
					_writer = new BinaryWriter(_response.OutputStream, Encoding.UTF8, true);
				else
					_writer = new BinaryWriter(_gzipStream, Encoding.UTF8, true);
			}

			_writer.Write(buffer);
			_bodyIsSend = true;
		}

		/// <summary>
		/// 启用GZIP压缩
		/// </summary>
		public void EnableGzip()
		{
			if( _bodyIsSend )
				throw new InvalidOperationException("响应内容后不能再切换输出流。");

			if( _gzipStream != null )
				//throw new InvalidOperationException("不要重复调用这个方法。");
				return;

			_gzipStream = new GZipStream(_response.OutputStream, CompressionMode.Compress, true);
			this.AppendHeader("Content-Encoding", "gzip");
		}


		internal static readonly string DllVersion
            = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(HttpResponse).Assembly.Location).FileVersion;

        /// <summary>
        /// 设置基本的请求头（HTTP200， text/plain ， 允许跨域访问）
        /// </summary>
        public virtual void SetBasicHeaders()
		{
			// 设置正常响应头（适合于大多数据场景）
			_response.StatusCode = 200;
			_response.ContentType = "text/plain; charset=utf-8";
			_response.ContentEncoding = Encoding.UTF8;
			_response.AppendHeader("X-Server", "ClownFish.HttpServer/" + DllVersion);


			// 允许跨域访问
			string origin = _context.OriginalContext.Request.Headers["Origin"];
			if( string.IsNullOrEmpty(origin) == false ) {
				_response.AppendHeader("Access-Control-Allow-Origin", origin);
				_response.AppendHeader("Access-Control-Allow-Credentials", "true");
				_response.AppendHeader("Access-Control-Allow-Methods", "*");
				_response.AppendHeader("Access-Control-Allow-Headers", "*");
				_response.AppendHeader("p3p", "CP=\"CAO PSA OUR\"");
			}
		}

        /// <summary>
        /// 将指定的 System.Net.Cookie 添加到此响应的 Cookie 集合。
        /// </summary>
        /// <param name="cookie"></param>
        public virtual void AppendCookie(Cookie cookie)
        {
            _response.AppendCookie(cookie);
        }

        /// <summary>
        /// 向随此响应发送的指定 HTTP 标头追加值。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public virtual void AppendHeader(string name, string value)
        {
            _response.AppendHeader(name, value);
        }

        /// <summary>
        /// 将客户端重定向到指定的 URL
        /// </summary>
        /// <param name="url"></param>
        public virtual void Redirect(string url)
        {
            _response.Redirect(url);
        }

        /// <summary>
        /// 结束本次请求
        /// </summary>
        public void End()
        {
            // 为了照顾ASP.NET的开发习惯，所以保留这个方法。
            // 其实它只是一个快捷方式。

            this._context.Application.CompleteRequest();
        }


        #region IDisposable 成员

        /// <summary>
        /// 实现 IDispose 接口
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
		{
			try {
				if( _gzipStream != null ) {
					_gzipStream.Close();
					_gzipStream = null;
				}
			}
			catch { /* 关闭连接再出异常，就不处理了  */}

			try {
				if( _writer != null ) {
					_writer.Close();
					_writer = null;
				}
			}
			catch { /* 关闭连接再出异常，就不处理了  */}
		}

		#endregion
	}
}
