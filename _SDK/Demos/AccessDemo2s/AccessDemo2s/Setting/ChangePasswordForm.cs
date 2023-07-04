﻿using NetSDKCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccessDemo2s
{
    public partial class ChangePasswordForm : Form
    {
        private IntPtr m_LoginID = IntPtr.Zero;

        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        public ChangePasswordForm(IntPtr loginid)
        {
            InitializeComponent();
            m_LoginID = loginid;
        }

        private void btn_Change_Click(object sender, EventArgs e)
        {
            #region Modify Password 修改密码
            if (txt_Name.Text == null || txt_Name.Text == "")
            {
                MessageBox.Show("Please input user name(请输入用户名)");
                return;
            }
            if (txt_OldPwd.Text == null || txt_OldPwd.Text == "")
            {
                MessageBox.Show("Please input old password(请输入旧用户名)");
                return;
            }
            if (txt_NewPwd.Text == null || txt_NewPwd.Text == "")
            {
                MessageBox.Show("Please input new password(请输入新密码)");
                return;
            }
            if (txt_Repeat.Text == null || txt_Repeat.Text == "")
            {
                MessageBox.Show("Please input check password(请输入确认密码)");
                return;
            }
            if (txt_Repeat.Text != txt_NewPwd.Text)
            {
                MessageBox.Show("Two passwords are different, please check again.(两个密码不一致，请确认)");
                return;
            }

            NET_USER_INFO_NEW userInfo = new NET_USER_INFO_NEW();
            userInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_INFO_NEW));
            userInfo.name = txt_Name.Text.Trim();
            userInfo.passWord = txt_OldPwd.Text.Trim();
            IntPtr inPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));


            NET_USER_INFO_NEW stuModifyInfo = new NET_USER_INFO_NEW();
            stuModifyInfo.dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_INFO_NEW));
            stuModifyInfo.passWord = txt_NewPwd.Text.Trim();
            IntPtr insubPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));

            try
            {

                Marshal.StructureToPtr(userInfo, inPtr, true);
                Marshal.StructureToPtr(stuModifyInfo, insubPtr, true);

                bool ret = NETClient.OperateUserInfoNew(m_LoginID, EM_OPERATE_USER_TYPE.MODIFY_PASSWORD, insubPtr, inPtr, 10000);
                if (!ret)
                {
                    MessageBox.Show(NETClient.GetLastError());
                    return;
                }
            }
            finally
            {
                Marshal.FreeHGlobal(inPtr);
                Marshal.FreeHGlobal(insubPtr);
            }
            MessageBox.Show("Modify password successfully.(修改密码成功)");

            #endregion
        }
    }
}
