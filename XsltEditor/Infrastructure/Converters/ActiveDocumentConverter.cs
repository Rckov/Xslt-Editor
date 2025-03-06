using System.Windows.Data;

using XsltEditor.ViewModels;

namespace XsltEditor.Infrastructure.Converters;

internal class ActiveDocumentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DocumentViewModel)
        {
            return value;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DocumentViewModel)
        {
            return value;
        }

        return Binding.DoNothing;
    }
}