using NetSDKCS;
using QCW4.Properties;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace QCW4
{
    public class RecordFileInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isSelected;
        private int _progressPercentValue;
        private string _progressString;

        public RecordFileInfo(NET_RECORDFILE_INFO source, int id)
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
                switch (Channel)
                {
                    case 0:
                        return Settings.Default.Channel1Name ?? "1";
                    case 1:
                        return Settings.Default.Channel2Name ?? "2";
                    case 2:
                        return Settings.Default.Channel3Name ?? "3";
                    case 3:
                        return Settings.Default.Channel4Name ?? "4";
                }

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

        internal void ResetProgress()
        {
            ProgressPercentValue = 0;
            ProgressString = string.Empty;
        }
    }
}
