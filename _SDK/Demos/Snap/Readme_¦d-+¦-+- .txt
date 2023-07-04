[Introduction]
The demo program introduces login, logout, starting real time monitor, stopping real time monitor, local snapshot, remote snapshot and timing snapshot.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.SetDVRMessCallBack Set snapshot callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.RealPlay Start real time monitor
NETClient.StopRealPlay Stop real time monitor
NETClient.SnapPictureEx  Local snapshot
NETClient.SnapPictureEx  Remote snapshot and timing  snapshot.
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The min time of real time snapshot is 1 s.
Ensure that you have turned on the real time monitor before local snapshot.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ�����ץͼ��Զ��ץͼ����ʱץͼ��

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.SetSnapRevCallBack ����ץͼ�ص�
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.RealPlay ʵʱ����
NETClient.StopRealPlay �ر�ʵʱ����
NETClient.CapturePicture ����ץͼ
NETClient.SnapPictureEx Զ��ץͼ�붨ʱץͼ
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2��ʵʱץͼ��Сʱ��Ϊ1�롣
3������ץͼ����Ҫ�ȴ�ʵʱ���ӡ�
4�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�