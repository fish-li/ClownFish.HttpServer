using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ClownFish.HttpServer.Utils
{
    /// <summary>
    /// 包含一些网络相关的工具方法
    /// </summary>
    public static class NetHelper
    {
        /// <summary>
        /// 判断某个TCP端口是否在使用中
        /// </summary>
        /// <param name="port">要检测的TCP端口</param>
        /// <returns></returns>
        public static bool TcpPortIsUse(int port)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = properties.GetActiveTcpListeners();

            return endPoints.FirstOrDefault(x => x.Port == port) != null;
        }


        /// <summary>
        /// 获取一个空间的TCP端口，并要求在某个区间范围内。
        /// 如果指定范围的所有端口全部占用，将返回 0
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetFreeTcpPort(int min, int max)
        {
            if( min <= 0 || max <= 0 || max <= min )
                throw new ArgumentException("参数范围无效。");

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endPoints = properties.GetActiveTcpListeners();

            for(int n = min; n <= max; n++ ) {
                var e = endPoints.FirstOrDefault(x => x.Port == n);
                if( e == null )
                    return n;
            }

            return 0;
        }

        /// <summary>
        /// 获取当前计算的机器名，优先获取完整名称
        /// </summary>
        /// <returns></returns>
        public static string GetComputerName()
        {
            // 注意：这段代码需要在Windows XP及较新版本的操作系统中才能正常运行。
            SelectQuery query = new SelectQuery("SELECT PartOfDomain, DNSHostName, Domain FROM  Win32_ComputerSystem");
            using( ManagementObjectSearcher searcher = new ManagementObjectSearcher(query) ) {
                foreach( ManagementObject mo in searcher.Get() ) {
                    if( (bool)mo["PartOfDomain"] )
                        return mo["DNSHostName"].ToString() + "." + mo["Domain"].ToString();
                }
            }

            return System.Environment.MachineName;
        }
    }
}
