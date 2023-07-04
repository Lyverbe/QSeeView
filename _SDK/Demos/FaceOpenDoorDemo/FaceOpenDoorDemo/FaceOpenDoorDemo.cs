using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using NetSDKCS;
using System.Runtime.InteropServices;
using System.IO;
using System.Globalization;
using System.Threading;

namespace FaceOpenDoorDemo
{
    public partial class FaceOpenDoor : Form
    {
        IntPtr _LoginID = IntPtr.Zero;
        NET_DEVICEINFO_Ex _DeviceInfo = new NET_DEVICEINFO_Ex();
        IntPtr _PlayID = IntPtr.Zero;
        IntPtr _AttachID = IntPtr.Zero;
        fAnalyzerDataCallBack _AnalyzerDataCallBack;
        TextInfo _TextInfo = Thread.CurrentThread.CurrentCulture.TextInfo;
        fDisConnectCallBack _DisConnectCallBack;
        fHaveReConnectCallBack _ReConnectCallBack;

        public FaceOpenDoor()
        {
            InitializeComponent();
            try
            {
                _DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
                _ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
                NETClient.Init(_DisConnectCallBack, IntPtr.Zero, null);
                NETClient.SetAutoReconnect(_ReConnectCallBack, IntPtr.Zero);
                _AnalyzerDataCallBack = new fAnalyzerDataCallBack(AnalyzerDataCallBack);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
                return;
            }
            this.Load += new EventHandler(FaceOpenDoor_Load);
        }

        void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.Text = "FaceOpenDoorDemo(人脸开门Demo)---offline(离线)";
            }));
        }

        void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.Text = "FaceOpenDoorDemo(人脸开门Demo)---online(在线)";
            }));
        }

        void FaceOpenDoor_Load(object sender, EventArgs e)
        {
            this.comboBox_channel.Enabled = false;
            this.button_play.Enabled = false;
            this.button_attach.Enabled = false;
            this.button_operate.Enabled = false;
        }

        private void button_login_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == _LoginID)
            {
                ushort port = 0;
                try
                {
                    port = Convert.ToUInt16(this.textBox_port.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("The port value must be 1 -65535(端口值必须是1-65535)");
                    return;
                }
                _LoginID = NETClient.LoginWithHighLevelSecurity(this.textBox_ip.Text.Trim(), port, this.textBox_name.Text.Trim(), this.textBox_password.Text, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref _DeviceInfo);
                if (IntPtr.Zero == _LoginID)
                {
                    MessageBox.Show(NETClient.GetLastError());
                    return;
                }
                for (int i = 0; i < _DeviceInfo.nChanNum; i++)
                {
                    this.comboBox_channel.Items.Add(i + 1);
                }
                //某些型号的设备，比如“BSC”门禁主机，是没有通道的，也不支持监视和订阅
                if (this.comboBox_channel.Items.Count > 0)
                {
                    this.comboBox_channel.SelectedIndex = 0;
                    this.comboBox_channel.Enabled = true;
                    this.button_attach.Enabled = true;
                    this.button_play.Enabled = true;
                }
                this.button_operate.Enabled = true;
                this.button_login.Text = "Logout(登出)";
                this.Text = "FaceOpenDoorDemo(人脸开门Demo)---online(在线)";
            }
            else
            {
                NETClient.Logout(_LoginID);
                _LoginID = IntPtr.Zero;
                this.button_login.Text = "Login(登录)";
                this.comboBox_channel.Items.Clear();
                this.comboBox_channel.Enabled = false;
                this.button_attach.Enabled = false;
                this.button_attach.Text = "Attach(订阅)";
                _AttachID = IntPtr.Zero;
                this.button_play.Enabled = false;
                this.button_play.Text = "RealPlay(监视)";
                _PlayID = IntPtr.Zero;
                this.button_operate.Enabled = false;
                this.pictureBox_play.Refresh();
                if (this.pictureBox_image.Image != null)
                {
                    this.pictureBox_image.Image.Dispose();
                    this.pictureBox_image.Image = null;
                }
                this.pictureBox_image.Refresh();
                this.label_time.Text = "";
                this.label_openmethod.Text = "";
                this.label_cardname.Text = "";
                this.label_cardno.Text = "";
                this.label_userid.Text = "";
                this.label_openstatus.Text = "";
                this.label_Over.Text = "";
                this.label_Temp.Text = "";
                this.label_Temp_Unit.Text = "";
                this.label_mask.Text = "";
                this.Text = "FaceOpenDoorDemo(人脸开门Demo)";
            }
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == _PlayID)
            {
                _PlayID = NETClient.RealPlay(_LoginID, comboBox_channel.SelectedIndex, this.pictureBox_play.Handle);
                if (IntPtr.Zero == _PlayID)
                {
                    MessageBox.Show(NETClient.GetLastError());
                    return;
                }
                bool ret = NETClient.RenderPrivateData(_PlayID, true);
                if (!ret)
                {
                    MessageBox.Show(NETClient.GetLastError());
                    return;
                }
                this.button_play.Text = "StopPlay(停止监视)";
            }
            else
            {
                NETClient.RenderPrivateData(_PlayID, false);
                NETClient.StopRealPlay(_PlayID);
                _PlayID = IntPtr.Zero;
                this.button_play.Text = "RealPlay(监视)";
                this.pictureBox_play.Refresh();
            }
        }

        private void button_attach_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == _AttachID)
            {
                _AttachID = NETClient.RealLoadPicture(_LoginID, comboBox_channel.SelectedIndex, (uint)EM_EVENT_IVS_TYPE.ALL, true, _AnalyzerDataCallBack, IntPtr.Zero, IntPtr.Zero);
                if (IntPtr.Zero == _AttachID)
                {
                    MessageBox.Show(NETClient.GetLastError());
                    return;
                }
                this.button_attach.Text = "Detach(取消订阅)";
            }
            else
            {
                NETClient.StopLoadPic(_AttachID);
                _AttachID = IntPtr.Zero;
                this.button_attach.Text = "Attach(订阅)";
                if (this.pictureBox_image.Image != null)
                {
                    this.pictureBox_image.Image.Dispose();
                    this.pictureBox_image.Image = null;
                }
                this.pictureBox_image.Refresh();
                this.label_time.Text = "";
                this.label_openmethod.Text = "";
                this.label_cardname.Text = "";
                this.label_cardno.Text = "";
                this.label_userid.Text = "";
                this.label_openstatus.Text = "";
                this.label_Over.Text = "";
                this.label_Temp.Text = "";
                this.label_Temp_Unit.Text = "";
                this.label_mask.Text = "";
            }
        }

        private int AnalyzerDataCallBack(IntPtr lAnalyzerHandle, uint dwEventType, IntPtr pEventInfo, IntPtr pBuffer, uint dwBufSize, IntPtr dwUser, int nSequence, IntPtr reserved)
        {
            EM_EVENT_IVS_TYPE type = (EM_EVENT_IVS_TYPE)dwEventType;
            switch (type)
            {
                case EM_EVENT_IVS_TYPE.ACCESS_CTL:       // Event Type -> Access Control
                    // get event info from pointer pEventInfo
                    NET_DEV_EVENT_ACCESS_CTL_INFO info = (NET_DEV_EVENT_ACCESS_CTL_INFO)Marshal.PtrToStructure(pEventInfo, typeof(NET_DEV_EVENT_ACCESS_CTL_INFO));
                    // get event snap image from pointer pBuffer with length of dwBufSize
                    byte[] personFaceInfo = new byte[dwBufSize];
                    Marshal.Copy(pBuffer, personFaceInfo, 0, (int)dwBufSize);
                    // display the snap image
                    using (MemoryStream stream = new MemoryStream(personFaceInfo))
                    {
                        try // add try catch for catch exception when the stream is not image format,and the stream is from device.
                        {
                            Image faceImage = Image.FromStream(stream);
                            this.pictureBox_image.Image = faceImage;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    // display the event info
                    this.BeginInvoke(new Action(() =>
                    {

                        DateTime UTC = info.UTC.ToDateTime();      // Coordinated Universal Time
                        // DateTime CST = UTC.AddHours(8);
                        // this.label_time.Text = CST.ToString();  // China Standard Time UT+8:00
                        this.label_time.Text = UTC.ToString();
                        switch (info.emOpenMethod)
                        {
                            case EM_ACCESS_DOOROPEN_METHOD.CARD:
                                this.label_openmethod.Text = "Card(卡)";
                                break;
                            case EM_ACCESS_DOOROPEN_METHOD.FACE_RECOGNITION:
                                this.label_openmethod.Text = "Face recognition(人脸识别)";
                                break;
                            default:
                                this.label_openmethod.Text = "Unknown(未知)";
                                break;
                        }
                        if (info.bStatus)
                        {
                            this.label_openstatus.Text = "Success(成功)";
                        }
                        else
                        {
                            this.label_openstatus.Text = "Failure(失败)";
                        }
                        this.label_cardname.Text = info.szCardName;
                        this.label_cardno.Text = info.szCardNo;
                        this.label_userid.Text = info.szUserID;

                        if (info.bManTemperature)
                        {
                            this.label_Over.Text = info.stuManTemperatureInfo.bIsOverTemperature.ToString();
                            this.label_Temp.Text = info.stuManTemperatureInfo.fCurrentTemperature.ToString();
                            this.label_Temp_Unit.Text = PraseTempUnit(info.stuManTemperatureInfo.emTemperatureUnit);
                        }
                        else
                        {
                            this.label_Over.Text = "";
                            this.label_Temp.Text = "";
                            this.label_Temp_Unit.Text = "";
                        }
                        this.label_mask.Text = PraseMask(info.emMask);
                    }));
                    break;
                default:
                    break;
            }
            return 0;
        }

        // get mask type
        private string PraseMask(EM_MASK_STATE_TYPE emMask)
        {
            string res = "";
            switch (emMask)
            {
                case EM_MASK_STATE_TYPE.UNKNOWN:
                    res = "";
                    break;
                case EM_MASK_STATE_TYPE.NODISTI:
                    res = "Unrecognized(未识别)";
                    break;
                case EM_MASK_STATE_TYPE.NOMASK:
                    res = "No Mask(没戴口罩)";
                    break;
                case EM_MASK_STATE_TYPE.WEAR:
                    res = "Wear(戴口罩)";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        // get temp unit type
        private string PraseTempUnit(EM_HUMAN_TEMPERATURE_UNIT emTempUnit)
        {

            string res = "";
            switch (emTempUnit)
            {
                case EM_HUMAN_TEMPERATURE_UNIT.UNKNOWN:
                    res = "Unknown(未知)";
                    break;
                case EM_HUMAN_TEMPERATURE_UNIT.CENTIGRADE:
                    res = "Centigrade(摄氏度)";
                    break;
                case EM_HUMAN_TEMPERATURE_UNIT.FAHRENHEIT:
                    res = "Fahrenheit(华氏度)";
                    break;
                case EM_HUMAN_TEMPERATURE_UNIT.KELVIN:
                    res = "Kelvin(开尔文)";
                    break;
                default:
                    res = "Unknown(未知)";
                    break;
            }
            return res;
        }

        private void button_operate_Click(object sender, EventArgs e)
        {
            CardManager cardManager = new CardManager(_LoginID);
            cardManager.ShowDialog();
            cardManager.Dispose();
        }

        private void textBox_port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            NETClient.Cleanup();
            base.OnClosed(e);
        }

    }
}
