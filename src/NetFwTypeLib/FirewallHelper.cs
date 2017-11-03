using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFwTypeLib
{
    /// <summary>
    /// Windows防火墙例外设置的辅助类
    /// </summary>
    public sealed class FirewallHelper
    {

        /// <summary>
        /// 添加一个应用程序完整路径到Windows防火墙的“受信”列表中
        /// </summary>
        /// <param name="path"></param>
        public static void AllowApplication(string path)
        {
            if( string.IsNullOrEmpty(path) )
                throw new ArgumentNullException(nameof(path));
            if( File.Exists(path) == false )
                throw new FileNotFoundException("File not found: " + path);

            string ruleName = Path.GetFileNameWithoutExtension(path);

            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);

            try {
                // 检查规则是否已存在
                if( fwPolicy2.Rules.Item(ruleName) != null )
                    return;
            }
            catch {// 如果规则不存在，会抛出异常，这里就直接吃掉异常
            }

            // 创建一个入站规则实例
            INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            inboundRule.Enabled = true;
            //设置为允许
            inboundRule.Action = NET_FW_ACTION.NET_FW_ACTION_ALLOW;
            //指定使用TCP协议
            inboundRule.ApplicationName = path;
            //规则名称
            inboundRule.Name = ruleName;
            // 规则影响范围（配置文件）
            inboundRule.Profiles = (int)NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_ALL;

            // 添加规则到防火墙
            fwPolicy2.Rules.Add(inboundRule);
        }

        /// <summary>
        /// 将指定的端口添加到防火墙例外值列表
        /// </summary>
        /// <param name="port">需要例外的TCP端口号</param>
        /// <param name="displayName">防火墙规则列表中的显示名称</param>
        public static void AllowTcpPort(int port, string displayName)
        {
            if( port <= 0 )
                throw new ArgumentOutOfRangeException("无效的端口号：" + port.ToString());
            if( string.IsNullOrEmpty(displayName) )
                throw new ArgumentNullException(nameof(displayName));

            string ruleName = displayName + "(TCP)";
            AllowPort(port, ruleName, NET_FW_IP_PROTOCOL.NET_FW_IP_PROTOCOL_TCP);
        }

        /// <summary>
        /// 将指定的端口添加到防火墙例外值列表
        /// </summary>
        /// <param name="port">需要例外的TCP端口号</param>
        /// <param name="displayName">防火墙规则列表中的显示名称</param>
        public static void AllowUdpPort(int port, string displayName)
        {
            if( port <= 0 )
                throw new ArgumentOutOfRangeException("无效的端口号：" + port.ToString());
            if( string.IsNullOrEmpty(displayName) )
                throw new ArgumentNullException(nameof(displayName));

            string ruleName = displayName + "(UDP)";
            AllowPort(port, ruleName, NET_FW_IP_PROTOCOL.NET_FW_IP_PROTOCOL_UDP);
        }


        /// <summary>
        /// 将指定的端口添加到防火墙例外值列表
        /// </summary>
        /// <param name="port">需要例外的TCP端口号</param>
        /// <param name="ruleName">防火墙规则列表中的显示名称</param>
        /// <param name="protocol">协议类型</param>
        private static void AllowPort(int port, string ruleName, NET_FW_IP_PROTOCOL protocol)
        {
            Type tNetFwPolicy2 = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(tNetFwPolicy2);

            try {
                // 检查规则是否已存在
                if( fwPolicy2.Rules.Item(ruleName) != null )
                    return;
            }
            catch {// 如果规则不存在，会抛出异常，这里就直接吃掉异常
            }

            // 创建一个入站规则实例
            INetFwRule2 inboundRule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
            inboundRule.Enabled = true;
            //设置为允许
            inboundRule.Action = NET_FW_ACTION.NET_FW_ACTION_ALLOW;
            //指定使用TCP协议
            inboundRule.Protocol = (int)protocol;
            inboundRule.LocalPorts = port.ToString();
            //规则名称
            inboundRule.Name = ruleName;
            // 规则影响范围（配置文件）
            inboundRule.Profiles = (int)NET_FW_PROFILE_TYPE2.NET_FW_PROFILE2_ALL;

            // 添加规则到防火墙
            fwPolicy2.Rules.Add(inboundRule);
        }

    }
}
