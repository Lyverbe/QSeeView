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
    }
}
