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
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace DeviceInit
{
    public partial class DeviceInitDemo : Form
    {
        private fSearchDevicesCB m_SearchDevicesCB;

        private fSearchDevicesCBEx m_SearchDevicesCBEx;
        private int m_DeviceCount_ByIp = 0;
        private List<DEVICE_NET_INFO_EX> m_DeviceList_ByIp = new List<DEVICE_NET_INFO_EX>();
        private List<NET_DEVICE_NET_INFO_EX2> m_DeviceList = new List<NET_DEVICE_NET_INFO_EX2>();
        private int m_DeviceCount = 0;

        private List<string> m_LocalIPList = new List<string>();
        private List<IntPtr> m_SearchIDList = new List<IntPtr>();

        public DeviceInitDemo()
        {
            InitializeComponent();
            try
            {
                m_SearchDevicesCB = new fSearchDevicesCB(SearchDevicesCB);
                m_SearchDevicesCBEx = new fSearchDevicesCBEx(SearchDevicesCBEx);
                NETClient.Init(null, IntPtr.Zero, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            button_init.Enabled = true;
            m_SearchIDList.Clear();
            GetAllNetworkInterface();
            m_DeviceList.Clear();
            listView_device.Items.Clear();
            m_DeviceCount = 0;
            foreach (var item in m_LocalIPList)
            {
                NET_IN_STARTSERACH_DEVICE stuIn = new NET_IN_STARTSERACH_DEVICE();
                stuIn.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_STARTSERACH_DEVICE));
                stuIn.emSendType = EM_SEND_SEARCH_TYPE.MULTICAST_AND_BROADCAST;
                stuIn.cbSearchDevices = m_SearchDevicesCBEx;
                stuIn.szLocalIp = item;

                NET_OUT_STARTSERACH_DEVICE stuOut = new NET_OUT_STARTSERACH_DEVICE();
                stuOut.dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_STARTSERACH_DEVICE));

                IntPtr searchID = NETClient.StartSearchDevicesEx(ref stuIn, ref stuOut);
                if (searchID == IntPtr.Zero)
                {
                    continue;
                }
                else
                {
                    m_SearchIDList.Add(searchID);
                }
            }
        }

        private void SearchDevicesCB(IntPtr pDevNetInfo, IntPtr pUserData)
        {
            DEVICE_NET_INFO_EX info = (DEVICE_NET_INFO_EX)Marshal.PtrToStructure(pDevNetInfo, typeof(DEVICE_NET_INFO_EX));
            this.BeginInvoke(new Action<DEVICE_NET_INFO_EX>(UpdateSearchUI), info);
        }

        private void SearchDevicesCBEx(IntPtr lSearchHandle, IntPtr pDevNetInfo, IntPtr dwUser)
        {
            NET_DEVICE_NET_INFO_EX2 info = (NET_DEVICE_NET_INFO_EX2)Marshal.PtrToStructure(pDevNetInfo, typeof(NET_DEVICE_NET_INFO_EX2));
            this.BeginInvoke(new Action<NET_DEVICE_NET_INFO_EX2>(UpdateSearchUI), info);
        }

        private void UpdateSearchUI(DEVICE_NET_INFO_EX info)
        {
            int index = m_DeviceList_ByIp.FindIndex(p => p.szMac == info.szMac);
            if (-1 == index)
            {
                m_DeviceCount_ByIp++;
                m_DeviceList_ByIp.Add(info);
                var viewItem = new ListViewItem();
                viewItem.Text = m_DeviceCount_ByIp.ToString();
                if ((info.byInitStatus & 0x1) == 1)
                {
                    viewItem.BackColor = Color.Red;
                    viewItem.SubItems.Add("Uninitialized(未初始化)");
                }
                else
                {
                    viewItem.BackColor = Color.White;
                    viewItem.SubItems.Add("Initialized(已初始化)");
                }
                viewItem.SubItems.Add(info.iIPVersion.ToString());
                viewItem.SubItems.Add(info.szIP);
                viewItem.SubItems.Add(info.nPort.ToString());
                viewItem.SubItems.Add(info.szSubmask);
                viewItem.SubItems.Add(info.szGateway);
                viewItem.SubItems.Add(info.szMac);
                viewItem.SubItems.Add(info.szDeviceType);
                viewItem.SubItems.Add(info.szNewDetailType);
                viewItem.SubItems.Add(info.nHttpPort.ToString());
                listView_device.BeginUpdate();
                listView_device.Items.Add(viewItem);
                listView_device.EndUpdate();
            }
        }

        private void UpdateSearchUI(NET_DEVICE_NET_INFO_EX2 info)
        {
            int index = m_DeviceList.FindIndex(p => p.stuDevInfo.szMac == info.stuDevInfo.szMac);
            if (-1 == index)
            {
                m_DeviceCount++;
                m_DeviceList.Add(info);

                var viewItem = new ListViewItem();
                viewItem.Text = m_DeviceCount.ToString();
                if ((info.stuDevInfo.byInitStatus & 0x1) == 1)
                {
                    viewItem.BackColor = Color.Red;
                    viewItem.SubItems.Add("Uninitialized(未初始化)");
                }
                else
                {
                    viewItem.BackColor = Color.White;
                    viewItem.SubItems.Add("Initialized(已初始化)");
                }
                viewItem.SubItems.Add(info.stuDevInfo.iIPVersion.ToString());
                viewItem.SubItems.Add(info.stuDevInfo.szIP);
                viewItem.SubItems.Add(info.stuDevInfo.nPort.ToString());
                viewItem.SubItems.Add(info.stuDevInfo.szSubmask);
                viewItem.SubItems.Add(info.stuDevInfo.szGateway);
                viewItem.SubItems.Add(info.stuDevInfo.szMac);
                viewItem.SubItems.Add(info.stuDevInfo.szDeviceType);
                viewItem.SubItems.Add(info.stuDevInfo.szNewDetailType);
                viewItem.SubItems.Add(info.stuDevInfo.nHttpPort.ToString());
                listView_device.BeginUpdate();
                listView_device.Items.Add(viewItem);
                listView_device.EndUpdate();
            }
        }

        private void button_init_Click(object sender, EventArgs e)
        {
            if (listView_device.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select an Uninitialized device(请选择一个未初始化的设备)");
                return;
            }
            var selectInfo = m_DeviceList[listView_device.SelectedItems[0].Index];
            if ((selectInfo.stuDevInfo.byInitStatus & 0x1) != 1)
            {
                MessageBox.Show(this, "Please select an Uninitialized device(请选择一个未初始化的设备)");
                return;
            }
            InitDeviceDialog initDeviceDialog = new InitDeviceDialog(selectInfo);
            var ret = initDeviceDialog.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                Task task = new Task(() =>
                {
                    NET_IN_INIT_DEVICE_ACCOUNT inParam = new NET_IN_INIT_DEVICE_ACCOUNT();
                    inParam.dwSize = (uint)Marshal.SizeOf(typeof(NET_IN_INIT_DEVICE_ACCOUNT));
                    if (initDeviceDialog.IsEmail)
                    {
                        inParam.szMail = initDeviceDialog.RestStr;
                    }
                    else
                    {
                        inParam.szCellPhone = initDeviceDialog.RestStr;
                    }
                    inParam.szMac = selectInfo.stuDevInfo.szMac;
                    inParam.szUserName = initDeviceDialog.UserName;
                    if (initDeviceDialog.Passwrod.Length > 127)
                    {
                        string password = initDeviceDialog.Passwrod.Substring(0, 127);
                        inParam.szPwd = password;
                    }
                    else
                    {
                        inParam.szPwd = initDeviceDialog.Passwrod;
                    }
                    inParam.byPwdResetWay = selectInfo.stuDevInfo.byPwdResetWay;
                    NET_OUT_INIT_DEVICE_ACCOUNT outParam = new NET_OUT_INIT_DEVICE_ACCOUNT();
                    outParam.dwSize = (uint)Marshal.SizeOf(typeof(NET_OUT_INIT_DEVICE_ACCOUNT));
                    bool res = NETClient.InitDevAccount(inParam, ref outParam, 5000, selectInfo.szLocalIp);
                    if (!res)
                    {
                        string errormsg = NETClient.GetLastError();
                        this.BeginInvoke(new Action(() =>
                        {
                            MessageBox.Show(this, errormsg);
                        }));
                    }
                    else
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            listView_device.SelectedItems[0].BackColor = Color.White;
                            listView_device.SelectedItems[0].SubItems[1].Text = "Initialized(已初始化)";
                        }));

                        selectInfo.stuDevInfo.byInitStatus = 2;
                    }
                });
                task.Start();
            }
            initDeviceDialog.Dispose();
        }

        private void button_searchbypoint_Click(object sender, EventArgs e)
        {
            GetAllNetworkInterface();
            button_init.Enabled = false;
            StopAllSearchDevice();
            PointSetDialog setIPDialog = new PointSetDialog();
            var ret = setIPDialog.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                m_DeviceList_ByIp.Clear();
                listView_device.Items.Clear();
                m_DeviceCount_ByIp = 0;

                NET_DEVICE_IP_SEARCH_INFO info = new NET_DEVICE_IP_SEARCH_INFO();
                info.dwSize = (uint)Marshal.SizeOf(typeof(NET_DEVICE_IP_SEARCH_INFO));
                info.nIpNum = setIPDialog.IPCount;
                info.szIPs = new NET_IPADDRESS[256];
                for (int i = 0; i < setIPDialog.IPCount; i++)
                {
                    info.szIPs[i].szIP = setIPDialog.IPList[i];
                }
                Task task = new Task(() =>
                {
                    foreach (var item in m_LocalIPList)
                    {
                        bool res = NETClient.SearchDevicesByIPs(info, m_SearchDevicesCB, IntPtr.Zero, item, 10000);
                        if (!res)
                        {
                            continue;
                        }
                    }
                });
                task.Start();
            }
            setIPDialog.Dispose();
        }

        private void StopAllSearchDevice()
        {
            try
            {
                for (int i = 0; i < m_SearchIDList.Count; i++)
                {
                    if (IntPtr.Zero != m_SearchIDList[i])
                    {
                        NETClient.StopSearchDevice(m_SearchIDList[i]);
                        m_SearchIDList[i] = IntPtr.Zero;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            
        }

        private void GetAllNetworkInterface()
        {
            m_LocalIPList.Clear();
            string temp_ip = "";
            //获取所有网卡信息
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                //Wireless80211         无线网卡    
                //Ppp                   宽带连接
                //Ethernet              以太网卡
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)//判断是否为以太网卡
                {
                    //获取以太网卡网络接口信息
                    IPInterfaceProperties ip = adapter.GetIPProperties();
                    //获取单播地址集
                    UnicastIPAddressInformationCollection ipCollection = ip.UnicastAddresses;
                    foreach (UnicastIPAddressInformation ipadd in ipCollection)
                    {
                        //InterNetwork      IPV4地址      
                        //InterNetworkV6    IPV6地址
                        //Max               MAX 位址
                        if (ipadd.Address.AddressFamily == AddressFamily.InterNetwork)//判断是否为ipv4
                        {
                            temp_ip = ipadd.Address.ToString();//获取ip
                            if (!m_LocalIPList.Contains(temp_ip))
                            {
                                m_LocalIPList.Add(temp_ip);
                            }
                        }

                    }
                }
            }
        }
    }
}
