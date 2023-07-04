[Introduction]
The demo program introduces SDK initialization, login, logout, auto reconnection, Lattice Screen 

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.SetAutoReconnect Set auto reconnection callback
NETClient.SetDVRMessCallBack Set snapshot callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout  Logout
NETClient.ControlDevice Set park info, platform is set to camera,the content is used for Lattice Screen
NETClient.ControlDeviceEx New set screen info and broad info,the content is used for Lattice Screen
NETClient.Cleanup Release SDK resources

[Notice]
1. When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
2. The demo program does not support multiple devices login.
3. Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

【演示程序功能】
1、演示程序介绍了SDK初始化、登陆设备、登出设备、自动重连、点阵屏配置。

【接口列表】
NETClient.Init SDK初始化与设置断线回调
NETClient.SetAutoReconnect 设置自动重连回调
NETClient.LoginWithHighLevelSecurity 登录设备
NETClient.Logout 登录
NETClient.ControlDevice 设置停车信息，平台设置给相机，内容用于点阵屏显示
NETClient.ControlDeviceEx 新一代设置屏幕信息播报信息，内容用于点阵屏显示
NETClient.Cleanup 释放SDK资源

【注意事项】
1、编译环境为VS2010，NETSDKCS库最低只支持.NET Framework 4.0,如用户需要支持低于4.0的版本需要更改NetSDK.cs文件中使用到IntPtr.Add的方法,我们不提供修改。
2、此演示程序不支持多设备登陆。
3、请把General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX中libs目录中的所有文件复制到对应该演示程序中bin目录下的生成目录中。

