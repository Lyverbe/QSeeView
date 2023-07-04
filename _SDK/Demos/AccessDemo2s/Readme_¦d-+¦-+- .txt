[Introduction]
1.Demo SDK initialization,login device,logout device, control the card password, open door, query status,receive real-time information about open door function.
2.Demo how to add,delete and edit the card information, query the door status,open door mode,show receive the information.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.SetDVRMessCallBack Set alarm callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.GetNewDevConfig configuration get,Access control, multi-person combination to open the door, Repeat Enter Route, card time, door configuration, holiday configuration
NETClient.SetNewDevConfig configuration set,Access control, multi-person combination to open the door, Repeat Enter Route, card time, door configuration, holiday configuration
NETClient.GetDevConfig Get the time zone
NETClient.RealPlay Start real time monitor
NETClient.StopRealPlay Stop monitor
NETClient.RebootDev Reboot device
NETClient.GetDevConfig Get device time
NETClient.SetDevConfig Set device time
NETClient.ControlDevice Collect fingerprint
NETClient.StartQueryLog query log
NETClient.ControlDevice Open door, close door, 
NETClient.StartListen Start listening event
NETClient.StopListen Stop listening event
NETClient.FindRecord  Query card
NETClient.FindNextRecord Query the details of the card
NETClient.QueryDevState Get fingerprint
NETClient.FindRecordClose Close query
NETClient.InsertOperateAccessUserService   insert user info
NETClient.GetOperateAccessUserService      get user info
NETClient.RemoveOperateAccessUserService   remove user info
NETClient.ClearOperateAccessUserService    clear user info
NETClient.StartFindUserInfo  start find user info
NETClient.DoFindUserInfo  do find user info
NETClient.StopFindUserInfo stop find user info 
NETClient.InsertOperateAccessCardService   insert card 
NETClient.UpdateOperateAccessCardService   update card
NETClient.GetOperateAccessCardService      get card
NETClient.RemoveOperateAccessCardService   remove card
NETClient.ClearOperateAccessCardService    clear card
NETClient.StartFindCardInfo  start find card info 
NETClient.DoFindCardInfo     do find card info 
NETClient.StopFindCardInfo   stop find card info 
NETClient.InsertOperateAccessFingerprintService  insert Fingerprint
NETClient.GetOperateAccessFingerprintService    get Fingerprint
NETClient.RemoveOperateAccessFingerprintService remove Fingerprint
NETClient.UpdateOperateAccessFingerprintService update Fingerprint
NETClient.ClearOperateAccessFingerprintService  clear Fingerprint
NETClient.OperateAccessFaceService the operation function of face info,insert,get,update,remove,clear
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

【演示程序功能】
1、Demo介绍SDK初始化、登陆设备、登出设备、门禁卡片密码操作、远程开门控制、状态查询、开门信息实时上报等功能。
2、Demo演示了门禁卡片密码增删改查的核心功能，查询开门状态，远程开门方式，实时接收开门信息上报等功能。

【接口列表】
NETClient.Init SDK初始化
NETClient.SetDVRMessCallBack 设置报警回调
NETClient.LoginWithHighLevelSecurity 登录设备
NETClient.Logout 登出设备
NETClient.GetNewDevConfig 配置获取,门禁，多人组合开门，防反潜，刷卡时间，门配置，假期配置
NETClient.SetNewDevConfig 配置设置，门禁，多人组合开门，防反潜，刷卡时间，门配置，假期配置
NETClient.GetDevConfig 获取时区
NETClient.RealPlay 实时监视
NETClient.StopRealPlay 停止监视
NETClient.RebootDev 重启设备
NETClient.GetDevConfig 获取设备时间
NETClient.SetDevConfig 设置设备时间
NETClient.ControlDevice 采集指纹
NETClient.StartQueryLog 日志查询
NETClient.ControlDevice 开门、关门、
NETClient.StartListen 开始监听报警事件
NETClient.StopListen 停止监听报警事件
NETClient.FindRecord 查询卡
NETClient.FindNextRecord 查询卡详细信息
NETClient.QueryDevState 获取指纹信息
NETClient.FindRecordClose 关闭查询
NETClient.InsertOperateAccessUserService   添加人员信息
NETClient.GetOperateAccessUserService   获取人员信息
NETClient.RemoveOperateAccessUserService   删除人员信息
NETClient.ClearOperateAccessUserService    清空所有人员信息
NETClient.StartFindUserInfo  开始查询人员信息
NETClient.DoFindUserInfo 获取人员信息
NETClient.StopFindUserInfo 停止查询人员信息
NETClient.InsertOperateAccessCardService   新增门禁卡操作
NETClient.UpdateOperateAccessCardService   更新门禁卡操作
NETClient.GetOperateAccessCardService      获取门禁卡操作
NETClient.RemoveOperateAccessCardService   删除门禁卡操作
NETClient.ClearOperateAccessCardService    清空门禁卡操作
NETClient.StartFindCardInfo  开始查询卡片信息
NETClient.DoFindCardInfo     获取卡片信息
NETClient.StopFindCardInfo   停止查询卡片信息
NETClient.InsertOperateAccessFingerprintService 人员新增指纹操作，
NETClient.GetOperateAccessFingerprintService    人员获取指纹操作，
NETClient.RemoveOperateAccessFingerprintService 人员删除指纹操作，
NETClient.UpdateOperateAccessFingerprintService 人员更新指纹操作，
NETClient.ClearOperateAccessFingerprintService  人员清空指纹操作，
NETClient.OperateAccessFaceService 人员人脸操作，添加，获取，更新，删除，清空
NETClient.Cleanup 释放SDK资源

【注意事项】
1、编译环境为VS2010，NETSDKCS库最低只支持.NET Framework 4.0,如用户需要支持低于4.0的版本需要更改NetSDK.cs文件中使用到IntPtr.Add的方法,我们不提供修改。
2、请把General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX中libs目录中的所有文件复制到对应该演示程序中bin目录下的生成目录中。
