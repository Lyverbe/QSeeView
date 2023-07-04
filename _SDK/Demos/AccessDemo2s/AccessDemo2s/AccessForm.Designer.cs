﻿namespace AccessDemo2s
{
    partial class AccessForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.pwd_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.user_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.port_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ip_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_SystemConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DeviceInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Net = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DeviceTime = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_ChangePwd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Reboot = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_ConfigReset = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Upgrade = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_AutoMatrix = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_AdvanceConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_OpenDoorGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_FirstEnter = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_MultidoorInterlock = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_RepeatEnter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_DeviceList = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_GetState = new System.Windows.Forms.Button();
            this.btn_CloseAlways = new System.Windows.Forms.Button();
            this.btn_OpenAlways = new System.Windows.Forms.Button();
            this.btn_CloseDoor = new System.Windows.Forms.Button();
            this.channel_comboBox = new System.Windows.Forms.ComboBox();
            this.btn_OpenDoor = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btn_AccessPassword = new System.Windows.Forms.Button();
            this.btn_UserOperate = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btn_GeneralConfig = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_OpenEvent = new System.Windows.Forms.Button();
            this.listView_event = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Query = new System.Windows.Forms.Button();
            this.btn_StartListen = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.btn_Login);
            this.groupBox1.Controls.Add(this.pwd_textBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.user_textBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.port_textBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ip_textBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 33);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1097, 77);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Login(设备登录)";
            // 
            // btn_Login
            // 
            this.btn_Login.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Login.Location = new System.Drawing.Point(937, 33);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(140, 24);
            this.btn_Login.TabIndex = 12;
            this.btn_Login.Text = "Login(登录)";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // pwd_textBox
            // 
            this.pwd_textBox.Location = new System.Drawing.Point(796, 35);
            this.pwd_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.pwd_textBox.Name = "pwd_textBox";
            this.pwd_textBox.Size = new System.Drawing.Size(120, 23);
            this.pwd_textBox.TabIndex = 7;
            this.pwd_textBox.Text = "admin123";
            this.pwd_textBox.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(676, 38);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password(密码):";
            // 
            // user_textBox
            // 
            this.user_textBox.Location = new System.Drawing.Point(537, 35);
            this.user_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.user_textBox.Name = "user_textBox";
            this.user_textBox.Size = new System.Drawing.Size(120, 23);
            this.user_textBox.TabIndex = 5;
            this.user_textBox.Text = "admin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(431, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name(用户名):";
            // 
            // port_textBox
            // 
            this.port_textBox.Location = new System.Drawing.Point(328, 35);
            this.port_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.port_textBox.Name = "port_textBox";
            this.port_textBox.Size = new System.Drawing.Size(80, 23);
            this.port_textBox.TabIndex = 3;
            this.port_textBox.Text = "37777";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port(端口):";
            // 
            // ip_textBox
            // 
            this.ip_textBox.Location = new System.Drawing.Point(100, 35);
            this.ip_textBox.Margin = new System.Windows.Forms.Padding(4);
            this.ip_textBox.Name = "ip_textBox";
            this.ip_textBox.Size = new System.Drawing.Size(120, 23);
            this.ip_textBox.TabIndex = 1;
            this.ip_textBox.Text = "172.23.26.33";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP(设备IP):";
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_SystemConfig,
            this.menu_AdvanceConfig,
            this.toolStripMenuItem1,
            this.menu_DeviceList});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1134, 28);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu_SystemConfig
            // 
            this.menu_SystemConfig.AutoSize = false;
            this.menu_SystemConfig.BackColor = System.Drawing.Color.Transparent;
            this.menu_SystemConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_DeviceInfo,
            this.menu_Net,
            this.menu_DeviceTime,
            this.menu_ChangePwd,
            this.toolStripMenuItem3,
            this.menu_Reboot,
            this.menu_ConfigReset,
            this.menu_Upgrade,
            this.menu_AutoMatrix});
            this.menu_SystemConfig.Enabled = false;
            this.menu_SystemConfig.Name = "menu_SystemConfig";
            this.menu_SystemConfig.Size = new System.Drawing.Size(245, 20);
            this.menu_SystemConfig.Text = "System Config(系统配置)";
            this.menu_SystemConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menu_DeviceInfo
            // 
            this.menu_DeviceInfo.Name = "menu_DeviceInfo";
            this.menu_DeviceInfo.Size = new System.Drawing.Size(242, 22);
            this.menu_DeviceInfo.Text = "Device Info(设备信息)";
            this.menu_DeviceInfo.Click += new System.EventHandler(this.menu_DeviceInfo_Click);
            // 
            // menu_Net
            // 
            this.menu_Net.Name = "menu_Net";
            this.menu_Net.Size = new System.Drawing.Size(242, 22);
            this.menu_Net.Text = "Net(网络)";
            this.menu_Net.Click += new System.EventHandler(this.menu_Net_Click);
            // 
            // menu_DeviceTime
            // 
            this.menu_DeviceTime.Name = "menu_DeviceTime";
            this.menu_DeviceTime.Size = new System.Drawing.Size(242, 22);
            this.menu_DeviceTime.Text = "Device Time(设备时间)";
            this.menu_DeviceTime.Click += new System.EventHandler(this.menu_DeviceTime_Click);
            // 
            // menu_ChangePwd
            // 
            this.menu_ChangePwd.Name = "menu_ChangePwd";
            this.menu_ChangePwd.Size = new System.Drawing.Size(242, 22);
            this.menu_ChangePwd.Text = "Change Pwd(修改密码)";
            this.menu_ChangePwd.Click += new System.EventHandler(this.menu_ChangePwd_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(242, 22);
            this.toolStripMenuItem3.Text = "————";
            // 
            // menu_Reboot
            // 
            this.menu_Reboot.Name = "menu_Reboot";
            this.menu_Reboot.Size = new System.Drawing.Size(242, 22);
            this.menu_Reboot.Text = "Reboot(重启)";
            this.menu_Reboot.Click += new System.EventHandler(this.menu_Reboot_Click);
            // 
            // menu_ConfigReset
            // 
            this.menu_ConfigReset.Name = "menu_ConfigReset";
            this.menu_ConfigReset.Size = new System.Drawing.Size(242, 22);
            this.menu_ConfigReset.Text = "Config Reset(配置重置)";
            this.menu_ConfigReset.Click += new System.EventHandler(this.menu_ConfigReset_Click);
            // 
            // menu_Upgrade
            // 
            this.menu_Upgrade.Name = "menu_Upgrade";
            this.menu_Upgrade.Size = new System.Drawing.Size(242, 22);
            this.menu_Upgrade.Text = "Device Upgrade(设备升级)";
            this.menu_Upgrade.Click += new System.EventHandler(this.menu_Upgrade_Click);
            // 
            // menu_AutoMatrix
            // 
            this.menu_AutoMatrix.Name = "menu_AutoMatrix";
            this.menu_AutoMatrix.Size = new System.Drawing.Size(242, 22);
            this.menu_AutoMatrix.Text = "Auto Matrix(自动维护)";
            this.menu_AutoMatrix.Click += new System.EventHandler(this.menu_AutoMatrix_Click);
            // 
            // menu_AdvanceConfig
            // 
            this.menu_AdvanceConfig.AutoSize = false;
            this.menu_AdvanceConfig.BackColor = System.Drawing.Color.Transparent;
            this.menu_AdvanceConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_OpenDoorGroup,
            this.menu_FirstEnter,
            this.menu_MultidoorInterlock,
            this.menu_RepeatEnter});
            this.menu_AdvanceConfig.Enabled = false;
            this.menu_AdvanceConfig.Name = "menu_AdvanceConfig";
            this.menu_AdvanceConfig.Size = new System.Drawing.Size(245, 20);
            this.menu_AdvanceConfig.Text = "Advanced Config(高级配置)";
            this.menu_AdvanceConfig.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // menu_OpenDoorGroup
            // 
            this.menu_OpenDoorGroup.Name = "menu_OpenDoorGroup";
            this.menu_OpenDoorGroup.Size = new System.Drawing.Size(284, 22);
            this.menu_OpenDoorGroup.Text = "Open Door Group(多人组合开门)";
            this.menu_OpenDoorGroup.Click += new System.EventHandler(this.menu_OpenDoorGroup_Click);
            // 
            // menu_FirstEnter
            // 
            this.menu_FirstEnter.Name = "menu_FirstEnter";
            this.menu_FirstEnter.Size = new System.Drawing.Size(284, 22);
            this.menu_FirstEnter.Text = "First Enter(首卡)";
            this.menu_FirstEnter.Click += new System.EventHandler(this.menu_FirstEnter_Click);
            // 
            // menu_MultidoorInterlock
            // 
            this.menu_MultidoorInterlock.Name = "menu_MultidoorInterlock";
            this.menu_MultidoorInterlock.Size = new System.Drawing.Size(284, 22);
            this.menu_MultidoorInterlock.Text = "Multi-door Interlock(多门互锁)";
            this.menu_MultidoorInterlock.Click += new System.EventHandler(this.menu_MultidoorInterlock_Click);
            // 
            // menu_RepeatEnter
            // 
            this.menu_RepeatEnter.Name = "menu_RepeatEnter";
            this.menu_RepeatEnter.Size = new System.Drawing.Size(284, 22);
            this.menu_RepeatEnter.Text = "Repeat Enter(防反潜)";
            this.menu_RepeatEnter.Click += new System.EventHandler(this.menu_RepeatEnter_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(26, 24);
            this.toolStripMenuItem1.Text = "|";
            // 
            // menu_DeviceList
            // 
            this.menu_DeviceList.AutoSize = false;
            this.menu_DeviceList.Name = "menu_DeviceList";
            this.menu_DeviceList.Size = new System.Drawing.Size(245, 20);
            this.menu_DeviceList.Text = "Match Device List(匹配设备列表)";
            this.menu_DeviceList.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.menu_DeviceList.Click += new System.EventHandler(this.menu_DeviceList_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_GetState);
            this.groupBox2.Controls.Add(this.btn_CloseAlways);
            this.groupBox2.Controls.Add(this.btn_OpenAlways);
            this.groupBox2.Controls.Add(this.btn_CloseDoor);
            this.groupBox2.Controls.Add(this.channel_comboBox);
            this.groupBox2.Controls.Add(this.btn_OpenDoor);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(13, 118);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(553, 118);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            // 
            // btn_GetState
            // 
            this.btn_GetState.Enabled = false;
            this.btn_GetState.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_GetState.Location = new System.Drawing.Point(22, 77);
            this.btn_GetState.Margin = new System.Windows.Forms.Padding(4);
            this.btn_GetState.Name = "btn_GetState";
            this.btn_GetState.Size = new System.Drawing.Size(160, 24);
            this.btn_GetState.TabIndex = 17;
            this.btn_GetState.Text = "Get State(获取状态)";
            this.btn_GetState.UseVisualStyleBackColor = true;
            this.btn_GetState.Click += new System.EventHandler(this.btn_GetState_Click);
            // 
            // btn_CloseAlways
            // 
            this.btn_CloseAlways.Enabled = false;
            this.btn_CloseAlways.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CloseAlways.Location = new System.Drawing.Point(385, 77);
            this.btn_CloseAlways.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CloseAlways.Name = "btn_CloseAlways";
            this.btn_CloseAlways.Size = new System.Drawing.Size(140, 24);
            this.btn_CloseAlways.TabIndex = 16;
            this.btn_CloseAlways.Text = "CloseAlways(常闭)";
            this.btn_CloseAlways.UseVisualStyleBackColor = true;
            this.btn_CloseAlways.Click += new System.EventHandler(this.btn_CloseAlways_Click);
            // 
            // btn_OpenAlways
            // 
            this.btn_OpenAlways.Enabled = false;
            this.btn_OpenAlways.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_OpenAlways.Location = new System.Drawing.Point(385, 30);
            this.btn_OpenAlways.Margin = new System.Windows.Forms.Padding(4);
            this.btn_OpenAlways.Name = "btn_OpenAlways";
            this.btn_OpenAlways.Size = new System.Drawing.Size(140, 24);
            this.btn_OpenAlways.TabIndex = 15;
            this.btn_OpenAlways.Text = "OpenAlways(常开)";
            this.btn_OpenAlways.UseVisualStyleBackColor = true;
            this.btn_OpenAlways.Click += new System.EventHandler(this.btn_OpenAlways_Click);
            // 
            // btn_CloseDoor
            // 
            this.btn_CloseDoor.Enabled = false;
            this.btn_CloseDoor.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_CloseDoor.Location = new System.Drawing.Point(214, 77);
            this.btn_CloseDoor.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CloseDoor.Name = "btn_CloseDoor";
            this.btn_CloseDoor.Size = new System.Drawing.Size(140, 24);
            this.btn_CloseDoor.TabIndex = 14;
            this.btn_CloseDoor.Text = "Close Door(关门)";
            this.btn_CloseDoor.UseVisualStyleBackColor = true;
            this.btn_CloseDoor.Click += new System.EventHandler(this.btn_CloseDoor_Click);
            // 
            // channel_comboBox
            // 
            this.channel_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channel_comboBox.FormattingEnabled = true;
            this.channel_comboBox.Location = new System.Drawing.Point(120, 30);
            this.channel_comboBox.Name = "channel_comboBox";
            this.channel_comboBox.Size = new System.Drawing.Size(71, 22);
            this.channel_comboBox.TabIndex = 13;
            // 
            // btn_OpenDoor
            // 
            this.btn_OpenDoor.Enabled = false;
            this.btn_OpenDoor.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_OpenDoor.Location = new System.Drawing.Point(214, 30);
            this.btn_OpenDoor.Margin = new System.Windows.Forms.Padding(4);
            this.btn_OpenDoor.Name = "btn_OpenDoor";
            this.btn_OpenDoor.Size = new System.Drawing.Size(140, 24);
            this.btn_OpenDoor.TabIndex = 12;
            this.btn_OpenDoor.Text = "Open Door(开门)";
            this.btn_OpenDoor.UseVisualStyleBackColor = true;
            this.btn_OpenDoor.Click += new System.EventHandler(this.btn_OpenDoor_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 33);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 14);
            this.label8.TabIndex = 0;
            this.label8.Text = "Channel(通道):";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_AccessPassword);
            this.groupBox3.Controls.Add(this.btn_UserOperate);
            this.groupBox3.Location = new System.Drawing.Point(574, 118);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(264, 118);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            // 
            // btn_AccessPassword
            // 
            this.btn_AccessPassword.Enabled = false;
            this.btn_AccessPassword.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AccessPassword.Location = new System.Drawing.Point(48, 77);
            this.btn_AccessPassword.Margin = new System.Windows.Forms.Padding(4);
            this.btn_AccessPassword.Name = "btn_AccessPassword";
            this.btn_AccessPassword.Size = new System.Drawing.Size(169, 24);
            this.btn_AccessPassword.TabIndex = 16;
            this.btn_AccessPassword.Text = "Pwd Operate(密码管理)";
            this.btn_AccessPassword.UseVisualStyleBackColor = true;
            this.btn_AccessPassword.Click += new System.EventHandler(this.btn_AccessPassword_Click);
            // 
            // btn_UserOperate
            // 
            this.btn_UserOperate.Enabled = false;
            this.btn_UserOperate.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_UserOperate.Location = new System.Drawing.Point(48, 30);
            this.btn_UserOperate.Margin = new System.Windows.Forms.Padding(4);
            this.btn_UserOperate.Name = "btn_UserOperate";
            this.btn_UserOperate.Size = new System.Drawing.Size(169, 24);
            this.btn_UserOperate.TabIndex = 15;
            this.btn_UserOperate.Text = "User Operate(用户管理)";
            this.btn_UserOperate.UseVisualStyleBackColor = true;
            this.btn_UserOperate.Click += new System.EventHandler(this.btn_UserOperate_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btn_GeneralConfig);
            this.groupBox4.Location = new System.Drawing.Point(846, 118);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(264, 118);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            // 
            // btn_GeneralConfig
            // 
            this.btn_GeneralConfig.Enabled = false;
            this.btn_GeneralConfig.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_GeneralConfig.Location = new System.Drawing.Point(35, 30);
            this.btn_GeneralConfig.Margin = new System.Windows.Forms.Padding(4);
            this.btn_GeneralConfig.Name = "btn_GeneralConfig";
            this.btn_GeneralConfig.Size = new System.Drawing.Size(188, 24);
            this.btn_GeneralConfig.TabIndex = 15;
            this.btn_GeneralConfig.Text = "General Config(通用配置)";
            this.btn_GeneralConfig.UseVisualStyleBackColor = true;
            this.btn_GeneralConfig.Click += new System.EventHandler(this.btn_GeneralConfig_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_OpenEvent);
            this.groupBox5.Controls.Add(this.listView_event);
            this.groupBox5.Controls.Add(this.btn_Query);
            this.groupBox5.Controls.Add(this.btn_StartListen);
            this.groupBox5.Location = new System.Drawing.Point(13, 244);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox5.Size = new System.Drawing.Size(1097, 425);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Listen Event(事件订阅)";
            // 
            // btn_OpenEvent
            // 
            this.btn_OpenEvent.Enabled = false;
            this.btn_OpenEvent.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_OpenEvent.Location = new System.Drawing.Point(851, 30);
            this.btn_OpenEvent.Margin = new System.Windows.Forms.Padding(4);
            this.btn_OpenEvent.Name = "btn_OpenEvent";
            this.btn_OpenEvent.Size = new System.Drawing.Size(174, 24);
            this.btn_OpenEvent.TabIndex = 16;
            this.btn_OpenEvent.Text = "OpenEvent(开门事件)";
            this.btn_OpenEvent.UseVisualStyleBackColor = true;
            this.btn_OpenEvent.Click += new System.EventHandler(this.btn_OpenEvent_Click);
            // 
            // listView_event
            // 
            this.listView_event.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader1,
            this.columnHeader7,
            this.columnHeader8});
            this.listView_event.FullRowSelect = true;
            this.listView_event.GridLines = true;
            this.listView_event.Location = new System.Drawing.Point(7, 73);
            this.listView_event.Name = "listView_event";
            this.listView_event.Size = new System.Drawing.Size(1083, 345);
            this.listView_event.TabIndex = 16;
            this.listView_event.UseCompatibleStateImageBehavior = false;
            this.listView_event.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "No.(编号)";
            this.columnHeader2.Width = 75;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Time(时间)";
            this.columnHeader3.Width = 160;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Type(类型)";
            this.columnHeader4.Width = 180;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "PersonNo.(人员编号)";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Card ID(卡号)";
            this.columnHeader6.Width = 120;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Door(门)";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type(验证方式)";
            this.columnHeader7.Width = 140;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "State(状态)";
            this.columnHeader8.Width = 100;
            // 
            // btn_Query
            // 
            this.btn_Query.Enabled = false;
            this.btn_Query.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Query.Location = new System.Drawing.Point(278, 30);
            this.btn_Query.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Query.Name = "btn_Query";
            this.btn_Query.Size = new System.Drawing.Size(174, 24);
            this.btn_Query.TabIndex = 15;
            this.btn_Query.Text = "Record Query(记录查询)";
            this.btn_Query.UseVisualStyleBackColor = true;
            this.btn_Query.Click += new System.EventHandler(this.btn_Query_Click);
            // 
            // btn_StartListen
            // 
            this.btn_StartListen.Enabled = false;
            this.btn_StartListen.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_StartListen.Location = new System.Drawing.Point(8, 30);
            this.btn_StartListen.Margin = new System.Windows.Forms.Padding(4);
            this.btn_StartListen.Name = "btn_StartListen";
            this.btn_StartListen.Size = new System.Drawing.Size(174, 24);
            this.btn_StartListen.TabIndex = 12;
            this.btn_StartListen.Text = "StartListen(开启订阅)";
            this.btn_StartListen.UseVisualStyleBackColor = true;
            this.btn_StartListen.Click += new System.EventHandler(this.btn_StartListen_Click);
            // 
            // AccessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 682);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "AccessForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccessControl2S Demo(门禁二代Demo)";
            this.Load += new System.EventHandler(this.AccessDemo2s_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.TextBox pwd_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox user_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox port_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ip_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_SystemConfig;
        private System.Windows.Forms.ToolStripMenuItem menu_DeviceInfo;
        private System.Windows.Forms.ToolStripMenuItem menu_Net;
        private System.Windows.Forms.ToolStripMenuItem menu_DeviceTime;
        private System.Windows.Forms.ToolStripMenuItem menu_ChangePwd;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem menu_Reboot;
        private System.Windows.Forms.ToolStripMenuItem menu_ConfigReset;
        private System.Windows.Forms.ToolStripMenuItem menu_Upgrade;
        private System.Windows.Forms.ToolStripMenuItem menu_AutoMatrix;
        private System.Windows.Forms.ToolStripMenuItem menu_AdvanceConfig;
        private System.Windows.Forms.ToolStripMenuItem menu_OpenDoorGroup;
        private System.Windows.Forms.ToolStripMenuItem menu_FirstEnter;
        private System.Windows.Forms.ToolStripMenuItem menu_MultidoorInterlock;
        private System.Windows.Forms.ToolStripMenuItem menu_RepeatEnter;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menu_DeviceList;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_OpenDoor;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox channel_comboBox;
        private System.Windows.Forms.Button btn_CloseDoor;
        private System.Windows.Forms.Button btn_CloseAlways;
        private System.Windows.Forms.Button btn_OpenAlways;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btn_AccessPassword;
        private System.Windows.Forms.Button btn_UserOperate;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btn_GeneralConfig;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_Query;
        private System.Windows.Forms.Button btn_StartListen;
        private System.Windows.Forms.ListView listView_event;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.Button btn_GetState;
        private System.Windows.Forms.Button btn_OpenEvent;
    }
}

