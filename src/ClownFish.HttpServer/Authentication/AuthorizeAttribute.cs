using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Authentication
{
    /// <summary>
	/// 用于验证用户身份的修饰属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthorizeAttribute : Attribute
    {
        private string _user;
        private string[] _users;
        private string _role;
        private string[] _roles;

        private static readonly char[] CommaSeparatorArray = new char[] { ',' };

        /// <summary>
        /// 允许访问的用户列表，用逗号分隔。
        /// </summary>
        public string Users {
            get { return _user; }
            set {
                _user = value;
                _users = value.SplitTrim(CommaSeparatorArray);
            }
        }

        /// <summary>
        /// 允许访问的角色列表，用逗号分隔。
        /// </summary>
        public string Roles {
            get { return _role; }
            set {
                _role = value;
                _roles = value.SplitTrim(CommaSeparatorArray);
            }
        }

        /// <summary>
        /// 执行授权检查
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool AuthenticateRequest(HttpContext context)
        {
            // 扩展点：允许自定义授权检查逻辑

            if( context.Request.IsAuthenticated == false )
                return false;

            if( _users != null &&
                _users.Contains(context.User.Identity.Name, StringComparer.OrdinalIgnoreCase) == false )
                return false;

            if( _roles != null && _roles.Any(context.User.IsInRole) == false )
                return false;

            return true;
        }
    }
}
