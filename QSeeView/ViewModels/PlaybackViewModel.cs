using NetSDKCS;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;

namespace QSeeView.ViewModels
{
    class PlaybackViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Close;
        public event EventHandler PlaybackTimeChanged;
        public event EventHandler PreviousVideo;
        public event EventHandler NextVideo;
        public event EventHandler ReduceSpeed;
        public event EventHandler IncreaseSpeed;
        public event EventHandler Replay;
        public event EventHandler<PlayBackType> SetPlaybackControl;
        public event EventHandler UpdateSlider;
        public event EventHandler SaveSnapshot;
        public event EventHandler SelectedInQueryChanged;

        private double _playbackSliderMinimum;
        private double _playbackSliderMaximum;
        private double _playbackSliderValue;
        private Timer _playbackUpdateTimer;
        private string _sliderTimeText;
        private double _sliderLargeChange;
        private bool _isPaused;
        private bool _isSelectedInQuery;
        private string _recordLength;
        private int _playIndex;

        public PlaybackViewModel(int playIndex, int recordCount)
        {
            PlaybackID = IntPtr.Zero;
            PlayIndex = playIndex;
            RecordCount = recordCount;

            _playbackUpdateTimer = new Timer(500);
            _playbackUpdateTimer.Elapsed += (s, e) => UpdateSlider?.Invoke(this, EventArgs.Empty);

            ReplayCommand = new RelayCommand(() => Replay?.Invoke(this, EventArgs.Empty));
            StopCommand = new RelayCommand(() => Close?.Invoke(this, new EventArgs()));
            PlayCommand = new RelayCommand(() => IsPaused = false, () => IsPaused);
            PauseCommand = new RelayCommand(() => IsPaused = true, () => !IsPaused);
            SlowerCommand = new RelayCommand(() => ReduceSpeed?.Invoke(this, new EventArgs()), () => !IsPaused);
            FasterCommand = new RelayCommand(() => IncreaseSpeed?.Invoke(this, new EventArgs()), () => !IsPaused);
            PreviousCommand = new RelayCommand(() => PreviousVideo?.Invoke(this, new EventArgs()));
            NextCommand = new RelayCommand(() => NextVideo?.Invoke(this, new EventArgs()));
            SaveSnapshotCommand = new RelayCommand(() => SaveSnapshot?.Invoke(this, new EventArgs()), () => IsPaused);
        }

        public ICommand ReplayCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand SlowerCommand { get; }
        public ICommand FasterCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand SaveSnapshotCommand { get; }

        public IntPtr PlaybackID { get; set; }

        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    _playbackUpdateTimer.Stop();
                    SetPlaybackControl?.Invoke(this, PlayBackType.Pause);
                }
                else
                {
                    _playbackUpdateTimer.Start();
                    SetPlaybackControl?.Invoke(this, PlayBackType.Play);
                }
                OnPropertyChanged(nameof(IsPaused));
            }
        }

        public double PlaybackSliderMinimum
        {
            get => _playbackSliderMinimum;
            set
            {
                _playbackSliderMinimum = value;
                OnPropertyChanged(nameof(PlaybackSliderMinimum));
            }
        }

        public double PlaybackSliderMaximum
        {
            get => _playbackSliderMaximum;
            set
            {
                _playbackSliderMaximum = value;
                OnPropertyChanged(nameof(PlaybackSliderMaximum));
            }
        }

        public double PlaybackSliderValue
        {
            get => _playbackSliderValue;
            set
            {
                _playbackSliderValue = value;
                OnPropertyChanged(nameof(PlaybackSliderValue));

                PlaybackTimeChanged?.Invoke(this, new EventArgs());
                SliderTimeText = new DateTime((long)PlaybackSliderValue).ToString("T");
            }
        }

        public string SliderTimeText
        {
            get => _sliderTimeText;
            set
            {
                _sliderTimeText = value;
                OnPropertyChanged(nameof(SliderTimeText));
            }
        }

        public double SliderLargeChange
        {
            get => _sliderLargeChange;
            set
            {
                _sliderLargeChange = value;
                OnPropertyChanged(nameof(SliderLargeChange));
            }
        }

        public bool IsSelectedInQuery
        {
            get => _isSelectedInQuery;
            set
            {
                _isSelectedInQuery = value;
                OnPropertyChanged(nameof(IsSelectedInQuery));
                SelectedInQueryChanged?.Invoke(this, new EventArgs());
            }
        }

        public string RecordLength
        {
            get => _recordLength;
            set
            {
                _recordLength = value;
                OnPropertyChanged(nameof(RecordLength));
            }
        }

        public int PlayIndex
        {
            get => _playIndex;
            set
            {
                _playIndex = value;
                OnPropertyChanged(nameof(PlayIndex));
                OnPropertyChanged(nameof(PlayIndexPlusOne));
            }
        }
        public int PlayIndexPlusOne => PlayIndex + 1;

        public bool IsLandscape { get; set; }
        public double ImageInitialWidth => IsLandscape ? App.HDSize.Width : App.HDSize.Height;
        public double ImageInitialHeight => IsLandscape ? App.HDSize.Height : App.HDSize.Width;
        public int RecordCount { get; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void RefreshPlaybackImageSize()
        {
            OnPropertyChanged(nameof(ImageInitialWidth));
            OnPropertyChanged(nameof(ImageInitialHeight));
        }

        public void Start() => IsPaused = false;

        public void Stop()
        {
            _playbackUpdateTimer.Stop();
            SetPlaybackControl?.Invoke(this, PlayBackType.Stop);
        }
    }
}
