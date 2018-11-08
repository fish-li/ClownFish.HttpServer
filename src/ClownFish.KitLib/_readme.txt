
关于此项目的一些说明
---------------------------------------------------
1、这个项目包含了一些工具类，主要是解决HTTP服务的安装部署问题
2、SslCertBinding_Net目录是一个外部项目的代码，https://github.com/segor/SslCertBinding.Net
3、NetFwTypeLib 是引用同名的COM之后得到DLL，再反编译后得到的代码，放到这里是为做强名称
4、NetFwTypeLib 的部分代码有问题，主要是有些索引器属性就变成了普通属性，例如：INetFwPolicy2.FirewallEnabled

