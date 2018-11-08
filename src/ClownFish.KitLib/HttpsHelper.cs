using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SslCertBinding.Net;

namespace ClownFish.KitLib
{
    /// <summary>
    /// HTTPS相关操作的工具类
    /// </summary>
    public static class HttpsHelper
    {
        // 测试命令行
        // netsh http show sslcert ipport=0.0.0.0:53963
        // netsh http add sslcert ipport=0.0.0.0:53963 appid={51D241DB-BFFB-4674-8E9E-D6428CF6539D} certhash=A553937A733BDD9B3A4663C6497484D0C17ECDF4

        // netsh http show sslcert ipport=0.0.0.0:53963
        // netsh http delete sslcert ipport = 0.0.0.0:53963


        /// <summary>
        /// 判断指定的端口是否存在HTTPS的绑定。
        /// 注意：在WindowsXP中，如果是非管理员，没有查询SSL相关的权限
        /// </summary>
        /// <param name="httpsPort"></param>
        /// <returns></returns>
        public static bool BindIsExist(int httpsPort)
        {
            var configuration = new CertificateBindingConfiguration();
            IPEndPoint sslPort = new IPEndPoint(IPAddress.Any, httpsPort);
            var certificateBindings = configuration.Query(sslPort);
            return certificateBindings.Length > 0;
        }


        /// <summary>
        /// 将指定的SSL证书绑定到指定的端口，并与应用程序关联
        /// </summary>
        /// <param name="httpsPort"></param>
        /// <param name="sslCert"></param>
        /// <param name="appId"></param>
        public static void BindCertToIP(int httpsPort, X509Certificate2 sslCert, Guid appId)
        {
            if( sslCert == null )
                throw new ArgumentNullException(nameof(sslCert));

            // netsh http add sslcert ipport=0.0.0.0:53963 appid={A24092A5-F73D-4033-9F40-1BF9004A41A1} certhash=DF51794312354DE531D8B2E6414864F433A2769B
            // netsh http add sslcert hostnameport=www.fish-test.com:53963 appid={A24092A5-F73D-4033-9F40-1BF9004A41A1} certhash=DC4C95714651C086D325FF481F4E217A5C431A74 certstorename=MY

            var configuration = new CertificateBindingConfiguration();
            IPEndPoint sslPort = new IPEndPoint(IPAddress.Any, httpsPort);
            CertificateBinding binding = new CertificateBinding(sslCert.Thumbprint, StoreName.My, sslPort, appId);
            configuration.Bind(binding);
        }


        /// <summary>
        /// 删除指定端口的HTTPS绑定
        /// </summary>
        /// <param name="httpsPort"></param>
        public static void RemoveBind(int httpsPort)
        {
            UserHelper.CheckIsAdministrator();

            var configuration = new CertificateBindingConfiguration();
            IPEndPoint sslPort = new IPEndPoint(IPAddress.Any, httpsPort);
            configuration.Delete(sslPort);
        }


        /// <summary>
        /// 注册HTTPS的URL监听保留
        /// </summary>
        /// <param name="listenerUrls"></param>
        public static void AddUrlAcl(IEnumerable<string> listenerUrls)
        {
            // 测试命令： netsh http show urlacl

            foreach( var url in listenerUrls ) {
                // ms-help://MS.MSDNQTR.v90.chs/http/http/http_service_config_urlacl_param.htm

                // S-1-1-0 , Everyone
                HttpApiUrlAcl.ReserveURL(url, "D:(A;;GA;;;S-1-1-0)");
            }
        }


        /// <summary>
        /// 删除HTTPS的URL监听保留
        /// </summary>
        /// <param name="listenerUrls"></param>
        public static void DeleteUrlAcl(IEnumerable<string> listenerUrls)
        {
            foreach( var url in listenerUrls ) {
                HttpApiUrlAcl.DeleteReserveURL(url);
            }

        }




    }
}
