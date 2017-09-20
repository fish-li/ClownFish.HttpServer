using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.CustomMarshalers;

namespace ClownFish.HttpServer.Firewall.NetFwTypeLib
{
	[ComImport, TypeLibType((short)0x1040), Guid("B5E64FFA-C2C5-444E-A301-FB5E00018050")]
	internal interface INetFwAuthorizedApplication
	{
		[DispId(1)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] set; }
		[DispId(2)]
		string ProcessImageFileName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		NET_FW_IP_VERSION_ IpVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		NET_FW_SCOPE_ Scope { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		string RemoteAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		bool Enabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
	}

	[ComImport, Guid("644EFD52-CCF9-486C-97A2-39F352570B30"), TypeLibType((short)0x1040)]
    internal interface INetFwAuthorizedApplications : IEnumerable
	{
		[DispId(1)]
		int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		void Add([In, MarshalAs(UnmanagedType.Interface)] INetFwAuthorizedApplication app);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
		void Remove([In, MarshalAs(UnmanagedType.BStr)] string imageFileName);
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
		INetFwAuthorizedApplication Item([In, MarshalAs(UnmanagedType.BStr)] string imageFileName);
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)1)]
		new IEnumerator GetEnumerator();
	}

	[ComImport, TypeLibType((short)0x1040), Guid("A6207B2E-7CDD-426A-951E-5E1CBC5AFEAD")]
    internal interface INetFwIcmpSettings
	{
		[DispId(1)]
		bool AllowOutboundDestinationUnreachable { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] set; }
		[DispId(2)]
		bool AllowRedirect { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		bool AllowInboundEchoRequest { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		bool AllowOutboundTimeExceeded { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		bool AllowOutboundParameterProblem { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		bool AllowOutboundSourceQuench { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
		[DispId(7)]
		bool AllowInboundRouterRequest { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] set; }
		[DispId(8)]
		bool AllowInboundTimestampRequest { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] set; }
		[DispId(9)]
		bool AllowInboundMaskRequest { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)] set; }
		[DispId(10)]
		bool AllowOutboundPacketTooBig { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)] set; }
	}

	[ComImport, Guid("F7898AF5-CAC4-4632-A2EC-DA06E5111AF2"), TypeLibType((short)0x1040)]
    internal interface INetFwMgr
	{
		[DispId(1)]
		INetFwPolicy LocalPolicy { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[DispId(2)]
		NET_FW_PROFILE_TYPE_ CurrentProfileType { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
		void RestoreDefaults();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
		void IsPortAllowed([In, MarshalAs(UnmanagedType.BStr)] string imageFileName, [In] NET_FW_IP_VERSION_ IpVersion, [In] int portNumber, [In, MarshalAs(UnmanagedType.BStr)] string localAddress, [In] NET_FW_IP_PROTOCOL_ ipProtocol, [MarshalAs(UnmanagedType.Struct)] out object allowed, [MarshalAs(UnmanagedType.Struct)] out object restricted);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
		void IsIcmpTypeAllowed([In] NET_FW_IP_VERSION_ IpVersion, [In, MarshalAs(UnmanagedType.BStr)] string localAddress, [In] byte Type, [MarshalAs(UnmanagedType.Struct)] out object allowed, [MarshalAs(UnmanagedType.Struct)] out object restricted);
	}

	[ComImport, TypeLibType((short)0x1040), Guid("E0483BA0-47FF-4D9C-A6D6-7741D0B195F7")]
    internal interface INetFwOpenPort
	{
		[DispId(1)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] set; }
		[DispId(2)]
		NET_FW_IP_VERSION_ IpVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		NET_FW_IP_PROTOCOL_ Protocol { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		int Port { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		NET_FW_SCOPE_ Scope { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		string RemoteAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
		[DispId(7)]
		bool Enabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] set; }
		[DispId(8)]
		bool BuiltIn { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
	}

	[ComImport, TypeLibType((short)0x1040), Guid("C0E9D7FA-E07E-430A-B19A-090CE82D92E2")]
    internal interface INetFwOpenPorts : IEnumerable
	{
		[DispId(1)]
		int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		void Add([In, MarshalAs(UnmanagedType.Interface)] INetFwOpenPort Port);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
		void Remove([In] int portNumber, [In] NET_FW_IP_PROTOCOL_ ipProtocol);
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
		INetFwOpenPort Item([In] int portNumber, [In] NET_FW_IP_PROTOCOL_ ipProtocol);
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)1)]
		new IEnumerator GetEnumerator();
	}

	[ComImport, TypeLibType((short)0x1040), Guid("D46D2478-9AC9-4008-9DC7-5563CE5536CC")]
    internal interface INetFwPolicy
	{
		[DispId(1)]
		INetFwProfile CurrentProfile { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		INetFwProfile GetProfileByType([In] NET_FW_PROFILE_TYPE_ profileType);
	}

    //[ComImport, Guid("98325047-C671-4174-8D81-DEFCD3F03186"), TypeLibType((short)0x1040)]
    //internal interface INetFwPolicy2
    //{
    //    [DispId(1)]
    //    int CurrentProfileTypes { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
    //    [DispId(2)]
    //    bool this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
    //    [DispId(3)]
    //    object this[NET_FW_PROFILE_TYPE2_ profileType] { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
    //    [DispId(4)]
    //    bool this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
    //    [DispId(5)]
    //    bool this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
    //    [DispId(6)]
    //    bool this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
    //    [DispId(7)]
    //    INetFwRules Rules { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; }
    //    [DispId(8)]
    //    INetFwServiceRestriction ServiceRestriction { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
    //    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)]
    //    void EnableRuleGroup([In] int profileTypesBitmask, [In, MarshalAs(UnmanagedType.BStr)] string group, [In] bool enable);
    //    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)]
    //    bool IsRuleGroupEnabled([In] int profileTypesBitmask, [In, MarshalAs(UnmanagedType.BStr)] string group);
    //    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)]
    //    void RestoreLocalFirewallDefaults();
    //    [DispId(12)]
    //    NET_FW_ACTION_ this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)] set; }
    //    [DispId(13)]
    //    NET_FW_ACTION_ this[NET_FW_PROFILE_TYPE2_ profileType] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)] set; }
    //    [DispId(14)]
    //    bool this[string group] { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)] get; }
    //    [DispId(15)]
    //    NET_FW_MODIFY_STATE_ LocalPolicyModifyState { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)] get; }
    //}

    [ComImport, Guid("174A0DDA-E9F9-449D-993B-21AB667CA456"), TypeLibType((short)0x1040)]
    internal interface INetFwProfile
	{
		[DispId(1)]
		NET_FW_PROFILE_TYPE_ Type { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[DispId(2)]
		bool FirewallEnabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		bool ExceptionsNotAllowed { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		bool NotificationsDisabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		bool UnicastResponsesToMulticastBroadcastDisabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		INetFwRemoteAdminSettings RemoteAdminSettings { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; }
		[DispId(7)]
		INetFwIcmpSettings IcmpSettings { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; }
		[DispId(8)]
		INetFwOpenPorts GloballyOpenPorts { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
		[DispId(9)]
		INetFwServices Services { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)] get; }
		[DispId(10)]
		INetFwAuthorizedApplications AuthorizedApplications { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)] get; }
	}

	[ComImport, Guid("D4BECDDF-6F73-4A83-B832-9C66874CD20E"), TypeLibType((short)0x1040)]
    internal interface INetFwRemoteAdminSettings
	{
		[DispId(1)]
		NET_FW_IP_VERSION_ IpVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] set; }
		[DispId(2)]
		NET_FW_SCOPE_ Scope { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		string RemoteAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		bool Enabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
	}

	[ComImport, TypeLibType((short)0x1040), Guid("AF230D27-BABA-4E42-ACED-F524F22CFCE2")]
    internal interface INetFwRule
	{
		[DispId(1)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] set; }
		[DispId(2)]
		string Description { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] set; }
		[DispId(3)]
		string ApplicationName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] set; }
		[DispId(4)]
		string serviceName { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		int Protocol { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		string LocalPorts { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
		[DispId(7)]
		string RemotePorts { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] set; }
		[DispId(8)]
		string LocalAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] set; }
		[DispId(9)]
		string RemoteAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(9)] set; }
		[DispId(10)]
		string IcmpTypesAndCodes { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(10)] set; }
		[DispId(11)]
		NET_FW_RULE_DIRECTION_ Direction { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(11)] set; }
		[DispId(12)]
		object Interfaces { [return: MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)] get; [param: In, MarshalAs(UnmanagedType.Struct)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(12)] set; }
		[DispId(13)]
		string InterfaceTypes { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(13)] set; }
		[DispId(14)]
		bool Enabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(14)] set; }
		[DispId(15)]
		string Grouping { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(15)] set; }
		[DispId(0x10)]
		int Profiles { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x10)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x10)] set; }
		[DispId(0x11)]
		bool EdgeTraversal { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x11)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x11)] set; }
		[DispId(0x12)]
		NET_FW_ACTION_ Action { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x12)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x12)] set; }
	}

	[ComImport, TypeLibType((short)0x1040), Guid("9C4C6277-5027-441E-AFAE-CA1F542DA009")]
    internal interface INetFwRules : IEnumerable
	{
		[DispId(1)]
		int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		void Add([In, MarshalAs(UnmanagedType.Interface)] INetFwRule rule);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
		void Remove([In, MarshalAs(UnmanagedType.BStr)] string Name);
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
		INetFwRule Item([In, MarshalAs(UnmanagedType.BStr)] string Name);
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)1)]
		new IEnumerator GetEnumerator();
	}

	[ComImport, Guid("79FD57C8-908E-4A36-9888-D5B3F0A444CF"), TypeLibType((short)0x1040)]
    internal interface INetFwService
	{
		[DispId(1)]
		string Name { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[DispId(2)]
		NET_FW_SERVICE_TYPE_ Type { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)] get; }
		[DispId(3)]
		bool Customized { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; }
		[DispId(4)]
		NET_FW_IP_VERSION_ IpVersion { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)] set; }
		[DispId(5)]
		NET_FW_SCOPE_ Scope { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)] set; }
		[DispId(6)]
		string RemoteAddresses { [return: MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] get; [param: In, MarshalAs(UnmanagedType.BStr)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(6)] set; }
		[DispId(7)]
		bool Enabled { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] get; [param: In] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(7)] set; }
		[DispId(8)]
		INetFwOpenPorts GloballyOpenPorts { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(8)] get; }
	}

	[ComImport, TypeLibType((short)0x1040), Guid("8267BBE3-F890-491C-B7B6-2DB1EF0E5D2B")]
    internal interface INetFwServiceRestriction
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
		void RestrictService([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string appName, [In] bool RestrictService, [In] bool serviceSidRestricted);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		bool ServiceRestricted([In, MarshalAs(UnmanagedType.BStr)] string serviceName, [In, MarshalAs(UnmanagedType.BStr)] string appName);
		[DispId(3)]
		INetFwRules Rules { [return: MarshalAs(UnmanagedType.Interface)] [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)] get; }
	}

	[ComImport, Guid("79649BB4-903E-421B-94C9-79848E79F6EE"), TypeLibType((short)0x1040)]
    internal interface INetFwServices : IEnumerable
	{
		[DispId(1)]
		int Count { [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)] get; }
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
		INetFwService Item([In] NET_FW_SERVICE_TYPE_ svcType);
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "", MarshalTypeRef = typeof(EnumeratorToEnumVariantMarshaler), MarshalCookie = "")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(-4), TypeLibFunc((short)1)]
		new IEnumerator GetEnumerator();
	}

    internal enum NET_FW_ACTION_
	{
		NET_FW_ACTION_BLOCK,
		NET_FW_ACTION_ALLOW,
		NET_FW_ACTION_MAX
	}

    internal enum NET_FW_IP_PROTOCOL_
	{
		NET_FW_IP_PROTOCOL_ANY = 0x100,
		NET_FW_IP_PROTOCOL_TCP = 6,
		NET_FW_IP_PROTOCOL_UDP = 0x11
	}

    internal enum NET_FW_IP_VERSION_
	{
		NET_FW_IP_VERSION_V4,
		NET_FW_IP_VERSION_V6,
		NET_FW_IP_VERSION_ANY,
		NET_FW_IP_VERSION_MAX
	}

    internal enum NET_FW_MODIFY_STATE_
	{
		NET_FW_MODIFY_STATE_OK,
		NET_FW_MODIFY_STATE_GP_OVERRIDE,
		NET_FW_MODIFY_STATE_INBOUND_BLOCKED
	}

    internal enum NET_FW_PROFILE_TYPE_
	{
		NET_FW_PROFILE_DOMAIN,
		NET_FW_PROFILE_STANDARD,
		NET_FW_PROFILE_CURRENT,
		NET_FW_PROFILE_TYPE_MAX
	}

    internal enum NET_FW_PROFILE_TYPE2_
	{
		NET_FW_PROFILE2_ALL = 0x7fffffff,
		NET_FW_PROFILE2_DOMAIN = 1,
		NET_FW_PROFILE2_PRIVATE = 2,
		NET_FW_PROFILE2_PUBLIC = 4
	}

    internal enum NET_FW_RULE_DIRECTION_
	{
		NET_FW_RULE_DIR_IN = 1,
		NET_FW_RULE_DIR_MAX = 3,
		NET_FW_RULE_DIR_OUT = 2
	}

    internal enum NET_FW_SCOPE_
	{
		NET_FW_SCOPE_ALL,
		NET_FW_SCOPE_LOCAL_SUBNET,
		NET_FW_SCOPE_CUSTOM,
		NET_FW_SCOPE_MAX
	}

    internal enum NET_FW_SERVICE_TYPE_
	{
		NET_FW_SERVICE_FILE_AND_PRINT,
		NET_FW_SERVICE_UPNP,
		NET_FW_SERVICE_REMOTE_DESKTOP,
		NET_FW_SERVICE_NONE,
		NET_FW_SERVICE_TYPE_MAX
	}

}
