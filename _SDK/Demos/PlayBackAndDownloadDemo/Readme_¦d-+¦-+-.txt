﻿[Introduction]
The demo program introduces SDK initialization,login, logout, playing back record by time (one day), downloading record by time, and functions such as fast play, slow play, pause, and stop. 
The demo program demonstrates selecting channel and stream type before playing back and downloading record.
For the display control with one day record in the demo program, we don’t provide technical support. 

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.SetAutoReconnect Set auto reconnection callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.PlayBackControl playback control, play, pause, stop, fast play, slow play, and normal play.
NETClient.QueryRecordFile Query record file 
NETClient.PlayBackByTime Playback record
NETClient.GetPlayBackOsdTime  Get the current time of playing record
NETClient.DownloadByTime Download record by time
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The demo program does not support the playback over one day. If you need the playback of multiple days, develop the corresponding display control by yourself.
The demo program supports the playback and downloading record of single channel only.
The demo program does not support multiple devices login.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

【演示程序功能】
1、演示程序介绍了SDK初始化、登陆设备、登出设备、按时间(一天时间)回放录像、按时间下载录像功能，快放，慢放、暂停，停止等功能。
2、演示程序演示了在回放或下载前可选择通道、码流类型。
3、演示程序中带有一天时间录像的显示控件，不提供技术支持。

【接口列表】
NETClient.Init SDK初始化与设置断线回调
NETClient.SetAutoReconnect 设置自动重连回调
NETClient.LoginWithHighLevelSecurity 登录设备
NETClient.Logout 登出设备
NETClient.PlayBackControl 回放控制，播放、暂停、停止、快放、慢放、正常播放
NETClient.QueryRecordFile 查询录像文件
NETClient.PlayBackByTime 录像回放
NETClient.GetPlayBackOsdTime 获取播放的当前时间
NETClient.DownloadByTime 通过时间下载录像
NETClient.Cleanup 释放SDK资源

【注意事项】
1、编译环境为VS2010，NETSDKCS库最低只支持.NET Framework 4.0,如用户需要支持低于4.0的版本需要更改NetSDK.cs文件中使用到IntPtr.Add的方法,我们不提供修改。
2、此演示程序不支持超一天的回放，如用户需要多天的录像回放，请自行开发相应的显示控件。
3、此演示程序只支持单通道录像回放及下载功能。
4、此演示程序不支持多设备登陆。
5、请把General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX中libs目录中的所有文件复制到对应该演示程序中bin目录下的生成目录中。
