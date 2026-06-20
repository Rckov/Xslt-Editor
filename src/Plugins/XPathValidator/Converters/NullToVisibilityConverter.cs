using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace XPathValidator.Converters;

internal sealed class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is null or "" ? Visibility.Hidden : Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}