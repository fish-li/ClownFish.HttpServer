using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Common
{
    internal static class AssemblyLoader
    {
        public static void GetBinAssemblyReferences(string appPath)
        {
            if( Directory.Exists(appPath) == false ) 
                return ;
            
            foreach(string assemblyPath in Directory.EnumerateFiles(appPath, "*.dll") ) {
                Assembly assembly = Assembly.LoadFrom(assemblyPath);
                foreach( var asmName in assembly.GetReferencedAssemblies() )
                    Assembly.Load(asmName.FullName);
            }
        }
    }
}
