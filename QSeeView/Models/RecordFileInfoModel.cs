using NetSDKCS;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace QSeeView.Models
{
    public class RecordFileInfoModel : INotifyPropertyChanged
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
        public uint Channel => Source.ch;
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

        public string ChannelName
        {
            get
            {
                if (string.IsNullOrEmpty(App.Settings.ChannelsInfo[(int)Channel].Name))
                    return Channel.ToString();
                else
                    return App.Settings.ChannelsInfo[(int)Channel].Name;

                throw new Exception();
            }
        }

        public Brush LengthColor
        {
            get
            {
                if (Length >= TimeSpan.FromMinutes(1))
                    return new SolidColorBrush(Colors.Red);
                if (Length >= TimeSpan.FromSeconds(45))
                    return new SolidColorBrush(Colors.Yellow);
                return new SolidColorBrush(Colors.White);
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

        public string FileName => ToString() + "_ch" + Channel;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{StartTime.Year}-{StartTime.Month:00}-{StartTime.Day:00}" +
                   $"_{StartTime.Hour:00}h{StartTime.Minute:00}m{StartTime.Second:00}";
        }

        public void ResetProgress()
        {
            ProgressPercentValue = 0;
            ProgressString = string.Empty;
        }

        public void RefreshChannelNames() => OnPropertyChanged(nameof(ChannelName));
    }
}
