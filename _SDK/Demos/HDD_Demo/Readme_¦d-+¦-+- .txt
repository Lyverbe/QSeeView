[Introduction]
The demo program introduces login, logout, and getting the information of DHH disk.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.QueryDevState Get the information of DHH disk
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ȡHDDӲ��Ϣ��

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.QueryDevState ��ȡHDDӲ����Ϣ
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
