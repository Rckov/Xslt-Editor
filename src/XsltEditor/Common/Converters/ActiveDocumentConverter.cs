using System.Globalization;
using System.Windows.Data;

using XsltEditor.ViewModels;

namespace XsltEditor.Common.Converters;

internal class ActiveDocumentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is DocumentViewModel ? value : Binding.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is DocumentViewModel ? value : Binding.DoNothing;
    }
}