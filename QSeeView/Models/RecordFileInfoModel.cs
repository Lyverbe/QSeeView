using NetSDKCS;
using QSeeView.Tools;
using QSeeView.Types;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace QSeeView.Models
{
    public sealed class RecordFileInfoModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSelected;
        private int _progressPercentValue;
        private string _progressString;

        public RecordFileInfoModel(NET_RECORDFILE_INFO source, int id)
        {
            ID = id;
            Source = source;
        }

        public int ID { get; private set; }
        public NET_RECORDFILE_INFO Source { get; private set; }

        public DateTime StartTime => Source.starttime.ToDateTime();
        public string StartTimeString => StartTime.ToString("G");
        public DateTime EndTime => Source.endtime.ToDateTime();
        public string EndTimeString => EndTime.ToString("G");
        public int Channel => (int)Source.ch;
        public uint SizeInBytes => Source.size;
        public TimeSpan Length => EndTime - StartTime;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string ChannelName => App.Settings.ChannelsInfo[Channel].Name;

        public Brush LengthColor
        {
            get
            {
                if (Length.TotalSeconds >= App.Settings.QueryRedColorSeconds)
                    return new SolidColorBrush(Colors.Red);
                if (App.Settings.ThemeId == ThemeType.Dark)
                {
                    if (Length.TotalSeconds >= App.Settings.QueryYellowColorSeconds)
                        return new SolidColorBrush(Colors.Yellow);
                    return new SolidColorBrush(Colors.White);
                }
                else
                {
                    if (Length.TotalSeconds >= App.Settings.QueryYellowColorSeconds)
                        return new SolidColorBrush(Colors.DarkGoldenrod);
                    return new SolidColorBrush(Colors.Black);
                }
            }
        }

        public int ProgressPercentValue
        {
            get => _progressPercentValue;
            set
            {
                _progressPercentValue = value;
                OnPropertyChanged(nameof(ProgressPercentValue));
            }
        }

        public string ProgressString
        {
            get => _progressString;
            set
            {
                _progressString = value;
                OnPropertyChanged(nameof(ProgressString));
            }
        }

        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(App.Settings.FileNamesPattern))
                    return $"{StartTime.Year}-{StartTime.Month:00}-{StartTime.Day:00}" +
                           $"_{StartTime.Hour:00}h{StartTime.Minute:00}m{StartTime.Second:00}" +
                           "_ch" + Channel;
                else
                    return FileNameBuilder.Build(App.Settings.FileNamesPattern, StartTime, Channel);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetProgress()
        {
            ProgressPercentValue = 0;
            ProgressString = string.Empty;
        }

        public void RefreshChannelNames() => OnPropertyChanged(nameof(ChannelName));
    }
}
