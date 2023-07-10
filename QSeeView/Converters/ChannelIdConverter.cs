using System;
using System.Globalization;
using System.Windows.Data;

namespace QSeeView.Converters
{
    // Changes a channel ID from 0-based to 1-based
    public class ChannelIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var success = int.TryParse(value.ToString(), out var channelId);
            if (success)
                return channelId + 1;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
