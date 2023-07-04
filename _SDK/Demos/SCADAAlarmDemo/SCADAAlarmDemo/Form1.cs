using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NetSDKCS;
using System.Collections.Generic;

namespace SCADAAlarmDemo
{
    public partial class SCADAAlarmDemo : Form
    {
        private static readonly string titleName = "Field Surveillance Unit(动环主机)";
        private IntPtr m_LoginID = IntPtr.Zero;
        private IntPtr m_AttachAlarm = IntPtr.Zero;
        private IntPtr m_AttachInfo = IntPtr.Zero;
        private bool m_IsListen = false;

        private NET_DEVICEINFO_Ex m_DeviceInfo;
        private const int m_WaitTime = 5000;
        private const int n_MaxCount = 50;
        private static int attachAlarm_ID = 0;
        private static int attachInfo_ID = 0;
        private static int startlisten_ID = 0;


        private List<NET_SCADA_DEVICE_ID_INFO> deviceInfo_list = new List<NET_SCADA_DEVICE_ID_INFO>();

        private static fDisConnectCallBack m_DisConnectCallBack;//断线回调
        private static fHaveReConnectCallBack m_ReConnectCallBack;//重连回调
        private static fMessCallBackEx m_AlarmCallBack;
        private static fSCADAAlarmAttachInfoCallBack m_SCADAAlarmAttachInfoCallBack;
        private static fSCADAAttachInfoCallBack m_SCADAAttachInfoCallBack;

        public SCADAAlarmDemo()
        {
            InitializeComponent();
        }

        private void SCADAAlarmDemo_Load(object sender, EventArgs e)
        {
            Text = titleName;
            m_DisConnectCallBack = new fDisConnectCallBack(DisConnectCallBack);
            m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack);
            m_AlarmCallBack = new fMessCallBackEx(AlarmCallBackEx);
            m_SCADAAlarmAttachInfoCallBack = new fSCADAAlarmAttachInfoCallBack(SCADAAlarmAttachInfoCallBack);
            m_SCADAAttachInfoCallBack = new fSCADAAttachInfoCallBack(SCADAAttachInfoCallBack);
            try
            {
                //初始化
                NETClient.Init(m_DisConnectCallBack, IntPtr.Zero, null);
                //打开日志
                NET_LOG_SET_PRINT_INFO logInfo = new NET_LOG_SET_PRINT_INFO()
                {
                    dwSize = (uint)Marshal.SizeOf(typeof(NET_LOG_SET_PRINT_INFO))
                };
                NETClient.LogOpen(logInfo);

                //设置断线重连回调
                NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero);
                NETClient.SetDVRMessCallBack(m_AlarmCallBack, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Process.GetCurrentProcess().Kill();
            }
        }

        #region Update UI 更新UI

        private void UpdateDisConnectUI()
        {
            this.Text = titleName + " -- Offline(离线)";
        }

        private void UpdateReConnectUI()
        {
            this.Text = titleName + " -- Online(在线)";
        }


        private void InitOrLogoutUI()
        {
            this.Text = titleName;
            btn_Login.Text = "Login(登录)";
            InitOrCloseOtherUI();
        }

        private void LoginUI()
        {
            this.Text = titleName + " -- Online(在线)";
            btn_Login.Text = "Logout(登出)";
            OpenOtherUI();
        }

        private void InitOrCloseOtherUI()
        {
            //其他控件操作初始化
            btn_AlarmAttach.Text = "Attach(订阅)";
            btn_Attach.Text = "Attach(订阅)";
            btn_StartListen.Text = "Attach(订阅)";

            btn_GetDevice.Enabled = false;
            btn_GetPoint.Enabled = false;
            btn_AlarmAttach.Enabled = false;
            btn_Attach.Enabled = false;
            btn_StartListen.Enabled = false;

            device_listView.Items.Clear();
            point_listView.Items.Clear();
            attachalarm_listView.Items.Clear();
            attachinfo_listView.Items.Clear();
            startlisten_listView.Items.Clear();
        }

        private void OpenOtherUI()
        {
            //其他控件操作使能
            btn_GetDevice.Enabled = true;
            btn_GetPoint.Enabled = true;
            btn_AlarmAttach.Enabled = true;
            btn_Attach.Enabled = true;
            btn_StartListen.Enabled = true;
        }

        #endregion

        #region CallBack 回调

        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateDisConnectUI);
        }

        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            this.BeginInvoke((Action)UpdateReConnectUI);
        }

        private bool AlarmCallBackEx(int lCommand, IntPtr lLoginID, IntPtr pBuf, uint dwBufLen, IntPtr pchDVRIP, int nDVRPort, bool bAlarmAckFlag, int nEventID, IntPtr dwUser)
        {
            EM_ALARM_TYPE type = (EM_ALARM_TYPE)lCommand;
            switch (type)
            {
                case EM_ALARM_TYPE.ALARM_SCADA_DEV_ALARM:
                    NET_ALARM_SCADA_DEV_INFO info = (NET_ALARM_SCADA_DEV_INFO)Marshal.PtrToStructure(pBuf, typeof(NET_ALARM_SCADA_DEV_INFO));

                    startlisten_ID++;
                    var list_item = new ListViewItem();
                    list_item.Text = startlisten_ID.ToString();
                    list_item.SubItems.Add(info.nChannel.ToString());
                    list_item.SubItems.Add(info.stuTime.ToString());
                    list_item.SubItems.Add(info.szDevName);
                    list_item.SubItems.Add(info.szDesc);
                    list_item.SubItems.Add(info.szID);
                    list_item.SubItems.Add(info.szSensorID);
                    list_item.SubItems.Add(info.szDevID);
                    list_item.SubItems.Add(info.szPointName);

                    this.BeginInvoke(new Action(() =>
                    {
                        if (startlisten_listView.Items.Count >= 50)
                        {
                            startlisten_listView.Items.RemoveAt(0);
                        }
                        startlisten_listView.BeginUpdate();
                        startlisten_listView.Items.Add(list_item);
                        startlisten_listView.EndUpdate();
                    }));
                    break;
                default:
                    break;
            }

            return true;
        }

        private void SCADAAlarmAttachInfoCallBack(IntPtr lAttachHandle, IntPtr pInfo, int nBufLen, IntPtr dwUser)
        {
            try
            {
                NET_SCADA_NOTIFY_POINT_ALARM_INFO_LIST info = (NET_SCADA_NOTIFY_POINT_ALARM_INFO_LIST)Marshal.PtrToStructure(pInfo, typeof(NET_SCADA_NOTIFY_POINT_ALARM_INFO_LIST));
                attachAlarm_ID++;

                for (int i = 0; i < info.nList; i++)
                {
                    var list_item = new ListViewItem();
                    list_item.Text = attachAlarm_ID.ToString();
                    list_item.SubItems.Add(info.stuList[i].szDevID);
                    list_item.SubItems.Add(info.stuList[i].szPointID);
                    list_item.SubItems.Add(info.stuList[i].bAlarmFlag.ToString());
                    list_item.SubItems.Add(info.stuList[i].stuAlarmTime.ToString());
                    list_item.SubItems.Add(info.stuList[i].nAlarmLevel.ToString());
                    list_item.SubItems.Add(info.stuList[i].nSerialNo.ToString());
                    list_item.SubItems.Add(info.stuList[i].szAlarmDesc);

                    this.BeginInvoke(new Action(() =>
                    {
                        if (attachalarm_listView.Items.Count >= 50)
                        {
                            attachalarm_listView.Items.RemoveAt(0);
                        }
                        attachalarm_listView.BeginUpdate();
                        attachalarm_listView.Items.Add(list_item);
                        attachalarm_listView.EndUpdate();
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
           
        }

        private void SCADAAttachInfoCallBack(IntPtr lLoginID, IntPtr lAttachHandle, IntPtr pInfo, int nBufLen, IntPtr dwUser)
        {
            try
            {
                NET_SCADA_NOTIFY_POINT_INFO_LIST info = (NET_SCADA_NOTIFY_POINT_INFO_LIST)Marshal.PtrToStructure(pInfo, typeof(NET_SCADA_NOTIFY_POINT_INFO_LIST));
                attachInfo_ID++;

                for (int i = 0; i < info.nList; i++)
                {
                    var list_item = new ListViewItem();
                    list_item.Text = attachInfo_ID.ToString();
                    list_item.SubItems.Add(info.stuList[i].szDevName);
                    list_item.SubItems.Add(info.stuList[i].emPointType.ToString());
                    list_item.SubItems.Add(info.stuList[i].szPointName.ToString());
                    string str_value = "0";
                    if (info.stuList[i].emPointType == EM_NET_SCADA_POINT_TYPE.YC)
                    {
                        str_value = info.stuList[i].fValue.ToString();
                    }
                    if (info.stuList[i].emPointType == EM_NET_SCADA_POINT_TYPE.YX)
                    {
                        str_value = info.stuList[i].nValue.ToString();
                    }
                    list_item.SubItems.Add(str_value);
                    list_item.SubItems.Add(info.stuList[i].szFSUID);
                    list_item.SubItems.Add(info.stuList[i].szID);
                    list_item.SubItems.Add(info.stuList[i].szSensorID);

                    this.BeginInvoke(new Action(() =>
                    {
                        if (attachinfo_listView.Items.Count >= 50)
                        {
                            attachinfo_listView.Items.RemoveAt(0);
                        }
                        attachinfo_listView.BeginUpdate();
                        attachinfo_listView.Items.Add(list_item);
                        attachinfo_listView.EndUpdate();
                    }));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        #endregion

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
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
                        MessageBox.Show("Input port error");
                        return;
                    }
                    m_DeviceInfo = new NET_DEVICEINFO_Ex();

                    m_LoginID = NETClient.Login(ip_textBox.Text.Trim(), port, user_textBox.Text.Trim(), pwd_textBox.Text.Trim(), EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref m_DeviceInfo);
                    if (IntPtr.Zero == m_LoginID)
                    {
                        MessageBox.Show(this, NETClient.GetLastError());
                        return;
                    }
                    LoginUI();
                }
                else
                {
                    if (IntPtr.Zero != m_AttachAlarm)
                    {
                        NETClient.SCADAAlarmDetachInfo(m_AttachAlarm);
                        m_AttachAlarm = IntPtr.Zero;
                    }
                    if (IntPtr.Zero != m_AttachInfo)
                    {
                        NETClient.SCADADetachInfo(m_AttachInfo);
                        m_AttachInfo = IntPtr.Zero;
                    }
                    if (m_IsListen)
                    {
                        NETClient.StopListen(m_LoginID);
                        m_IsListen = false;
                    }

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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            
        }

        private void btn_GetDevice_Click(object sender, EventArgs e)
        {
            deviceInfo_list.Clear();
            device_listView.Items.Clear();

            NET_SCADA_DEVICE_LIST device_list = new NET_SCADA_DEVICE_LIST();
            device_list.dwSize = (uint)Marshal.SizeOf(typeof(NET_SCADA_DEVICE_LIST));
            device_list.nMax = 64;
            device_list.pstuDeviceIDInfo = IntPtr.Zero;
            device_list.pstuDeviceIDInfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SCADA_DEVICE_ID_INFO)) * device_list.nMax);

            NET_SCADA_DEVICE_ID_INFO[] array_deviceinfo = new NET_SCADA_DEVICE_ID_INFO[64];

            for (int i = 0; i < 64; i++)
            {
                array_deviceinfo[i] = new NET_SCADA_DEVICE_ID_INFO();
                Marshal.StructureToPtr(array_deviceinfo[i], IntPtr.Add(device_list.pstuDeviceIDInfo, i * Marshal.SizeOf(typeof(NET_SCADA_DEVICE_ID_INFO))), true);
            }

            object objInfo = device_list;
            bool ret = NETClient.QueryDevState(m_LoginID, EM_DEVICE_STATE.SCADA_DEVICE_LIST, ref objInfo, typeof(NET_SCADA_DEVICE_LIST), 10000);
            if (!ret)
            {
                MessageBox.Show(NETClient.GetLastError());
                return;
            }
            else
            {
                device_list = (NET_SCADA_DEVICE_LIST)objInfo;

                for (int i = 0; i < device_list.nRet; i++)
                {
                    var deviceInfo_item = (NET_SCADA_DEVICE_ID_INFO)Marshal.PtrToStructure(IntPtr.Add(device_list.pstuDeviceIDInfo, i * Marshal.SizeOf(typeof(NET_SCADA_DEVICE_ID_INFO))), typeof(NET_SCADA_DEVICE_ID_INFO));
                    deviceInfo_list.Add(deviceInfo_item);

                    var list_item = new ListViewItem();
                    list_item.Text = deviceInfo_item.szDeviceID;
                    list_item.SubItems.Add(deviceInfo_item.szDevName);

                    this.BeginInvoke(new Action(() =>
                    {
                        device_listView.BeginUpdate();
                        device_listView.Items.Add(list_item);
                        device_listView.EndUpdate();
                    }));
                }
            }
        }

        private void btn_GetPoint_Click(object sender, EventArgs e)
        {
            if (deviceInfo_list.Count == 0)
            {
                MessageBox.Show("Please get the device list first(请先获取设备列表)!");
            }
            point_listView.Items.Clear();
            for (int i = 0; i < deviceInfo_list.Count; i++)
            {
                try
                {
                    NET_IN_SCADA_GET_ATTRIBUTE_INFO stuAttributeInfoIn = new NET_IN_SCADA_GET_ATTRIBUTE_INFO();
                    stuAttributeInfoIn.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_SCADA_GET_ATTRIBUTE_INFO));
                    stuAttributeInfoIn.stuCondition = new NET_GET_CONDITION_INFO();
                    stuAttributeInfoIn.stuCondition.szDeviceID = deviceInfo_list[i].szDeviceID;

                    NET_OUT_SCADA_GET_ATTRIBUTE_INFO stuAttributeInfoOut = new NET_OUT_SCADA_GET_ATTRIBUTE_INFO();
                    stuAttributeInfoOut.dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_SCADA_GET_ATTRIBUTE_INFO));
                    stuAttributeInfoOut.nMaxAttributeInfoNum = 20;
                    stuAttributeInfoOut.pstuAttributeInfo = IntPtr.Zero;
                    stuAttributeInfoOut.pstuAttributeInfo = Marshal.AllocHGlobal((int)(Marshal.SizeOf(typeof(NET_ATTRIBUTE_INFO)) * stuAttributeInfoOut.nMaxAttributeInfoNum));

                    NET_ATTRIBUTE_INFO[] array_attributeInfo = new NET_ATTRIBUTE_INFO[stuAttributeInfoOut.nMaxAttributeInfoNum];
                    for (int index = 0; index < stuAttributeInfoOut.nMaxAttributeInfoNum; index++)
                    {
                        IntPtr pIndexBuf = IntPtr.Add(stuAttributeInfoOut.pstuAttributeInfo, index * Marshal.SizeOf(typeof(NET_ATTRIBUTE_INFO)));
                        Marshal.StructureToPtr(array_attributeInfo[index], pIndexBuf, true);
                    }

                    IntPtr pstInParam = IntPtr.Zero;
                    pstInParam = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_IN_SCADA_GET_ATTRIBUTE_INFO)));
                    Marshal.StructureToPtr(stuAttributeInfoIn, pstInParam, true);

                    IntPtr pstOutParam = IntPtr.Zero;
                    pstOutParam = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_OUT_SCADA_GET_ATTRIBUTE_INFO)));
                    Marshal.StructureToPtr(stuAttributeInfoOut, pstOutParam, true);

                    bool ret = NETClient.SCADAGetAttributeInfo(m_LoginID, pstInParam, pstOutParam, 5000);
                    if (!ret)
                    {
                        MessageBox.Show(NETClient.GetLastError());
                        return;
                    }
                    else
                    {
                        var attribute_info = (NET_OUT_SCADA_GET_ATTRIBUTE_INFO)Marshal.PtrToStructure(pstOutParam, typeof(NET_OUT_SCADA_GET_ATTRIBUTE_INFO));
                        if (attribute_info.nRetAttributeInfoNum > 0)
                        {
                            for (int j = 0; j < attribute_info.nRetAttributeInfoNum; j++)
                            {
                                var attributeInfo_item = (NET_ATTRIBUTE_INFO)Marshal.PtrToStructure(IntPtr.Add(attribute_info.pstuAttributeInfo, Marshal.SizeOf(typeof(NET_ATTRIBUTE_INFO)) * j), typeof(NET_ATTRIBUTE_INFO));

                                var list_item = new ListViewItem();
                                list_item.Text = deviceInfo_list[i].szDeviceID;
                                list_item.SubItems.Add(attributeInfo_item.szID);
                                list_item.SubItems.Add(attributeInfo_item.szSignalName);
                                list_item.SubItems.Add(attributeInfo_item.szUnit);
                                list_item.SubItems.Add(attributeInfo_item.emPointType.ToString());
                                list_item.SubItems.Add(attributeInfo_item.nDelay.ToString());
                                list_item.SubItems.Add(attributeInfo_item.szDescribe);

                                this.BeginInvoke(new Action(() =>
                                {
                                    point_listView.BeginUpdate();
                                    point_listView.Items.Add(list_item);
                                    point_listView.EndUpdate();
                                }));
                            }
                        }
                    }

                    Marshal.FreeHGlobal(pstInParam);
                    pstInParam = IntPtr.Zero;
                    Marshal.FreeHGlobal(pstOutParam);
                    pstOutParam = IntPtr.Zero;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void btn_AlarmAttach_Click(object sender, EventArgs e)
        {
            attachAlarm_ID = 0;
            attachalarm_listView.Items.Clear();
            if (IntPtr.Zero == m_AttachAlarm)
            {
                NET_IN_SCADA_ALARM_ATTACH_INFO inInfo = new NET_IN_SCADA_ALARM_ATTACH_INFO();
                inInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_SCADA_ALARM_ATTACH_INFO));
                inInfo.cbCallBack = m_SCADAAlarmAttachInfoCallBack;

                NET_OUT_SCADA_ALARM_ATTACH_INFO outInfo = new NET_OUT_SCADA_ALARM_ATTACH_INFO();
                outInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_SCADA_ALARM_ATTACH_INFO));

                m_AttachAlarm = NETClient.SCADAAlarmAttachInfo(m_LoginID, inInfo, outInfo, 3000);
                if (IntPtr.Zero == m_AttachAlarm)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                btn_AlarmAttach.Text = "Detach(取消)";
            }
            else
            {
                bool ret = NETClient.SCADAAlarmDetachInfo(m_AttachAlarm);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_AttachAlarm = IntPtr.Zero;
                btn_AlarmAttach.Text = "Attach(订阅)";
            }
           
        }

        private void btn_Attach_Click(object sender, EventArgs e)
        {
            attachInfo_ID = 0;
            attachinfo_listView.Items.Clear();

            if (IntPtr.Zero == m_AttachInfo)
            {
                NET_IN_SCADA_ATTACH_INFO inInfo = new NET_IN_SCADA_ATTACH_INFO();
                inInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_SCADA_ATTACH_INFO));
                inInfo.cbCallBack = m_SCADAAttachInfoCallBack;
                inInfo.emPointType = EM_NET_SCADA_POINT_TYPE.ALL;

                NET_OUT_SCADA_ATTACH_INFO outInfo = new NET_OUT_SCADA_ATTACH_INFO();
                outInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_SCADA_ATTACH_INFO));

                m_AttachInfo = NETClient.SCADAAttachInfo(m_LoginID, inInfo, outInfo, 3000);
                if (IntPtr.Zero == m_AttachInfo)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                btn_Attach.Text = "Detach(取消)";
            }
            else
            {
                bool ret = NETClient.SCADADetachInfo(m_AttachInfo);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_AttachInfo = IntPtr.Zero;
                btn_Attach.Text = "Attach(订阅)";
            }

        }

        private void btn_StartListen_Click(object sender, EventArgs e)
        {
            startlisten_ID = 0;
            startlisten_listView.Items.Clear();
            if (!m_IsListen)
            {
                bool ret = NETClient.StartListen(m_LoginID);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_IsListen = true;
                btn_StartListen.Text = "Detach(取消)";
            }
            else
            {
                bool ret = NETClient.StopListen(m_LoginID);
                if (!ret)
                {
                    MessageBox.Show(this, NETClient.GetLastError());
                    return;
                }
                m_IsListen = false;
                btn_StartListen.Text = "Attach(订阅)";
            }
        }
    }
}
