using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.KitLib
{
    public static class ServiceHelper
    {
        /// <summary>
        /// 设置指定的Windows服务可自动启动
        /// </summary>
        /// <param name="serviceName"></param>
        public static void SetServiceAutoRun(string serviceName)
        {
            ChangeServiceStartType(serviceName, StartupTypeOptions.Automatic);
        }

        /// <summary>
        /// 禁止指定的Windows服务
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool DisableService(string serviceName)
        {
            return ChangeServiceStartType(serviceName, StartupTypeOptions.Disabled);
        }


        internal static bool ChangeServiceStartType(string serviceName, StartupTypeOptions startType)
        {
            //Obtain a handle to the service control manager database
            IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_CONNECT);
            if( scmHandle == IntPtr.Zero ) {
                //throw new Exception("Failed to obtain a handle to the service control manager database.");
                return false;
            }

            IntPtr serviceHandle = IntPtr.Zero;
            try {
                //Obtain a handle to the specified windows service
                serviceHandle = OpenService(scmHandle, serviceName, SERVICE_QUERY_CONFIG | SERVICE_CHANGE_CONFIG);
                if( serviceHandle == IntPtr.Zero ) {
                    //throw new Exception(string.Format("Failed to obtain a handle to service \"{0}\".", serviceName));
                    return false;
                }

                bool changeServiceSuccess = ChangeServiceConfig(serviceHandle, SERVICE_NO_CHANGE, (uint)startType, SERVICE_NO_CHANGE, null, null, IntPtr.Zero, null, null, null, null);
                return changeServiceSuccess;
                //if( !changeServiceSuccess ) {
                //    string msg = string.Format("Failed to update service configuration for service \"{0}\". ChangeServiceConfig returned error {1}.", serviceName, Marshal.GetLastWin32Error().ToString());
                //    throw new Exception(msg);
                //}
            }
            finally {
                //Clean up
                if( scmHandle != IntPtr.Zero ) CloseServiceHandle(scmHandle);
                if( serviceHandle != IntPtr.Zero ) CloseServiceHandle(serviceHandle);
            }
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr OpenSCManager(string machineName, string databaseName, uint dwAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean ChangeServiceConfig(
            IntPtr hService,
            UInt32 nServiceType,
            UInt32 nStartType,
            UInt32 nErrorControl,
            String lpBinaryPathName,
            String lpLoadOrderGroup,
            IntPtr lpdwTagId,
            [In] char[] lpDependencies,
            String lpServiceStartName,
            String lpPassword,
            String lpDisplayName);

        [DllImport("advapi32.dll", EntryPoint = "CloseServiceHandle")]
        private static extern int CloseServiceHandle(IntPtr hSCObject);

        private const uint SC_MANAGER_CONNECT = 0x0001;
        private const uint SERVICE_QUERY_CONFIG = 0x00000001;
        private const uint SERVICE_CHANGE_CONFIG = 0x00000002;
        private const uint SERVICE_NO_CHANGE = 0xFFFFFFFF;

        internal enum StartupTypeOptions : uint
        {
            BootStart = 0,      //A device driver started by the system loader. This value is valid only for driver services.
            SystemStart = 1,    //A device driver started by the IoInitSystem function. This value is valid only for driver services.
            Automatic = 2,      //A service started automatically by the service control manager during system startup.
            Manual = 3,         //A service started by the service control manager when a process calls the StartService function.
            Disabled = 4        //A service that cannot be started. Attempts to start the service result in the error code ERROR_SERVICE_DISABLED.
        }
    }
}
