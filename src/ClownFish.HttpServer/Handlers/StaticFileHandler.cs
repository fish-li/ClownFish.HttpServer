using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ClownFish.HttpServer.Web;
using Microsoft.Win32;

namespace ClownFish.HttpServer.Handlers
{
    /// <summary>
    /// 一个简单的静态文件处理器，用于响应静态文件并在输出时设置缓存响应头，
    /// 注意：使用这个类型时，必须先重新实现FilePathMapper
    /// </summary>
    internal sealed class StaticFileHandler : IHttpHandler
    {
        // 每种扩展名对应诉Mime类型对照表
        private static readonly Hashtable s_mineTable 
				= Hashtable.Synchronized(new Hashtable(10, StringComparer.OrdinalIgnoreCase));

		private HttpContext _context;

		private FileInfo _fileinfo;


		/// <summary>
		/// 处理请求，输出文件内容以及设置缓存响应头
		/// </summary>
		/// <param name="context"></param>
		public void ProcessRequest(HttpContext context)
        {
			_context = context;

			string filePath = context.Request.Path;
			string physicalPath = Path.Combine(context.Request.WebsitePath, filePath.TrimStart('/'));


            if( File.Exists(physicalPath) == false ) {
                new Http404Handler().ProcessRequest(context);
                return;
            }

			// 获取文件信息
			_fileinfo = new FileInfo(physicalPath);


			// 判断是不是可能用 304 来响应
			if( Can304Response() )
				return;

			// 读取文件内容
			byte[] filebody = File.ReadAllBytes(physicalPath);

			// 设置响应头
			SetHeaders();

			// 输出文件内容			
			context.Response.EnableGzip();
			context.Response.Write(filebody);
        }


		/// <summary>
		/// 是否以304做为响应并结束请求
		/// </summary>
		/// <returns></returns>
		private bool Can304Response()
		{
			string etagHeader = _context.Request.Headers["If-None-Match"];
			if( string.IsNullOrEmpty(etagHeader) == false ) {
				// 如果文件没有修改，就返回304响应
				if( _fileinfo.LastWriteTime.Ticks.ToString() == etagHeader ) {
					_context.Response.StatusCode = 304;
					_context.Response.End();
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// 设置响应头
		/// </summary>
		private void SetHeaders()
		{
			// 默认缓存时间：10分钟
			_context.Response.AppendHeader("Cache-Control", "public, max-age=600");
			_context.Response.AppendHeader("X-StaticFileHandler", "10-Minutes");
			_context.Response.AppendHeader("ETag", _fileinfo.LastWriteTime.Ticks.ToString());


			// 设置响应内容标头
			_context.Response.ContentType = GetContentType(_fileinfo.Extension);
		}


		/// <summary>
		/// 计算响应头ContentType对应的内容
		/// </summary>
		/// <param name="extname"></param>
		/// <returns></returns>
		private string GetContentType(string extname)
		{
			string contentType = (string)s_mineTable[extname];
			if( contentType == null ) {
				contentType = GetMimeType(extname);
				s_mineTable[extname] = contentType;
			}
			return contentType;
		}		


		/// <summary>
		/// 根据扩展名获取对应的MimeType
		/// </summary>
		/// <param name="extname"></param>
		/// <returns></returns>
		private string GetMimeType(string extname)
        {
            string mimeType = "application/octet-stream";
            if( string.IsNullOrEmpty(extname) )
                return mimeType;

            using( RegistryKey regKey = Registry.ClassesRoot.OpenSubKey(extname.ToLower()) ) {
                if( regKey != null ) {
                    object regValue = regKey.GetValue("Content Type");
                    if( regValue != null )
                        mimeType = regValue.ToString();
                }
            }
            return mimeType;
        }




    }
}
