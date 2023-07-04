[Introduction]
The demo program introduces login, logout, setting and getting  video code configuration by channel.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.GetNewDevConfig Get device configuration
NETClient.GetEncodeConfig Get video code configuration.
NETClient.SetEncodeConfig Set video code configuration.

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��½���豸�ǳ���ͨ��ͨ���������ȡ��Ƶ��������

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.GetNewDevConfig ������Ϣ
NETClient.GetEncodeConfig ��ȡ��Ƶ������Ϣ
NETClient.SetEncodeConfig ������Ƶ������Ϣ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
