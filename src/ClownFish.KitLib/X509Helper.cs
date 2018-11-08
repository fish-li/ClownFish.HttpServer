using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.KitLib
{
    public static class X509Helper
    {
        // 证书的获取方式，可参考下面的代码
        //public static readonly X509Certificate2 CACertificate = Load("DemoApp.NameSpace.cert_files.ca.crt", null);
        //public static readonly X509Certificate2 WebCertificate = Load("DemoApp.NameSpace.cert_files.web.pfx", "your_password");

        /// <summary>
        /// 从程序集中加载X509证书对象
        /// </summary>
        /// <param name="name">证书文件在程序集中的资源路径</param>
        /// <param name="password">证书密码。 pfx文件肯定是包含密码的。如果是 crt文件，可以指定为 null</param>
        /// <param name="asm">包含证书的程序集，如果不指定，默认从主程序集中加载</param>
        /// <returns></returns>
        public static X509Certificate2 Load(string name, string password, Assembly asm)
        {
            if( string.IsNullOrEmpty(name) )
                throw new ArgumentNullException(nameof(name));
            
            if( asm == null )
                asm = System.Reflection.Assembly.GetEntryAssembly();

            using( Stream m1 = asm.GetManifestResourceStream(name) ) {
                byte[] buffer = new byte[m1.Length];
                m1.Read(buffer, 0, (int)m1.Length);

                if( password == null )
                    return new X509Certificate2(buffer);
                else
                    return new X509Certificate2(buffer, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            }
        }

        /// <summary>
        /// 从Windows证书管理器中查找X509证书
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <param name="storeName"></param>
        /// <returns></returns>
        public static X509Certificate2 Find(string thumbprint, StoreName storeName)
        {
            if( string.IsNullOrEmpty(thumbprint) )
                throw new ArgumentNullException(nameof(thumbprint));

            // CA，SSL证书必须放在 LocalMachine
            X509Store x509Store = new X509Store(storeName, StoreLocation.LocalMachine);
            try {
                x509Store.Open(OpenFlags.ReadOnly);
                foreach( X509Certificate2 current in x509Store.Certificates )
                    if( current.Thumbprint == thumbprint )
                        return current;
            }
            finally {
                x509Store.Close();
            }
            return null;
        }

        /// <summary>
        /// 将X509证书导入到Windows证书管理器
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="storeName"></param>
        public static void Import(X509Certificate2 cert, StoreName storeName)
        {
            if( cert == null )
                throw new ArgumentNullException(nameof(cert));

            // CA，SSL证书必须放在 LocalMachine
            X509Store x509Store = new X509Store(storeName, StoreLocation.LocalMachine);
            try {
                x509Store.Open(OpenFlags.ReadWrite);

                x509Store.Add(cert);
            }
            finally {
                x509Store.Close();
            }
        }

        /// <summary>
        /// 从Windows证书管理器中删除指定的X509证书
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="storeName"></param>
        public static void Remove(X509Certificate2 cert, StoreName storeName)
        {
            if( cert == null )
                throw new ArgumentNullException(nameof(cert));


            X509Store x509Store = new X509Store(storeName, StoreLocation.LocalMachine);
            try {
                x509Store.Open(OpenFlags.ReadWrite);

                x509Store.Remove(cert);
            }
            finally {
                x509Store.Close();
            }
        }

        /// <summary>
        /// 将指定的X509证书安装到Windows证书管理器。
        /// 此方法先判断证书是否存在，如果不存在则执行导入，如果存在则忽略。
        /// </summary>
        /// <param name="cert">X509Certificate2实例，表示一个X509证书</param>
        /// <param name="storeName">证书存储区的名称。安装【根证书】需要指定为 Root，安装【SSL证书】需要指定为 My</param>
        public static void SetupCert(X509Certificate2 cert, StoreName storeName)
        {
            if( cert == null )
                throw new ArgumentNullException(nameof(cert));

            if( Find(cert.Thumbprint, storeName) == null )
                Import(cert, storeName);
        }

        /// <summary>
        /// 从Windows证书管理器中删除指定的证书。
        /// 此方法先判断证书是否存在，如要存在则删除，如果不存在则忽略。
        /// </summary>
        /// <param name="cert">X509Certificate2实例，表示一个X509证书</param>
        /// <param name="storeName">证书存储区的名称。</param>
        /// <returns></returns>
        public static bool DeleteCert(X509Certificate2 cert, StoreName storeName)
        {
            if( cert == null )
                throw new ArgumentNullException(nameof(cert));

            if( Find(cert.Thumbprint, storeName) != null ) {
                Remove(cert, storeName);
                return true;
            }

            return false;
        }


        //public static string GetCertStatus()
        //{
        //    string result = null;
        //    X509Certificate2 rootCert = Find(CACertificate.Thumbprint, StoreName.Root);
        //    result = rootCert == null ? "CA: NOT SET, " : "CA: OK, ";


        //    X509Certificate2 sslCert = Find(WebCertificate.Thumbprint, StoreName.My);
        //    result += sslCert == null ? "SSL: NOT SET " : "SSL: OK ";

        //    return result;
        //}


    }
}
