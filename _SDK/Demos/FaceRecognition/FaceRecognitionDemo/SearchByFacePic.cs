using NetSDKCS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinForm_IVS_FaceRecognition_Demo
{
    public partial class SearchByFacePicWindow : Form
    {
        private FaceRecognitionDemo parentFrame;         // parent winForm (父窗口)
        private int nChanNum = 0;                        // count of device channels（设备通道数量）
        private IntPtr m_nLoginID = IntPtr.Zero;         // login handle （登录句柄）
        private bool b_startSearch = false;

        private const string saveRegistryFolder = "Find_Registry/";             // 保存注册库图片文件夹
        private const string saveHistoryFolder = "Find_History/";               // 保存历史库图片文件夹

        /// <summary>
        /// Fetch type 区分查询上一组还是下一组
        /// </summary>
        private enum EmDoFindDirectType
        {
            PREBATCH,   // Pre batch 上一组
            NEXTBATCH,  // Nest batch 下一组
            SPECIFY     // Given batch 指定起始序号
        }

        private Dictionary<string, FaceLibSearchMethods> TestSelector = new Dictionary<string, FaceLibSearchMethods>();

        private string keySelected = "Find_Registry";

        public SearchByFacePicWindow(FaceRecognitionDemo parentFrame, int nChanNum, IntPtr m_LoginID)
        {
            this.parentFrame = parentFrame;
            this.nChanNum = nChanNum;
            this.m_nLoginID = m_LoginID;

            InitializeComponent();

            this.displayCandidateInfoClear();          // 清理展示数据
            label_info.Text = "";
            button_startFind.Enabled = true;
            button_doFindNext.Enabled = false;
            button_doFindPre.Enabled = false;
            button_doFindSpecify.Enabled = false;

            initChannel(nChanNum);
            initTestSelector();
            createImgSaveFolder();
        }

        /// <summary>
        /// Create the download folder
        /// 创建保存图片的文件夹
        /// </summary>
        private void createImgSaveFolder()
        {
            if (!Directory.Exists(saveRegistryFolder))
                Directory.CreateDirectory(saveRegistryFolder);
            if (!Directory.Exists(saveHistoryFolder))
                Directory.CreateDirectory(saveHistoryFolder);
        }

        /// <summary>
        /// Initialization method select dictionary
        /// 初始化方法集选择字典
        /// </summary>
        private void initTestSelector()
        {
            TestSelector.Add("Find_Registry", new FaceLibSearchMethods(saveRegistryFolder, FaceFindInRegistryDBStartFind, FaceFindInRegistryDBDoFind, FaceFindInRegistryDBEndSearch));
            TestSelector.Add("Find_History", new FaceLibSearchMethods(saveHistoryFolder, FaceFindInHistoryStartFind, FaceFindInHistoryDoFind, FaceFindInHistoryEndSearch));

        }

        /// <summary>
        /// Initialize Channel Drop-down Box
        /// 初始化通道下拉框
        /// </summary>
        /// <param name="nChanNum"></param>
        private void initChannel(int nChanNum)
        {
            for (int i = 0; i < nChanNum; i++)
            {
                comboBox_channelPic.Items.Add(i + 1);
            }
            comboBox_channelPic.SelectedIndex = 0;
        }

        /// <summary>
        /// Init Datetime Calendars
        /// 初始化日期
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchByFacePicWindow_Load(object sender, EventArgs e)
        {
            this.dateTimePicker_start.Value = DateTime.Today;
            this.dateTimePicker_end.Value = DateTime.Today.AddDays(1).AddSeconds(-1);            
        }

        /// <summary>
        /// Add local face image 
        /// 加载本地图片
        /// </summary>
        private void button_selectPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG|*.jpg";
            var ret = openFileDialog.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    string path = openFileDialog.FileName;
                    localImageData = File.ReadAllBytes(path);
                    using (MemoryStream stream = new MemoryStream(localImageData))
                    {
                        Image image = Image.FromStream(stream);
                        this.pictureBox_localFace.Image = image;
                        this.pictureBox_localFace.Refresh();
                        this.pictureBox_localFace.Visible = true;

                        this.button_delete.Visible = true;
                        this.button_selectPic.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            openFileDialog.Dispose();
        }

        /// <summary>
        /// Delete selected iamge
        /// 删除加载的图片
        /// </summary>
        private void button_delete_Click(object sender, EventArgs e)
        {
            this.localImageData = null;
            this.pictureBox_localFace.Image = null;
            this.pictureBox_localFace.Refresh();
            this.pictureBox_localFace.Visible = true;

            this.button_delete.Visible = false;
            this.button_selectPic.Visible = true;
        }

        /// <summary>
        /// choose a method and search
        /// 选择一个方法再启动搜索
        /// </summary>
        private void button_startFind_Click(object sender, EventArgs e)
        {
            if (!b_startSearch)
            {
                if (localImageData == null || localImageData.Length == 0)
                {
                    MessageBox.Show("Please load a picture first.");
                    return;
                }

                FaceLibSearchMethods faceDetection;
                TestSelector.TryGetValue(keySelected, out faceDetection);
                faceDetection.fStartFind();

                button_startFind.Text = "EndSearch(结束)";
                button_doFindNext.Enabled = true;
                button_doFindPre.Enabled = true;
                button_doFindSpecify.Enabled = true;

                groupBox_SelectMode.Enabled = false;
                b_startSearch = true;

                this.checkDoFindButton();
            }
            else
            {
                FaceLibSearchMethods faceDetection;
                TestSelector.TryGetValue(keySelected, out faceDetection);
                faceDetection.fEndFind();

                button_startFind.Text = "StartSearch(开始)";
                button_doFindNext.Enabled = false;
                button_doFindPre.Enabled = false;
                button_doFindSpecify.Enabled = false;

                groupBox_SelectMode.Enabled = true;
                b_startSearch = false;
            }

        }

        /// <summary>
        /// Choose a method and do find next
        /// 选择一个方法再执行搜索下一批次
        /// </summary>
        private void button_doFindNext_Click(object sender, EventArgs e)
        {
            FaceLibSearchMethods faceDetection;
            TestSelector.TryGetValue(keySelected, out faceDetection);
            faceDetection.fDoFind(EmDoFindDirectType.NEXTBATCH);

            this.checkDoFindButton();
        }

        /// <summary>
        /// Choose a method and do find pre
        /// 选择一个方法再执行搜索上一批次
        /// </summary>
        private void button_doFindPre_Click(object sender, EventArgs e)
        {
            FaceLibSearchMethods faceDetection;
            TestSelector.TryGetValue(keySelected, out faceDetection);
            faceDetection.fDoFind(EmDoFindDirectType.PREBATCH);

            this.checkDoFindButton();
        }

        /// <summary>
        /// Choose a method and do find specify
        /// 选择一个方法再执行搜索指定起始序号批次
        /// </summary>
        private void button_doFindSpecify_Click(object sender, EventArgs e)
        {
            FaceLibSearchMethods faceDetection;
            TestSelector.TryGetValue(keySelected, out faceDetection);
            faceDetection.fDoFind(EmDoFindDirectType.SPECIFY);

            this.checkDoFindButton();
        }

        private void radioBox_historyLib_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBox_historyLib.Checked)
            {
                groupBox_historyLibParam.Enabled = true;
                keySelected = "Find_History";
            }
            else
            {
                groupBox_historyLibParam.Enabled = false;
                keySelected = "Find_Registry";
            }
        }

        private void radioBox_registerLib_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBox_registerLib.Checked)
            {
                groupBox_historyLibParam.Enabled = false;
                keySelected = "Find_Registry";
            }
            else
            {
                groupBox_historyLibParam.Enabled = true;
                keySelected = "Find_History";
            }
        }

        private void listView_searchInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {   // Show data 展示详细数据
                ListView.SelectedIndexCollection indexes = this.listView_searchInfo.SelectedIndices;
                if (indexes.Count == 1)
                {
                    int index = indexes[0];
                    if (index >= findOutData.nCadidateExNum)
                    {
                        this.displayCandidateInfoClear();
                    }
                    else
                    {
                        this.displayCandidateInfoDetail(index);
                    }
                }
            }
            catch (Exception ex)
            {
                label_info.Text = "详细数据获取失败！";
                MessageBox.Show(ex.Message);
            }
        }

        private void text_specifyIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
