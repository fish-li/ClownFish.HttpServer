using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.KitLib
{
    /// <summary>
    /// 目录操作工具类
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// 检查指定的目录是否存在，如果不存在就创建
        /// </summary>
        /// <param name="path"></param>
        /// <returns>返回 true ，表示目录是已存在的，false 表示目录刚被创建出来。</returns>
        public static bool EnsureExist(string path)
        {
            try {
                bool exist = Directory.Exists(path);

                if( exist == false )
                    Directory.CreateDirectory(path);

                return exist;
            }
            catch( System.Exception ex ) {
                throw new DirectoryNotFoundException(string.Format("路径{0}不存在，且无法创建。", path), ex);
            }
        }


        // 设置相关文件、目录权限
        // 解决以普通用户身份运行时，目录没有权限的问题
        public static void SetPermission(string path)
        {
            if( System.Environment.OSVersion.Version.Major <= 5 )
                return;


            // 如果目录不存在，创建日志目录
            EnsureExist(path);

            // Users组
            string userGroupName = "Users";

            DirectorySecurity dirSecurity = Directory.GetAccessControl(path, AccessControlSections.All);

            //添加Users组的访问权限规则 完全控制权限
            FileSystemAccessRule usersFileSystemAccessRule = new FileSystemAccessRule(userGroupName,
                                                                FileSystemRights.FullControl,
                                                                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                                                                PropagationFlags.None,
                                                                AccessControlType.Allow);
            bool isModified = false;
            dirSecurity.ModifyAccessRule(AccessControlModification.Add, usersFileSystemAccessRule, out isModified);

            // 保存权限设置
            Directory.SetAccessControl(path, dirSecurity);
        }
    }
}
