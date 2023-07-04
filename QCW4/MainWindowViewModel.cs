using Microsoft.Win32;
using NetSDKCS;
using QCW4.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;

namespace QCW4
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public event EventHandler<string> ShowMessage;
        public event EventHandler<RecordFileInfo> PlayRecord;
        public event EventHandler SetChannels;

        public event PropertyChangedEventHandler PropertyChanged;

        private enum StreamRecordType
        {
            MainStream,
            Sub1Stream,
            Sub2Stream,
            Sub3Stream
        }
        public enum StateType
        {
            Idle,
            Running,
            Pausing,
            Paused
        };

        private const int MaxConversionProcess = 3;

        private Downloader _downloader;
        private IList<RecordFileInfo> _pendingDownloads;
        private IList<RecordFileInfo> _pendingConversions;

        private bool _isRecordsListEnabled;
        private bool _isQueryEnabled;
        private string _downloadCommandString;
        private string _downloadFolder;
        private bool _isDownloadFolderEnabled;
        private bool _checkAll;
        private string _statusBarInfo;
        private int _conversionProcessCount;
        private StateType _state;
        private string _pauseCommandString;
        private bool _isErrorSectionVisible;
        private double _taskbarProgressValue;
        private int _totalDownloadCount;

        public MainWindowViewModel()
        {
            DownloadErrors = new ObservableCollection<string>();
            QueryCommand = new RelayCommand(OnQuery);
            DownloadStopCommand = new RelayCommand(OnDownloadOrStop, IsDownloadCommandAvailable);
            PauseCommand = new RelayCommand(OnPause, IsPauseCommandAvailable);
            ToggleSelectCommand = new RelayCommand<ObservableCollection<object>>(OnToggleSelect, IsToggleSelectAvailable);
            BrowseDownloadFolderCommand = new RelayCommand(OnBrowseDownloadFolder);
            OpenDownloadFolderCommand = new RelayCommand(() => Process.Start(DownloadFolder));
            PlayCommand = new RelayCommand<RecordFileInfo>((record) => PlayRecord?.Invoke(this, record));
            SetChannelsCommand = new RelayCommand(() => SetChannels?.Invoke(this, new EventArgs()));

            _downloader = new Downloader();
            _pendingDownloads = new List<RecordFileInfo>();
            _pendingConversions = new List<RecordFileInfo>();

            InitializeSdk();
            State = StateType.Idle;

            _downloader.LoginId = LoginId;
            _downloader.DownloadCompleted += Downloader_DownloadCompleted;

            EndDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            StartDateTime = EndDateTime.AddDays(-1);
            IsIgnoringNightFiles = true;
            DoConvert = true;
            DownloadFolder = Settings.Default.DownloadFolder;
            StatusBarInfo = "Ready";
        }

        public ICommand QueryCommand { get; private set; }
        public ICommand SetChannelsCommand { get; private set; }
        public ICommand DownloadStopCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand ToggleSelectCommand { get; private set; }
        public ICommand BrowseDownloadFolderCommand { get; private set; }
        public ICommand OpenDownloadFolderCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }

        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public bool IsIgnoringNightFiles { get; set; }
        public bool DoConvert { get; set; }
        public IList<RecordFileInfo> Records { get; private set; }
        public ObservableCollection<string> DownloadErrors { get; private set; }

        public IntPtr LoginId { get; private set; }
        public bool IsLoggedIn => ((long)LoginId > 0);

        public string StatusBarInfo
        {
            get => _statusBarInfo;
            set
            {
                _statusBarInfo = value;
                OnPropertyChanged(nameof(StatusBarInfo));
            }
        }

        public string DownloadCommandString
        {
            get => _downloadCommandString;
            set
            {
                _downloadCommandString = value;
                OnPropertyChanged(nameof(DownloadCommandString));
            }
        }

        public bool IsRecordsListEnabled
        {
            get => _isRecordsListEnabled;
            set
            {
                _isRecordsListEnabled = value;
                OnPropertyChanged(nameof(IsRecordsListEnabled));
            }
        }

        public bool IsQueryEnabled
        {
            get => _isQueryEnabled;
            set
            {
                _isQueryEnabled = value;
                OnPropertyChanged(nameof(IsQueryEnabled));
            }
        }

        public bool IsDownloadFolderEnabled
        {
            get => _isDownloadFolderEnabled;
            set
            {
                _isDownloadFolderEnabled = value;
                OnPropertyChanged(nameof(IsDownloadFolderEnabled));
            }
        }

        public string DownloadFolder
        {
            get => _downloadFolder;
            set
            {
                _downloadFolder = value;
                OnPropertyChanged(nameof(DownloadFolder));
                Settings.Default.DownloadFolder = DownloadFolder;
                _downloader.DownloadFolder = DownloadFolder;
            }
        }

        public bool CheckAll
        {
            get => _checkAll;
            set
            {
                _checkAll = value;
                OnPropertyChanged(nameof(CheckAll));
                if (Records != null)
                    Records.ToList().ForEach(record => record.IsSelected = value);
            }
        }

        public bool IsIdle => (State == StateType.Idle);

        public StateType State
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
                OnPropertyChanged(nameof(IsIdle));
                DownloadCommandString = IsIdle ? "Download selected" : "Stop downloads";
                PauseCommandString = (State == StateType.Pausing) ? "Pausing..." : (State == StateType.Paused) ? "Resume" : "Pause";
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    (PauseCommand as RelayCommand).RaiseCanExecuteChanged();
                }));
            }
        }

        public bool IsErrorSectionVisible
        {
            get => _isErrorSectionVisible;
            set
            {
                _isErrorSectionVisible = value;
                OnPropertyChanged(nameof(IsErrorSectionVisible));
            }
        }

        public double TaskbarProgressValue
        {
            get => _taskbarProgressValue;
            set
            {
                _taskbarProgressValue = value;
                OnPropertyChanged(nameof(TaskbarProgressValue));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DisconnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
        }

        /// <summary>
        /// Initialize the SDK
        /// </summary>
        private void InitializeSdk()
        {
            var IsInitialized = OriginalSDK.CLIENT_InitEx(DisconnectCallBack, IntPtr.Zero, IntPtr.Zero);
            if (IsInitialized)
            {
                NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
                int errorCode = 0;
                LoginId = OriginalSDK.CLIENT_LoginEx2("192.168.1.114", 37777, "qcwin", "q!w!e!", EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo, ref errorCode);
            }
        }

        /// <summary>
        /// Callback when the Query button has been clicked
        /// </summary>
        public void OnQuery()
        {
            var startTime = NET_TIME.FromDateTime(StartDateTime);
            var endTime = NET_TIME.FromDateTime(EndDateTime);

            const int MaxRecords = 5000;    // QCW4's limit
            Records = new List<RecordFileInfo>();
            int fileCount = 0;
            unsafe
            {
                int allocSize = Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * MaxRecords;
                var recordFileInfoPtr = Marshal.AllocHGlobal(allocSize);
                var success = OriginalSDK.CLIENT_QueryRecordFile(LoginId, -1, 0, ref startTime, ref endTime, null, recordFileInfoPtr, allocSize, ref fileCount, 25000, false);
                if (success)
                {
                    for (var recordId = 0; recordId < fileCount; recordId++)
                    {
                        var source = (NET_RECORDFILE_INFO)Marshal.PtrToStructure(IntPtr.Add(recordFileInfoPtr, Marshal.SizeOf(typeof(NET_RECORDFILE_INFO)) * recordId), typeof(NET_RECORDFILE_INFO));
                        if (IsRecordValid(source))
                        {
                            var recordFileInfo = new RecordFileInfo(source, Records.Count + 1);
                            Records.Add(recordFileInfo);
                        }
                    }

                    if (fileCount == MaxRecords)
                        ShowMessage?.Invoke(this, $"Limit of {MaxRecords} records has been reached. Some files may be missing.");
                }
                Marshal.FreeHGlobal(recordFileInfoPtr);
            }

            CheckAll = true;

            OnPropertyChanged(nameof(Records));
        }

        /// <summary>
        /// Determines if the record successfully passes all filters
        /// </summary>
        private bool IsRecordValid(NET_RECORDFILE_INFO source)
        {
            if (source.bRecType != (int)StreamRecordType.MainStream)
                return false;
            if (IsIgnoringNightFiles && (source.starttime.dwHour >= 23 || source.starttime.dwHour < 6))
                return false;

            return true;
        }

        /// <summary>
        /// Callback when the Download/Stop button has been clicked
        /// </summary>
        private void OnDownloadOrStop()
        {
            if (_downloader.IsDownloading || State == StateType.Paused)
            {
                _downloader.Stop();
                _pendingDownloads.Clear();
                State = StateType.Idle;
            }
            else
            {
                _downloader.DownloadFolder = DownloadFolder;
                Records.ToList().ForEach(record => record.ResetProgress());

                _pendingDownloads = Records.Where(record => record.IsSelected).ToList();
                if (_pendingDownloads.Any())
                {
                    State = StateType.Running;
                    _pendingDownloads.ToList().ForEach(record => record.ProgressString = "Pending download...");
                    _totalDownloadCount = _pendingDownloads.Count();
                    StartNextDownload();
                }
            }
        }
        private bool IsDownloadCommandAvailable()
        {
            return !string.IsNullOrEmpty(DownloadFolder) && Records != null && Records.Any(record => record.IsSelected);
        }

        /// <summary>
        /// Callback when the "Pause" button has been clicked
        /// </summary>
        private void OnPause()
        {
            switch (State)
            {
                case StateType.Running:
                    State = StateType.Pausing;
                    break;
                case StateType.Paused:
                    State = StateType.Running;
                    if (_pendingDownloads.Any())
                        StartNextDownload();
                    if (_pendingConversions.Any())
                        ProcessNextConversion();
                    break;
                default:
                    throw new Exception("Unexpected state at this point");
            };
        }
        private bool IsPauseCommandAvailable() => (State == StateType.Running || State == StateType.Paused);
        public string PauseCommandString
        {
            get => _pauseCommandString;
            set
            {
                _pauseCommandString = value;
                OnPropertyChanged(nameof(PauseCommandString));
            }
        }

        /// <summary>
        /// Callback when the "[Un]Select highlighted" button has been clicked
        /// </summary>
        private void OnToggleSelect(ObservableCollection<object> highlightedItems)
        {
            highlightedItems.ToList().ForEach(record => (record as RecordFileInfo).IsSelected = !(record as RecordFileInfo).IsSelected);
        }
        private bool IsToggleSelectAvailable(ObservableCollection<object> highlightedItems)
        {
            return (Records != null && Records.Any());
        }

        /// <summary>
        /// Starts the next download
        /// </summary>
        private void StartNextDownload()
        {
            var record = _pendingDownloads.First();
            _downloader.Start(record);
            StatusBarInfo = $"Downloading {record} ({_pendingDownloads.Count} remaining)";
            TaskbarProgressValue = 1.0 - (_pendingDownloads.Count() / (double)_totalDownloadCount);
            _pendingDownloads.Remove(record);
        }

        /// <summary>
        /// Callback when a download is completed
        /// </summary>
        private void Downloader_DownloadCompleted(object sender, bool success)
        {
            if (!success)
            {
                if (!string.IsNullOrEmpty(_downloader.LastErrorString))
                {
                    DownloadErrors.Add($"{_downloader.Record} - {_downloader.LastErrorString}");
                    IsErrorSectionVisible = true;
                }
            }

            _pendingConversions.Add(_downloader.Record);
            _downloader.Record.ProgressString = DoConvert ? "Pending conversion..." : "Done";

            if (State == StateType.Running || State == StateType.Idle)
            {
                ProcessNextConversion();
                if (_pendingDownloads.Any())
                    StartNextDownload();
                else
                    DownloadsDone();
            }
            else if (State == StateType.Pausing)
                State = StateType.Paused;
        }

        /// <summary>
        /// Callback when the "..." button has been clicked to change the download folder
        /// </summary>
        private void OnBrowseDownloadFolder()
        {
            // Create a "Save As" dialog for selecting a directory (HACK)
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = DownloadFolder;
            dialog.Title = "Select a Directory";
            dialog.Filter = "Directory|*.this.directory";
            dialog.FileName = "select";
            if (dialog.ShowDialog() == true)
            {
                var path = dialog.FileName;
                path = path.Replace("\\select.this.directory", "");
                path = path.Replace(".this.directory", "");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                DownloadFolder = path;
            }
        }

        private void ProcessNextConversion()
        {
            if (!DoConvert || _conversionProcessCount >= MaxConversionProcess)
                return;

            var record = _pendingConversions.First();
            _pendingConversions.Remove(record);
            StartConverter(record);
        }

        private void StartConverter(RecordFileInfo recordFileInfo)
        {
            var process = new Process();
            process.StartInfo.FileName = $"{Directory.GetCurrentDirectory()}\\ffmpeg.exe";
            //process.StartInfo.Arguments = $"-y -r 24 -i \"{DownloadFolder}\\{recordFileInfo.FileName}.dav\" -preset fast -b:v 1000k -c libx264 \"{DownloadFolder}\\{recordFileInfo.FileName}.avi\"";
            process.StartInfo.Arguments = $"-y -f dhav -i \"{DownloadFolder}\\{recordFileInfo.FileName}.dav\" -vcodec copy \"{DownloadFolder}\\{recordFileInfo.FileName}.avi\"";
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
                {
                    _conversionProcessCount++;
                    recordFileInfo.ProgressString = "Converting...";
                }
                else
                {
                    recordFileInfo.ProgressString = "";
                    DownloadErrors.Add(recordFileInfo + "Failed to start conversion");
                    IsErrorSectionVisible = true;
                }
            }
            catch (Win32Exception exception)
            {
                ShowMessage?.Invoke(this, "Conversion process error: " + exception.Message);
                DownloadErrors.Add(recordFileInfo + ": Conversion process error " + process.ExitCode);
                IsErrorSectionVisible = true;
            }
        }

        private void Process_Exited(Process process, RecordFileInfo recordFileInfo)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                ConversionProcess_Exited(process, recordFileInfo);
            }));
        }

        private void ConversionProcess_Exited(Process process, RecordFileInfo recordFileInfo)
        {
            if (process.ExitCode == 0)
            {
                recordFileInfo.ProgressString = "Done";
                try
                {
                    File.Delete($"{DownloadFolder}\\{recordFileInfo.FileName}.dav");
                }
                catch (Exception exception)
                {
                    ShowMessage?.Invoke(this, exception.Message);
                }
            }
            else
            {
                recordFileInfo.ProgressString = "Conversion ended with code " + process.ExitCode;
                DownloadErrors.Add(recordFileInfo + ": Conversion ended with code " + process.ExitCode);
                IsErrorSectionVisible = true;
            }

            _conversionProcessCount--;
            if (_pendingConversions.Any())
            {
                if (State == StateType.Running)
                    ProcessNextConversion();
            }
        }

        /// <summary>
        /// Every selected records is downloaded and converted
        /// </summary>
        private void DownloadsDone()
        {
            State = StateType.Idle;
            StatusBarInfo = "Ready";
            TaskbarProgressValue = 0;
        }
    }
}
