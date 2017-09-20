using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Xml;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Utils
{
    /// <summary>
    /// 包含记录日志的HTTP相关信息
    /// </summary>
    public class NetHttpInfo
    {
        /// <summary>
        /// 当前登录用户的用户名，可不填。
        /// </summary>
        public string UserName { get; set; }


        /// <summary>
        /// 请求头信息
        /// </summary>
        public XmlCdata RequestText { get; set; }


        /// <summary>
        /// 页面地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 页面原始URL
        /// </summary>
        public string RawUrl { get; set; }


        ///// <summary>
        ///// 浏览器类型。注意：此信息可能不准确。
        ///// </summary>
        //public string Browser { get; set; }




        /// <summary>
        /// 根据HttpContext实例创建并填充HttpInfo对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static NetHttpInfo Create(HttpContext context)
        {
            if( context == null )
                return null;

            NetHttpInfo info = new NetHttpInfo();
            info.SetHttpInfo(context);
            return info;
        }


        /// <summary>
        /// 设置请求信息
        /// </summary>
        private void SetHttpInfo(HttpContext context)
        {
            if( context == null )
                return;


            if( context.Request.IsAuthenticated )
                this.UserName = context.User.Identity.Name;


            this.Url = context.Request.Url.ToString();
            this.RawUrl = context.Request.RawUrl;

            //if( context.Request.UserAgent != null )
            //    this.Browser = context.Request.UserAgent;


            GetRequestText(context);
        }


        private void GetRequestText(HttpContext context)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine()
                .Append(context.Request.HttpMethod)
                .Append(" ")
                .Append(context.Request.Url.ToString())
                .AppendLine(" HTTP/1.1");


            if( context.Request.Headers.Count > 0 ) {
                foreach( string key in context.Request.Headers.AllKeys ) {
                    string value = context.Request.Headers[key];
                    sb.Append(key).Append(": ").Append(value).AppendLine();
                }
            }
            
            if( context.Request.HasEntityBody ) {
                string postData = context.Request.GetPostText();
                sb.AppendLine().AppendLine(postData);
            }

            this.RequestText = sb.ToString();
        }


    }
}
