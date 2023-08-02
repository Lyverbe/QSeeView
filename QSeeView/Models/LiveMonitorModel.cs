using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace QSeeView.Models
{
    public sealed class LiveMonitorModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double _zoomLevel;
        private bool _showScrollBars;
        private double _horizontalScrollValue;
        private double _verticalScrollValue;
        private double _horizontalScrollMaximum;
        private double _verticalScrollMaximum;

        public LiveMonitorModel(int channelId)
        {
            ChannelId = channelId;

            var channelInfo = App.Settings.ChannelsInfo[ChannelId];
            Width = channelInfo.IsLandscape ? App.HDSize.Width : App.HDSize.Height;
            Height = channelInfo.IsLandscape ? App.HDSize.Height : App.HDSize.Width;
        }

        public double ZoomLevelMaximum => 2000;

        public WindowsFormsHost Host { get; set; }
        public int ChannelId { get; }
        public double Width { get; }
        public double Height { get; }

        public Size DisplayOriginalSize { get; set; }
        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = value;
                OnPropertyChanged(nameof(ZoomLevel));
                OnZoomLevelChanged();
            }
        }

        public bool ShowScrollBars
        {
            get => _showScrollBars;
            set
            {
                _showScrollBars = value;
                OnPropertyChanged(nameof(ShowScrollBars));
            }
        }

        public double HorizontalScrollMaximum
        {
            get => _horizontalScrollMaximum;
            set
            {
                _horizontalScrollMaximum = value;
                OnPropertyChanged(nameof(HorizontalScrollMaximum));
            }
        }

        public double VerticalScrollMaximum
        {
            get => _verticalScrollMaximum;
            set
            {
                _verticalScrollMaximum = value;
                OnPropertyChanged(nameof(VerticalScrollMaximum));
            }
        }

        public double HorizontalScrollValue
        {
            get => _horizontalScrollValue;
            set
            {
                _horizontalScrollValue = value;
                OnPropertyChanged(nameof(HorizontalScrollValue));
                OnScrollChanged();
            }
        }

        public double VerticalScrollValue
        {
            get => _verticalScrollValue;
            set
            {
                _verticalScrollValue = value;
                OnPropertyChanged(nameof(VerticalScrollValue));
                OnScrollChanged();
            }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void OnZoomLevelChanged()
        {
            var pictureBox = Host.Child as PictureBox;
            if (ZoomLevel == 0)
            {
                pictureBox.Width = (int)DisplayOriginalSize.Width;
                pictureBox.Height = (int)DisplayOriginalSize.Height;

                ShowScrollBars = false;
                HorizontalScrollValue = 0;
                VerticalScrollValue = 0;
            }
            else
            {
                var ratio = (double)pictureBox.Width / pictureBox.Height;
                var widthWidthZoom = (DisplayOriginalSize.Width + ZoomLevel);
                pictureBox.Width = (int)widthWidthZoom;
                pictureBox.Height = (int)(widthWidthZoom / ratio);

                HorizontalScrollMaximum = pictureBox.Width - DisplayOriginalSize.Width;
                VerticalScrollMaximum = pictureBox.Height - DisplayOriginalSize.Height;
                ShowScrollBars = true;

                var restrictedOffsets = RestrictImageOffsets(HorizontalScrollValue, VerticalScrollValue);
                if (restrictedOffsets.X != HorizontalScrollValue)
                    HorizontalScrollValue = restrictedOffsets.X;
                if (restrictedOffsets.Y != VerticalScrollValue)
                    VerticalScrollValue = restrictedOffsets.Y;
            }
        }

        private void OnScrollChanged()
        {
            var pictureBox = Host.Child as PictureBox;
            pictureBox.Left = -(int)HorizontalScrollValue;
            pictureBox.Top = -(int)VerticalScrollValue;
        }

        public void OffsetScroll(Point offset)
        {
            var newHorizontalValue = HorizontalScrollValue -= offset.X;
            var newVerticalValue = VerticalScrollValue -= offset.Y;
            var restrictedOffsets = RestrictImageOffsets(newHorizontalValue, newVerticalValue);

            HorizontalScrollValue = restrictedOffsets.X;
            VerticalScrollValue = restrictedOffsets.Y;
        }

        private (double X, double Y) RestrictImageOffsets(double desiredX, double desiredY)
        {
            var X = desiredX;
            if (X < 0)
                X = 0;
            else if (X > HorizontalScrollMaximum)
                X = HorizontalScrollMaximum;

            var Y = desiredY;
            if (Y < 0)
                Y = 0;
            else if (Y > VerticalScrollMaximum)
                Y = VerticalScrollMaximum;

            return (X, Y);
        }
    }
}
