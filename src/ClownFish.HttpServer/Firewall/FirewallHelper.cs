using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ClownFish.HttpServer.Firewall.NetFwTypeLib;

namespace ClownFish.HttpServer.Firewall
{
    /// <summary>
    /// Windows防火墙例外设置的辅助类
    /// </summary>
    public sealed class FirewallHelper
    {
        //Set objFirewall = CreateObject("HNetCfg.FwMgr")
        //Set objPolicy = objFirewall.LocalPolicy.CurrentProfile

        //Set objApplication = CreateObject("HNetCfg.FwAuthorizedApplication")
        //objApplication.Name = "Free Cell"
        //objApplication.IPVersion = 2
        //objApplication.ProcessImageFileName = "c:\windows\system32\freecell.exe"
        //objApplication.RemoteAddresses = "*"
        //objApplication.Scope = 0
        //objApplication.Enabled = True

        //Set colApplications = objPolicy.AuthorizedApplications
        //colApplications.Add(objApplication)


        /// <summary>
        /// 添加一个应用程序完整路径到Windows防火墙的“受信”列表中
        /// </summary>
        /// <param name="path"></param>
        public static void AddToFwAuthorized(string path)
        {
            //创建firewall管理类的实例
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

            try {
                // 如果已经添加到防火墙中，就不再添加
                object existObject = netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Item(path);
                if( existObject != null )
                    return;
            }
            catch { // 如果指定的文件不在防火墙列表中，调用 Item 方法时会抛出异常，所以这里就吃掉异常
            }


            //创建一个认证程序类的实例
            INetFwAuthorizedApplication app = (INetFwAuthorizedApplication)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));

            //在例外列表里，程序显示的名称
            app.Name = Path.GetFileNameWithoutExtension(path);

            //程序的决定路径，这里使用程序本身
            app.ProcessImageFileName = path;

            //是否启用该规则
            app.Enabled = true;


            //加入到防火墙的管理策略
            netFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
        }

        /// <summary>
        /// 将指定的端口添加到防火墙例外值列表
        /// </summary>
        /// <param name="port">需要例外的TCP端口号</param>
        /// <param name="displayName">防火墙规则列表中的显示名称</param>
        public static void AddToOpenPort(int port, string displayName)
        {
            if( port <= 0 )
                throw new ArgumentOutOfRangeException("无效的TCP端口号：" + port.ToString());
            if( string.IsNullOrEmpty(displayName) )
                throw new ArgumentNullException(nameof(displayName));


            //创建firewall管理类的实例
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

            try {
                // 如果已经添加到防火墙中，就不再添加
                object existObject = netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Item(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
                if( existObject != null )
                    return;
            }
            catch( Exception ex ) { // 如果指定的端口不在防火墙列表中，调用 Item 方法时会抛出异常，所以这里就吃掉异常
                string s = ex.Message;
            }


            //创建一个认证程序类的实例
            INetFwOpenPort openPort = (INetFwOpenPort)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));
            
            openPort.Name = displayName;
            openPort.Port = port;

            // 一些默认值
            //openPort.Enabled = true;
            //openPort.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_ANY;
            //openPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            //openPort.RemoteAddresses = "*";
            //openPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;


            //加入到防火墙的管理策略
            netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(openPort);
        }

    }
}