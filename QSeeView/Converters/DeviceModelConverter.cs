using QSeeView.Tools;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace QSeeView.Converters
{
    // Changes a device model to a string
    public class DeviceModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Enum.IsDefined(typeof(DeviceModelType), value))
            {
                var deviceModel = (DeviceModelType)value;
                var fieldInfo = deviceModel.GetType().GetField(deviceModel.ToString());
                var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                    return attributes[0].Description;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
