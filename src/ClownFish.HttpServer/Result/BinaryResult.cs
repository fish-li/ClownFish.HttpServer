using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Http;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Result
{
    /// <summary>
    /// 表示一个二进制Action的返回结果
    /// </summary>
    public sealed class BinaryResult : IActionResult
    {
        private byte[] _buffer;
        private string _contentType;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="buffer">需要输出的数据</param>
        public BinaryResult(byte[] buffer) : this(buffer, ResponseContentType.Bin)
		{
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="buffer">需要输出的数据</param>
        /// <param name="contentType">需要设置的 ContentType 标头</param>
        public BinaryResult(byte[] buffer, string contentType)
        {
            if( buffer == null )
                throw new ArgumentNullException("buffer");
            if( string.IsNullOrEmpty(contentType) )
                throw new ArgumentNullException("contentType");

            this._buffer = buffer;
            this._contentType = contentType;
        }

        void IActionResult.Ouput(HttpContext context)
        {
            context.Response.ContentType = this._contentType;
            context.Response.Write(this._buffer);
        }
    }
}
