using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClownFish.Base;

namespace ClownFish.HttpServer.Web
{
	/// <summary>
	/// 用于封装HttpListenerRequest的读取操作，提供与HttpRequest类似的访问API
	/// </summary>
	public class HttpRequest
	{
		private HttpContext _context;
		private HttpListenerRequest _request;

		private NameValueCollection _fromData;
		private string _postData;


		internal Match RegexMatch { get; set; }

		/// <summary>
		/// 构造方法
		/// </summary>
		protected HttpRequest() { }

		internal HttpRequest(HttpContext context)
		{
			_context = context;
			_request = context.OriginalContext.Request;
		}

		/// <summary>
		/// 网站根目录（如果不启用网站模式，则为 NULL）
		/// </summary>
		public string WebsitePath { get; internal set; }

		/// <summary>
		/// 获取由客户端指定的 HTTP 方法。 
		/// </summary>
		public virtual string HttpMethod {
			get {
                if( _httpMethod == null )
                    _httpMethod = _request.HttpMethod;
                return _httpMethod;
            }
            set { _httpMethod = value; }
		}
        private string _httpMethod;

        /// <summary>
        /// 获取一个 Boolean 值，该值指示请求是否有关联的正文数据。
        /// </summary>
        public virtual bool HasEntityBody {
            get { return _request.HasEntityBody; }
        }

        /// <summary>
        /// 获取随请求发送的 Cookie。
        /// </summary>
        public virtual CookieCollection Cookies {
            get { return _request.Cookies; }
        }

        /// <summary>
        /// 获取客户端提供的用户代理。
        /// </summary>
        public virtual string UserAgent {
            get { return _request.UserAgent; }
        }

        /// <summary>
        /// 获取以下资源的统一资源标识符 (URI)，该资源将使客户端与服务器相关。
        /// </summary>
        public virtual Uri UrlReferrer {
            get { return _request.UrlReferrer; }
        }

        /// <summary>
        /// 获取客户端请求的 System.Uri 对象。
        /// </summary>
        public virtual Uri Url {
            get { return _request.Url; }
        }

        

        /// <summary>
        /// 获取 System.Boolean 值，该值指示该请求是否来自本地计算机。
        /// </summary>
        public virtual bool IsLocal {
            get { return _request.IsLocal; }
        }


        /// <summary>
        /// 获取一个 System.Boolean 值，该值指示发送此请求的客户端是否经过身份验证。
        /// </summary>
        public virtual bool IsAuthenticated {
            get {
                return this._context.User != null
                    && this._context.User.Identity != null
                    && this._context.User.Identity.IsAuthenticated;
            }
        }


        /// <summary>
        /// 获取客户端请求的 URL 信息（不包括主机和端口）。
        /// </summary>
        public virtual string RawUrl {
            get {
                if( _rawUrl == null )
                    SetUrlVars();
                return _rawUrl;
            }
        }


        private string _rawUrl;
        private string _path;
        private string _query;
        private NameValueCollection _queryString;


        private void SetUrlVars()
        {
            _rawUrl = _request.RawUrl;

            int p = _rawUrl.IndexOf('?');
            if( p < 0 ) { // 没有查询字符串参数
                _queryString = new NameValueCollection();
                _path = _rawUrl;
                _query = string.Empty;
            }
            else {
                _path = _rawUrl.Substring(0, p);

                if( p == _rawUrl.Length - 1 ) {   // 问号是最后个字符，没有意义，忽略
                    _query = string.Empty;
                    _queryString = new NameValueCollection();
                }
                else {
                    _query = _rawUrl.Substring(p + 1);
                    _queryString = System.Web.HttpUtility.ParseQueryString(_query);      // 固定按UTF-8来解析
                }
            }
        }

        /// <summary>
        /// 获取查询字符串参数
        /// </summary>
        public virtual NameValueCollection QueryString {
            //get { return _request.QueryString; }  // .net framework 设计极不合理，原因与ContentEncoding属性有关
            get {
				if( _queryString == null ) 
                    SetUrlVars();
				return _queryString;
			}
		}
        
		/// <summary>
		/// 当前请求的路径（不含查询字符串部分）
		/// </summary>
		public virtual string Path {
            get {
                //return _request.Url.AbsolutePath;
                if(_path == null ) 
                    SetUrlVars();
                return _path;
            }
			set { _path = value; }
		}

        /// <summary>
        /// 原始的查询字符串参数
        /// </summary>
        public virtual string Query {
            get {
                if( _query == null )
                    SetUrlVars();
                return _query;
            }
        }
		


		/// <summary>
		/// 根据指定的名称，尝试从QueryString, Form, Headers获取对应的值
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public virtual string this[string name] {
			get {
				return GetRouteValue(name)
                    ?? QueryString[name]
                    ?? Form[name]
					?? Headers[name];
			}
		}

		/// <summary>
		/// 获取URL路由匹配中获取的参数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private string GetRouteValue(string name)
		{
			if( this.RegexMatch == null )
				return null;

			string value = this.RegexMatch.Groups[name].Value;

            if( string.IsNullOrEmpty(value) )
                return null;

            return value;
		}




		/// <summary>
		/// 获取请求头参数
		/// </summary>
		public virtual NameValueCollection Headers {
			get { return _request.Headers; }
		}



		/// <summary>
		/// 获取当前请求的表单数据
		/// </summary>
		/// <returns></returns>
		public virtual NameValueCollection Form
		{
			get {
				if( _fromData == null ) {
					string postData = GetPostText();

					if( string.IsNullOrEmpty(postData) )
						_fromData = new NameValueCollection();

					else if( string.IsNullOrEmpty(this.ContentType) ||
							 this.ContentType.StartsWith("application/x-www-form-urlencoded") )
						_fromData = System.Web.HttpUtility.ParseQueryString(postData);

					else
						_fromData = new NameValueCollection();
				}

				return _fromData;
			}
		}

		/// <summary>
		/// 获取当前请求的请求体内容， 通常用于获取 JSON，XML 文本
		/// </summary>
		/// <returns></returns>
		public virtual string GetPostText()
		{
			if( _postData == null ) {
				if( _request.HasEntityBody == false )        // 没有请求体内容
					_postData = string.Empty;

				else {
					using( StreamReader reader = new StreamReader(_request.InputStream, Encoding.UTF8, true, 1024, true) ) {
						_postData = reader.ReadToEnd();
					}
				}
			}

			return _postData;
		}

		/// <summary>
		/// 获取包含在请求中的正文数据的 MIME 类型。
		/// </summary>
		public virtual string ContentType {
			get { return _request.ContentType; }
		}



		internal RequestDataType GetDataType()
		{
			string contentType = this.ContentType;

			if( string.IsNullOrEmpty(contentType) )
				return RequestDataType.NoSet;

			if( contentType.IndexOfIgnoreCase("application/x-www-form-urlencoded") >= 0
				|| contentType.IndexOfIgnoreCase("multipart/form-data") >= 0 )
				return RequestDataType.Form;

			if( contentType.IndexOfIgnoreCase("application/json") >= 0 )
				return RequestDataType.Json;

			if( contentType.IndexOfIgnoreCase("application/xml") >= 0 )
				return RequestDataType.Xml;

			return RequestDataType.Unknown;
		}
                     

    }
}
