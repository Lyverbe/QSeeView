[Introduction]
The demo program introduces login, logout, starting real time monitor, stopping real time monitor, getting and configuring OSD of main stream (channel title), getting and configuring OSD of main stream (time title), and getting and configuring OSD of main stream (customized title).

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.SetAutoReconnect Set reconnection callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.RealPlay Start real time monitor
NETClient.StopRealPlay Stop real time monitor
NETClient.GetOSDConfig Get OSD configuration
NETClient.SetOSDConfig Configure OSD
NETClient.Cleanup Release SDK resources


[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The demo program supports main stream configuration only.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ���ȡ��������������ͨ�������OSD���á���ȡ��������������ʱ������OSD���á���ȡ���������������Զ�������OSD���á�

���ӿ��б�
NETClient.Init SDK��ʼ�������ö��߻ص�
NETClient.SetAutoReconnect ���������ص�
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.RealPlay ʵʱ����
NETClient.StopRealPlay ֹͣʵʱ����
NETClient.GetOSDConfig ��ȡOSD����
NETClient.SetOSDConfig ����OSD����
NETClient.Cleanup �ͷ�SDK��Դ


��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2��ֻ֧�����������á�
3�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�