using NetSDKCS;
using QSeeView.Models;
using QSeeView.Types;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace QSeeView.Tools
{
    public class QCW4DeviceManager : IDeviceManager
    {
        public event EventHandler<string> DownloadCompleted;

        private bool _isInitialized;
        private bool _downloadAbort;
        private static fDownLoadPosCallBack _downloadPosCallBack;

        public QCW4DeviceManager()
        {
            _isInitialized = OriginalSDK.CLIENT_InitEx(DisconnectCallBack, IntPtr.Zero, IntPtr.Zero);
            _downloadPosCallBack = new fDownLoadPosCallBack(DownloadPosCallback);
        }

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

        public void Login(string deviceIp, ushort devicePort, string username, string password)
        {
            LoginId = IntPtr.Zero;
            if (!_isInitialized)
                return;

            var deviceInfo = new NET_DEVICEINFO_Ex();
            int errorCode = 0;
            LoginId = OriginalSDK.CLIENT_LoginEx2(deviceIp, devicePort, username, password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo, ref errorCode);
        }

        public void Shutdown()
        {
            OriginalSDK.CLIENT_Cleanup();
        }

        public IList<RecordFileInfoModel> Query(DateTime startTime, DateTime endTime, bool isIgnoringNightFiles)
        {
            var queryStartTime = NET_TIME.FromDateTime(startTime);
            var queryEndTime = NET_TIME.FromDateTime(endTime);

            var records = new List<RecordFileInfoModel>();
            int fileCount = 0;
            unsafe
            {
                int allocSize = Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * MaxQueryRecords;
                var recordFileInfoPtr = Marshal.AllocHGlobal(allocSize);
                var success = OriginalSDK.CLIENT_QueryRecordFile(LoginId, -1, 0, ref queryStartTime, ref queryEndTime, null, recordFileInfoPtr, allocSize, ref fileCount, 25000, false);
                if (success)
                {
                    for (var recordId = 0; recordId < fileCount; recordId++)
                    {
                        var source = (NET_RECORDFILE_INFO)Marshal.PtrToStructure(IntPtr.Add(recordFileInfoPtr, Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * recordId), typeof(NET_RECORDFILE_INFO));
                        if (IsRecordValid(source, isIgnoringNightFiles))
                        {
                            var recordFileInfo = new RecordFileInfoModel(source, records.Count + 1);
                            records.Add(recordFileInfo);
                        }
                    }
                }
                Marshal.FreeHGlobal(recordFileInfoPtr);
            }

            return records;
        }

        /// <summary>
        /// Determines if the record successfully passes all filters
        /// </summary>
        private bool IsRecordValid(NET_RECORDFILE_INFO source, bool isIgnoringNightFiles)
        {
            if (source.bRecType != (int)StreamRecordType.MainStream)
                return false;

            if (isIgnoringNightFiles)
            {
                if (App.Settings.NightFilesStartHour < App.Settings.NightFilesEndHour)  // Ex. Between 1h00 and 6h00
                {
                    if (source.starttime.dwHour >= App.Settings.NightFilesStartHour && source.starttime.dwHour < App.Settings.NightFilesEndHour)
                        return false;
                }
                else // Ex. Between 23h00 and 6h00
                {
                    if (source.starttime.dwHour >= App.Settings.NightFilesStartHour || source.starttime.dwHour < App.Settings.NightFilesEndHour)
                        return false;
                }
            }

            return true;
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
            inputInfo.cbDownLoadPos = null;
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
        /// Saves the current playback frame in a file
        /// </summary>
        public bool CapturePlaybackPicture(IntPtr playbackId, string outputFileName, EM_NET_CAPTURE_FORMATS captureFormat)
        {
            return NETClient.CapturePicture(playbackId, outputFileName, captureFormat);
        }

        public IntPtr StartLiveView(int channelId, IntPtr windowHandle) => NETClient.RealPlay(LoginId, channelId, windowHandle, EM_RealPlayType.Realplay);
        public void StopLiveView(IntPtr monitorHandle) => NETClient.StopRealPlay(monitorHandle);

        public (uint spaceRemaining, uint capacity) GetDiskInfo(int diskId)
        {
            object state = new NET_HARDDISK_STATE();
            NETClient.QueryDevState(LoginId, (int)EM_DEVICE_STATE.DISK, ref state, typeof(NET_HARDDISK_STATE), 1000);

            var hddState = (NET_HARDDISK_STATE)state;
            if (diskId > hddState.stDisks.Length)
                throw new ArgumentOutOfRangeException(nameof(diskId));
            return (hddState.stDisks[diskId].dwFreeSpace, hddState.stDisks[diskId].dwVolume);
        }
    }
}
