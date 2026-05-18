using NetSDKCS;
using QSeeView.Models;
using QSeeView.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace QSeeView.Tools
{
    public class QCW4DeviceManager : IDeviceManager
    {
        public event EventHandler<string> DownloadCompleted;
        public event EventHandler<IntPtr> PlaybackCompleted;

        private bool _isInitialized;
        private bool _downloadAbort;
        private static fDownLoadPosCallBack _downloadPosCallBack;
        private static fDownLoadPosCallBack _playbackPosCallBack;

        public QCW4DeviceManager()
        {
            _isInitialized = OriginalSDK.CLIENT_InitEx(DisconnectCallBack, IntPtr.Zero, IntPtr.Zero);
            _downloadPosCallBack = new fDownLoadPosCallBack(DownloadPosCallback);
            _playbackPosCallBack = new fDownLoadPosCallBack(PlaybackPosCallback);
        }

        public DeviceModelType DeviceModelType => DeviceModelType.QCW4;
        public bool IsConnected => _isInitialized && LoginId != IntPtr.Zero;
        public IntPtr LoginId { get; private set;  }
        public int MaxQueryRecords => 5000;
        public int ChannelsCount => 4;
        public RecordFileInfoModel DownloadRecord { get; private set; }
        public long DownloadId { get; private set; }

        public string GetLastError() => NETClient.GetLastError();

        public void DisconnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
        }

        /// <summary>
        /// Logs the user into the device
        /// </summary>
        public void Login(string deviceIp, ushort devicePort, string username, string password)
        {
            LoginId = IntPtr.Zero;
            if (!_isInitialized)
                return;

            var deviceInfo = new NET_DEVICEINFO_Ex();
            int errorCode = 0;
            LoginId = OriginalSDK.CLIENT_LoginEx2(deviceIp, devicePort, username, password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo, ref errorCode);
        }

        /// <summary>
        /// Logs out the user and shuts down the connection
        /// </summary>
        public void Shutdown()
        {
            OriginalSDK.CLIENT_Logout(LoginId);
            OriginalSDK.CLIENT_Cleanup();
        }

        /// <summary>
        /// Performs a record query
        /// </summary>
        public IList<RecordFileInfoModel> Query(DateTime startTime, DateTime endTime)
        {
            var queryStartTime = NET_TIME.FromDateTime(startTime);
            var queryEndTime = NET_TIME.FromDateTime(endTime);

            var records = new List<RecordFileInfoModel>();
            var fileCount = 0;
            int allocSize = Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * MaxQueryRecords;
            var recordFileInfoPtr = Marshal.AllocHGlobal(allocSize);
            var success = OriginalSDK.CLIENT_QueryRecordFile(LoginId, -1, 0, ref queryStartTime, ref queryEndTime, null, recordFileInfoPtr, allocSize, ref fileCount, 25000, false);
            if (success)
            {
                for (var recordId = 0; recordId < fileCount; recordId++)
                {
                    var source = (NET_RECORDFILE_INFO)Marshal.PtrToStructure(IntPtr.Add(recordFileInfoPtr, Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * recordId), typeof(NET_RECORDFILE_INFO));
                    if (source.bRecType == (int)StreamRecordType.MainStream)
                    {
                        var recordFileInfo = new RecordFileInfoModel(source, records.Count + 1);
                        records.Add(recordFileInfo);
                    }
                }
            }
            Marshal.FreeHGlobal(recordFileInfoPtr);

            return records;
        }

        /// <summary>
        /// Starts the download process
        /// </summary>
        public void DownloadStart(RecordFileInfoModel record)
        {
            DownloadRecord = record;

            _downloadAbort = false;

            var fileName = $"{App.Settings.DownloadFolder}\\{record.FileName}.dav";
            var source = record.Source;
            var downloadId = OriginalSDK.CLIENT_DownloadByRecordFile(LoginId, ref source, fileName, _downloadPosCallBack, IntPtr.Zero);
            DownloadId = (long)downloadId;
            if (DownloadId == 0)
                ProcessDownloadError();
            else
            {
                record.ProgressString = "Downloading...";
            }
        }

        /// <summary>
        /// Callback when the download progress has changed
        /// </summary>
        private void DownloadPosCallback(IntPtr downloadId, uint totalSize, uint downloadPos, IntPtr userData)
        {
            if ((ResponseType)downloadPos == ResponseType.Completed || _downloadAbort)
            {
                NETClient.StopDownload(downloadId);
                if (_downloadAbort)
                    DownloadRecord.ProgressString = "Incomplete";
                else
                {
                    DownloadRecord.ProgressPercentValue = 100;
                    DownloadRecord.ProgressString = "Done";
                    DownloadCompleted?.Invoke(this, ((long)downloadId > 0) ? string.Empty : "Couldn't initialize download process");
                }
            }
            else if ((ResponseType)downloadPos == ResponseType.Error)
                ProcessDownloadError();
            else
                DownloadRecord.ProgressPercentValue = (int)((downloadPos / (double)totalSize) * 100);
        }

        /// <summary>
        /// Handles download errors
        /// </summary>
        private void ProcessDownloadError()
        {
            var error = OriginalSDK.CLIENT_GetLastError();
            var errorString = NETClient.GetLastError();
            Console.WriteLine(string.Format("Download failed.  Error code {0:X} ({1})", error, errorString));
            DownloadCompleted?.Invoke(this, errorString);
            DownloadRecord.ProgressString = "ERROR";
        }

        /// <summary>
        /// Process a request to stop downloads
        /// </summary>
        public void DownloadStop() => _downloadAbort = true;

        /// <summary>
        /// Starts playback of a video
        /// </summary>
        public IntPtr StartPlayback(NET_TIME startTime, NET_TIME endTime, uint channelId, IntPtr windowHandle)
        {
            var stream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr((int)EM_STREAM_TYPE.MAIN, stream, true);
            NETClient.SetDeviceMode(LoginId, EM_USEDEV_MODE.RECORD_STREAM_TYPE, stream);

            var inputInfo = new NET_IN_PLAY_BACK_BY_TIME_INFO();
            var outputInfo = new NET_OUT_PLAY_BACK_BY_TIME_INFO();
            inputInfo.stStartTime = startTime;
            inputInfo.stStopTime = endTime;
            inputInfo.hWnd = windowHandle;
            inputInfo.cbDownLoadPos = _playbackPosCallBack;
            inputInfo.dwPosUser = IntPtr.Zero;
            inputInfo.fDownLoadDataCallBack = null;
            inputInfo.dwDataUser = IntPtr.Zero;
            inputInfo.nPlayDirection = 0;
            inputInfo.nWaittime = 0;

            return NETClient.PlayBackByTime(LoginId, (int)channelId, inputInfo, ref outputInfo);
        }

        /// <summary>
        /// Changes the playback operation
        /// </summary>
        public void PlaybackControl(IntPtr playbackId, PlayBackType command) => NETClient.PlayBackControl(playbackId, command);

        /// <summary>
        /// Gets the current OSD (On-Screen Display) tick of a playback
        /// </summary>
        public long? GetPlayBackOsdTick(IntPtr playbackId)
        {
            var currentTime = new NET_TIME();
            var startTime = new NET_TIME();
            var endTime = new NET_TIME();
            NETClient.GetPlayBackOsdTime(playbackId, ref currentTime, ref startTime, ref endTime);
            return (currentTime.dwYear > 0) ? currentTime.ToDateTime().Ticks : (long?)null;
        }

        /// <summary>
        /// Callback when the playback progress has changed
        /// </summary>
        private void PlaybackPosCallback(IntPtr downloadId, uint totalSize, uint downloadPos, IntPtr userData)
        {
            if ((ResponseType)downloadPos == ResponseType.Completed)
                PlaybackCompleted?.Invoke(this, downloadId);
        }

        /// <summary>
        /// Saves the current playback frame in a file
        /// </summary>
        public bool CapturePlaybackPicture(IntPtr playbackId, string outputFileName, EM_NET_CAPTURE_FORMATS captureFormat)
        {
            return NETClient.CapturePicture(playbackId, outputFileName, captureFormat);
        }

        /// <summary>
        /// Live view functions
        /// </summary>
        public IntPtr StartLiveView(int channelId, IntPtr windowHandle) => NETClient.RealPlay(LoginId, channelId, windowHandle, EM_RealPlayType.Realplay);
        public void StopLiveView(IntPtr handle) => NETClient.StopRealPlay(handle);
        public bool SaveRealData(IntPtr handle, string fileName) => NETClient.SaveRealData(handle, fileName);
        public bool StopSaveRealData(IntPtr handle) => NETClient.StopSaveRealData(handle);

        /// <summary>
        /// Retrieves information about the hard disk
        /// </summary>
        public IEnumerable<HardDiskInfoModel> GetHardDisksInfo()
        {
            object state = new NET_HARDDISK_STATE();
            NETClient.QueryDevState(LoginId, (int)EM_DEVICE_STATE.DISK, ref state, typeof(NET_HARDDISK_STATE), 1000);

            var hddStates = (NET_HARDDISK_STATE)state;
            var hardDiskInfo = new List<HardDiskInfoModel>();
            foreach (var hddState in hddStates.stDisks)
            {
                var model = new HardDiskInfoModel()
                {
                    Id = hardDiskInfo.Count + 1,
                    Capacity = hddState.dwVolume,
                    FreeSpace = hddState.dwFreeSpace
                };
                hardDiskInfo.Add(model);
            }

            return hardDiskInfo;
        }

        /// <summary>
        /// Retrieves the accounts information stored on the device
        /// </summary>
        /// <param name="accounts">Reference to a buffer for the list of accounts on the device.</param>
        /// <param name="rights">Reference to a buffer for the list of available rights for the device.</param>
        /// <param name="groups">Reference to a buffer for the list of groups on the device.</param>
        public bool GetAccounts(ref IList<AccountModel> accounts, ref IList<RightModel> rights, ref IList<AccountGroupModel> groups)
        {
            accounts = new List<AccountModel>();
            rights = new List<RightModel>();
            groups = new List<AccountGroupModel>();

            var info = new NET_USER_MANAGE_INFO_NEW();
            info.dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_MANAGE_INFO_NEW));
            info.rightList = new NET_OPR_RIGHT_NEW[1024];
            for (var itemId = 0; itemId < info.rightList.Length; itemId++)
                info.rightList[itemId].dwSize = (uint)Marshal.SizeOf(typeof(NET_OPR_RIGHT_NEW));
            info.groupList = new NET_USER_GROUP_INFO_NEW[20];
            for (var itemId = 0; itemId < info.groupList.Length; itemId++)
                info.groupList[itemId].dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_GROUP_INFO_NEW));
            info.userList = new NET_USER_INFO_NEW[200];
            for (var itemId = 0; itemId < info.userList.Length; itemId++)
                info.userList[itemId].dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_INFO_NEW));
            info.groupListEx = new NET_USER_GROUP_INFO_EX2[20];
            for (var itemId = 0; itemId < info.groupListEx.Length; itemId++)
                info.groupListEx[itemId].dwSize = (uint)Marshal.SizeOf(typeof(NET_USER_GROUP_INFO_EX2));
            var success = NETClient.QueryUserInfoNew(LoginId, ref info, 5000);
            if (!success)
                return false;

            for (var userId = 0; userId < info.dwUserNum; userId++)
                accounts.Add(new AccountModel(info.userList[userId]));
            for (var rightId = 0; rightId < info.dwRightNum; rightId++)
                rights.Add(new RightModel(info.rightList[rightId]));
            for (var groupId = 0; groupId < info.dwGroupNum; groupId++)
                groups.Add(new AccountGroupModel(info.groupListEx[groupId]));

            return true;
        }

        /// <summary>
        /// Adds an account on the device
        /// </summary>
        public bool AddAccount(NET_USER_INFO_NEW userInfo)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));
            Marshal.StructureToPtr(userInfo, ptr, true);

            var success = NETClient.OperateUserInfoNew(LoginId, EM_OPERATE_USER_TYPE.ADD_USER, ptr, IntPtr.Zero, 5000);
            Marshal.FreeHGlobal(ptr);
            return success;
        }

        /// <summary>
        /// Deletes an account on the device
        /// </summary>
        public bool DeleteAccount(NET_USER_INFO_NEW userInfo)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));
            Marshal.StructureToPtr(userInfo, ptr, true);

            var success = NETClient.OperateUserInfoNew(LoginId, EM_OPERATE_USER_TYPE.DEL_USER, ptr, IntPtr.Zero, 5000);
            Marshal.FreeHGlobal(ptr);
            return success;
        }

        public bool UpdateAccount(NET_USER_INFO_NEW originalUserInfo, NET_USER_INFO_NEW updatedUserInfo) =>
            UpdateAccount(originalUserInfo, updatedUserInfo, EM_OPERATE_USER_TYPE.MODIFY_USER);
        public bool UpdatePassword(NET_USER_INFO_NEW originalUserInfo, NET_USER_INFO_NEW updatedUserInfo) =>
            UpdateAccount(originalUserInfo, updatedUserInfo, EM_OPERATE_USER_TYPE.MODIFY_PASSWORD);
        private bool UpdateAccount(NET_USER_INFO_NEW originalUserInfo, NET_USER_INFO_NEW updatedUserInfo, EM_OPERATE_USER_TYPE operation)
        {
            var originalPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));
            Marshal.StructureToPtr(originalUserInfo, originalPtr, true);
            var updatedPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_USER_INFO_NEW)));
            Marshal.StructureToPtr(updatedUserInfo, updatedPtr, true);

            var success = NETClient.OperateUserInfoNew(LoginId, operation, updatedPtr, originalPtr, 5000);
            Marshal.FreeHGlobal(originalPtr);
            Marshal.FreeHGlobal(updatedPtr);
            return success;
        }

        /// <summary>
        /// Retrieves log records
        /// </summary>
        public IEnumerable<LogModel> GetLogs(DateTime startDate, DateTime endDate)
        {
            NET_QUERY_DEVICE_LOG_PARAM queryDeviceLogParam = new NET_QUERY_DEVICE_LOG_PARAM();
            queryDeviceLogParam.emLogType = EM_LOG_QUERY_TYPE.ALL;
            queryDeviceLogParam.stuStartTime = new NET_TIME()
            {
                dwYear = (uint)startDate.Year,
                dwMonth = (uint)startDate.Month,
                dwDay = (uint)startDate.Day
            };
            queryDeviceLogParam.stuEndTime = new NET_TIME()
            {
                dwYear = (uint)endDate.Year,
                dwMonth = (uint)endDate.Month,
                dwDay = (uint)endDate.Day
            };
            queryDeviceLogParam.nChannelID = 0;
            queryDeviceLogParam.nLogStuType = 1;

            queryDeviceLogParam.nStartNum = 0;
            var recLogNum = 0;
            const int maxPerBatch = 100;
            var bufferSize = (maxPerBatch + 1) * Marshal.SizeOf(typeof(NET_DEVICE_LOG_ITEM_EX));
            var buffer = Marshal.AllocHGlobal(bufferSize);

            List<LogModel> logs = null;
            do
            {
                queryDeviceLogParam.nEndNum = queryDeviceLogParam.nStartNum + maxPerBatch - 1;
                var success = NETClient.QueryDeviceLog(LoginId, ref queryDeviceLogParam, buffer, bufferSize, ref recLogNum, 10000);

                if (success)
                {
                    if (logs == null)
                        logs = new List<LogModel>();
                    for (var logId = 0; logId < recLogNum; ++logId)
                    {
                        var source = (NET_DEVICE_LOG_ITEM_EX)Marshal.PtrToStructure(IntPtr.Add(buffer, Marshal.SizeOf(typeof(NET_DEVICE_LOG_ITEM_EX)) * logId), typeof(NET_DEVICE_LOG_ITEM_EX));
                        var logModel = new LogModel((uint)(queryDeviceLogParam.nStartNum + logId + 1), source);
                        logs.Add(logModel);
                    }
                }

                queryDeviceLogParam.nStartNum += maxPerBatch;
            } while (recLogNum == maxPerBatch);
            Marshal.FreeHGlobal(buffer);

            return logs;
        }

        /// <summary>
        /// Clears all log entries.  The only remaining log will be the one to tell who cleared the logs.
        /// </summary>
        public bool ClearLogs()
        {
            return NETClient.ControlDevice(LoginId, EM_CtrlType.CLEARLOG, IntPtr.Zero, 1000);
        }
    }
}
