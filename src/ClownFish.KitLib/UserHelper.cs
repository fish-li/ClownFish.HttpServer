using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.KitLib
{
    public static class UserHelper
    {
        /// <summary>
        /// 判断当前用户是否为【管理员】身份
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);

            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


        /// <summary>
        /// 检查当前用户是否为管理员，如果不是则抛出异常
        /// </summary>
        public static void CheckIsAdministrator()
        {
            bool isAdmin = IsAdministrator();

            if( isAdmin == false )
                throw new RequireAdministratorException();
        }
    }



    [Serializable]
    public sealed class RequireAdministratorException : Exception
    {
        public RequireAdministratorException() :
            base("当前用户权限不够，请使用管理员身份运行本程序。")
        {
        }
    }
}
