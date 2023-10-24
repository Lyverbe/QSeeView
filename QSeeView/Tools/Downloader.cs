using QSeeView.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace QSeeView.Tools
{
    public class Downloader
    {
        public event EventHandler<RecordFileInfoModel> DownloadStarted;
        public event EventHandler<string> DownloadError;
        public event EventHandler DownloadsCompleted;

        private IDeviceManager _deviceManager;
        private IList<RecordFileInfoModel> _pendingConversions;

        public Downloader(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;

            _pendingConversions = new List<RecordFileInfoModel>();

            _deviceManager.DownloadCompleted += DownloadCompleted;
        }

        public IList<RecordFileInfoModel> PendingDownloads { get; private set; }
        public bool IsDownloading { get; private set; }

        public void StartDownloads(IList<RecordFileInfoModel> records)
        {
            PendingDownloads = records;
            PendingDownloads.ToList().ForEach(record => record.ProgressString = "Pending download...");
            StartDownload();
        }
        private void StartDownload()
        {
            var record = PendingDownloads.First();
            _deviceManager.DownloadStart(record);
            IsDownloading = true;
            DownloadStarted?.Invoke(this, record);
            PendingDownloads.Remove(record);
        }

        public void StopDownload()
        {
            if (IsDownloading)
                _deviceManager.DownloadStop();
            PendingDownloads.Clear();
        }

        /// <summary>
        /// Callback when a download is completed
        /// </summary>
        private void DownloadCompleted(object sender, string errorMessage)
        {
            if (App.Settings.IsConvertingToAvi)
            {
                _pendingConversions.Add(_deviceManager.DownloadRecord);
                _deviceManager.DownloadRecord.ProgressString = "Pending conversion...";
            }
            else
                _deviceManager.DownloadRecord.ProgressString = "Done";

            if (PendingDownloads.Any())
                StartDownload();
            else
                IsDownloading = false;

            if (_pendingConversions.Any())
                StartNextPendingConversion();
        }

        private void StartNextPendingConversion()
        {
            var record = _pendingConversions.First();
            _pendingConversions.Remove(record);
            StartConverter(record);
        }

        private void StartConverter(RecordFileInfoModel recordFileInfo)
        {
            var converter = new VideoConverter();
            converter.ConversionDone += (s, returnCode) => ConversionProcess_Exited(returnCode, recordFileInfo);
            try
            {
                converter.Convert(recordFileInfo.FileName);

                if (converter.IsConverting)
                    recordFileInfo.ProgressString = "Converting...";
                else
                {
                    recordFileInfo.ProgressString = "";
                    DownloadError?.Invoke(this, recordFileInfo + "Failed to start conversion");
                }
            }
            catch (Win32Exception exception)
            {
                DownloadError?.Invoke(this, $"{recordFileInfo}: Conversion process error - {exception.Message}");
            }
        }

        private void ConversionProcess_Exited(int exitCode, RecordFileInfoModel recordFileInfo)
        {
            if (exitCode == 0)
            {
                recordFileInfo.ProgressString = "Done";
                File.Delete($"{App.Settings.DownloadFolder}\\{recordFileInfo.FileName}.dav");
            }
            else
            {
                recordFileInfo.ProgressString = "Conversion ended with code " + exitCode;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    DownloadError?.Invoke(this, recordFileInfo.FileName + ": Conversion ended with code " + exitCode);
                });
            }

            if (_pendingConversions.Any())
                StartNextPendingConversion();
            else if (!IsDownloading)
                DownloadsCompleted?.Invoke(this, EventArgs.Empty);
        }

    }
}
