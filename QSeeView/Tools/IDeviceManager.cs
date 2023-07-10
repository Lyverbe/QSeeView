using NetSDKCS;
using QSeeView.Models;
using System;
using System.Collections.Generic;

namespace QSeeView.Tools
{
    public interface IDeviceManager
    {
        event EventHandler<string> DownloadCompleted;

        bool IsConnected { get; }
        IntPtr LoginId { get; }
        int MaxQueryRecords { get; }
        int ChannelsCount { get; }
        RecordFileInfoModel DownloadRecord { get; }

        string GetLastError();

        void Login(string deviceIp, ushort devicePort, string username, string password);
        void Shutdown();
        IList<RecordFileInfoModel> Query(DateTime startTime, DateTime endTime, bool isIgnoringNightFiles);

        void DownloadStart(RecordFileInfoModel record);
        void DownloadStop();

        IntPtr StartPlayback(NET_TIME startTime, NET_TIME endTIme, uint channelId, IntPtr windowHandle);
        void PlaybackControl(IntPtr playbackId, PlayBackType command);
        long? GetPlayBackOsdTick(IntPtr playbackId);

        IntPtr StartLiveView(int channelId, IntPtr windowHandle);
        void StopLiveView(IntPtr monitorHandle);
    }
}
