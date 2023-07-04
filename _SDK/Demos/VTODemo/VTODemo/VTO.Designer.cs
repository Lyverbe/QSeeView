namespace VTODemo
{
    partial class VTO
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
            this.button_login = new System.Windows.Forms.Button();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_ip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox_play = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView_event = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_stopRealLoad = new System.Windows.Forms.Button();
            this.button_stoplisten = new System.Windows.Forms.Button();
            this.button_startlisten = new System.Windows.Forms.Button();
            this.button_startRealLoad = new System.Windows.Forms.Button();
            this.button_realplay = new System.Windows.Forms.Button();
            this.button_stopplay = new System.Windows.Forms.Button();
            this.button_talk = new System.Windows.Forms.Button();
            this.button_stoptalk = new System.Windows.Forms.Button();
            this.button_operatecard = new System.Windows.Forms.Button();
            this.button_open = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tabControl_eventType = new System.Windows.Forms.TabControl();
            this.tabEvent_normal = new System.Windows.Forms.TabPage();
            this.tabPage_realLoad = new System.Windows.Forms.TabPage();
            this.pictureBox_realLoadEvent = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.listView_realLoadEvent = new System.Windows.Forms.ListView();
            this.realLoadEvent_RoomID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.realLoadEvent_cardNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.realLoadEvent_eventTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.realLoadEvent_eventInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabControl_eventType.SuspendLayout();
            this.tabEvent_normal.SuspendLayout();
            this.tabPage_realLoad.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_realLoadEvent)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_login);
            this.groupBox1.Controls.Add(this.textBox_password);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_name);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_port);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox_ip);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(946, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Login(设备登录)";
            // 
            // button_login
            // 
            this.button_login.Location = new System.Drawing.Point(700, 20);
            this.button_login.Name = "button_login";
            this.button_login.Size = new System.Drawing.Size(196, 23);
            this.button_login.TabIndex = 8;
            this.button_login.Text = "Login(登录)";
            this.button_login.UseVisualStyleBackColor = true;
            this.button_login.Click += new System.EventHandler(this.button_login_Click);
            // 
            // textBox_password
            // 
            this.textBox_password.Location = new System.Drawing.Point(593, 22);
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.PasswordChar = '*';
            this.textBox_password.Size = new System.Drawing.Size(100, 21);
            this.textBox_password.TabIndex = 7;
            this.textBox_password.Text = "admin123";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(498, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password(密码):";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(419, 22);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(71, 21);
            this.textBox_name.TabIndex = 5;
            this.textBox_name.Text = "admin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(334, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Name(用户名):";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(289, 22);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(40, 21);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.Text = "37777";
            this.textBox_port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_port_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port(端口):";
            // 
            // textBox_ip
            // 
            this.textBox_ip.Location = new System.Drawing.Point(112, 22);
            this.textBox_ip.Name = "textBox_ip";
            this.textBox_ip.Size = new System.Drawing.Size(100, 21);
            this.textBox_ip.TabIndex = 1;
            this.textBox_ip.Text = "172.23.10.233";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP(设备IP):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox_play);
            this.groupBox2.Location = new System.Drawing.Point(5, 67);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(383, 277);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "PreView(预览)";
            // 
            // pictureBox_play
            // 
            this.pictureBox_play.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_play.Location = new System.Drawing.Point(14, 20);
            this.pictureBox_play.Name = "pictureBox_play";
            this.pictureBox_play.Size = new System.Drawing.Size(354, 248);
            this.pictureBox_play.TabIndex = 0;
            this.pictureBox_play.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView_event);
            this.groupBox4.Location = new System.Drawing.Point(7, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(925, 244);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "EventList(事件列表)";
            // 
            // listView_event
            // 
            this.listView_event.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView_event.FullRowSelect = true;
            this.listView_event.GridLines = true;
            this.listView_event.Location = new System.Drawing.Point(6, 20);
            this.listView_event.Name = "listView_event";
            this.listView_event.Size = new System.Drawing.Size(913, 218);
            this.listView_event.TabIndex = 0;
            this.listView_event.UseCompatibleStateImageBehavior = false;
            this.listView_event.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "RoomNo.(房间号)";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "CardNo.(卡号)";
            this.columnHeader3.Width = 180;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Time(时间)";
            this.columnHeader4.Width = 150;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "OpenMethod(开门方式)";
            this.columnHeader5.Width = 180;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Status(状态)";
            this.columnHeader6.Width = 170;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_stopRealLoad);
            this.groupBox5.Controls.Add(this.button_stoplisten);
            this.groupBox5.Controls.Add(this.button_startlisten);
            this.groupBox5.Controls.Add(this.button_startRealLoad);
            this.groupBox5.Location = new System.Drawing.Point(394, 248);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(557, 96);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Event Operate(事件操作)";
            // 
            // button_stopRealLoad
            // 
            this.button_stopRealLoad.Location = new System.Drawing.Point(311, 54);
            this.button_stopRealLoad.Name = "button_stopRealLoad";
            this.button_stopRealLoad.Size = new System.Drawing.Size(196, 23);
            this.button_stopRealLoad.TabIndex = 6;
            this.button_stopRealLoad.Text = "StopRealLoad(停止智能监听)";
            this.button_stopRealLoad.UseVisualStyleBackColor = true;
            this.button_stopRealLoad.Click += new System.EventHandler(this.button_stopRealLoad_Click);
            // 
            // button_stoplisten
            // 
            this.button_stoplisten.Location = new System.Drawing.Point(30, 54);
            this.button_stoplisten.Name = "button_stoplisten";
            this.button_stoplisten.Size = new System.Drawing.Size(196, 23);
            this.button_stoplisten.TabIndex = 4;
            this.button_stoplisten.Text = "StopListen(停止监听)";
            this.button_stoplisten.UseVisualStyleBackColor = true;
            this.button_stoplisten.Click += new System.EventHandler(this.button_stoplisten_Click);
            // 
            // button_startlisten
            // 
            this.button_startlisten.Location = new System.Drawing.Point(30, 25);
            this.button_startlisten.Name = "button_startlisten";
            this.button_startlisten.Size = new System.Drawing.Size(196, 23);
            this.button_startlisten.TabIndex = 3;
            this.button_startlisten.Text = "StartListen(开始监听)";
            this.button_startlisten.UseVisualStyleBackColor = true;
            this.button_startlisten.Click += new System.EventHandler(this.button_startlisten_Click);
            // 
            // button_startRealLoad
            // 
            this.button_startRealLoad.Location = new System.Drawing.Point(311, 25);
            this.button_startRealLoad.Name = "button_startRealLoad";
            this.button_startRealLoad.Size = new System.Drawing.Size(196, 23);
            this.button_startRealLoad.TabIndex = 5;
            this.button_startRealLoad.Text = "StartRealLoad(开始智能监听)";
            this.button_startRealLoad.UseVisualStyleBackColor = true;
            this.button_startRealLoad.Click += new System.EventHandler(this.button_startRealLoad_Click);
            // 
            // button_realplay
            // 
            this.button_realplay.Location = new System.Drawing.Point(30, 22);
            this.button_realplay.Name = "button_realplay";
            this.button_realplay.Size = new System.Drawing.Size(196, 23);
            this.button_realplay.TabIndex = 0;
            this.button_realplay.Text = "RealPlay(监视)";
            this.button_realplay.UseVisualStyleBackColor = true;
            this.button_realplay.Click += new System.EventHandler(this.button_realplay_Click);
            // 
            // button_stopplay
            // 
            this.button_stopplay.Location = new System.Drawing.Point(30, 51);
            this.button_stopplay.Name = "button_stopplay";
            this.button_stopplay.Size = new System.Drawing.Size(196, 23);
            this.button_stopplay.TabIndex = 1;
            this.button_stopplay.Text = "StopPlay(停止监视)";
            this.button_stopplay.UseVisualStyleBackColor = true;
            this.button_stopplay.Click += new System.EventHandler(this.button_stopplay_Click);
            // 
            // button_talk
            // 
            this.button_talk.Location = new System.Drawing.Point(311, 20);
            this.button_talk.Name = "button_talk";
            this.button_talk.Size = new System.Drawing.Size(196, 23);
            this.button_talk.TabIndex = 2;
            this.button_talk.Text = "Talk(对讲)";
            this.button_talk.UseVisualStyleBackColor = true;
            this.button_talk.Click += new System.EventHandler(this.button_talk_Click);
            // 
            // button_stoptalk
            // 
            this.button_stoptalk.Location = new System.Drawing.Point(311, 49);
            this.button_stoptalk.Name = "button_stoptalk";
            this.button_stoptalk.Size = new System.Drawing.Size(196, 23);
            this.button_stoptalk.TabIndex = 3;
            this.button_stoptalk.Text = "StopTalk(停止对讲)";
            this.button_stoptalk.UseVisualStyleBackColor = true;
            this.button_stoptalk.Click += new System.EventHandler(this.button_stoptalk_Click);
            // 
            // button_operatecard
            // 
            this.button_operatecard.Location = new System.Drawing.Point(311, 105);
            this.button_operatecard.Name = "button_operatecard";
            this.button_operatecard.Size = new System.Drawing.Size(196, 23);
            this.button_operatecard.TabIndex = 4;
            this.button_operatecard.Text = "Operate(操作)";
            this.button_operatecard.UseVisualStyleBackColor = true;
            this.button_operatecard.Click += new System.EventHandler(this.button_operatecard_Click);
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(30, 96);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(196, 23);
            this.button_open.TabIndex = 7;
            this.button_open.Text = "OpenDoor(开门)";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(30, 125);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(196, 23);
            this.button_close.TabIndex = 8;
            this.button_close.Text = "CloseDoor(关门)";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_close);
            this.groupBox3.Controls.Add(this.button_open);
            this.groupBox3.Controls.Add(this.button_operatecard);
            this.groupBox3.Controls.Add(this.button_stoptalk);
            this.groupBox3.Controls.Add(this.button_talk);
            this.groupBox3.Controls.Add(this.button_stopplay);
            this.groupBox3.Controls.Add(this.button_realplay);
            this.groupBox3.Location = new System.Drawing.Point(394, 67);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(557, 169);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Operate(操作)";
            // 
            // tabControl_eventType
            // 
            this.tabControl_eventType.Controls.Add(this.tabEvent_normal);
            this.tabControl_eventType.Controls.Add(this.tabPage_realLoad);
            this.tabControl_eventType.Location = new System.Drawing.Point(5, 350);
            this.tabControl_eventType.Name = "tabControl_eventType";
            this.tabControl_eventType.SelectedIndex = 0;
            this.tabControl_eventType.Size = new System.Drawing.Size(946, 281);
            this.tabControl_eventType.TabIndex = 5;
            // 
            // tabEvent_normal
            // 
            this.tabEvent_normal.Controls.Add(this.groupBox4);
            this.tabEvent_normal.Location = new System.Drawing.Point(4, 22);
            this.tabEvent_normal.Name = "tabEvent_normal";
            this.tabEvent_normal.Padding = new System.Windows.Forms.Padding(3);
            this.tabEvent_normal.Size = new System.Drawing.Size(938, 255);
            this.tabEvent_normal.TabIndex = 0;
            this.tabEvent_normal.Text = "AlarmEvent(报警事件)";
            this.tabEvent_normal.UseVisualStyleBackColor = true;
            // 
            // tabPage_realLoad
            // 
            this.tabPage_realLoad.Controls.Add(this.pictureBox_realLoadEvent);
            this.tabPage_realLoad.Controls.Add(this.groupBox6);
            this.tabPage_realLoad.Location = new System.Drawing.Point(4, 22);
            this.tabPage_realLoad.Name = "tabPage_realLoad";
            this.tabPage_realLoad.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_realLoad.Size = new System.Drawing.Size(938, 255);
            this.tabPage_realLoad.TabIndex = 1;
            this.tabPage_realLoad.Text = "RealLoadEvent(智能事件)";
            this.tabPage_realLoad.UseVisualStyleBackColor = true;
            // 
            // pictureBox_realLoadEvent
            // 
            this.pictureBox_realLoadEvent.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_realLoadEvent.Location = new System.Drawing.Point(673, 15);
            this.pictureBox_realLoadEvent.Name = "pictureBox_realLoadEvent";
            this.pictureBox_realLoadEvent.Size = new System.Drawing.Size(259, 229);
            this.pictureBox_realLoadEvent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_realLoadEvent.TabIndex = 4;
            this.pictureBox_realLoadEvent.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.listView_realLoadEvent);
            this.groupBox6.Location = new System.Drawing.Point(7, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(660, 244);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "EventList(事件列表)";
            // 
            // listView_realLoadEvent
            // 
            this.listView_realLoadEvent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.realLoadEvent_RoomID,
            this.realLoadEvent_cardNo,
            this.realLoadEvent_eventTime,
            this.realLoadEvent_eventInfo});
            this.listView_realLoadEvent.FullRowSelect = true;
            this.listView_realLoadEvent.GridLines = true;
            this.listView_realLoadEvent.Location = new System.Drawing.Point(6, 20);
            this.listView_realLoadEvent.Name = "listView_realLoadEvent";
            this.listView_realLoadEvent.Size = new System.Drawing.Size(644, 218);
            this.listView_realLoadEvent.TabIndex = 0;
            this.listView_realLoadEvent.UseCompatibleStateImageBehavior = false;
            this.listView_realLoadEvent.View = System.Windows.Forms.View.Details;
            // 
            // realLoadEvent_RoomID
            // 
            this.realLoadEvent_RoomID.Text = "RoomID.(房间号)";
            this.realLoadEvent_RoomID.Width = 105;
            // 
            // realLoadEvent_cardNo
            // 
            this.realLoadEvent_cardNo.Text = "CardNo.(卡号)";
            this.realLoadEvent_cardNo.Width = 100;
            // 
            // realLoadEvent_eventTime
            // 
            this.realLoadEvent_eventTime.Text = "Time(时间)";
            this.realLoadEvent_eventTime.Width = 150;
            // 
            // realLoadEvent_eventInfo
            // 
            this.realLoadEvent_eventInfo.Text = "EventInfo.(事件信息)";
            this.realLoadEvent_eventInfo.Width = 500;
            // 
            // VTO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(963, 635);
            this.Controls.Add(this.tabControl_eventType);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "VTO";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VTODemo(室外机Demo) -- OffLine";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_play)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tabControl_eventType.ResumeLayout(false);
            this.tabEvent_normal.ResumeLayout(false);
            this.tabPage_realLoad.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_realLoadEvent)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_login;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox_play;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView listView_event;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_stoplisten;
        private System.Windows.Forms.Button button_startlisten;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button button_realplay;
        private System.Windows.Forms.Button button_stopplay;
        private System.Windows.Forms.Button button_talk;
        private System.Windows.Forms.Button button_stoptalk;
        private System.Windows.Forms.Button button_operatecard;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_close;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_stopRealLoad;
        private System.Windows.Forms.Button button_startRealLoad;
        private System.Windows.Forms.TabControl tabControl_eventType;
        private System.Windows.Forms.TabPage tabEvent_normal;
        private System.Windows.Forms.TabPage tabPage_realLoad;
        private System.Windows.Forms.PictureBox pictureBox_realLoadEvent;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.ListView listView_realLoadEvent;
        private System.Windows.Forms.ColumnHeader realLoadEvent_RoomID;
        private System.Windows.Forms.ColumnHeader realLoadEvent_cardNo;
        private System.Windows.Forms.ColumnHeader realLoadEvent_eventTime;
        private System.Windows.Forms.ColumnHeader realLoadEvent_eventInfo;
    }
}

