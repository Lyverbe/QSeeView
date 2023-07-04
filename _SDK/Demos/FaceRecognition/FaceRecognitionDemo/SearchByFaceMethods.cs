using NetSDKCS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WinForm_IVS_FaceRecognition_Demo
{
    public partial class SearchByFacePicWindow : Form
    {
        /* Query variables and constants (查询相关变量与常量) */
        private IntPtr m_lFindHandle = IntPtr.Zero;  // Query handle (数据查询句柄)

        private const int MaxCount = 10;             // Maximum number for one batch of query data (一次获取查询数据的最大数量)
        private int searchBeginOrder = 0;            // The initial serial number of a given fetch of query data 查询数据获取的起始序号

        /*  Query of Results Set Preparation Progress （结果集进度查询） */
        private IntPtr m_nAttachFindStateHandle = IntPtr.Zero;    // The handle of results set preparation progress (结果集准备进度查询句柄)
        private Int32 m_nWaitToken = 0;                           // The token of result set (结果集令牌)（设备处于准备中的搜索集可能有多个，依靠这个令牌作区分）

        private int startFindprogress = 0;            // Ongoing progress of the preparation of results set (结果集准备进度（0-100）
        private int startFindTotalCount = 0;          // Number of query results obtained form result set (结果集获取的查询结果数量)
        private bool bFindFinishFlag = false;         // Is the result set ready or not (结果集是否准备完毕标志位)

        /* 资源数据 */
        private NET_OUT_DOFIND_FACERECONGNITION findOutData = new NET_OUT_DOFIND_FACERECONGNITION();  // Used to save the current batch of the query data (用于保存当前批次查询数据)
        public byte[] localImageData;                    // Local binary image data (本地的二进制图像数据)
        public byte[] candidateImageData;                // Candidate binary image data (候选人二进制图像数据)

        /* 查询方法注册相关 */
        private delegate void fFaceDetectionStartSearch();      // Prepare for query (准备查询)
        private delegate void fFaceDetectionDoFind(EmDoFindDirectType type);     // fetch a batch of query data (获取一组查询数据)
        private delegate void fFaceDetectionEndSearch();        // End the query (结束查询)

        /// <summary>
        /// Used to register a kind of query methods
        /// 用于统一注册查询方法集
        /// </summary>
        private class FaceLibSearchMethods
        {
            private string imgSaveFolder;

            public string ImgSaveFolder
            {
                get { return imgSaveFolder; }
                set { imgSaveFolder = value; }
            }

            public fFaceDetectionStartSearch fStartFind;
            public fFaceDetectionDoFind fDoFind;
            public fFaceDetectionEndSearch fEndFind;

            public FaceLibSearchMethods(string saveFolder,
                                 fFaceDetectionStartSearch fStartFind,
                                 fFaceDetectionDoFind fDoFind,
                                 fFaceDetectionEndSearch fEndFind)
            {
                this.imgSaveFolder = saveFolder;
                this.fStartFind = new fFaceDetectionStartSearch(fStartFind);
                this.fDoFind = new fFaceDetectionDoFind(fDoFind);
                this.fEndFind = new fFaceDetectionEndSearch(fEndFind);
            }
        }

        # region  Common methods (常用工具方法)

        /// <summary>
        /// 初始化句柄及flag等参数
        /// </summary>
        private void searchParamRefresh()
        {
            /* 结果集进度查询相关 */
            startFindprogress = 0;                   // Reset the progress value (清空进度条值)
            progressBar_startFind.Value = startFindprogress; // Empty the progress bar values (重置进度条)
            m_nAttachFindStateHandle = IntPtr.Zero;  // Reset the handle of preparation progress of results set (重置句柄)
            m_nWaitToken = 0;                        // Reset the token (重置令牌)
            startFindTotalCount = 0;                 // Reset the number of query data (重置数量)
            bFindFinishFlag = false;                 // Reset the flag of the preparation of results set (重置标志位)

            /* 重置查询相关 */
            this.listView_searchInfo.Items.Clear();   // Empty the dsiplay list (清空列表)
            m_lFindHandle = IntPtr.Zero;              // Empty the handle of query (清空查询句柄)
            searchBeginOrder = 0;                     // Reset the initial serial number (重置起始序号)
        }

        /// <summary>
        /// Check the status of PreBatch and NextBatch
        /// 检查 PreBatch 和 NextBatch 的状态
        /// </summary>
        private void checkDoFindButton()
        {

            if (searchBeginOrder >= startFindTotalCount)
            {
                button_doFindNext.Enabled = false;  // no next doFindNext
            }
            else
            {
                button_doFindNext.Enabled = true;
            }

            if (searchBeginOrder - findOutData.nCadidateExNum <= 0)
            {
                button_doFindPre.Enabled = false;  // no next doFindPre
            }
            else
            {
                button_doFindPre.Enabled = true;
            }
        }

        /// <summary>
        /// constructing arrays in the struct. this method can make sure the array's size is right
        /// 初始化结构体，可以自动 new 数组
        /// </summary>
        /// <param name="stu">structure 结构体</param>
        private void InitStruct(ref object stu)
        {
            IntPtr p_stu = IntPtr.Zero;
            Type type = stu.GetType();
            try
            {
                p_stu = Marshal.AllocHGlobal(Marshal.SizeOf(type));
                Marshal.StructureToPtr(stu, p_stu, true);
                stu = Marshal.PtrToStructure(p_stu, type);
            }
            finally
            {
                Marshal.FreeHGlobal(p_stu);
            }
        }

        /// <summary>
        /// Get the size of file
        /// 获取文件大小
        /// </summary>
        /// <param name="sFilePath">file name 文件名</param>
        /// <returns>The size of file 文件大小</returns>
        private UInt32 FileSize(string sFilePath)
        {
            if (false == File.Exists(sFilePath))
            {
                return 0;
            }

            FileInfo fileInfo = new FileInfo(sFilePath);
            return Convert.ToUInt32(fileInfo.Length);
        }

        /// <summary>
        /// Read file with binary bytes
        /// 读取文件为二进制字节
        /// </summary>
        /// <param name="sFilePath">file name 文件名</param>
        /// <returns>binary byte array 二进制字节数组</returns>
        private byte[] FileDataRead(string sFilePath)
        {
            if (false == File.Exists(sFilePath))
            {
                return null;
            }

            return File.ReadAllBytes(sFilePath);
        }

        #endregion

        #region Query for the preparation of results set (结果集查询)

        /// <summary>
        /// The callback function of the query of result set preparation progress
        /// 结果集准备进度查询回调函数
        /// </summary>
        /// <param name="lLoginID">Logon handle 登录句柄</param>
        /// <param name="lAttachHandle"> Handle of preparation progress of results set 搜索结果集准备进度查询句柄</param>
        /// <param name="pstStates"> Pointer of preparation progress data 准备进度数据指针</param>
        /// <param name="nStateNum"> Number of the queries the device is preparing 准备中的结果集数量</param>
        /// <param name="dwUser"> User data 用户数据</param>
        private void FaceFindStateCB(IntPtr lLoginID, IntPtr lAttachHandle, IntPtr pstStates, Int32 nStateNum, UInt32 dwUser)
        {
            if (lAttachHandle == m_nAttachFindStateHandle)       // Match handle (匹配句柄)
            {
                NET_CB_FACE_FIND_STATE[] sTmp = new NET_CB_FACE_FIND_STATE[nStateNum];   // Get all the result set preparation states 拿到设备所有的结果集准备信息
                for (Int32 i = 0; i < nStateNum; ++i)
                {
                    sTmp[i] = (NET_CB_FACE_FIND_STATE)Marshal.PtrToStructure(pstStates + i * Marshal.SizeOf(typeof(NET_CB_FACE_FIND_STATE)), typeof(NET_CB_FACE_FIND_STATE));
                    if (m_nWaitToken == sTmp[i].nToken)         // Match token (匹配结果当前的结果集准备令牌)
                    {
                        startFindprogress = sTmp[i].nProgress;  // Update progress (更新结果集准备进度)
                        Console.WriteLine("FindState:{0}", startFindprogress);
                        startFindTotalCount = sTmp[i].nCurrentCount; // Update the number of query data (更新结果集获取数量进度)
                        Console.WriteLine("current finding percent is [{0}%], found [{1}] now", sTmp[i].nProgress, sTmp[i].nCurrentCount);
                        if (100 == sTmp[i].nProgress)           // Progress reaches 100 indicates the query has completed 如果进度已到达100%，说明查询完毕
                        {
                            bFindFinishFlag = true;
                            break;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Waiting for the device to complete the preparation of result set asynchronously
        /// 等待设备异步完成结果集准备
        /// </summary>
        /// <param name="nToken"> Token 结果集准备的识别令牌</param>
        private void WaitFindPrepareSucceed(Int32 nToken)
        {
            m_nWaitToken = nToken;

            /* In Param 结果集准备进度查询 入参 */
            NET_IN_FACE_FIND_STATE stInParam = new NET_IN_FACE_FIND_STATE();
            stInParam.dwSize = (uint)(Marshal.SizeOf(typeof(NET_IN_FACE_FIND_STATE)));
            stInParam.cbFaceFindState = new fFaceFindStateCallBack(FaceFindStateCB);    // Register callback function (注册结果集准备进度查询回调)
            stInParam.nTokenNum = 1;
            Int32[] iTmp = new Int32[stInParam.nTokenNum];
            iTmp[0] = nToken;          // Fill in token (填写令牌)

            stInParam.nTokens = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Int32)) * stInParam.nTokenNum);
            for (Int32 i = 0; i < stInParam.nTokenNum; ++i)
            {
                Marshal.StructureToPtr(iTmp[i], stInParam.nTokens + i * Marshal.SizeOf(typeof(Int32)), true);
            }

            /* Out Param 结果集准备进度查询 出参 */
            NET_OUT_FACE_FIND_STATE stOutParam = new NET_OUT_FACE_FIND_STATE();
            stOutParam.dwSize = Convert.ToUInt32(Marshal.SizeOf(typeof(NET_OUT_FACE_FIND_STATE)));

            /* Implementation 查询结果集准备进度 */
            m_nAttachFindStateHandle = NETClient.AttachFaceFindState(m_nLoginID, ref stInParam, ref stOutParam, 2000);
            if (m_nAttachFindStateHandle != IntPtr.Zero)
            {
                Console.WriteLine("finding state attach succeed (人脸结果集准备进度查询订阅成功)");
                label_info.Text = "人脸结果集准备进度查询订阅成功\n\nfinding state attach succeed";
            }
            else
            {
                Console.WriteLine("finding state attach failed (人脸结果集准备进度查询订阅失败)");
                label_info.Text = "人脸结果集准备进度查询订阅失败\n\nfinding state attach failed";
                return;
            }

            // Block, wait for query progress to 100% (阻塞，等待查询进度到达100%)
            while (!bFindFinishFlag)
            {
                progressBar_startFind.Value = startFindprogress;
                Thread.Sleep(100);    // Update progress bar for every 100ms (100ms更新一次进度条)
            }
            progressBar_startFind.Value = startFindprogress;
            label_info.Text = String.Format("current finding percent is [{0}%]\n\nfound [{1}] now", startFindprogress, startFindTotalCount);

            bFindFinishFlag = false;
            NETClient.DetachFaceFindState(m_nAttachFindStateHandle);  // Unsubscribe (退订结果集查询)
        }

        #endregion

        #region SDK Toolkits (SDK工具方法)

        /// <summary>
        /// Download image from device
        /// 从设备下载图片
        /// </summary>
        /// <param name="remoteFile"> Remote image path 远程设备中的图片地址 </param>
        /// <param name="storeFile"> Local save path 本地保存路径 </param>
        /// <returns> Download succeed or failed 是否成功 </returns>
        private bool DownloadRemoteFile(string remoteFile, string storeFile)
        {
            IntPtr pSrc = IntPtr.Zero;
            IntPtr pStoreFile = IntPtr.Zero;
            try
            {
                pSrc = Marshal.StringToHGlobalAnsi(remoteFile);
                pStoreFile = Marshal.StringToHGlobalAnsi(storeFile);
                NET_IN_DOWNLOAD_REMOTE_FILE stuIn = new NET_IN_DOWNLOAD_REMOTE_FILE();
                stuIn.dwSize = (uint)Marshal.SizeOf(stuIn);
                stuIn.pszFileDst = pStoreFile;
                stuIn.pszFileName = pSrc;
                if (NETClient.DownloadRemoteFile(m_nLoginID, stuIn, 3000))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }

        /// <summary>
        /// Analyse the enumeration of ID types
        /// 获取证件类型
        /// </summary>
        /// <param name="type"> Enumeration of ID types 枚举类型 </param>
        /// <returns> Description of ID types 枚举描述 </returns>
        private string ParseIdType(EM_CERTIFICATE_TYPE type)
        {
            string typeStr;
            switch (type)
            {
                case EM_CERTIFICATE_TYPE.UNKNOWN:
                    typeStr = "Unknown(未知)";
                    break;
                case EM_CERTIFICATE_TYPE.PASSPORT:
                    typeStr = "Passport(护照)";
                    break;
                case EM_CERTIFICATE_TYPE.IC:
                    typeStr = "IC(身份证)";
                    break;
                case EM_CERTIFICATE_TYPE.OffICER_CARD:
                    typeStr = "OfficerCard(军官证)";
                    break;
                default:
                    typeStr = "Unknown(未知)";
                    break;
            }
            return typeStr;
        }

        /// <summary>
        /// Analyse the enumeration of sex types
        /// 获取性别类型
        /// </summary>
        /// <param name="type"> Enumeration of sex types 枚举类型 </param>
        /// <returns> Description of sex types 枚举描述 </returns>
        private string ParseSexType(byte type)
        {
            string typeStr;
            switch (type)
            {
                case 1:
                    typeStr = "male(男性)";
                    break;
                case 2:
                    typeStr = "female(女性)";
                    break;
                default:
                    typeStr = "Unknown(未知)";
                    break;
            }
            return typeStr;

        }

        /// <summary>
        /// Set the starting serial number of DoFind 
        /// 设置DoFind起始序号
        /// </summary>
        /// <param name="directType">fetch direction 获取数据方向(前一批、后一批、自定义)</param>
        private bool setSearchBeginIndex(EmDoFindDirectType directType)
        {
            switch (directType)
            {
                case EmDoFindDirectType.PREBATCH:
                    searchBeginOrder = searchBeginOrder - findOutData.nCadidateExNum - MaxCount;
                    if (searchBeginOrder < 0)
                    {
                        MessageBox.Show("Can not generate a max size batch, the start index will be set with 1.");
                        searchBeginOrder = 0;
                    }
                    break;
                case EmDoFindDirectType.SPECIFY:
                    if (text_specifyIndex.Text == null || text_specifyIndex.Text == "")
                    {
                        MessageBox.Show("Please input an index first!");
                        return false;
                    }
                    int specifyIndex = Int32.Parse(text_specifyIndex.Text);
                    if (specifyIndex > startFindTotalCount || specifyIndex < 1)
                    {
                        MessageBox.Show("Index out of range!");
                        return false;
                    }
                    else
                    {
                        searchBeginOrder = specifyIndex - 1;
                    }
                    break;
                default: break;
            }
            return true;
        }

        /// <summary>
        /// Add a new row of display data
        /// 添加一行新的展示数据
        /// </summary>
        /// <param name="stuFindOut"> Total information of candidates fetched in this batch 本批次查询到的候选人总信息/param>
        /// <param name="i">The ith candidate 第i个候选人</param>
        private void DisplayPersonInfo(NET_OUT_DOFIND_FACERECONGNITION stuFindOut, int i)
        {
            ListViewItem listViewItem = new ListViewItem();

            listViewItem.Text = Convert.ToString(searchBeginOrder + i + 1);                            // index

            listViewItem.SubItems.Add(stuFindOut.stuCandidatesEx[i].stPersonInfo.szUID);               // UID
            listViewItem.SubItems.Add(stuFindOut.stuCandidatesEx[i].stPersonInfo.szPersonName);        // Name
            listViewItem.SubItems.Add(ParseSexType(stuFindOut.stuCandidatesEx[i].stPersonInfo.bySex)); // Sex
            listViewItem.SubItems.Add(String.Format("{0}/{1}/{2}",                                     // birthday
                stuFindOut.stuCandidatesEx[i].stPersonInfo.wYear,
                stuFindOut.stuCandidatesEx[i].stPersonInfo.byMonth,
                stuFindOut.stuCandidatesEx[i].stPersonInfo.byDay));
            listViewItem.SubItems.Add(ParseIdType((EM_CERTIFICATE_TYPE)stuFindOut.stuCandidatesEx[i].stPersonInfo.byIDType));  // ID Type
            listViewItem.SubItems.Add(stuFindOut.stuCandidatesEx[i].stPersonInfo.szID);                 // ID Number
            listViewItem.SubItems.Add(Convert.ToString(stuFindOut.stuCandidatesEx[i].bySimilarity));    // Similarity

            listView_searchInfo.BeginUpdate();
            listView_searchInfo.Items.Add(listViewItem);
            listView_searchInfo.EndUpdate();

        }

        /// <summary>
        /// Release pointer within data fetched before
        /// 释放上一批次保存的数据
        /// </summary>
        private void freeDoFindFaceReconData()
        {

            if (findOutData.pBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(findOutData.pBuffer);    // 释放图片指针
                findOutData.pBuffer = IntPtr.Zero;
            }

            if (findOutData.stuCandidatesEx != null)         // 释放字符串指针
            {
                for (int i = 0; i < findOutData.stuCandidatesEx.Length; i++)
                {
                    for (int j = 0; j < findOutData.stuCandidatesEx[i].stPersonInfo.szFacePicInfo.Length; j++)
                    {
                        Marshal.FreeHGlobal(findOutData.stuCandidatesEx[i].stPersonInfo.szFacePicInfo[j].pszFilePath);
                    }
                }
                findOutData.stuCandidatesEx = null;
            }
        }

        /// <summary>
        /// Display the ith candidate data
        /// 展示本批次数据中的第i个候选人数据
        /// </summary>
        /// <param name="i"> The ith data 第i个数据 </param>
        private void displayCandidateInfoDetail(int i)
        {
            NET_CANDIDATE_INFOEX candidateInfo = findOutData.stuCandidatesEx[i];

            // Refresh data (更新数据)
            label_candidate_UID.Text = candidateInfo.stPersonInfo.szUID;
            label_candidate_name.Text = candidateInfo.stPersonInfo.szPersonName;
            label_candidate_sex.Text = ParseSexType(candidateInfo.stPersonInfo.bySex);
            label_candidate_birthday.Text = String.Format("{0}/{1}/{2}",
                candidateInfo.stPersonInfo.wYear, candidateInfo.stPersonInfo.byMonth, candidateInfo.stPersonInfo.byDay);
            label_candidate_type.Text = ParseIdType((EM_CERTIFICATE_TYPE)candidateInfo.stPersonInfo.byIDType);
            label_candidate_id.Text = candidateInfo.stPersonInfo.szID;
            label_candidate_groupid.Text = candidateInfo.stPersonInfo.szGroupID;
            label_candidate_groupname.Text = candidateInfo.stPersonInfo.szGroupName;
            label_candidate_similarity.Text = Convert.ToString(candidateInfo.bySimilarity);

            // IVS device query does not always contain binary image data, so it is best to download them from device, here we only download the first image
            // IVS设备查询不一定会包含二进制图片数据，所以最好从远程下载图片，demo里只下载第一张图
            if (candidateInfo.stPersonInfo.wFacePicNum > 0)
            {
                string remotePath = Marshal.PtrToStringAnsi(candidateInfo.stPersonInfo.szFacePicInfo[0].pszFilePath);

                Console.WriteLine("fetch remote image from\n{0}", remotePath);
                Console.WriteLine("downloading and saving, please wait......");
                label_info.Text = "图片正在下载\n\ndownloading and saving, please wait......";

                FaceLibSearchMethods faceDetection;
                TestSelector.TryGetValue(keySelected, out faceDetection);
                string savePath = faceDetection.ImgSaveFolder + remotePath.Split('/')[remotePath.Split('/').Length - 1];
                if (DownloadRemoteFile(remotePath, savePath))  // Download and save
                {
                    label_info.Text = "图片保存成功\n\nimg saved successfully";
                    Console.WriteLine("img[{0}]saved successfully", savePath);
                    // Then read image as binary data (再从本地读取出来)
                    try
                    {
                        string path = savePath;
                        localImageData = File.ReadAllBytes(path);
                        using (MemoryStream stream = new MemoryStream(localImageData))
                        {
                            Image image = Image.FromStream(stream);
                            this.pictureBox_candidateFace.Image = image;
                            this.pictureBox_candidateFace.Refresh();
                            this.pictureBox_candidateFace.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                }
                else
                {
                    label_info.Text = "图片保存失败\n\nimg save failed";
                    Console.WriteLine("save failed");
                }
            }
        }

        /// <summary>
        /// Clear the candidate information and image
        /// 清除候选人信息和图片显示
        /// </summary>
        private void displayCandidateInfoClear()
        {
            label_candidate_UID.Text = "";
            label_candidate_name.Text = "";
            label_candidate_sex.Text = "";
            label_candidate_birthday.Text = "";
            label_candidate_type.Text = "";
            label_candidate_id.Text = "";
            label_candidate_groupid.Text = "";
            label_candidate_groupname.Text = "";
            label_candidate_similarity.Text = "";

            this.candidateImageData = null;
            this.pictureBox_candidateFace.Image = null;
            this.pictureBox_candidateFace.Refresh();
            this.pictureBox_candidateFace.Visible = true;
        }

        # endregion

        # region find record in registry DB (查询注册库)

        /// <summary>
        ///  Start the search of registry DB
        /// 开始搜索注册库，获取查询句柄并等待设备结果集准备完毕
        /// </summary>
        private void FaceFindInRegistryDBStartFind()
        {
            searchParamRefresh();  // 重置查询参数 (Reset Query Related Parameters)

            ///////////////////////////// StartFind Face Recon InParam ///////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////

            NET_IN_STARTFIND_FACERECONGNITION stuInParam = new NET_IN_STARTFIND_FACERECONGNITION();
            object obj = stuInParam;
            InitStruct(ref obj);

            stuInParam = (NET_IN_STARTFIND_FACERECONGNITION)obj;
            // dwSize
            stuInParam.dwSize = (uint)(Marshal.SizeOf(typeof(NET_IN_STARTFIND_FACERECONGNITION)));
            // Init personal info
            stuInParam.bPersonEnable = true;

            // Set local contrast face image 添加本地人脸图片的信息
            stuInParam.stPerson.wFacePicNum = 1;
            stuInParam.stPerson.szFacePicInfo[0].dwFileLenth = (uint)localImageData.Length;
            stuInParam.stPerson.szFacePicInfo[0].dwOffSet = 0;
            stuInParam.nBufferLen = Convert.ToInt32(localImageData.Length);
            stuInParam.pBuffer = Marshal.AllocHGlobal(stuInParam.nBufferLen);
            Marshal.Copy(localImageData, 0, stuInParam.pBuffer, stuInParam.nBufferLen);

            // -> Init match option (设置匹配选择)
            stuInParam.stMatchOptions.dwSize = (uint)(Marshal.SizeOf(typeof(NET_FACE_MATCH_OPTIONS)));
            stuInParam.stMatchOptions.nMaxCandidate = 20;    // 部分设备可能无效  maybe invalid on some devices

            // -> input similarity (0-100) (输入相似度(0-100))
            stuInParam.stMatchOptions.nSimilarity = Int32.Parse(textBox_similarity.Text.Trim());

            // -> Init filter (设置过滤条件)
            stuInParam.stFilterInfo.dwSize = (uint)(Marshal.SizeOf(typeof(NET_FACE_FILTER_CONDTION)));
            stuInParam.stFilterInfo.nRangeNum = 1;
            // database, do not change (数据库类型写死就是这个，不要修改)
            stuInParam.stFilterInfo.szRange[0] = Convert.ToByte(EM_FACE_DB_TYPE.BLACKLIST);

            /////////////////////////////// StartFind Face Recon OutParam ////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////

            NET_OUT_STARTFIND_FACERECONGNITION stuOutParam = new NET_OUT_STARTFIND_FACERECONGNITION();
            obj = stuOutParam;
            InitStruct(ref obj);
            stuOutParam = (NET_OUT_STARTFIND_FACERECONGNITION)obj;

            stuOutParam.dwSize = (uint)(Marshal.SizeOf(typeof(NET_OUT_STARTFIND_FACERECONGNITION)));

            /////////////////////////////// StartFind Face Recon /////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////
            // Requires equipment to prepare result sets (要求设备准备结果集)
            if (!NETClient.StartFindFaceRecognition(m_nLoginID, ref stuInParam, ref stuOutParam, 2000))
            {
                Console.WriteLine("Start Find Operation failed!");
                label_info.Text = "开始查询失败\n\nStart Find Operation failed!";
                Marshal.FreeHGlobal(stuInParam.pBuffer);
                return;
            }
            // Waiting for equipment to be ready (等待设备准备完成)
            WaitFindPrepareSucceed(stuOutParam.nToken);
            // Refresh query handle 跟新查询句柄
            m_lFindHandle = stuOutParam.lFindHandle;

            Console.WriteLine("Query Started(开始查询)\n\nHandel(句柄)：{0},TotalCount(总数):{1}", m_lFindHandle, startFindTotalCount);
            label_info.Text = String.Format("Query Started(开始查询)\n\nHandel(句柄)：{0},TotalCount(总数):{1}", m_lFindHandle, startFindTotalCount);
        }

        /// <summary>
        /// Fetch query data from device
        /// 从设备顺序取搜索结果
        /// </summary>
        private void FaceFindInRegistryDBDoFind(EmDoFindDirectType directType)
        {
            //////////////////////////////// 依据查询类型设置起始 index//////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            if (!this.setSearchBeginIndex(directType)) return;

            /////////////////////////////// Clear Stored Data ////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////

            this.listView_searchInfo.Items.Clear();    // Clear display list (清空展示列表)
            this.freeDoFindFaceReconData();            // Release the pointer of the previous batch of data (释放上一次保存的查询数据内的指针数据)

            /////////////////////////////// DoFind Face Recon InParam ////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////

            NET_IN_DOFIND_FACERECONGNITION stuFindIn = new NET_IN_DOFIND_FACERECONGNITION();
            stuFindIn.dwSize = (uint)(Marshal.SizeOf(typeof(NET_IN_DOFIND_FACERECONGNITION)));

            stuFindIn.lFindHandle = this.m_lFindHandle;
            stuFindIn.nCount = MaxCount;                             // The max count of query data in a single fetch request (每次最多获取MaxCount个数据)
            stuFindIn.nBeginNum = searchBeginOrder;                  // The initial sequence number of query data 获取到的数据的起始序号

            /////////////////////////////// DoFind Face Recon OutParam ////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////

            NET_OUT_DOFIND_FACERECONGNITION stuFindOut = new NET_OUT_DOFIND_FACERECONGNITION();
            Object obj = stuFindOut;
            InitStruct(ref obj);
            stuFindOut = (NET_OUT_DOFIND_FACERECONGNITION)obj;
            stuFindOut.dwSize = (uint)(Marshal.SizeOf(typeof(NET_OUT_DOFIND_FACERECONGNITION)));
            stuFindOut.bUseCandidatesEx = true;

            // 重要，由于 C++ 中 pszFilePath 为指针形式，所以用户需要提前给new好空间，C++才能把字符串写进入；后面还得记得手动回收
            // The type of pszFilePath is char * within C++, which means user need to alloc its space manually
            for (int i = 0; i < stuFindOut.stuCandidatesEx.Length; i++)
            {
                for (int j = 0; j < stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo.Length; j++)
                {
                    stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo[j].pszFilePath = Marshal.AllocHGlobal(1000);
                    stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo[j].nFilePathLen = 1000;
                }
            }

            /////////////////////////////// DoFind Face Recon /////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////

            if (NETClient.DoFindFaceRecognition(ref stuFindIn, ref stuFindOut, 2000))
            {
                Console.WriteLine("Find Count(查到数据数):{0}\n\nTotal Count(数据总数):{1}", stuFindOut.nCadidateNum, startFindTotalCount);
                label_info.Text = string.Format("Find Count(查到数据数):{0}\n\nTotal Count(数据总数):{1}", stuFindOut.nCadidateNum, startFindTotalCount);

                for (int i = 0; i < stuFindOut.nCadidateExNum; i++)
                {
                    // display the ith candidate in console
                    Console.WriteLine("index [{0}]", i);
                    Console.WriteLine("UID [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szUID);
                    Console.WriteLine("Name [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szPersonName);
                    Console.WriteLine("ID NO. [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szID);
                    Console.WriteLine("Group ID [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szGroupID);
                    Console.WriteLine("Similarity. [{0}]", stuFindOut.stuCandidatesEx[i].bySimilarity);

                    // display the ith candidate in DataGridView (展示第i个候选人数据)
                    DisplayPersonInfo(stuFindOut, i);
                }

                if (0 == stuFindOut.nCadidateNum)
                {
                    Console.WriteLine("Can not find any candidate");
                    label_info.Text = "没有任何候选人\n\nCan not find any candidate";
                }
                searchBeginOrder += stuFindOut.nCadidateExNum;

                this.findOutData = stuFindOut;       // Replacement of saved data (更换保存的数据)
            }
            else
            {
                Console.WriteLine("Do Find Operation failure!");
                label_info.Text = "获取查询数据失败\n\nDo Find Operation failure!";
            }
        }

        /// <summary>
        /// End the search of registry DB
        /// 结束搜索注册库
        /// </summary>
        private void FaceFindInRegistryDBEndSearch()
        {
            if (!NETClient.StopFindFaceRecognition(this.m_lFindHandle))
            {
                MessageBox.Show("Stop Find Operation failed!");
                label_info.Text = "结束查询失败\n\nStop Find Operation failed!";
                return;
            }
            this.listView_searchInfo.Items.Clear();    // Clear display list (清空展示列表)
            this.freeDoFindFaceReconData();            // Release the pointers (释放指针数据)
            this.displayCandidateInfoClear();          // Clear the candidate information (清理展示数据)
            this.searchParamRefresh();                 // Reset the handles (句柄等信息重置)
            Console.WriteLine("Query Ended(查询结束)");
            label_info.Text = string.Format("Query Ended(查询结束)");
        }

        # endregion

        # region find record in History DB

        /// <summary>
        /// 开始搜索历史库，获取查询句柄并等待设备结果集准备完毕
        /// </summary>
        private void FaceFindInHistoryStartFind()
        {
            searchParamRefresh();       // 重置查询参数 (Reset Query Related Parameters)

            ///////////////////////////// StartFind Face Recon InParam ///////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////

            NET_IN_STARTFIND_FACERECONGNITION stuInParam = new NET_IN_STARTFIND_FACERECONGNITION();
            object obj = stuInParam;
            InitStruct(ref obj);
            stuInParam = (NET_IN_STARTFIND_FACERECONGNITION)obj;
            // dwSize
            stuInParam.dwSize = (uint)(Marshal.SizeOf(typeof(NET_IN_STARTFIND_FACERECONGNITION)));
            //Init personal info
            stuInParam.bPersonEnable = true;

            // Get local contrast face image (获取人脸图)
            stuInParam.stPerson.wFacePicNum = 1;
            stuInParam.stPerson.szFacePicInfo[0].dwFileLenth = (uint)localImageData.Length;
            stuInParam.stPerson.szFacePicInfo[0].dwOffSet = 0;
            stuInParam.nBufferLen = Convert.ToInt32(localImageData.Length);
            stuInParam.pBuffer = Marshal.AllocHGlobal(stuInParam.nBufferLen);
            Marshal.Copy(localImageData, 0, stuInParam.pBuffer, stuInParam.nBufferLen);

            // Init match option (设置匹配选择)
            stuInParam.stMatchOptions.dwSize = (uint)(Marshal.SizeOf(typeof(NET_FACE_MATCH_OPTIONS)));
            stuInParam.stMatchOptions.nMaxCandidate = 10;       // maybe invalid on some devices 部分设备可能无效  
            // Input Similarity (0-100) (输入相似度(0-100))
            stuInParam.stMatchOptions.nSimilarity = int.Parse(textBox_similarity.Text.Trim());

            // Init Filter (设置过滤条件)
            stuInParam.stFilterInfo.dwSize = (uint)(Marshal.SizeOf(typeof(NET_FACE_FILTER_CONDTION)));

            stuInParam.stFilterInfo.nRangeNum = 1;
            // database, do not change (数据库类型写死就是这个，不要修改)
            stuInParam.stFilterInfo.szRange[0] = Convert.ToByte(EM_FACE_DB_TYPE.HISTORY);

            // Set query time (查询时间段)
            stuInParam.stFilterInfo.stStartTime = NET_TIME.FromDateTime(dateTimePicker_start.Value);
            stuInParam.stFilterInfo.stEndTime = NET_TIME.FromDateTime(dateTimePicker_end.Value);

            // channel (start from 0) 输入通道号(0开始)
            stuInParam.nChannelID = comboBox_channelPic.SelectedIndex;

            /////////////////////////////// StartFind Face Recon OutParam ////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////

            NET_OUT_STARTFIND_FACERECONGNITION stuOutParam = new NET_OUT_STARTFIND_FACERECONGNITION();
            obj = stuOutParam;
            InitStruct(ref obj);
            stuOutParam = (NET_OUT_STARTFIND_FACERECONGNITION)obj;

            stuOutParam.dwSize = (uint)(Marshal.SizeOf(typeof(NET_OUT_STARTFIND_FACERECONGNITION)));

            /////////////////////////////// StartFind Face Recon /////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////
            // Requires equipment to prepare result sets (要求设备准备结果集)
            if (!NETClient.StartFindFaceRecognition(m_nLoginID, ref stuInParam, ref stuOutParam, 2000))
            {
                Console.WriteLine("Operation failure!");
                Marshal.FreeHGlobal(stuInParam.pBuffer);
                return;
            }

            // Waiting for equipment to be ready (等待设备准备完成)
            WaitFindPrepareSucceed(stuOutParam.nToken);
            // Refresh query handle 跟新查询句柄
            this.m_lFindHandle = stuOutParam.lFindHandle;

            Console.WriteLine("Query Started(开始查询)\n\nHandel(句柄)：{0},TotalCount(总数):{1}", m_lFindHandle, startFindTotalCount);
            label_info.Text = String.Format("Query Started(开始查询)\n\nHandel(句柄)：{0},TotalCount(总数):{1}", m_lFindHandle, startFindTotalCount);
        }

        /// <summary>
        /// Fetch query results orderly
        /// 从设备顺序取搜索结果
        /// </summary>
        /// <param name="directType"> Fetch type 获取方式 </param>
        private void FaceFindInHistoryDoFind(EmDoFindDirectType directType)
        {
            //////////////////////////////// 依据查询类型设置起始 index//////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////

            if (!this.setSearchBeginIndex(directType)) return;

            /////////////////////////////// Clear Stored Data ////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////

            this.listView_searchInfo.Items.Clear();    // Clear display list (清空展示列表)
            this.freeDoFindFaceReconData();            // Release the pointer of the previous batch of data (释放上一次保存的查询数据内的指针数据)

            /////////////////////////////// DoFind Face Recon InParam ////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////

            NET_IN_DOFIND_FACERECONGNITION stuFindIn = new NET_IN_DOFIND_FACERECONGNITION();
            stuFindIn.dwSize = (uint)(Marshal.SizeOf(typeof(NET_IN_DOFIND_FACERECONGNITION)));

            stuFindIn.lFindHandle = this.m_lFindHandle;
            stuFindIn.nCount = MaxCount;                             // The max count of query data in a single fetch request (每次最多获取MaxCount个数据)
            stuFindIn.nBeginNum = searchBeginOrder;                  // The initial sequence number of query data 获取到的数据的起始序号

            /////////////////////////////// DoFind Face Recon OutParam ////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////

            NET_OUT_DOFIND_FACERECONGNITION stuFindOut = new NET_OUT_DOFIND_FACERECONGNITION();
            object obj = stuFindOut;
            InitStruct(ref obj);
            stuFindOut = (NET_OUT_DOFIND_FACERECONGNITION)obj;
            stuFindOut.dwSize = (uint)(Marshal.SizeOf(typeof(NET_OUT_DOFIND_FACERECONGNITION)));
            stuFindOut.bUseCandidatesEx = true;

            // 重要，由于C++中pszFilePath为指针形式，所以用户需要提前给new好空间，C++才能把字符串写进入；后面还得记得手动回收
            // in C++, pszFilePath is a char *, user need to alloc space to store ansi string
            for (int i = 0; i < stuFindOut.stuCandidatesEx.Length; i++)
            {
                for (int j = 0; j < stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo.Length; j++)
                {
                    stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo[j].pszFilePath = Marshal.AllocHGlobal(1000);
                    stuFindOut.stuCandidatesEx[i].stPersonInfo.szFacePicInfo[j].nFilePathLen = 1000;
                }
            }

            /////////////////////////////// DoFind Face Recon /////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////

            if (NETClient.DoFindFaceRecognition(ref stuFindIn, ref stuFindOut, 2000))
            {
                Console.WriteLine("Find Count(查到数据数):{0}\n\nTotal Count(数据总数):{1}", stuFindOut.nCadidateNum, startFindTotalCount);
                label_info.Text = string.Format("Find Count(查到数据数):{0}\n\nTotal Count(数据总数):{1}", stuFindOut.nCadidateNum, startFindTotalCount);

                for (int i = 0; i < stuFindOut.nCadidateExNum; i++)
                {
                    Console.WriteLine("index [{0}]", i);
                    Console.WriteLine("UID [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szUID);
                    Console.WriteLine("Name [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szPersonName);
                    Console.WriteLine("ID NO. [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szID);
                    Console.WriteLine("Group ID [{0}]", stuFindOut.stuCandidatesEx[i].stPersonInfo.szGroupID);
                    Console.WriteLine("Similarity. [{0}]", stuFindOut.stuCandidatesEx[i].bySimilarity);

                    // display the ith candidate in DataGridView (展示第i个候选人数据)
                    DisplayPersonInfo(stuFindOut, i);
                }

                if (0 == stuFindOut.nCadidateNum)
                {
                    Console.WriteLine("Didn't find any person");
                    label_info.Text = "没有任何候选人\n\nCan not find any person";
                }
                searchBeginOrder += stuFindOut.nCadidateExNum;

                this.findOutData = stuFindOut;      // Replacement of saved data (更换保存的数据)
            }
            else
            {
                Console.WriteLine("Do Find Operation failure!");
                label_info.Text = "获取查询数据失败\n\nDo Find Operation failure!";
            }
        }

        /// <summary>
        /// End the search of history library
        /// 结束搜索历史库
        /// </summary>
        private void FaceFindInHistoryEndSearch()
        {
            if (!NETClient.StopFindFaceRecognition(this.m_lFindHandle))
            {
                MessageBox.Show("Stop Find Operation failure!");
                label_info.Text = "Stop Find Operation failure!";
                return;
            }
            this.listView_searchInfo.Items.Clear();    // Clear display list (清空展示列表)
            this.freeDoFindFaceReconData();            // Release the pointers (释放指针数据)
            this.displayCandidateInfoClear();          // Clear the candidate information (清理展示数据)
            this.searchParamRefresh();                 // Reset the handles (句柄等信息重置)

            Console.WriteLine("Query Ended(查询结束)");
            label_info.Text = string.Format("Query Ended(查询结束)");
        }

        # endregion
    }
}