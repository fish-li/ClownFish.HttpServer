using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SslCertBinding.Net;

namespace ClownFish.KitLib
{
    // 内部封装类

    internal static class HttpApiUrlAcl
    {
        [StructLayout(LayoutKind.Sequential)]
        struct HTTP_SERVICE_CONFIG_URLACL_SET
        {
            public HTTP_SERVICE_CONFIG_URLACL_KEY KeyDesc;
            public HTTP_SERVICE_CONFIG_URLACL_PARAM ParamDesc;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct HTTP_SERVICE_CONFIG_URLACL_KEY
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pUrlPrefix;

            public HTTP_SERVICE_CONFIG_URLACL_KEY(string urlPrefix)
            {
                pUrlPrefix = urlPrefix;
            }
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct HTTP_SERVICE_CONFIG_URLACL_PARAM
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pStringSecurityDescriptor;

            public HTTP_SERVICE_CONFIG_URLACL_PARAM(string securityDescriptor)
            {
                pStringSecurityDescriptor = securityDescriptor;
            }
        }

        private class ErrorCodes
        {
            public static readonly int NOERROR = 0;
            public static readonly int ERROR_ALREADY_EXISTS = 183;
            public static readonly int ERROR_NOT_EXISTS = 2;
        }

        public static void ReserveURL(string networkURL, string securityDescriptor)
        {
            uint retVal = (uint)ErrorCodes.NOERROR; // NOERROR = 0
            HttpApi.HTTPAPI_VERSION HttpApiVersion = new HttpApi.HTTPAPI_VERSION(1, 0);

            retVal = HttpApi.HttpInitialize(HttpApiVersion, HttpApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            if( (uint)ErrorCodes.NOERROR == retVal ) {
                HTTP_SERVICE_CONFIG_URLACL_KEY keyDesc = new HTTP_SERVICE_CONFIG_URLACL_KEY(networkURL);
                HTTP_SERVICE_CONFIG_URLACL_PARAM paramDesc = new HTTP_SERVICE_CONFIG_URLACL_PARAM(securityDescriptor);

                HTTP_SERVICE_CONFIG_URLACL_SET inputConfigInfoSet = new HTTP_SERVICE_CONFIG_URLACL_SET();
                inputConfigInfoSet.KeyDesc = keyDesc;
                inputConfigInfoSet.ParamDesc = paramDesc;

                IntPtr pInputConfigInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_SET)));
                Marshal.StructureToPtr(inputConfigInfoSet, pInputConfigInfo, false);

                retVal = HttpApi.HttpSetServiceConfiguration(IntPtr.Zero,
                                HttpApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                                pInputConfigInfo,
                                Marshal.SizeOf(inputConfigInfoSet),
                                IntPtr.Zero);

                if( (uint)ErrorCodes.ERROR_ALREADY_EXISTS == retVal )  // ERROR_ALREADY_EXISTS = 183
                {
                    retVal = HttpApi.HttpDeleteServiceConfiguration(IntPtr.Zero,
                    HttpApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                    pInputConfigInfo,
                    Marshal.SizeOf(inputConfigInfoSet),
                    IntPtr.Zero);

                    if( (uint)ErrorCodes.NOERROR == retVal ) {
                        retVal = HttpApi.HttpSetServiceConfiguration(IntPtr.Zero,
                                        HttpApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                                        pInputConfigInfo,
                                        Marshal.SizeOf(inputConfigInfoSet),
                                        IntPtr.Zero);
                    }
                }

                Marshal.FreeCoTaskMem(pInputConfigInfo);
                HttpApi.HttpTerminate(HttpApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            }

            if( (uint)ErrorCodes.NOERROR != retVal ) {
                throw new Win32Exception(Convert.ToInt32(retVal));
            }
        }



        public static void DeleteReserveURL(string networkURL)
        {
            uint retVal = (uint)ErrorCodes.NOERROR; // NOERROR = 0
            HttpApi.HTTPAPI_VERSION HttpApiVersion = new HttpApi.HTTPAPI_VERSION(1, 0);

            retVal = HttpApi.HttpInitialize(HttpApiVersion, HttpApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            if( (uint)ErrorCodes.NOERROR == retVal ) {
                HTTP_SERVICE_CONFIG_URLACL_KEY keyDesc = new HTTP_SERVICE_CONFIG_URLACL_KEY(networkURL);

                HTTP_SERVICE_CONFIG_URLACL_SET inputConfigInfoSet = new HTTP_SERVICE_CONFIG_URLACL_SET();
                inputConfigInfoSet.KeyDesc = keyDesc;

                IntPtr pInputConfigInfo = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(HTTP_SERVICE_CONFIG_URLACL_SET)));
                Marshal.StructureToPtr(inputConfigInfoSet, pInputConfigInfo, false);

                retVal = HttpApi.HttpDeleteServiceConfiguration(IntPtr.Zero,
                                    HttpApi.HTTP_SERVICE_CONFIG_ID.HttpServiceConfigUrlAclInfo,
                                    pInputConfigInfo,
                                    Marshal.SizeOf(inputConfigInfoSet),
                                    IntPtr.Zero);

                // 已经删除过了，不存在，就把状态标记为正常值
                if( (uint)ErrorCodes.ERROR_NOT_EXISTS == retVal )
                    retVal = (uint)ErrorCodes.NOERROR;

                Marshal.FreeCoTaskMem(pInputConfigInfo);
                HttpApi.HttpTerminate(HttpApi.HTTP_INITIALIZE_CONFIG, IntPtr.Zero);
            }

            if( (uint)ErrorCodes.NOERROR != retVal ) {
                throw new Win32Exception(Convert.ToInt32(retVal));
            }
        }


    }
}
