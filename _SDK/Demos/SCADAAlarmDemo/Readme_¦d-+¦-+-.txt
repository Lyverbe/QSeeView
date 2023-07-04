[Introduction]
The demo program introduces SDK initialization, login, logout, auto reconnection, obtain equipment information, obtain equipment point information, subscribe to monitoring point alarms, subscribe to monitoring point information, and subscribe to common alarms.
The demo program demonstrates detection collection device alarm, monitoring point alarms, monitoring point information.

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.SetAutoReconnect Set auto reconnection callback
NETClient.SetDVRMessCallBack Set alarm callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.QueryDevState  Get the ID of the external device connected to the current host
NETClient.SCADAGetAttributeInfo Get device point information
NETClient.SCADAAlarmAttachInfo Subscribe to monitoring point alarm information
NETClient.SCADAAlarmDetachInfo Unsubscribe from monitoring point alarm information
NETClient.SCADAAttachInfo Subscribe to monitoring point information
NETClient.SCADADetachInfo Unsubscribe from monitoring point information subscription
NETClient.StartListen Listen alarm
NETClient.StopListen Stop listening
NETClient.GetLastError Get the last function error code
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The demo program demonstrates listening alarm of single device, and it doesn’t support listening alarm of multiple devices. If you need listen alarm of multiple devices, modify it by yourself. 
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

【演示程序功能】
1、演示程序介绍了SDK初始化、登陆设备、登出设备、自动重连、获取设备信息，获取设备点位信息，订阅和取消监测点位报警，订阅和取消监测点位信息，订阅和取消普通报警。
2、演示程序演示了订阅和取消普通报警的检测采集设备报警、订阅和取消监测点位报警、订阅和取消监测点位信息。

【接口列表】
NETClient.Init SDK初始化与设置断线回调
NETClient.SetAutoReconnect 设置自动重连回调
NETClient.SetDVRMessCallBack 设置报警回调
NETClient.LoginWithHighLevelSecurity 设备登录
NETClient.Logout 设备登出
NETClient.QueryDevState 获取当前主机所接入的外部设备ID
NETClient.SCADAGetAttributeInfo 获取设备点位信息
NETClient.SCADAAlarmAttachInfo 订阅监测点位报警信息
NETClient.SCADAAlarmDetachInfo 取消订阅监测点位报警信息
NETClient.SCADAAttachInfo 订阅监测点位信息
NETClient.SCADADetachInfo 取消监测点位信息订阅
NETClient.StartListen 监听报警
NETClient.StopListen 停止监听
NETClient.GetLastError 获取错误信息
NETClient.Cleanup SDK释放资源


【注意事项】
1、编译环境为VS2010，NETSDKCS库最低只支持.NET Framework 4.0,如用户需要支持低于4.0的版本需要更改NetSDK.cs文件中使用到IntPtr.Add的方法,我们不提供修改。
2、此演示程序只演示监听单设备报警功能，不支持监听多设备报警功能，如用户有需求请自行修改。
3、请把General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX中libs目录中的所有文件复制到对应该演示程序中bin目录下的生成目录中。
