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

namespace LatticeScreenDemo
{
    public partial class LatticeScreenDemo : Form
    {
        private static fDisConnectCallBack m_DisConnectCallBack;
        private static fHaveReConnectCallBack m_ReConnectCallBack;
        private const int m_WaitTime = 5000;
        private const int ListViewCount = 100;

        private IntPtr m_LoginID = IntPtr.Zero;
        private NET_DEVICEINFO_Ex m_DeviceInfo;

        private List<ScreenShow> screenShowList = new List<ScreenShow>();
        private int maxScreenShowCount = 16;

        private List<BroadCast> broadCastList = new List<BroadCast>();
        private int maxBroadCaetCount = 16;

        public LatticeScreenDemo()
        {
            InitializeComponent();
            this.Load += new EventHandler(LatticeScreenDemo_Load);
        }

        private void LatticeScreenDemo_Load(object sender, EventArgs e)
        {
            m_DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
            m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
            try
            {
                NETClient.Init(m_DisConnectCallBack, IntPtr.Zero, null);
                NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero);
                InitOrLogoutUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
            }
        }

        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateDisConnectUI);
        }

        private void UpdateDisConnectUI()
        {
            this.Text = "LatticeScreenDemo(点阵屏Demo) --- Offline(离线)";
        }

        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateReConnectUI);
        }
        private void UpdateReConnectUI()
        {
            this.Text = "LatticeScreenDemo(点阵屏Demo) --- Online(在线)";
        }

        private void InitOrLogoutUI()
        {
            login_button.Text = "Login(登录)";
            m_LoginID = IntPtr.Zero;
            this.Text = "LatticeScreenDemo(点阵屏Demo)";
        }

        private void LoginUI()
        {
            this.Text = "LatticeScreenDemo(点阵屏Demo) --- Online(在线)";
            login_button.Text = "Logout(登出)";
            cmb_PassState.SelectedIndex = 0;
        }

        private void port_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero == m_LoginID)
            {
                ushort port = 0;
                try
                {
                    port = Convert.ToUInt16(port_textBox.Text.Trim());
                }
                catch
                {
                    MessageBox.Show("Input port error(输入端口错误)!");
                    return;
                }
                m_DeviceInfo = new NET_DEVICEINFO_Ex();
                m_LoginID = NETClient.LoginWithHighLevelSecurity(ip_textBox.Text.Trim(), port, name_textBox.Text.Trim(), pwd_textBox.Text.Trim(), EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref m_DeviceInfo);
                if (IntPtr.Zero == m_LoginID)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                LoginUI();
            }
            else
            {
                bool result = NETClient.Logout(m_LoginID);
                if (!result)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_LoginID = IntPtr.Zero;
                InitOrLogoutUI();
            }
        }
     
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            NETClient.Cleanup();
        }

        private void cmb_PassState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_PassState.SelectedIndex == 0)
            {
                txt_PlateNum.Enabled = true;
                txt_CarMaster.Enabled = true;
                cmb_UserType.Enabled = true;
                txt_RemainDay.Enabled = true;
                time_In.Enabled = true;
                time_Out.Enabled = true;
                txt_ParkTime.Enabled = true;
                txt_ParkCharge.Enabled = true;
                rb_NoPassage.Enabled = true;
                rb_LetThrough.Enabled = true;
                txt_SubUserType.Enabled = true;
                txt_Remarks.Enabled = true;

                txt_RemainSpace.Enabled = false;
            }

            if (cmb_PassState.SelectedIndex == 1)
            {
                txt_PlateNum.Enabled = false;
                txt_CarMaster.Enabled = false;
                cmb_UserType.Enabled = false;
                txt_RemainDay.Enabled = false;
                time_In.Enabled = false;
                time_Out.Enabled = false;
                txt_ParkTime.Enabled = false;
                txt_ParkCharge.Enabled = false;
                rb_NoPassage.Enabled = false;
                rb_LetThrough.Enabled = false;
                txt_SubUserType.Enabled = false;
                txt_Remarks.Enabled = false;

                txt_RemainSpace.Enabled = true;
            }
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            NET_CTRL_SET_PARK_INFO stSetParkInfo = new NET_CTRL_SET_PARK_INFO();
            stSetParkInfo.dwSize = (uint)Marshal.SizeOf(stSetParkInfo);

            try
            {
                int pass_state = cmb_PassState.SelectedIndex;
                stSetParkInfo.emCarStatus = (EM_CARPASS_STATUS)(pass_state + 1);
                stSetParkInfo.szPlateNumber = txt_PlateNum.Text.Trim();
                stSetParkInfo.szMasterofCar = txt_CarMaster.Text.Trim();
                if (cmb_UserType.Enabled)
                {
                    int user_type = cmb_UserType.SelectedIndex;
                    switch (user_type)
                    {
                        case 0:
                            stSetParkInfo.szUserType = "monthlyCardUser";
                            break;
                        case 1:
                            stSetParkInfo.szUserType = "yearlyCardUser";
                            break;
                        case 2:
                            stSetParkInfo.szUserType = "longTimeUser";
                            break;
                        case 3:
                            stSetParkInfo.szUserType = "casualUser";
                            break;
                        default:
                            break;
                    }
                }

                if (txt_RemainDay.Enabled && !string.IsNullOrEmpty(txt_RemainDay.Text.Trim()))
                {
                    stSetParkInfo.nRemainDay = uint.Parse(txt_RemainDay.Text.Trim());
                }
                if (time_In.Enabled)
                {
                    DateTime inTime = new DateTime(time_In.Value.Year, time_In.Value.Month, time_In.Value.Day, time_In.Value.Hour, time_In.Value.Minute, time_In.Value.Second);
                    stSetParkInfo.stuInTime = NET_TIME.FromDateTime(inTime);
                }
                if (time_Out.Enabled)
                {
                    DateTime outTime = new DateTime(time_Out.Value.Year, time_Out.Value.Month, time_Out.Value.Day, time_Out.Value.Hour, time_Out.Value.Minute, time_Out.Value.Second);
                    stSetParkInfo.stuOutTime = NET_TIME.FromDateTime(outTime);
                }
                if (txt_ParkTime.Enabled && !string.IsNullOrEmpty(txt_ParkTime.Text.Trim()))
                {
                    stSetParkInfo.nParkTime = uint.Parse(txt_ParkTime.Text.Trim());
                }
                stSetParkInfo.szParkCharge = txt_ParkCharge.Text.Trim();
                if (txt_RemainSpace.Enabled && !string.IsNullOrEmpty(txt_RemainSpace.Text.Trim()))
                {
                    stSetParkInfo.nRemainSpace = uint.Parse(txt_RemainSpace.Text.Trim());
                }
                if (rb_LetThrough.Enabled && rb_LetThrough.Checked)
                {
                    stSetParkInfo.nPassEnable = 1;
                }
                if (rb_NoPassage.Enabled && rb_NoPassage.Checked)
                {
                    stSetParkInfo.nPassEnable = 0;
                }
                stSetParkInfo.szSubUserType = txt_SubUserType.Text.Trim();
                stSetParkInfo.szRemarks = txt_Remarks.Text.Trim();
                stSetParkInfo.szCustom = txt_Custom.Text.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            


            IntPtr pInfo = Marshal.AllocHGlobal(Marshal.SizeOf(stSetParkInfo));
            Marshal.StructureToPtr(stSetParkInfo, pInfo, true);

            bool bRet = NETClient.ControlDevice(m_LoginID, EM_CtrlType.SET_PARK_INFO, pInfo, 5000);
            if (bRet)
            {
                Console.WriteLine("ControlDevice SET_PARK_INFO success!");
            }
            else
            {
                MessageBox.Show(NETClient.GetLastError());
                Console.WriteLine("ControlDevice SET_PARK_INFO fail!");
            }
        }

        private void listView_screenConfigInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   // Show data 展示详细数据
                ListView.SelectedIndexCollection indexes = this.listView_screenConfigInfo.SelectedIndices;
                if (indexes.Count == 1)
                {
                    int idx = indexes[0];
                    label_screenIdx.Text = (idx + 1).ToString();
                    richTextBox_screenText.Text = screenShowList[idx].Text;
                    textBox_ScreenNo.Text = screenShowList[idx].ScreenNo.ToString();
                    cmb_screenTextType.SelectedIndex = (int)screenShowList[idx].EmTextType;
                    cmb_screenTextColor.SelectedIndex = (int)screenShowList[idx].EmTextColor;
                    cmb_screenTextMode.SelectedIndex = (int)screenShowList[idx].EmTextRollMode;
                    cmb_screenSpeed.SelectedIndex = (int)screenShowList[idx].RollSpeed - 1;

                    button_ScreenModify.Enabled = true;
                    button_ScreenDelete.Enabled = true;
                }
                else
                {
                    label_screenIdx.Text = "---";
                    button_ScreenModify.Enabled = false;
                    button_ScreenDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView_broadConfigInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   // Show data 展示详细数据
                ListView.SelectedIndexCollection indexes = this.listView_broadConfigInfo.SelectedIndices;
                if (indexes.Count == 1)
                {
                    int idx = indexes[0];
                    label_broadIdx.Text = (idx + 1).ToString();
                    richTextBox_broadText.Text = broadCastList[idx].Text;
                    cmb_broadTextType.SelectedIndex = (int)broadCastList[idx].EmTextType;

                    button_BroadModify.Enabled = true;
                    button_BroadDelete.Enabled = true;
                }
                else
                {
                    label_broadIdx.Text = "---";
                    button_BroadModify.Enabled = false;
                    button_BroadDelete.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_ScreenAdd_Click(object sender, EventArgs e)
        {
            if (screenShowList.Count >= maxScreenShowCount)
            {
                MessageBox.Show(String.Format("最多只能有{0}组", maxScreenShowCount));
                return;
            }

            try
            {
                byte[] infoArray = Encoding.Default.GetBytes(richTextBox_screenText.Text);
                int infoCount = infoArray.Length < 256 ? infoArray.Length : 256;
                byte[] tempArray = new byte[256];
                for (int i = 0; i < infoCount; i++)
                {
                    tempArray[i] = infoArray[i];
                }
                string infoMessage = Encoding.Default.GetString(tempArray);

                ScreenShow newInfo = new ScreenShow()
                {
                    Text = infoMessage,
                    ScreenNo = UInt32.Parse(textBox_ScreenNo.Text.Trim()),
                    EmTextType = (EM_SCREEN_TEXT_TYPE)cmb_screenTextType.SelectedIndex,
                    EmTextColor = (EM_SCREEN_TEXT_COLOR)cmb_screenTextColor.SelectedIndex,
                    EmTextRollMode = (EM_SCREEN_TEXT_ROLL_MODE)cmb_screenTextMode.SelectedIndex,
                    RollSpeed = (uint)cmb_screenSpeed.SelectedIndex + 1
                };

                screenShowList.Add(newInfo);

                this.updateListViewScreen(screenShowList.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void button_ScreenModify_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = Int32.Parse(label_screenIdx.Text) - 1;

                if (idx >= 0 && idx < screenShowList.Count)
                {
                    byte[] infoArray = Encoding.Default.GetBytes(richTextBox_screenText.Text);
                    int infoCount = infoArray.Length < 256 ? infoArray.Length : 256;
                    byte[] tempArray = new byte[256];
                    for (int i = 0; i < infoCount; i++)
                    {
                        tempArray[i] = infoArray[i];
                    }
                    screenShowList[idx].Text = Encoding.Default.GetString(tempArray);
                    screenShowList[idx].ScreenNo = UInt32.Parse(textBox_ScreenNo.Text.Trim());
                    screenShowList[idx].EmTextType = (EM_SCREEN_TEXT_TYPE)cmb_screenTextType.SelectedIndex;
                    screenShowList[idx].EmTextColor = (EM_SCREEN_TEXT_COLOR)cmb_screenTextColor.SelectedIndex;
                    screenShowList[idx].EmTextRollMode = (EM_SCREEN_TEXT_ROLL_MODE)cmb_screenTextMode.SelectedIndex;
                    screenShowList[idx].RollSpeed = (uint)cmb_screenSpeed.SelectedIndex + 1;

                    this.updateListViewScreen(idx);

                    label_screenIdx.Text = "---";
                    button_ScreenModify.Enabled = false;
                    button_ScreenDelete.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a info first(请先选择一条信息).");
            }
            
        }

        private void button_ScreenDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = Int32.Parse(label_screenIdx.Text) - 1;
                if (idx >= 0 && idx < screenShowList.Count)
                {
                    screenShowList.RemoveAt(idx);
                    refreshScreenlistView();

                    label_screenIdx.Text = "---";
                    button_ScreenModify.Enabled = false;
                    button_ScreenDelete.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a info first(请先选择一条信息).");
            }
           
        }

        private void updateListViewScreen(int idx)
        {
            ListViewItem listViewItem = new ListViewItem();

            listViewItem.Text = (idx + 1).ToString();
            listViewItem.SubItems.Add(screenShowList[idx].ScreenNo.ToString());
            listViewItem.SubItems.Add(ParseScreenType(screenShowList[idx].EmTextType));
            listViewItem.SubItems.Add(ParseScreenRollMode(screenShowList[idx].EmTextRollMode));
            listViewItem.SubItems.Add(screenShowList[idx].RollSpeed.ToString());

            listView_screenConfigInfo.BeginUpdate();

            if (listView_screenConfigInfo.Items.Count <= idx)   // 这是新增的
                listView_screenConfigInfo.Items.Add(listViewItem);
            else
            {
                listView_screenConfigInfo.Items.RemoveAt(idx);  // 这是修改的
                listView_screenConfigInfo.Items.Insert(idx, listViewItem);
            }

            listView_screenConfigInfo.EndUpdate();
        }

        private string ParseScreenType(EM_SCREEN_TEXT_TYPE emType)
        {

            string szType = "";
            switch (emType)
            {
                case EM_SCREEN_TEXT_TYPE.ORDINARY:
                    szType = "普通";
                    break;
                case EM_SCREEN_TEXT_TYPE.LOCAL_TIME:
                    szType = "本地时间";
                    break;
                case EM_SCREEN_TEXT_TYPE.QR_CODE:
                    szType = "二维码";
                    break;
                default:
                    szType = "未知";
                    break;
            }
            return szType;
        }

        private string ParseScreenRollMode(EM_SCREEN_TEXT_ROLL_MODE emType)
        {

            string szType = "";
            switch (emType)
            {
                case EM_SCREEN_TEXT_ROLL_MODE.NO:
                    szType = "不滚动";
                    break;
                case EM_SCREEN_TEXT_ROLL_MODE.LEFT_RIGHT:
                    szType = "左右滚动";
                    break;
                case EM_SCREEN_TEXT_ROLL_MODE.UP_DOWN:
                    szType = "上下翻页滚动";
                    break;
                default:
                    szType = "未知";
                    break;
            }
            return szType;
        }

        private void refreshScreenlistView()
        {
            this.listView_screenConfigInfo.Items.Clear();

            for (int i = 0; i < screenShowList.Count; i++)
            {
                this.updateListViewScreen(i);
            }
        }

        private void button_BroadAdd_Click(object sender, EventArgs e)
        {
            if (broadCastList.Count >= maxBroadCaetCount)
            {
                MessageBox.Show(String.Format("最多只能有{0}组", maxBroadCaetCount));
                return;
            }

            byte[] infoArray = Encoding.Default.GetBytes(richTextBox_broadText.Text);
            int infoCount = infoArray.Length < 256 ? infoArray.Length : 256;
            byte[] tempArray = new byte[256];
            for (int i = 0; i < infoCount; i++)
            {
                tempArray[i] = infoArray[i];
            }
            string infoMessage = Encoding.Default.GetString(tempArray);

            BroadCast newInfo = new BroadCast()
            {
                Text = infoMessage,
                EmTextType = (EM_BROADCAST_TEXT_TYPE)cmb_broadTextType.SelectedIndex,
            };

            broadCastList.Add(newInfo);

            this.updateListViewBroad(broadCastList.Count - 1);
        }

        private void button_BroadModify_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = Int32.Parse(label_broadIdx.Text) - 1;

                if (idx >= 0 && idx < broadCastList.Count)
                {
                    byte[] infoArray = Encoding.Default.GetBytes(richTextBox_broadText.Text);
                    int infoCount = infoArray.Length < 256 ? infoArray.Length : 256;
                    byte[] tempArray = new byte[256];
                    for (int i = 0; i < infoCount; i++)
                    {
                        tempArray[i] = infoArray[i];
                    }
                    broadCastList[idx].Text = Encoding.Default.GetString(tempArray);
                    broadCastList[idx].EmTextType = (EM_BROADCAST_TEXT_TYPE)cmb_broadTextType.SelectedIndex;

                    this.updateListViewBroad(idx);

                    label_broadIdx.Text = "---";
                    button_BroadModify.Enabled = false;
                    button_BroadDelete.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a info first(请先选择一条信息).");
            }
            
        }

        private void button_BroadDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = Int32.Parse(label_broadIdx.Text) - 1;
                if (idx >= 0 && idx < broadCastList.Count)
                {
                    broadCastList.RemoveAt(idx);
                    refreshBroadlistView();

                    label_broadIdx.Text = "---";
                    button_BroadModify.Enabled = false;
                    button_BroadDelete.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select a info first(请先选择一条信息).");
            }
        }

        private void updateListViewBroad(int idx)
        {
            ListViewItem listViewItem = new ListViewItem();

            listViewItem.Text = (idx + 1).ToString();
            listViewItem.SubItems.Add(ParseBroadType(broadCastList[idx].EmTextType));

            listView_broadConfigInfo.BeginUpdate();

            if (listView_broadConfigInfo.Items.Count <= idx)   // 这是新增的
                listView_broadConfigInfo.Items.Add(listViewItem);
            else
            {
                listView_broadConfigInfo.Items.RemoveAt(idx);  // 这是修改的
                listView_broadConfigInfo.Items.Insert(idx, listViewItem);
            }

            listView_broadConfigInfo.EndUpdate();
        }

        private string ParseBroadType(EM_BROADCAST_TEXT_TYPE emType)
        {

            string szType = "";
            switch (emType)
            {
                case EM_BROADCAST_TEXT_TYPE.ORDINARY:
                    szType = "普通";
                    break;
                case EM_BROADCAST_TEXT_TYPE.PLATE_NUMBER:
                    szType = "车牌号";
                    break;
                case EM_BROADCAST_TEXT_TYPE.TIME:
                    szType = "时间";
                    break;
                case EM_BROADCAST_TEXT_TYPE.NUMBER_STRING:
                    szType = "数字字符串";
                    break;
                default:
                    szType = "未知";
                    break;
            }
            return szType;
        }

        private void refreshBroadlistView()
        {

            this.listView_broadConfigInfo.Items.Clear();

            for (int i = 0; i < broadCastList.Count; i++)
            {
                this.updateListViewBroad(i);
            }

        }

        private void btn_Issue_Click(object sender, EventArgs e)
        {
            EM_CtrlType emType = EM_CtrlType.SET_PARK_CONTROL_INFO;

            NET_IN_SET_PARK_CONTROL_INFO stuInParam = new NET_IN_SET_PARK_CONTROL_INFO();
            stuInParam.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_SET_PARK_CONTROL_INFO));

            stuInParam.nScreenShowInfoNum = screenShowList.Count;

            stuInParam.stuScreenShowInfo = new NET_SCREEN_SHOW_INFO[16];
            for (int i = 0; i < screenShowList.Count; i++)
            {
                stuInParam.stuScreenShowInfo[i].nScreenNo = screenShowList[i].ScreenNo;
                stuInParam.stuScreenShowInfo[i].szText = screenShowList[i].Text;
                stuInParam.stuScreenShowInfo[i].emTextType = screenShowList[i].EmTextType;
                stuInParam.stuScreenShowInfo[i].emTextColor = screenShowList[i].EmTextColor;
                stuInParam.stuScreenShowInfo[i].emTextRollMode = screenShowList[i].EmTextRollMode;
                stuInParam.stuScreenShowInfo[i].nRollSpeed = screenShowList[i].RollSpeed;
            }

            stuInParam.nBroadcastInfoNum = broadCastList.Count;

            stuInParam.stuBroadcastInfo = new NET_BROADCAST_INFO[16];
            for (int i = 0; i < broadCastList.Count; i++)
            {
                stuInParam.stuBroadcastInfo[i].szText = broadCastList[i].Text;
                stuInParam.stuBroadcastInfo[i].emTextType = broadCastList[i].EmTextType;
            }

            NET_OUT_SET_PARK_CONTROL_INFO stuOutParam = new NET_OUT_SET_PARK_CONTROL_INFO()
            {
                dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_SET_PARK_CONTROL_INFO))
            };

            IntPtr pInBuf = IntPtr.Zero;
            IntPtr pOutBuf = IntPtr.Zero;

            try
            {
                pInBuf = Marshal.AllocHGlobal(Marshal.SizeOf(stuInParam));
                Marshal.StructureToPtr(stuInParam, pInBuf, true);
                pOutBuf = Marshal.AllocHGlobal(Marshal.SizeOf(stuOutParam));
                Marshal.StructureToPtr(stuOutParam, pOutBuf, true);
                bool ret = NETClient.ControlDeviceEx(m_LoginID, emType, pInBuf, pOutBuf, 3000);
                if (!ret)
                {
                    MessageBox.Show("配置下发失败! " + NETClient.GetLastError());
                }
                else
                {
                    MessageBox.Show("下发成功");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(pInBuf);
                pInBuf = IntPtr.Zero;

                Marshal.FreeHGlobal(pOutBuf);
                pOutBuf = IntPtr.Zero;
            }
        }
    }

    class ScreenShow
    {
        /// <summary>
        /// 屏幕编号
        /// </summary>
        public uint ScreenNo { get; set; }
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 文本类型
        /// </summary>
        public EM_SCREEN_TEXT_TYPE EmTextType { get; set; }
        /// <summary>
        /// 文本颜色
        /// </summary>
        public EM_SCREEN_TEXT_COLOR EmTextColor { get; set; }
        /// <summary>
        /// 文本滚动模式
        /// </summary>
        public EM_SCREEN_TEXT_ROLL_MODE EmTextRollMode { get; set; }
        /// <summary>
        /// 文本滚动速度由慢到快分为1~5
        /// </summary>
        public uint RollSpeed { get; set; }
    }

    class BroadCast
    {
        /// <summary>
        /// 语音文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 文本类型
        /// </summary>
        public EM_BROADCAST_TEXT_TYPE EmTextType { get; set; }

    }
}
