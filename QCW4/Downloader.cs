using NetSDKCS;
using System;
using System.ComponentModel;

namespace QCW4
{
    class Downloader : INotifyPropertyChanged
    {
        public event EventHandler<bool> DownloadCompleted;

        public event PropertyChangedEventHandler PropertyChanged;

        private static fDownLoadPosCallBack _downloadPosCallBack;

        private enum ResponseType
        {
            Completed = -1,
            Error = -2,
            Other
        };

        private bool _abort;

        public Downloader()
        {
            _downloadPosCallBack = new fDownLoadPosCallBack(DownloadPosCallback);
        }

        public RecordFileInfo Record { get; private set; }
        public IntPtr LoginId { get; set; }

        public long DownloadId { get; private set; }
        public string LastErrorString { get; private set; }
        public bool IsDownloading { get; set; }
        public string DownloadFolder { get; set; }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Starts the download process
        /// </summary>
        public void Start(RecordFileInfo record)
        {
            Record = record;

            IsDownloading = true;
            _abort = false;

            var fileName = $"{DownloadFolder}\\{record.FileName}.dav";
            var source = record.Source;
            var downloadId = OriginalSDK.CLIENT_DownloadByRecordFile(LoginId, ref source, fileName, _downloadPosCallBack, IntPtr.Zero);
            DownloadId = (long)downloadId;
            if (DownloadId == 0)
                ProcessError();
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
            if ((ResponseType)downloadPos == ResponseType.Completed || _abort)
            {
                NETClient.StopDownload(downloadId);
                IsDownloading = false;
                if (_abort)
                    Record.ProgressString = "Incomplete";
                else
                {
                    Record.ProgressPercentValue = 100;
                    Record.ProgressString = "Done";
                    DownloadCompleted?.Invoke(this, ((long)downloadId > 0));
                }
            }
            else if ((ResponseType)downloadPos == ResponseType.Error)
            {
                IsDownloading = false;
                ProcessError();
            }
            else
                Record.ProgressPercentValue = (int)((downloadPos / (double)totalSize) * 100);
        }

        private void ProcessError()
        {
            var error = OriginalSDK.CLIENT_GetLastError();
            LastErrorString = NETClient.GetLastError();
            Console.WriteLine(string.Format("Download failed.  Error code {0:X} ({1})", error, LastErrorString));
            DownloadCompleted?.Invoke(this, false);
            Record.ProgressString = "ERROR";
        }

        /// <summary>
        /// Process a request to stop downloads
        /// </summary>
        public void Stop() => _abort = true;
    }
}
