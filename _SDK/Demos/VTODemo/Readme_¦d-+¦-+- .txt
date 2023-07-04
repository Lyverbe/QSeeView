[Introduction]
The demo program introduces login, logout, starting real time monitor, stopping real time monitor, opening voice intercom, closing voice intercom, opening door, closing door, listening alarm event, stopping alarm listening, listening realLoadPicture event, stopping realLoadPicture listening, displaying uploaded event, querying card (with fingerprint), adding card(with fingerprint and face), modifying card (with fingerprint and face), deleting card, clearing card, and collecting fingerprint.

[Interfaces]
NETClient.Init Initialize SDK
NETClient.SetDVRMessCallBack Set alarm callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.GetDevConfig Get the time zone
NETClient.RealPlay Start real time monitor
NETClient.StopRealPlay Stop monitor
NETClient.SetDeviceMode  Set mode
NETClient.SetDeviceMode  Start voice intercom
NETClient.RecordStart Start PC sound record
NETClient.RecordStart Stop PC sound record
NETClient.StopTalk  Stop voice intercom
NETClient.TalkSendData Send audio data to the device
NETClient.AudioDecEx  Audio decode
NETClient.ControlDevice Open door, close door, add card, modify card, delete card, clear card, and collect fingerprint
NETClient.StartListen Start listening event
NETClient.StopListen Stop listening event
NETClient.RealLoadPicture Start listening intelligent event
NETClient.StopLoadPic Stop listening intelligent event
NETClient.FindRecord  Query card
NETClient.FindNextRecord Query the details of the card
NETClient.QueryDevState Get fingerprint
NETClient.FindRecordClose Close query
NETClient.FaceInfoOpreate Add, modify, delete and clear face
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ��򿪶Խ����رնԽ������š����š����������¼���ȡ���������������������¼���ȡ�������¼���������ʾ�¼��ϱ���Ϣ����ѯ��(��ָ����Ϣ)�����ӿ�(��ָ�ƺ�����ͼƬ)���޸Ŀ�(��ָ�ƺ�����ͼƬ)��ɾ��������տ����ɼ�ָ�ơ�

���ӿ��б�
NETClient.Init SDK��ʼ��
NETClient.SetDVRMessCallBack ���ñ����ص�
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.GetDevConfig ��ȡʱ��
NETClient.RealPlay ʵʱ����
NETClient.StopRealPlay ֹͣ����
NETClient.SetDeviceMode ����ģʽ
NETClient.StartTalk ��ʼ�Խ�
NETClient.RecordStart ��ʼ¼��
NETClient.RecordStop ֹͣ¼��
NETClient.StopTalk ֹͣ�Խ�
NETClient.TalkSendData ������������
NETClient.AudioDecEx �������ݽ���
NETClient.ControlDevice ���š����š����ӿ����޸Ŀ���ɾ��������տ���ָ�Ʋɼ�
NETClient.StartListen ��ʼ���������¼�
NETClient.StopListen ֹͣ���������¼�
NETClient.RealLoadPicture ��ʼ���������¼�
NETClient.StopLoadPic Stop ֹͣ���������¼�
NETClient.FindRecord ��ѯ��
NETClient.FindNextRecord ��ѯ����ϸ��Ϣ
NETClient.QueryDevState ��ȡָ����Ϣ
NETClient.FindRecordClose �رղ�ѯ
NETClient.FaceInfoOpreate �����������޸�������ɾ���������������
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�
