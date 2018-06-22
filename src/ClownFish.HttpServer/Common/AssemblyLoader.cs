using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Common
{
    /// <summary>
    /// 程序集加载工具类
    /// </summary>
    public static class AssemblyLoader
    {
        /// <summary>
        /// 加载指定目录下的全部程序集
        /// </summary>
        /// <param name="appPath"></param>
        public static void LoadAssemblies(string appPath)
        {
            if( Directory.Exists(appPath) == false ) 
                return ;
            
            // 遍历加载目录下的所有DLL
            // ## 注意：如果遇到非托管DLL，会产生异常。
            foreach(string assemblyPath in Directory.EnumerateFiles(appPath, "*.dll") ) {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);

                // 加载程序集的所有引用
                foreach( var asmName in assembly.GetReferencedAssemblies() )
                    Assembly.Load(asmName.FullName);
            }
        }
    }
}
