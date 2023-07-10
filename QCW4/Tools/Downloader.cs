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
        private IList<RecordFileInfoModel> _pendingDownloads;
        private IList<RecordFileInfoModel> _pendingConversions;

        public Downloader(IDeviceManager deviceManager)
        {
            _deviceManager = deviceManager;

            _pendingConversions = new List<RecordFileInfoModel>();

            _deviceManager.DownloadCompleted += DownloadCompleted;
        }

        public IList<RecordFileInfoModel> PendingDownloads
        {
            get => _pendingDownloads;
            set
            {
                _pendingDownloads = value;
                if (PendingDownloads.Any())
                {
                    PendingDownloads.ToList().ForEach(record => record.ProgressString = "Pending download...");
                    StartDownload();
                }
            }
        }

        public bool IsDownloading { get; private set; }

        private void StartDownload()
        {
            var record = _pendingDownloads.First();
            _deviceManager.DownloadStart(record);
            IsDownloading = true;
            DownloadStarted?.Invoke(this, record);
            _pendingDownloads.Remove(record);
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

            if (_pendingDownloads.Any())
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
            var process = new Process();
            process.StartInfo.FileName = $"{Directory.GetCurrentDirectory()}\\ffmpeg.exe";
            //process.StartInfo.Arguments = $"-y -r 24 -i \"{DownloadFolder}\\{recordFileInfo.FileName}.dav\" -preset fast -b:v 1000k -c libx264 \"{DownloadFolder}\\{recordFileInfo.FileName}.avi\"";
            process.StartInfo.Arguments = $"-y -f dhav -i \"{App.Settings.DownloadFolder}\\{recordFileInfo.FileName}.dav\" -vcodec copy \"{App.Settings.DownloadFolder}\\{recordFileInfo.FileName}.avi\"";
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => Process_Exited(process, recordFileInfo);

#if false   // For debugging ffmpeg
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            var sb = new System.Text.StringBuilder();
            process.OutputDataReceived += (s, e) => sb.AppendLine(e.Data);
            process.ErrorDataReceived += (s, e) => sb.AppendLine(e.Data);
#endif

            try
            {
                var success = process.Start();
#if false   // For debugging ffmpeg
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
                var _x = sb.ToString();
#endif
                if (success)
                    recordFileInfo.ProgressString = "Converting...";
                else
                {
                    recordFileInfo.ProgressString = "";
                    DownloadError?.Invoke(this, recordFileInfo + "Failed to start conversion");
                }
            }
            catch (Win32Exception exception)
            {
                DownloadError?.Invoke(this, $"{recordFileInfo}: Conversion process error {process.ExitCode} - {exception.Message}");
            }
        }

        private void Process_Exited(Process process, RecordFileInfoModel recordFileInfo)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ConversionProcess_Exited(process, recordFileInfo);
            }));
        }

        private void ConversionProcess_Exited(Process process, RecordFileInfoModel recordFileInfo)
        {
            if (process.ExitCode == 0)
            {
                recordFileInfo.ProgressString = "Done";
                File.Delete($"{App.Settings.DownloadFolder}\\{recordFileInfo.FileName}.dav");
            }
            else
            {
                recordFileInfo.ProgressString = "Conversion ended with code " + process.ExitCode;
                DownloadError?.Invoke(this, recordFileInfo + ": Conversion ended with code " + process.ExitCode);
            }

            if (_pendingConversions.Any())
                StartNextPendingConversion();
            else if (!IsDownloading)
                DownloadsCompleted?.Invoke(this, EventArgs.Empty);
        }

    }
}
