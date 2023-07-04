[Introduction]
The demo program introduces searching device by multicast and broadcast, searching device by IPs, and device initialization.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.StartSearchDevice Search device by multicast and broadcast
NETClient.StopSearchDevice Stop searching
NETClient.SearchDevicesByIPs Search device by IPs
NETClient.InitDevAccount Initialize account
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Password recovery is not supported. 
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ�������ͨ���鲥��㲥�����豸����Ե������豸����ʼ���豸��

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.StartSearchDevice �鲥��㲥����
NETClient.StopSearchDevice ֹͣ����
NETClient.SearchDevicesByIPs ��Ե�����
NETClient.InitDevAccount ��ʼ���û�
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2����֧�������һء�
3�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�