using NetSDKCS;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Input;

namespace QCW4
{
    class PlaybackViewModel : INotifyPropertyChanged
    {
        public static readonly Size HDSize = new Size(1920, 1080);

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Close;
        public event EventHandler PlaybackTimeChanged;
        public event EventHandler PreviousVideo;
        public event EventHandler NextVideo;
        public event EventHandler ReduceSpeed;
        public event EventHandler IncreaseSpeed;

        private double _playbackSliderMinimum;
        private double _playbackSliderMaximum;
        private double _playbackSliderValue;
        private Timer _playbackUpdateTimer;
        private bool _isSliderRangeInitialized;
        private string _sliderTimeText;
        private double _sliderLargeChange;

        public PlaybackViewModel()
        {
            PlaybackID = IntPtr.Zero;
            Speed = 1.0;
            _isSliderRangeInitialized = false;

            _playbackUpdateTimer = new Timer(500);
            _playbackUpdateTimer.Elapsed += PlaybackUpdateTimer_Elapsed;

            StopCommand = new RelayCommand(() => Close?.Invoke(this, new EventArgs()));
            PlayCommand = new RelayCommand(Play, () => IsPaused);
            PauseCommand = new RelayCommand(Pause, () => !IsPaused);
            SlowerCommand = new RelayCommand(() => ReduceSpeed?.Invoke(this, new EventArgs()), () => !IsPaused);
            FasterCommand = new RelayCommand(() => IncreaseSpeed?.Invoke(this, new EventArgs()), () => !IsPaused);
            PreviousCommand = new RelayCommand(() => PreviousVideo?.Invoke(this, new EventArgs()));
            NextCommand = new RelayCommand(() => NextVideo?.Invoke(this, new EventArgs()));
        }

        public ICommand StopCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand PauseCommand { get; private set; }
        public ICommand SlowerCommand { get; private set; }
        public ICommand FasterCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        public IntPtr PlaybackID { get; set; }
        public bool IsPaused { get; set; }
        private double Speed { get; set; }

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

        public bool IsLandscape { get; set; }
        public double ImageInitialWidth => IsLandscape ? HDSize.Width : HDSize.Height;
        public double ImageInitialHeight => IsLandscape ? HDSize.Height : HDSize.Width;

        public bool IsMouseDown { get; set; }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void PlaybackUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (PlaybackID == IntPtr.Zero || IsMouseDown)
                return;

            var osdTime = new NET_TIME();
            var osdStartTime = new NET_TIME();
            var osdEndTime = new NET_TIME();
            NETClient.GetPlayBackOsdTime(PlaybackID, ref osdTime, ref osdStartTime, ref osdEndTime);
            if (osdTime.dwMonth == 0)
                return;

            if (!_isSliderRangeInitialized)
            {
                PlaybackSliderMinimum = osdTime.ToDateTime().Ticks;
                PlaybackSliderMaximum = osdEndTime.ToDateTime().Ticks;
                SliderLargeChange = (PlaybackSliderMaximum - PlaybackSliderMinimum) / 10;
                _isSliderRangeInitialized = true;
            }

            PlaybackSliderValue = osdTime.ToDateTime().Ticks;
        }

        public void RefreshPlaybackImageSize()
        {
            OnPropertyChanged(nameof(ImageInitialWidth));
            OnPropertyChanged(nameof(ImageInitialHeight));
        }

        public void Start()
        {
            NETClient.PlayBackControl(PlaybackID, PlayBackType.Play);
            _isSliderRangeInitialized = false;
            _playbackUpdateTimer.Start();
        }

        public void Stop()
        {
            NETClient.PlayBackControl(PlaybackID, PlayBackType.Stop);
        }

        public void Play()
        {
            NETClient.PlayBackControl(PlaybackID, PlayBackType.Play);
            IsPaused = false;
        }

        public void Pause()
        {
            NETClient.PlayBackControl(PlaybackID, PlayBackType.Pause);
            IsPaused = true;
        }
    }
}
