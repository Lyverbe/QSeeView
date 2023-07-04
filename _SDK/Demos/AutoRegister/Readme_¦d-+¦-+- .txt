[Introduction]
The demo program introduces listening,device configuration, adding device, modifying device, deleting device, batch import, export, cleaning device list, login, logout, auto reconnection callback, starting real time monitor, stopping real time monitor, starting voice intercom, stopping voice intercom and snapshot.

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.SetDVRMessCallBack Set snapshot callback
NETClient.ListenServer  Start listening service
NETClient.StopListenServer Stop listening service
NETClient.Cleanup Release SDK resources
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.RealPlay Start real time monitor
NETClient.StopRealPlay Stop real time monitor
NETClient.SetDeviceMode  Set voice intercom mode
NETClient.StartTalk  Start voice intercom
NETClient.RecordStart Start PC sound record
NETClient.RecordStop Stop PC sound record
NETClient.StopTalk Stop voice intercom
NETClient.SnapPictureEx Snapshot
NETClient.GetNewDevConfig Get device configuration
NETClient.SetNewDevConfig Set device configuration
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ�������������ע��������豸����ע������á������豸���޸��豸��ɾ���豸���豸�������롢�������豸�б���ա��豸ע����¼�豸���豸�ǳ����豸�����Զ���������ʵʱ���ӡ��ر�ʵʱ���ӡ��򿪶Խ���ֹͣ�Խ���ץͼ��

���ӿ��б�
NETClient.Init SDK��ʼ�������ö��߻ص�
NETClient.SetSnapRevCallBack ����ץͼ�ص�
NETClient.ListenServer ����ע�����ʼ����
NETClient.StopListenServer ����ע�����ֹͣ����
NETClient.Cleanup �ͷ�SDK��Դ
NETClient.LoginWithHighLevelSecurity �豸��¼
NETClient.Logout �豸�ǳ�
NETClient.RealPlay ʵʱ����
NETClient.StopRealPlay ֹͣʵʱ����
NETClient.SetDeviceMode ���öԽ�ģʽ
NETClient.StartTalk ��ʼ�Խ�
NETClient.RecordStart ��ʼ¼��
NETClient.RecordStop ֹͣ¼��
NETClient.StopTalk ֹͣ�Խ�
NETClient.SnapPictureEx ץͼ
NETClient.GetNewDevConfig ��ȡ����ע��������Ϣ
NETClient.SetNewDevConfig ��������ע��������Ϣ
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
