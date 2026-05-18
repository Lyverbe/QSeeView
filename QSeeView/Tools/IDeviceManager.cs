using NetSDKCS;
using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QSeeView.Tools
{
    // Be careful since enum is stored in settings.  Always add at the end to not break backward compatibility.
    public enum DeviceModelType
    {
        [Description("QCW4")]  // Text in the login's ComboBox
        QCW4
    };

    public interface IDeviceManager
    {
        event EventHandler<string> DownloadCompleted;
        event EventHandler<IntPtr> PlaybackCompleted;

        DeviceModelType DeviceModelType { get; }

        bool IsConnected { get; }
        IntPtr LoginId { get; }
        int MaxQueryRecords { get; }
        int ChannelsCount { get; }
        RecordFileInfoModel DownloadRecord { get; }

        string GetLastError();

        void Login(string deviceIp, ushort devicePort, string username, string password);
        void Shutdown();
        IList<RecordFileInfoModel> Query(DateTime startTime, DateTime endTime);

        void DownloadStart(RecordFileInfoModel record);
        void DownloadStop();

        IntPtr StartPlayback(NET_TIME startTime, NET_TIME endTIme, uint channelId, IntPtr windowHandle);
        void PlaybackControl(IntPtr playbackId, PlayBackType command);
        long? GetPlayBackOsdTick(IntPtr playbackId);
        bool CapturePlaybackPicture(IntPtr playbackId, string outputFileName, EM_NET_CAPTURE_FORMATS captureFormat);

        IntPtr StartLiveView(int channelId, IntPtr windowHandle);
        void StopLiveView(IntPtr monitorHandle);
        bool SaveRealData(IntPtr handle, string fileName);
        bool StopSaveRealData(IntPtr handle);

        IEnumerable<HardDiskInfoModel> GetHardDisksInfo();
        bool GetAccounts(ref IList<AccountModel> accounts, ref IList<RightModel> rights, ref IList<AccountGroupModel> groups);
        bool AddAccount(NET_USER_INFO_NEW userInfo);
        bool DeleteAccount(NET_USER_INFO_NEW userInfo);
        bool UpdateAccount(NET_USER_INFO_NEW originalUserInfo, NET_USER_INFO_NEW updatedUserInfo);
        bool UpdatePassword(NET_USER_INFO_NEW originalUserInfo, NET_USER_INFO_NEW updatedUserInfo);
        IEnumerable<LogModel> GetLogs(DateTime startDate, DateTime endDate);
        bool ClearLogs();
    }
}
