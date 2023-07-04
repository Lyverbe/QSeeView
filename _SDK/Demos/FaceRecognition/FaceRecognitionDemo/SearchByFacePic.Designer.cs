namespace WinForm_IVS_FaceRecognition_Demo
{
    partial class SearchByFacePicWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_SelectMode = new System.Windows.Forms.GroupBox();
            this.radioBox_historyLib = new System.Windows.Forms.RadioButton();
            this.textBox_similarity = new System.Windows.Forms.TextBox();
            this.radioBox_registerLib = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_historyLibParam = new System.Windows.Forms.GroupBox();
            this.dateTimePicker_end = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_start = new System.Windows.Forms.DateTimePicker();
            this.comboBox_channelPic = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_doFindNext = new System.Windows.Forms.Button();
            this.button_startFind = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_selectPic = new System.Windows.Forms.Button();
            this.pictureBox_localFace = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.pictureBox_candidateFace = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.listView_searchInfo = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UserID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PersonName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Sex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BirthDay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.IDNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Similarity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_doFindSpecify = new System.Windows.Forms.Button();
            this.text_specifyIndex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button_doFindPre = new System.Windows.Forms.Button();
            this.progressBar_startFind = new System.Windows.Forms.ProgressBar();
            this.label_candidate_birthday = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label_candidate_name = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label_candidate_similarity = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label_candidate_groupname = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label_candidate_groupid = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label_candidate_id = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label_candidate_sex = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label_candidate_UID = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label_info = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label_candidate_type = new System.Windows.Forms.Label();
            this.groupBox_SelectMode.SuspendLayout();
            this.groupBox_historyLibParam.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_localFace)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_candidateFace)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_SelectMode
            // 
            this.groupBox_SelectMode.Controls.Add(this.radioBox_historyLib);
            this.groupBox_SelectMode.Controls.Add(this.textBox_similarity);
            this.groupBox_SelectMode.Controls.Add(this.radioBox_registerLib);
            this.groupBox_SelectMode.Controls.Add(this.label3);
            this.groupBox_SelectMode.Controls.Add(this.groupBox_historyLibParam);
            this.groupBox_SelectMode.Location = new System.Drawing.Point(12, 12);
            this.groupBox_SelectMode.Name = "groupBox_SelectMode";
            this.groupBox_SelectMode.Size = new System.Drawing.Size(215, 332);
            this.groupBox_SelectMode.TabIndex = 2;
            this.groupBox_SelectMode.TabStop = false;
            this.groupBox_SelectMode.Text = "SelectMode(选择模式)";
            // 
            // radioBox_historyLib
            // 
            this.radioBox_historyLib.AutoSize = true;
            this.radioBox_historyLib.Location = new System.Drawing.Point(13, 54);
            this.radioBox_historyLib.Name = "radioBox_historyLib";
            this.radioBox_historyLib.Size = new System.Drawing.Size(131, 16);
            this.radioBox_historyLib.TabIndex = 19;
            this.radioBox_historyLib.Text = "HistoryLib(历史库)";
            this.radioBox_historyLib.UseVisualStyleBackColor = true;
            this.radioBox_historyLib.CheckedChanged += new System.EventHandler(this.radioBox_historyLib_CheckedChanged);
            // 
            // textBox_similarity
            // 
            this.textBox_similarity.Location = new System.Drawing.Point(129, 296);
            this.textBox_similarity.Name = "textBox_similarity";
            this.textBox_similarity.Size = new System.Drawing.Size(65, 21);
            this.textBox_similarity.TabIndex = 22;
            this.textBox_similarity.Text = "80";
            this.textBox_similarity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioBox_registerLib
            // 
            this.radioBox_registerLib.AutoSize = true;
            this.radioBox_registerLib.Checked = true;
            this.radioBox_registerLib.Location = new System.Drawing.Point(13, 27);
            this.radioBox_registerLib.Name = "radioBox_registerLib";
            this.radioBox_registerLib.Size = new System.Drawing.Size(137, 16);
            this.radioBox_registerLib.TabIndex = 18;
            this.radioBox_registerLib.TabStop = true;
            this.radioBox_registerLib.Text = "RegisterLib(注册库)";
            this.radioBox_registerLib.UseVisualStyleBackColor = true;
            this.radioBox_registerLib.CheckedChanged += new System.EventHandler(this.radioBox_registerLib_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Similarity(相似度): ";
            // 
            // groupBox_historyLibParam
            // 
            this.groupBox_historyLibParam.Controls.Add(this.dateTimePicker_end);
            this.groupBox_historyLibParam.Controls.Add(this.dateTimePicker_start);
            this.groupBox_historyLibParam.Controls.Add(this.comboBox_channelPic);
            this.groupBox_historyLibParam.Controls.Add(this.label6);
            this.groupBox_historyLibParam.Controls.Add(this.label2);
            this.groupBox_historyLibParam.Controls.Add(this.label1);
            this.groupBox_historyLibParam.Enabled = false;
            this.groupBox_historyLibParam.Location = new System.Drawing.Point(6, 83);
            this.groupBox_historyLibParam.Name = "groupBox_historyLibParam";
            this.groupBox_historyLibParam.Size = new System.Drawing.Size(200, 202);
            this.groupBox_historyLibParam.TabIndex = 2;
            this.groupBox_historyLibParam.TabStop = false;
            this.groupBox_historyLibParam.Text = "HistoryLibParam(历史库条件)";
            // 
            // dateTimePicker_end
            // 
            this.dateTimePicker_end.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimePicker_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_end.Location = new System.Drawing.Point(16, 157);
            this.dateTimePicker_end.Name = "dateTimePicker_end";
            this.dateTimePicker_end.Size = new System.Drawing.Size(172, 21);
            this.dateTimePicker_end.TabIndex = 22;
            // 
            // dateTimePicker_start
            // 
            this.dateTimePicker_start.CustomFormat = "yyyy/MM/dd HH:mm:ss ";
            this.dateTimePicker_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_start.Location = new System.Drawing.Point(16, 89);
            this.dateTimePicker_start.Name = "dateTimePicker_start";
            this.dateTimePicker_start.Size = new System.Drawing.Size(172, 21);
            this.dateTimePicker_start.TabIndex = 21;
            // 
            // comboBox_channelPic
            // 
            this.comboBox_channelPic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_channelPic.FormattingEnabled = true;
            this.comboBox_channelPic.Location = new System.Drawing.Point(109, 29);
            this.comboBox_channelPic.Name = "comboBox_channelPic";
            this.comboBox_channelPic.Size = new System.Drawing.Size(79, 20);
            this.comboBox_channelPic.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "Channel(通道):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "EndTime(结束时间):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "StartTime(开始时间):";
            // 
            // button_doFindNext
            // 
            this.button_doFindNext.Location = new System.Drawing.Point(262, 17);
            this.button_doFindNext.Name = "button_doFindNext";
            this.button_doFindNext.Size = new System.Drawing.Size(115, 30);
            this.button_doFindNext.TabIndex = 18;
            this.button_doFindNext.Text = "NextBatch(下一组)";
            this.button_doFindNext.UseVisualStyleBackColor = true;
            this.button_doFindNext.Click += new System.EventHandler(this.button_doFindNext_Click);
            // 
            // button_startFind
            // 
            this.button_startFind.Location = new System.Drawing.Point(10, 17);
            this.button_startFind.Name = "button_startFind";
            this.button_startFind.Size = new System.Drawing.Size(115, 30);
            this.button_startFind.TabIndex = 17;
            this.button_startFind.Text = "StartSearch(开始)";
            this.button_startFind.UseVisualStyleBackColor = true;
            this.button_startFind.Click += new System.EventHandler(this.button_startFind_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_delete);
            this.groupBox1.Controls.Add(this.button_selectPic);
            this.groupBox1.Controls.Add(this.pictureBox_localFace);
            this.groupBox1.Location = new System.Drawing.Point(233, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 266);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SelectPic(选择图片)";
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(167, 15);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(31, 21);
            this.button_delete.TabIndex = 3;
            this.button_delete.Text = "Del";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_selectPic
            // 
            this.button_selectPic.Location = new System.Drawing.Point(45, 142);
            this.button_selectPic.Name = "button_selectPic";
            this.button_selectPic.Size = new System.Drawing.Size(112, 23);
            this.button_selectPic.TabIndex = 2;
            this.button_selectPic.Text = "Select(选择图片)";
            this.button_selectPic.UseVisualStyleBackColor = true;
            this.button_selectPic.Click += new System.EventHandler(this.button_selectPic_Click);
            // 
            // pictureBox_localFace
            // 
            this.pictureBox_localFace.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_localFace.Location = new System.Drawing.Point(15, 16);
            this.pictureBox_localFace.Name = "pictureBox_localFace";
            this.pictureBox_localFace.Size = new System.Drawing.Size(183, 245);
            this.pictureBox_localFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_localFace.TabIndex = 1;
            this.pictureBox_localFace.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.pictureBox_candidateFace);
            this.groupBox3.Location = new System.Drawing.Point(461, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(217, 266);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CandidatePic(候选图片)";
            // 
            // pictureBox_candidateFace
            // 
            this.pictureBox_candidateFace.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBox_candidateFace.Location = new System.Drawing.Point(16, 14);
            this.pictureBox_candidateFace.Name = "pictureBox_candidateFace";
            this.pictureBox_candidateFace.Size = new System.Drawing.Size(183, 246);
            this.pictureBox_candidateFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_candidateFace.TabIndex = 1;
            this.pictureBox_candidateFace.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listView_searchInfo);
            this.groupBox4.Location = new System.Drawing.Point(12, 351);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(957, 308);
            this.groupBox4.TabIndex = 19;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "QueryInfo(查询信息)";
            // 
            // listView_searchInfo
            // 
            this.listView_searchInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index,
            this.UserID,
            this.PersonName,
            this.Sex,
            this.BirthDay,
            this.IDType,
            this.IDNumber,
            this.Similarity});
            this.listView_searchInfo.FullRowSelect = true;
            this.listView_searchInfo.GridLines = true;
            this.listView_searchInfo.Location = new System.Drawing.Point(6, 16);
            this.listView_searchInfo.Name = "listView_searchInfo";
            this.listView_searchInfo.Size = new System.Drawing.Size(945, 285);
            this.listView_searchInfo.TabIndex = 3;
            this.listView_searchInfo.UseCompatibleStateImageBehavior = false;
            this.listView_searchInfo.View = System.Windows.Forms.View.Details;
            this.listView_searchInfo.SelectedIndexChanged += new System.EventHandler(this.listView_searchInfo_SelectedIndexChanged);
            // 
            // Index
            // 
            this.Index.Text = "Index(编号)";
            this.Index.Width = 80;
            // 
            // UserID
            // 
            this.UserID.Text = "UserID(UID)";
            this.UserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.UserID.Width = 100;
            // 
            // PersonName
            // 
            this.PersonName.Text = "Name(姓名)";
            this.PersonName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PersonName.Width = 120;
            // 
            // Sex
            // 
            this.Sex.Text = "Sex(姓别)";
            this.Sex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Sex.Width = 90;
            // 
            // BirthDay
            // 
            this.BirthDay.Text = "BirthDay(生日)";
            this.BirthDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.BirthDay.Width = 110;
            // 
            // IDType
            // 
            this.IDType.Text = "IDType(证件类型)";
            this.IDType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IDType.Width = 120;
            // 
            // IDNumber
            // 
            this.IDNumber.Text = "IDNumber(证件号)";
            this.IDNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IDNumber.Width = 180;
            // 
            // Similarity
            // 
            this.Similarity.Text = "Similarity(相似度)";
            this.Similarity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Similarity.Width = 130;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_doFindSpecify);
            this.groupBox5.Controls.Add(this.text_specifyIndex);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.button_doFindPre);
            this.groupBox5.Controls.Add(this.button_doFindNext);
            this.groupBox5.Controls.Add(this.progressBar_startFind);
            this.groupBox5.Controls.Add(this.button_startFind);
            this.groupBox5.Location = new System.Drawing.Point(233, 284);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(735, 60);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Query(查询)";
            // 
            // button_doFindSpecify
            // 
            this.button_doFindSpecify.Location = new System.Drawing.Point(613, 17);
            this.button_doFindSpecify.Name = "button_doFindSpecify";
            this.button_doFindSpecify.Size = new System.Drawing.Size(110, 30);
            this.button_doFindSpecify.TabIndex = 55;
            this.button_doFindSpecify.Text = "GetBench(获取)";
            this.button_doFindSpecify.UseVisualStyleBackColor = true;
            this.button_doFindSpecify.Click += new System.EventHandler(this.button_doFindSpecify_Click);
            // 
            // text_specifyIndex
            // 
            this.text_specifyIndex.Location = new System.Drawing.Point(534, 23);
            this.text_specifyIndex.Name = "text_specifyIndex";
            this.text_specifyIndex.Size = new System.Drawing.Size(69, 21);
            this.text_specifyIndex.TabIndex = 54;
            this.text_specifyIndex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_specifyIndex_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(390, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 12);
            this.label4.TabIndex = 53;
            this.label4.Text = "SpecifyIndex(指定序号):";
            // 
            // button_doFindPre
            // 
            this.button_doFindPre.Location = new System.Drawing.Point(138, 17);
            this.button_doFindPre.Name = "button_doFindPre";
            this.button_doFindPre.Size = new System.Drawing.Size(110, 30);
            this.button_doFindPre.TabIndex = 20;
            this.button_doFindPre.Text = "PreBatch(上一组)";
            this.button_doFindPre.UseVisualStyleBackColor = true;
            this.button_doFindPre.Click += new System.EventHandler(this.button_doFindPre_Click);
            // 
            // progressBar_startFind
            // 
            this.progressBar_startFind.Location = new System.Drawing.Point(0, 51);
            this.progressBar_startFind.Name = "progressBar_startFind";
            this.progressBar_startFind.Size = new System.Drawing.Size(734, 10);
            this.progressBar_startFind.Step = 1;
            this.progressBar_startFind.TabIndex = 21;
            // 
            // label_candidate_birthday
            // 
            this.label_candidate_birthday.AutoSize = true;
            this.label_candidate_birthday.Location = new System.Drawing.Point(129, 71);
            this.label_candidate_birthday.Name = "label_candidate_birthday";
            this.label_candidate_birthday.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_birthday.TabIndex = 50;
            this.label_candidate_birthday.Text = "11111111";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(34, 71);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(95, 12);
            this.label24.TabIndex = 49;
            this.label24.Text = "BirthDay(生日):";
            // 
            // label_candidate_name
            // 
            this.label_candidate_name.AutoSize = true;
            this.label_candidate_name.Location = new System.Drawing.Point(129, 39);
            this.label_candidate_name.Name = "label_candidate_name";
            this.label_candidate_name.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_name.TabIndex = 48;
            this.label_candidate_name.Text = "11111111";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(58, 39);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(71, 12);
            this.label26.TabIndex = 47;
            this.label26.Text = "Name(姓名):";
            // 
            // label_candidate_similarity
            // 
            this.label_candidate_similarity.AutoSize = true;
            this.label_candidate_similarity.Location = new System.Drawing.Point(129, 163);
            this.label_candidate_similarity.Name = "label_candidate_similarity";
            this.label_candidate_similarity.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_similarity.TabIndex = 46;
            this.label_candidate_similarity.Text = "11111111";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(11, 163);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(119, 12);
            this.label32.TabIndex = 45;
            this.label32.Text = "Similarity(相似度):";
            // 
            // label_candidate_groupname
            // 
            this.label_candidate_groupname.AutoSize = true;
            this.label_candidate_groupname.Location = new System.Drawing.Point(129, 144);
            this.label_candidate_groupname.Name = "label_candidate_groupname";
            this.label_candidate_groupname.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_groupname.TabIndex = 44;
            this.label_candidate_groupname.Text = "11111111";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(5, 144);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(125, 12);
            this.label34.TabIndex = 43;
            this.label34.Text = "GroupName(人脸库名):";
            // 
            // label_candidate_groupid
            // 
            this.label_candidate_groupid.AutoSize = true;
            this.label_candidate_groupid.Location = new System.Drawing.Point(129, 126);
            this.label_candidate_groupid.Name = "label_candidate_groupid";
            this.label_candidate_groupid.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_groupid.TabIndex = 42;
            this.label_candidate_groupid.Text = "11111111";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(17, 126);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(113, 12);
            this.label36.TabIndex = 41;
            this.label36.Text = "GroupID(人脸库ID):";
            // 
            // label_candidate_id
            // 
            this.label_candidate_id.AutoSize = true;
            this.label_candidate_id.Location = new System.Drawing.Point(129, 107);
            this.label_candidate_id.Name = "label_candidate_id";
            this.label_candidate_id.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_id.TabIndex = 40;
            this.label_candidate_id.Text = "11111111";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(22, 89);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(107, 12);
            this.label38.TabIndex = 39;
            this.label38.Text = "IDType(证件类型):";
            // 
            // label_candidate_sex
            // 
            this.label_candidate_sex.AutoSize = true;
            this.label_candidate_sex.Location = new System.Drawing.Point(129, 54);
            this.label_candidate_sex.Name = "label_candidate_sex";
            this.label_candidate_sex.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_sex.TabIndex = 38;
            this.label_candidate_sex.Text = "11111111";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(64, 54);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(65, 12);
            this.label40.TabIndex = 37;
            this.label40.Text = "Sex(姓别):";
            // 
            // label_candidate_UID
            // 
            this.label_candidate_UID.AutoSize = true;
            this.label_candidate_UID.Location = new System.Drawing.Point(129, 23);
            this.label_candidate_UID.Name = "label_candidate_UID";
            this.label_candidate_UID.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_UID.TabIndex = 52;
            this.label_candidate_UID.Text = "11111111";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(52, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 51;
            this.label5.Text = "UserID(UID):";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.label_candidate_type);
            this.groupBox6.Controls.Add(this.label_candidate_UID);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.label40);
            this.groupBox6.Controls.Add(this.label_candidate_birthday);
            this.groupBox6.Controls.Add(this.label_candidate_sex);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.label38);
            this.groupBox6.Controls.Add(this.label_candidate_name);
            this.groupBox6.Controls.Add(this.label_candidate_id);
            this.groupBox6.Controls.Add(this.label26);
            this.groupBox6.Controls.Add(this.label36);
            this.groupBox6.Controls.Add(this.label_candidate_similarity);
            this.groupBox6.Controls.Add(this.label_candidate_groupid);
            this.groupBox6.Controls.Add(this.label32);
            this.groupBox6.Controls.Add(this.label34);
            this.groupBox6.Controls.Add(this.label_candidate_groupname);
            this.groupBox6.Location = new System.Drawing.Point(683, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(285, 193);
            this.groupBox6.TabIndex = 53;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "CandidateInfo(候选信息)";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label_info);
            this.groupBox7.Location = new System.Drawing.Point(684, 211);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(285, 67);
            this.groupBox7.TabIndex = 20;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Message(信息):";
            // 
            // label_info
            // 
            this.label_info.AutoSize = true;
            this.label_info.Location = new System.Drawing.Point(10, 20);
            this.label_info.Name = "label_info";
            this.label_info.Size = new System.Drawing.Size(47, 12);
            this.label_info.TabIndex = 53;
            this.label_info.Text = "1111111";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 107);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 12);
            this.label7.TabIndex = 53;
            this.label7.Text = "IDNumber(证件号):";
            // 
            // label_candidate_type
            // 
            this.label_candidate_type.AutoSize = true;
            this.label_candidate_type.Location = new System.Drawing.Point(129, 89);
            this.label_candidate_type.Name = "label_candidate_type";
            this.label_candidate_type.Size = new System.Drawing.Size(53, 12);
            this.label_candidate_type.TabIndex = 54;
            this.label_candidate_type.Text = "11111111";
            // 
            // SearchByFacePicWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 664);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_SelectMode);
            this.MaximizeBox = false;
            this.Name = "SearchByFacePicWindow";
            this.Text = "SearchByFacePic(以图搜图)";
            this.Load += new System.EventHandler(this.SearchByFacePicWindow_Load);
            this.groupBox_SelectMode.ResumeLayout(false);
            this.groupBox_SelectMode.PerformLayout();
            this.groupBox_historyLibParam.ResumeLayout(false);
            this.groupBox_historyLibParam.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_localFace)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_candidateFace)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_SelectMode;
        private System.Windows.Forms.GroupBox groupBox_historyLibParam;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_selectPic;
        private System.Windows.Forms.PictureBox pictureBox_localFace;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.PictureBox pictureBox_candidateFace;
        private System.Windows.Forms.Button button_doFindNext;
        private System.Windows.Forms.Button button_startFind;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.TextBox textBox_similarity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.ProgressBar progressBar_startFind;
        private System.Windows.Forms.Label label_candidate_UID;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_candidate_birthday;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label_candidate_name;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label_candidate_similarity;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label_candidate_groupname;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label_candidate_groupid;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label_candidate_id;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label_candidate_sex;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton radioBox_historyLib;
        private System.Windows.Forms.RadioButton radioBox_registerLib;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label_info;
        private System.Windows.Forms.ListView listView_searchInfo;
        private System.Windows.Forms.ColumnHeader PersonName;
        private System.Windows.Forms.ColumnHeader Sex;
        private System.Windows.Forms.ColumnHeader BirthDay;
        private System.Windows.Forms.ColumnHeader IDType;
        private System.Windows.Forms.ColumnHeader IDNumber;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ColumnHeader UserID;
        private System.Windows.Forms.ColumnHeader Similarity;
        private System.Windows.Forms.Button button_doFindPre;
        private System.Windows.Forms.TextBox text_specifyIndex;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_doFindSpecify;
        private System.Windows.Forms.ComboBox comboBox_channelPic;
        private System.Windows.Forms.DateTimePicker dateTimePicker_end;
        private System.Windows.Forms.DateTimePicker dateTimePicker_start;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label_candidate_type;
    }
}