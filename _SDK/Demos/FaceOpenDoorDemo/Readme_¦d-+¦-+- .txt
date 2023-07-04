[Introduction]
The demo program introduce login, logout, starting real time monitor, stopping real time monitor, attaching event, detaching event, displaying uploaded event, querying card , adding card(with face), modifying card (with face), deleting card and clearing card.

[Interfaces]
NETClient.Init  Initialize SDK and set disconnection callback
NETClient.SetAutoReconnect Set reconnection callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.GetDevConfig Get the time zone
NETClient.RealPlay Start real time monitor
NETClient.RenderPrivateData Display/Not display headframe
NETClient.StopRealPlay Stop monitor
NETClient.RealLoadPicture  Attach event
NETClient.StopLoadPic Detach event
NETClient.FindRecord  Query card
NETClient.FindNextRecord Query the details of card
NETClient.FindRecordClose Close card query
NETClient.ControlDevice Add, modify, delete and clear card
NETClient.FaceInfoOpreate Add, modify, delete and clear face

[Notice]
When the compiling environment is VS2013, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The card in the demo program does not support fingerprint function.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.
FaceOpen Event is supported by many kind of devices, but there exist some differences in fitness among those devices. This demo is mainly suitable for devices belong to ASI all-in-one access control series.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ������¼���ȡ�������¼�����ʾ�ϱ����¼���Ϣ������ѯ�����ӿ�(������)���޸Ŀ�(������)��ɾ��������տ���

���ӿ��б�
NETClient.Init SDK��ʼ�������ö��߻ص�
NETClient.SetAutoReconnect ���������ص�
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.GetDevConfig ��ȡʱ��
NETClient.RealPlay ʵʱ����
NETClient.RenderPrivateData ��ʾ�벻��ʾ��ͷ��
NETClient.StopRealPlay ֹͣ����
NETClient.RealLoadPicture �����¼�
NETClient.StopLoadPic ֹͣ�����¼�
NETClient.FindRecord ��ѯ��
NETClient.FindNextRecord ��ѯ����ϸ��Ϣ
NETClient.FindRecordClose �رղ�ѯ
NETClient.ControlDevice ���ӿ����޸Ŀ���ɾ��������տ�
NETClient.FaceInfoOpreate �����������޸�������ɾ���������������

��ע�����
1�����뻷��ΪVS2013��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2����ʾ�����в���ָ�ƹ��ܡ�
3�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
4��֧�����������¼����豸�϶࣬����ͬϵ�е��豸֮�����䷽ʽ��һ���Ĳ��죬��demo��Ҫ����ASI�Ž�һ���ϵ�е��豸��