using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TareFlow.Center.Views;

/// <summary>true→Visible, false→Collapsed.</summary>
public sealed class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is Visibility.Visible;
}

/// <summary>true→Collapsed, false→Visible.</summary>
public sealed class InverseBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is Visibility.Collapsed;
}

/// <summary>true→yeşil, false→kırmızı (bağlantı göstergesi).</summary>
public sealed class BoolToConnectionBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50))
                         : new SolidColorBrush(Color.FromRgb(0xE5, 0x39, 0x35));

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Boolean tersleyici (x:Static ile XAML'de kullanılır).</summary>
public sealed class NotConverter : IValueConverter
{
    public static readonly NotConverter Instance = new();

    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is not true;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is not true;
}

/// <summary>null değilse true (örn. bir öğe seçiliyken paneli etkinleştirmek için).</summary>
public sealed class NotNullToBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is not null;

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>true→"Bağlı", false→"Bağlantı yok".</summary>
public sealed class BoolToConnectionTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? "Bağlı" : "Bağlantı yok";

    public object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
