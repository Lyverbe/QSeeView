[Introduction]
The demo program introduces login, logout, starting real time monitor, stopping real time monitor, attaching event, detaching event, displaying uploaded personnel entry information and clearing the number of statistics of the day.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.RealPlay Start real time monitor
NETClient.RenderPrivateData Display/Not display rule
NETClient.StopRealPlay Stop monitor
NETClient.AttachVideoStatSummary Attach the number of statistics 
NETClient.DetachVideoStatSummary  Detach the number of statistics 
NETClient.ControlDevice Clear the number of statistics of the day
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.


����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ������¼���ȡ�������¼�����ʾ�ϱ�����Ա��ȥ��Ϣ����յ��������ͳ����Ϣ��

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.RealPlay ʵʱ����
NETClient.RenderPrivateData ��ʾ�벻��ʾ����
NETClient.StopRealPlay ֹͣ����
NETClient.AttachVideoStatSummary ��������ͳ��
NETClient.DetachVideoStatSummary ȡ������
NETClient.ControlDevice ��յ��������ͳ��
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
