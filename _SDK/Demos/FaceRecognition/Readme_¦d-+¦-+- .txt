[Introduction]
The demo program introduces login, logout, starting real time monitor, stopping real time monitor, attaching event, detaching event, displaying uploaded face recognition and face detection event, querying  face library, adding  face library, modifying  face library, deleting face library, querying user, adding user, modifying user, deleting user, arming in channels according to face library, deleting face library in armed channels, and query face recognition records from registry database and history database through local contrast image.

[Interfaces]
NETClient.Init Initialize SDK and set disconnection callback
NETClient.LoginWithHighLevelSecurity Login
NETClient.Logout Logout
NETClient.RealLoadPicture  Attach event
NETClient.StopLoadPic Detach event
NETClient.RealPlay Start real time monitor
NETClient.RenderPrivateData Display/Not display headframe
NETClient.StopRealPlay Stop real time monitor
NETClient.FindGroupInfo Query face library
NETClient.OperateFaceRecognitionGroup Add, modify and delete face library
NETClient.StartFindFaceRecognition Start querying user
NETClient.StopFindFaceRecognition Stop querying user
NETClient.DoFindFaceRecognition Query user
NETClient.AttachFaceFindState Attach querying user
NETClient.DetachFaceFindState Detach querying user
NETClient.OperateFaceRecognitionDB Add, modify and delete user
NETClient.FaceRecognitionPutDisposition Arm according to the face library
NETClient.FaceRecognitionDelDisposition Disarm according to the face library
NETClient.StartFindFaceRecognition Start query face recognition records by local contrast image
NETClient.DoFindFaceRecognition Fetch query face recognition records by batch
NETClient.StopFindFaceRecognition Stop query face recognition records
NETClient.AttachFaceFindState Attach the preparation state of query results of face recognition records 
NETClient.DetachFaceFindState Detach the preparation state of query results of face recognition records
NETClient.DownloadRemoteFile Download file form remote device 
NETClient.Cleanup Release SDK resources

[Notice]
When the compiling environment is VS2010, NETSDKCS library can support the version of .NET Framework 4.0 or newer. If you want to use the version older than .NET Framework 4.0, change the method of using NetSDK.cs in IntPtr.Add. We will not support the modification.
The demo program does not support arming face library according to channels.
Copy all file in the libs directory of General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX to the build directory of bin directory of the corresponding demo programs.

����ʾ�����ܡ�
1����ʾ��������豸��¼���豸�ǳ�����ʵʱ���ӡ��ر�ʵʱ���ӡ������¼���ȡ�������¼�������ʶ������������¼��ϱ���Ϣ��ʾ��������Ĳ�ѯ�����������⡢�޸������⡢ɾ�������⡢����������Ա�Ĳ�ѯ��������Ա���޸���Ա��ɾ����Ա���������Ⲽ�ص�ͨ���С����������Ѳ��ص�ͨ����ɾ����ͨ�����ض���ͼƬ��������ע������ʷ���¼��

���ӿ��б�
NETClient.Init SDK��ʼ�������ö��߻ص�
NETClient.LoginWithHighLevelSecurity ��¼�豸
NETClient.Logout �ǳ��豸
NETClient.RealLoadPicture �����¼�
NETClient.StopLoadPic ֹͣ�����¼�
NETClient.RealPlay ʵʱ����
NETClient.RenderPrivateData ��ʾ�벻��ʾ��ͷ��
NETClient.StopRealPlay ֹͣʵʱ����
NETClient.FindGroupInfo ��ѯ������
NETClient.OperateFaceRecognitionGroup ���ӡ��޸ġ�ɾ��������
NETClient.StartFindFaceRecognition ��ʼ��ѯ��Ա
NETClient.StopFindFaceRecognitionֹͣ��ѯ��Ա
NETClient.DoFindFaceRecognition ��ѯ��Ա
NETClient.AttachFaceFindState ���Ĳ�ѯ��Ա
NETClient.DetachFaceFindState ȡ�����Ĳ�ѯ��Ա
NETClient.OperateFaceRecognitionDB ���ӡ��޸ġ�ɾ����Ա
NETClient.FaceRecognitionPutDisposition �������Ⲽ��
NETClient.FaceRecognitionDelDisposition �������⳷��
NETClient.StartFindFaceRecognition ��ʼ��ѯ�������¼
NETClient.DoFindFaceRecognition ���������ѯ������л�ȡ����
NETClient.StopFindFaceRecognition ֹͣ��ѯ�������¼
NETClient.AttachFaceFindState ����������¼��ѯ���׼������ 
NETClient.DetachFaceFindState �˶�������¼��ѯ���׼������
NETClient.DownloadRemoteFile ���豸�����ļ�
NETClient.Cleanup �ͷ�SDK��Դ

��ע�����
1�����뻷��ΪVS2010��NETSDKCS�����ֻ֧��.NET Framework 4.0,���û���Ҫ֧�ֵ���4.0�İ汾��Ҫ����NetSDK.cs�ļ���ʹ�õ�IntPtr.Add�ķ���,���ǲ��ṩ�޸ġ�
2������ʾ����֧�ְ�ͨ�����������⡣
3�����General_NetSDK_ChnEng_CSharpXX_IS_VX.XXX.XXXXXXXX.X.R.XXXXXX��libsĿ¼�е������ļ����Ƶ���Ӧ����ʾ������binĿ¼�µ�����Ŀ¼�С�