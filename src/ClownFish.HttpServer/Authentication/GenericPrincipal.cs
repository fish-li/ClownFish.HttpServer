using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Authentication
{
    /// <summary>
    /// 一个简单的IPrincipal实现类
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class GenericPrincipal<TUser> : IPrincipal where TUser : class, IUser, new()
    {
        private GenericIdentity<TUser> _identity;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="identity"></param>
        public GenericPrincipal(GenericIdentity<TUser> identity) 
        {
            if( identity == null )
                throw new ArgumentNullException(nameof(identity));

            _identity = identity;
        }


        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="user"></param>
        public GenericPrincipal(TUser user)
        {
            if( user == null )
                throw new ArgumentNullException(nameof(user));

            GenericIdentity<TUser> identity = new GenericIdentity<TUser>(user);

            _identity = identity;
        }

        /// <summary>
        /// 实现 IPrincipal的 Identity 属性
        /// </summary>
        public IIdentity Identity => _identity;

        /// <summary>
        /// 与用户信息相关的对象
        /// </summary>
        public TUser User => _identity.User;

        /// <summary>
        /// 实现 IPrincipal的 IsInRole 方法
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            return _identity.User.IsInRole(role);
        }
    }
}
