using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using ClownFish.Base;

namespace ClownFish.HttpServer.Authentication
{
    /// <summary>
    /// 与身份认证相关的工具类
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// 默认的登录COOKIE名称
        /// </summary>
        public static string CookieName = "_userflag";

        /// <summary>
        /// 登录完成后，创建登录Cookie。
        /// </summary>
        /// <param name="user">用户信息对象。</param>
        /// <param name="isPersistent">是否持久存储票据。</param>
        /// <returns>cookie值</returns>
        public static Cookie CreateLoginCookie<TUser>(TUser user, bool isPersistent) 
            where TUser : class, IUser, new()
        {
            if( user == null )
                throw new ArgumentNullException(nameof(user));

            DateTime utcNow = DateTime.UtcNow;
            DateTime expirationUtc = utcNow.AddYears(1);

            string json = user.ToJson();
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                3,  // version
                user.Name,
                utcNow, expirationUtc, isPersistent,
                json,
                "/");

            Cookie cookie = new Cookie();
            cookie.Name = CookieName;
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            cookie.HttpOnly = true;
            cookie.Path = "/";

            if( isPersistent )
                cookie.Expires = ticket.Expiration;

            return cookie;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <param name="context"></param>
        /// <param name="user"></param>
        public static void Login<TUser>(ClownFish.HttpServer.Web.HttpContext context, TUser user)
            where TUser : class, IUser, new()
        {
            if( context == null )
                throw new ArgumentNullException(nameof(context));
            if( user == null )
                throw new ArgumentNullException(nameof(user));

            Cookie cookie = CreateLoginCookie<TUser>(user, true);
            context.Response.AppendCookie(cookie);
        }


        /// <summary>
        /// 注销登录
        /// </summary>
        public static void LogOut(ClownFish.HttpServer.Web.HttpContext context)
        {
            if( context == null )
                throw new ArgumentNullException(nameof(context));

            Cookie cookie = new Cookie(CookieName, string.Empty, "/");
            cookie.HttpOnly = true; // 不允许客户端JS访问。
            cookie.Expires = new DateTime(2000, 1, 1);      // 设置成一个过期的时间

            // 写COOKIE到客户端
            context.Response.AppendCookie(cookie);

            // 清除当前用户对象引用
            context.User = null;
        }


        /// <summary>
        /// 检查用户是否登录，并填充 HttpContext.User 属性
        /// </summary>
        /// <param name="context"></param>
        public static void CheckLogin<TUser>(ClownFish.HttpServer.Web.HttpContext context)
            where TUser : class, IUser, new()
        {
            if( context == null )
                throw new ArgumentNullException(nameof(context));

            // 1. 读登录Cookie
            Cookie cookie = context.Request.Cookies[CookieName];
            if( cookie == null || string.IsNullOrEmpty(cookie.Value) )
                return;


            try {
                TUser userData = default(TUser);
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if( ticket != null && string.IsNullOrEmpty(ticket.UserData) == false )
                    // 3. 还原用户数据
                    userData = ticket.UserData.FromJson<TUser>();

                if( ticket != null && userData != null )
                    // 4. 构造 Principal实例，重新给context.User赋值。
                    context.User = new GenericPrincipal<TUser>(userData);
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
        }


    }
}
