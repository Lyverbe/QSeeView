using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSDKCS;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace VTODemo
{
    public partial class VTO : Form
    {
        IntPtr loginID = IntPtr.Zero;
        NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
        fDisConnectCallBack disConnectCB;
        fHaveReConnectCallBack reConnectCB;

        IntPtr playID = IntPtr.Zero;
        IntPtr talkID = IntPtr.Zero;

        fAudioDataCallBack audioDataCallBack;

        /* Alarm Event */
        bool isListen = false;
        fMessCallBackEx alarmCallBack;
        const int Line = 50;
        // An event is registered here and used in alarm callback function in order to indicate the fully reception of fingerprint data.
        // 这里注册了一个event，Alarm回调中会调用。调用的方法被定义在指纹录入窗体中，用以通知它已收到指纹数据
        public event Action<IntPtr, NET_ALARM_CAPTURE_FINGER_PRINT_INFO, byte[]> MessCallBackEx;
        private void OnMessageCallBackEx(IntPtr id, NET_ALARM_CAPTURE_FINGER_PRINT_INFO message, byte[] data)
        {
            if (MessCallBackEx != null)
            {
                MessCallBackEx(id, message, data);
            }
        }

        /* RealLoad Event */
        bool isRealLoad = false;
        IntPtr realRoadID = IntPtr.Zero;
        fAnalyzerDataCallBack analyzerDataCallBack;
        const string savePath = "RealLoadPath/";

        public VTO()
        {
            InitializeComponent();
            try
            {
                // Register DisConnect and ReConnect Callback Functions 注册短线回调和重连回调函数
                disConnectCB = new fDisConnectCallBack(DefaultDisConnectCallBack);
                reConnectCB = new fHaveReConnectCallBack(DefaultReConnectCallBack);
                NETClient.InitWithDefaultSetting(disConnectCB, reConnectCB, IntPtr.Zero, null);
                System.Console.WriteLine("使用默认断线回调和重连回调初始化SDK");

                // New Audio Callback Function 新建语音回调函数，但注册设在其他位置
                audioDataCallBack = new fAudioDataCallBack(AudioDataCallBack);

                // Register Alarm Callback Function 注册报警回调函数并设置
                alarmCallBack = new fMessCallBackEx(MessCallBack);
                NETClient.SetDVRMessCallBack(alarmCallBack, IntPtr.Zero);

                // New RealLoadPic Callback Function
                analyzerDataCallBack = new fAnalyzerDataCallBack(AnalyzerDataCallBack);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
            }
            this.Load += new EventHandler(VTO_Load);
        }

        void VTO_Load(object sender, EventArgs e)
        {
            this.button_realplay.Enabled = false;
            this.button_stopplay.Enabled = false;
            this.button_talk.Enabled = false;
            this.button_stoptalk.Enabled = false;
            this.button_operatecard.Enabled = false;
            this.button_open.Enabled = false;
            this.button_close.Enabled = false;
            this.button_startlisten.Enabled = false;
            this.button_stoplisten.Enabled = false;
            this.button_startRealLoad.Enabled = false;
            this.button_stopRealLoad.Enabled = false;
            createImgSaveFolder();
        }

        // Port端口 键入值校验
        private void textBox_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 不是退格键也不是0-9时触发
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;   // 过滤输入（即不执行输入）
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (talkID != IntPtr.Zero)
            {
                NETClient.RecordStop(loginID);
                NETClient.StopTalk(talkID);
            }
            NETClient.Cleanup();
        }

        void DefaultDisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            // Todo
            Console.WriteLine("Device[%s] Port[%d] DisConnect!", pchDVRIP, nDVRPort);
            this.BeginInvoke((Action)UpdateDisConnectedUI);
        }

        private void UpdateDisConnectedUI()
        {
            this.Text = "VTODemo(室外机Demo) -- OffLine";
        }

        void DefaultReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            // Todo
            Console.WriteLine("Device[%s] Port[%d] ReConnect!", pchDVRIP, nDVRPort);
            this.BeginInvoke((Action)UpdateReConnectedUI);
        }

        private void UpdateReConnectedUI()
        {
            this.Text = "VTODemo(室外机Demo) -- OnLine";
        }

        private void createImgSaveFolder()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
        }

        // Login & Logout
        private void button_login_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == loginID)    // 无登陆句柄，正常登录 
            {
                ushort port = 0;
                try
                {
                    port = Convert.ToUInt16(this.textBox_port.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Port error(端口错误)");
                    return;
                }
                loginID = NETClient.LoginWithHighLevelSecurity(
                    this.textBox_ip.Text.Trim(),      // ip addr
                    port,                             // port
                    this.textBox_name.Text.Trim(),    // username
                    this.textBox_password.Text,       // password
                    EM_LOGIN_SPAC_CAP_TYPE.TCP,       // Type
                    IntPtr.Zero, ref deviceInfo);     // null, null
                if (loginID == IntPtr.Zero) // login handle invalid 获取的登陆句柄无效
                {
                    MessageBox.Show(NETClient.GetLastError());
                    System.Console.WriteLine(NETClient.GetLastError());
                    return;
                }
                this.button_login.Text = "Logout(登出)";
                this.button_realplay.Enabled = true;
                this.button_talk.Enabled = true;
                this.button_operatecard.Enabled = true;
                this.button_open.Enabled = true;
                this.button_close.Enabled = true;
                this.button_startlisten.Enabled = true;
                this.button_startRealLoad.Enabled = true;
                this.Text = "VTODemo(室外机Demo) -- OnLine";
            }
            else
            {
                if (isListen)
                {
                    NETClient.StopListen(loginID);
                    isListen = false;
                }
                if (talkID != IntPtr.Zero)
                {
                    NETClient.RecordStop(loginID);
                    NETClient.StopTalk(talkID);
                    talkID = IntPtr.Zero;
                }
                if (isRealLoad)
                {
                    NETClient.StopLoadPic(realRoadID);
                    realRoadID = IntPtr.Zero;
                    isRealLoad = false;
                }

                NETClient.Logout(loginID);
                loginID = IntPtr.Zero;
                playID = IntPtr.Zero;
                this.button_login.Text = "Login(登录)";
                this.button_realplay.Enabled = false;
                this.button_stopplay.Enabled = false;
                this.button_talk.Enabled = false;
                this.button_stoptalk.Enabled = false;
                this.button_operatecard.Enabled = false;
                this.button_open.Enabled = false;
                this.button_close.Enabled = false;
                this.button_startlisten.Enabled = false;
                this.button_stoplisten.Enabled = false;
                this.button_startRealLoad.Enabled = false;
                this.button_stopRealLoad.Enabled = false;
                this.pictureBox_play.Refresh();
                this.pictureBox_realLoadEvent.Refresh();
                this.listView_event.Items.Clear();
                this.listView_realLoadEvent.Items.Clear();
                this.Text = "VTODemo(室外机Demo) -- OffLine";
            }
        }

        #region realPlay(预览)

        /// <summary>
        /// start realPlay 开始预览
        /// </summary>
        private void button_realplay_Click(object sender, EventArgs e)
        {
            playID = NETClient.RealPlay(
                loginID,                          // 登陆句柄
                0,                                // 通道号
                this.pictureBox_play.Handle);     // 指定显示视频的 Panel 指针
            if (playID == IntPtr.Zero) // 句柄无效
            {
                MessageBox.Show(NETClient.GetLastError());
                System.Console.WriteLine(NETClient.GetLastError());
                return;
            }
            this.button_realplay.Enabled = false;
            this.button_stopplay.Enabled = true;
        }

        /// <summary>
        /// stop realPlay 结束预览
        /// </summary>
        private void button_stopplay_Click(object sender, EventArgs e)
        {
            NETClient.StopRealPlay(playID);
            playID = IntPtr.Zero;
            this.button_realplay.Enabled = true;
            this.button_stopplay.Enabled = false;
            this.pictureBox_play.Refresh();
        }

        #endregion

        #region audio(语音)

        /// <summary>
        /// start talk 开始语音
        /// </summary>
        private void button_talk_Click(object sender, EventArgs e)
        {
            IntPtr talkEncodePointer = IntPtr.Zero;
            IntPtr talkSpeakPointer = IntPtr.Zero;

            NET_DEV_TALKDECODE_INFO talkCodeInfo = new NET_DEV_TALKDECODE_INFO();
            talkCodeInfo.encodeType = EM_TALK_CODING_TYPE.PCM;
            talkCodeInfo.dwSampleRate = 8000;
            talkCodeInfo.nAudioBit = 16;
            talkCodeInfo.nPacketPeriod = 25;
            talkCodeInfo.reserved = new byte[60];

            talkEncodePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_TALKDECODE_INFO)));
            Marshal.StructureToPtr(talkCodeInfo, talkEncodePointer, true);
            // set talk encode type 设置对讲编码类型
            NETClient.SetDeviceMode(
                loginID,                                 // login handle 登陆句柄
                EM_USEDEV_MODE.TALK_ENCODE_TYPE,         // type 类型：语音对讲编码格式
                talkEncodePointer);                      // param 指针->NET_DEV_TALKDECODE_INFO

            NET_SPEAK_PARAM speak = new NET_SPEAK_PARAM();
            speak.dwSize = (uint)Marshal.SizeOf(typeof(NET_SPEAK_PARAM));
            speak.nMode = 0;
            speak.bEnableWait = false;
            speak.nSpeakerChannel = 0;
            talkSpeakPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SPEAK_PARAM)));
            Marshal.StructureToPtr(speak, talkSpeakPointer, true);
            //set talk speak mode 设置对讲模式
            NETClient.SetDeviceMode(
                loginID,                                // login handle 登陆句柄
                EM_USEDEV_MODE.TALK_SPEAK_PARAM,        // type 类型：语音对讲参数 
                talkSpeakPointer);                      // param 指针->NET_SPEAK_PARAM

            // Start talking 开始对讲，Init里注册的语音回调函数在这里注册到SDK里
            talkID = NETClient.StartTalk(loginID, audioDataCallBack, IntPtr.Zero);
            Marshal.FreeHGlobal(talkEncodePointer);
            Marshal.FreeHGlobal(talkSpeakPointer);
            if (IntPtr.Zero == talkID)
            {
                MessageBox.Show(this, NETClient.GetLastError());
                System.Console.WriteLine(NETClient.GetLastError());
                return;
            }
            bool ret = NETClient.RecordStart(loginID);   // start record 开始录音
            if (!ret)
            {
                NETClient.StopTalk(talkID);  // start record failed， stop talk as well 录音启动失败，则终止语音
                talkID = IntPtr.Zero;
                MessageBox.Show(this, NETClient.GetLastError());
                System.Console.WriteLine(NETClient.GetLastError());
                return;
            }
            this.button_talk.Enabled = false;
            this.button_stoptalk.Enabled = true;
        }

        /// <summary>
        /// Audio Callback Function
        /// 语音回调函数，用户定义 
        /// </summary>
        /// <param name="lTalkHandle">StartTalk returns value StartTalk返回值</param>
        /// <param name="pDataBuf">audio data 语音数据缓存</param>
        /// <param name="dwBufSize">audio data size 数据缓存大小</param>
        /// <param name="byAudioFlag">audio flag,for send or dec 标志用于发送语音或解码</param>
        /// <param name="dwUser">user data 用户数据</param>
        private void AudioDataCallBack(IntPtr lTalkHandle, IntPtr pDataBuf, uint dwBufSize, byte byAudioFlag, IntPtr dwUser)
        {
            if (lTalkHandle != talkID)
            {
                return;
            }
            if (0 == byAudioFlag)
            {
                // send talk data 发送语音数据
                NETClient.TalkSendData(lTalkHandle, pDataBuf, dwBufSize);
            }
            if (1 == byAudioFlag || 2 == byAudioFlag)
            {
                try
                {
                    NETClient.AudioDecEx(lTalkHandle, pDataBuf, dwBufSize);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// stop talk 终止语音
        /// </summary>
        private void button_stoptalk_Click(object sender, EventArgs e)
        {
            NETClient.RecordStop(loginID);    // 终止录音、终止语音
            NETClient.StopTalk(talkID);
            talkID = IntPtr.Zero;    // 清空句柄
            this.button_talk.Enabled = true;
            this.button_stoptalk.Enabled = false;
        }

        #endregion

        #region operate(操作)

        // Operate 操作
        private void button_operatecard_Click(object sender, EventArgs e)
        {
            OperateManager cm = new OperateManager(loginID, this);   // 创建 VTODemo 窗口
            cm.ShowDialog();     // 显示窗口，窗口关闭前会一直阻塞
            cm.Dispose();
        }

        #endregion

        #region door(门)

        // OpenDoor 开门
        private void button_open_Click(object sender, EventArgs e)
        {
            NET_CTRL_ACCESS_OPEN openInfo = new NET_CTRL_ACCESS_OPEN();
            openInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_CTRL_ACCESS_OPEN));
            openInfo.nChannelID = 0;
            openInfo.szTargetID = IntPtr.Zero;
            openInfo.emOpenDoorType = EM_OPEN_DOOR_TYPE.REMOTE;
            IntPtr inPtr = IntPtr.Zero;
            try
            {
                inPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_CTRL_ACCESS_OPEN)));
                Marshal.StructureToPtr(openInfo, inPtr, true);
                bool ret = NETClient.ControlDevice(loginID, EM_CtrlType.ACCESS_OPEN, inPtr, 10000);
                if (!ret)
                {
                    MessageBox.Show("Open door failed(开门失败)");
                    return;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(inPtr);
            }
            MessageBox.Show("Open door successfully(开门成功)");
        }

        // CloseDoor 关门
        private void button_close_Click(object sender, EventArgs e)
        {
            NET_CTRL_ACCESS_CLOSE closeInfo = new NET_CTRL_ACCESS_CLOSE();
            closeInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_CTRL_ACCESS_CLOSE));
            closeInfo.nChannelID = 0;
            IntPtr inPtr = IntPtr.Zero;
            try
            {
                inPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_CTRL_ACCESS_CLOSE)));
                Marshal.StructureToPtr(closeInfo, inPtr, true);
                bool ret = NETClient.ControlDevice(loginID, EM_CtrlType.ACCESS_CLOSE, inPtr, 10000);
                if (!ret)
                {
                    MessageBox.Show("Close door failed(关门失败)");
                    return;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(inPtr);
            }
            MessageBox.Show("Close door successfully(关门成功)");
        }

        #endregion

        #region AlarmEvent(报警事件)

        /// <summary>
        /// 事件监听开始
        /// start alarm listen
        /// </summary>
        private void button_startlisten_Click(object sender, EventArgs e)
        {
            if (isListen == false)
            {
                NETClient.StartListen(loginID);
                isListen = true;
                this.button_startlisten.Enabled = false;
                this.button_stoplisten.Enabled = true;
                tabControl_eventType.SelectTab(0);
            }
        }

        /// <summary>
        /// 事件监听停止
        /// stop alarm listen
        /// </summary>
        private void button_stoplisten_Click(object sender, EventArgs e)
        {
            if (isListen)
            {
                NETClient.StopListen(loginID);
                isListen = false;
                this.button_startlisten.Enabled = true;
                this.button_stoplisten.Enabled = false;
                this.listView_event.Items.Clear();
            }
        }

        /// <summary>
        /// Alarm message callback function
        /// 报警回调函数
        /// </summary>
        /// <param name="lCommand">alarm type,see EM_ALARM_TYPE 报警类型</param>
        /// <param name="lLoginID">loginID:login returns value 登陆ID</param>
        /// <param name="pBuf">alarm data 报警数据缓存</param>
        /// <param name="dwBufLen">alarm data length 数据大小</param>
        /// <param name="pchDVRIP">device ip,string type 设备IP</param>
        /// <param name="nDVRPort">device port 设备端口</param>
        /// <param name="bAlarmAckFlag">true:the event is affirmable event;false:the event is not affirmable event TRUE,该事件为可以进行确认的事件；FALSE,该事件无法进行确认</param>
        /// <param name="nEventID">used to AlarmAck function,when the bAlarmAckFlag is true,this paramter is valid 用于对 CLIENT_AlarmAck 接口的入参进行赋值,当 bAlarmAckFlag 为 TRUE 时,该数据有效</param>
        /// <param name="dwUser">user data from SetDVRMessCallBack function 用户数据</param>
        /// <returns>reserved</returns>
        private bool MessCallBack(int lCommand, IntPtr lLoginID, IntPtr pBuf, uint dwBufLen, IntPtr pchDVRIP, int nDVRPort, bool bAlarmAckFlag, int nEventID, IntPtr dwUser)
        {
            if ((EM_ALARM_TYPE)lCommand == EM_ALARM_TYPE.ALARM_ACCESS_CTL_EVENT)
            {
                if (lLoginID != loginID)
                {
                    return true;
                }
                NET_ALARM_ACCESS_CTL_EVENT_INFO info = (NET_ALARM_ACCESS_CTL_EVENT_INFO)Marshal.PtrToStructure(pBuf, typeof(NET_ALARM_ACCESS_CTL_EVENT_INFO));
                this.BeginInvoke(new Action(() =>
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = Encoding.Default.GetString(info.szUserID);
                    item.SubItems.Add(info.szCardNo.ToUpper());
                    item.SubItems.Add(info.stuTime.ToString());
                    switch (info.emOpenMethod)
                    {
                        case EM_ACCESS_DOOROPEN_METHOD.CARD:
                            item.SubItems.Add("Card(卡)");
                            break;
                        case EM_ACCESS_DOOROPEN_METHOD.FACE_RECOGNITION:
                            item.SubItems.Add("Face recognition(人脸识别)");
                            break;
                        case EM_ACCESS_DOOROPEN_METHOD.FINGERPRINT:
                            item.SubItems.Add("Fingerprint(指纹)");
                            break;
                        case EM_ACCESS_DOOROPEN_METHOD.REMOTE:
                            item.SubItems.Add("Remote(远程)");
                            break;
                        default:
                            item.SubItems.Add("Unknown(未知)");
                            break;
                    }
                    if (info.bStatus)
                    {
                        item.SubItems.Add("True(成功)");
                    }
                    else
                    {
                        item.SubItems.Add("False(失败)");
                    }

                    listView_event.BeginUpdate();
                    listView_event.Items.Insert(0, item);
                    if (listView_event.Items.Count > Line)
                    {
                        listView_event.Items.RemoveAt(Line);
                    }
                    listView_event.EndUpdate();

                }));
            }
            if ((EM_ALARM_TYPE)lCommand == EM_ALARM_TYPE.ALARM_FINGER_PRINT)
            {
                NET_ALARM_CAPTURE_FINGER_PRINT_INFO info = (NET_ALARM_CAPTURE_FINGER_PRINT_INFO)Marshal.PtrToStructure(pBuf, typeof(NET_ALARM_CAPTURE_FINGER_PRINT_INFO));
                if (info.bCollectResult)
                {
                    byte[] data = new byte[info.nPacketLen * info.nPacketNum];
                    Marshal.Copy(info.szFingerPrintInfo, data, 0, data.Length);
                    OnMessageCallBackEx(lLoginID, info, data);
                }
            }
            return true;
        }

        #endregion

        # region RealLoadPic (智能事件)

        /// <summary>
        /// event data callback
        /// 事件数据回调函数
        /// </summary>
        /// <param name="lAnalyzerHandle">analyzerHandle:RealLoadPicture returns value 事件句柄</param>
        /// <param name="dwEventType">event type,see EM_EVENT_IVS_TYPE 事件类型</param>
        /// <param name="pEventInfo">event information 事件信息</param>
        /// <param name="pBuffer">picture buffer 数据缓存</param>
        /// <param name="dwBufSize">picture buffer size 数据缓存大小</param>
        /// <param name="dwUser">user data from RealLoadPicture function 用户数据</param>
        /// <param name="nSequence">means status of the same uploaded image, when it is 0, it appears first time.When it is 2, it appears last time or appears once.When it is 1, it will appear again. 序列号</param>
        /// <param name="reserved">int nState = (int) reserved means current callback data status;when it is 1, it means current data is real time and current callback data is offline;when it is 2,it means offline data send structure 保留</param>
        /// <returns>reserved 保留</returns>
        private int AnalyzerDataCallBack(IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved)
        {
            switch (dwEventType)
            {
                case (uint)EM_EVENT_IVS_TYPE.ACCESS_CTL:     // Access control event 门禁事件
                    {
                        Console.WriteLine("\n<<-----Access Control Event (门禁事件)------>>");
                        NET_DEV_EVENT_ACCESS_CTL_INFO info = (NET_DEV_EVENT_ACCESS_CTL_INFO)Marshal.PtrToStructure(pEventInfo, typeof(NET_DEV_EVENT_ACCESS_CTL_INFO));

                        this.BeginInvoke(new Action(() =>
                        {
                            var list_item = new ListViewItem();

                            list_item.Text = info.szUserID;
                            list_item.SubItems.Add(info.szCardNo);

                            if (!info.UTC.dwYear.Equals(0) && !info.UTC.dwMonth.Equals(0) && !info.UTC.dwDay.Equals(0))
                            {
                                list_item.SubItems.Add(info.UTC.ToString());
                            }
                            else {
                                list_item.SubItems.Add(info.stuFileInfo.stuFileTime.ToString());   
                            }
                            

                            StringBuilder infoBuilder = new StringBuilder()
                                .Append("Channel:").Append(info.nChannelID).Append(",")
                                .Append("Method:");
                            switch (info.emOpenMethod)
                            {
                                case EM_ACCESS_DOOROPEN_METHOD.CARD:
                                    infoBuilder.Append("Card(卡),");
                                    break;
                                case EM_ACCESS_DOOROPEN_METHOD.FACE_RECOGNITION:
                                    infoBuilder.Append("Face recognition(人脸识别),");
                                    break;
                                case EM_ACCESS_DOOROPEN_METHOD.FINGERPRINT:
                                    infoBuilder.Append("Fingerprint(指纹),");
                                    break;
                                case EM_ACCESS_DOOROPEN_METHOD.REMOTE:
                                    infoBuilder.Append("Remote(远程),");
                                    break;
                                default:
                                    infoBuilder.Append("Unknown(未知),");
                                    break;
                            }
                            infoBuilder.Append("Status:");
                            if (info.bStatus)
                            {
                                infoBuilder.Append("True(成功)");
                            }
                            else
                            {
                                infoBuilder.Append("False(失败)");
                            }
                            list_item.SubItems.Add(infoBuilder.ToString());


                            listView_realLoadEvent.BeginUpdate();
                            listView_realLoadEvent.Items.Add(list_item);
                            if (listView_realLoadEvent.Items.Count > Line)
                            {
                                listView_realLoadEvent.Items.RemoveAt(Line);
                            }
                            listView_realLoadEvent.EndUpdate();
                        }));

                        if (IntPtr.Zero != pBuffer && dwBufSize > 0)
                        {
                            byte[] pic = new byte[dwBufSize];
                            Marshal.Copy(pBuffer, pic, 0, (int)dwBufSize);

                            this.BeginInvoke(new Action(() =>
                            {
                                using (MemoryStream stream = new MemoryStream(pic))
                                {
                                    try
                                    {
                                        Image image = Image.FromStream(stream);
                                        this.pictureBox_realLoadEvent.Image = image;
                                        this.pictureBox_realLoadEvent.Refresh();
                                        this.pictureBox_realLoadEvent.Visible = true;
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine(e);
                                    }
                                }
                                String picPath = savePath + info.nEventID + "_" + new Regex(" |:|/").Replace(info.UTC.ToString(), "-") + ".jpg";
                                if (!SaveFile(pic, picPath, (int)dwBufSize))
                                {
                                    Console.WriteLine("Save Event Picture failed (图片保存失败)");
                                }
                            }));
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Other realLoad event received:" + Enum.GetName(typeof(EM_EVENT_IVS_TYPE), dwEventType));
                    break;
            }

            return 1;
        }

        internal static bool SaveFile(byte[] bytes, string path, int length)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                fs.Write(bytes, 0, length);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                fs.Close();
            }
            return false;
        }

        /// <summary>
        /// 智能监听开始
        /// start realLoad listen
        /// </summary>
        private void button_startRealLoad_Click(object sender, EventArgs e)
        {
            if (realRoadID == IntPtr.Zero)
            {
                realRoadID = NETClient.RealLoadPicture(loginID, -1, (uint)EM_EVENT_IVS_TYPE.ALL, true, analyzerDataCallBack, IntPtr.Zero, IntPtr.Zero);
                if (realRoadID == IntPtr.Zero)
                {
                    Console.WriteLine(NETClient.GetLastError());
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                button_startRealLoad.Enabled = false;
                button_stopRealLoad.Enabled = true;
                tabControl_eventType.SelectTab(1);
                isRealLoad = true;
            }
        }

        /// <summary>
        /// 智能监听停止
        /// stop realLoad listen
        /// </summary>
        private void button_stopRealLoad_Click(object sender, EventArgs e)
        {
            if (realRoadID != IntPtr.Zero)
            {
                NETClient.StopLoadPic(realRoadID);
                realRoadID = IntPtr.Zero;

                button_startRealLoad.Enabled = true;
                button_stopRealLoad.Enabled = false;
                this.pictureBox_realLoadEvent.Image = null;
                this.pictureBox_realLoadEvent.Refresh();
                isRealLoad = false;
            }
        }

        #endregion
    }
}
