using System;
using System.Security.Principal;

namespace ClownFish.HttpServer.Authentication
{
    /// <summary>
    /// 一个简单的IIdentity实现类
    /// </summary>
    public class GenericIdentity<TUser> : IIdentity where TUser : class, IUser, new()
    {
        private TUser _user;

        /// <summary>
        /// 与用户信息相关的对象
        /// </summary>
        public TUser User => _user;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="user"></param>
        public GenericIdentity(TUser user)
        {
            if( user == null )
                throw new ArgumentNullException(nameof(user));

            _user = user;
        }

        /// <summary>
        /// 实现 IIdentity的 Name 属性
        /// </summary>
        public string Name => this._user.Name;

        /// <summary>
        /// 实现 IIdentity的 AuthenticationType 属性
        /// </summary>
        public string AuthenticationType => "Generic";

        /// <summary>
        /// 实现 IIdentity的 IsAuthenticated 属性
        /// </summary>
        public bool IsAuthenticated => true;
    }
}
