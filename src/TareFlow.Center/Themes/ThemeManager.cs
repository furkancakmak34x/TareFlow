using System.Windows;
using System.Windows.Media;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;

namespace TareFlow.Center.Themes
{
    public enum Theme
    {
        Dark,
        Midnight,
        Magma,
        Ocean,
        Nebula,

        Light,
        Snow,
        Sky,
        Mint,
        Peach
    }

    public sealed class ThemeOption
    {
        public ThemeOption(Theme key, string displayName, string resourcePath, bool isDark)
        {
            Key = key;
            DisplayName = displayName;
            ResourcePath = resourcePath;
            IsDark = isDark;
            Source = new Uri(resourcePath, UriKind.Relative);
        }

        public Theme Key { get; }
        public string DisplayName { get; }
        public string ResourcePath { get; }
        public bool IsDark { get; }
        public Uri Source { get; }

        public override string ToString() => DisplayName;
    }

    public static class ThemeManager
    {
        private static readonly IReadOnlyDictionary<string, string> BrushColorMappings = new Dictionary<string, string>
        {
            ["PrimaryBrush"] = "PrimaryColor",
            ["SecondaryBrush"] = "SecondaryColor",
            ["BackgroundBrush"] = "BackgroundColor",
            ["SurfaceBrush"] = "SurfaceColor",
            ["SurfaceVariantBrush"] = "SurfaceVariantColor",
            ["InputBrush"] = "InputColor",
            ["TextPrimaryBrush"] = "TextPrimaryColor",
            ["TextSecondaryBrush"] = "TextSecondaryColor",
            ["BorderBrush"] = "BorderColor",
            ["AccentBrush"] = "AccentColor",
            ["ErrorBrush"] = "ErrorColor",
            ["SuccessBrush"] = "SuccessColor",
            ["WarningBrush"] = "WarningColor",
            ["InfoBrush"] = "InfoColor",
            ["NeutralBrush"] = "NeutralColor",
            ["ErrorSoftBrush"] = "ErrorSoftColor",
            ["SuccessSoftBrush"] = "SuccessSoftColor",
            ["WarningSoftBrush"] = "WarningSoftColor",
            ["InfoSoftBrush"] = "InfoSoftColor",
            ["AccentSoftBrush"] = "AccentSoftColor",
            ["DisabledBrush"] = "DisabledColor",
            ["OnPrimaryBrush"] = "OnPrimaryColor",
            ["OnSecondaryBrush"] = "OnSecondaryColor",
            ["OnAccentBrush"] = "OnAccentColor",
            ["OnSurfaceBrush"] = "OnSurfaceColor",
            ["HoverOverlayBrush"] = "HoverOverlayColor",
            ["PressedOverlayBrush"] = "PressedOverlayColor",
            ["ShadowBrush"] = "ShadowColor",
            ["FocusBrush"] = "FocusColor",
            ["BgBrush"] = "BackgroundColor",
            ["CardBrush"] = "SurfaceColor",
            ["InputBg"] = "InputColor",
            ["TextPrimary"] = "TextPrimaryColor",
            ["TextSecondary"] = "TextSecondaryColor",
            ["AccentColorBrush"] = "AccentColor"
        };

        private static readonly IReadOnlyList<string> ThemeColorKeys = BrushColorMappings.Values
            .Distinct(StringComparer.Ordinal)
            .ToList();

        private static readonly IReadOnlyDictionary<Theme, ThemeOption> ThemeOptions = new Dictionary<Theme, ThemeOption>
        {
            [Theme.Dark] = new ThemeOption(Theme.Dark, "Koyu", "Themes/Colors.Dark.xaml", true),
            [Theme.Midnight] = new ThemeOption(Theme.Midnight, "Gece", "Themes/Colors.Midnight.xaml", true),
            [Theme.Magma] = new ThemeOption(Theme.Magma, "Magma", "Themes/Colors.Magma.xaml", true),
            [Theme.Ocean] = new ThemeOption(Theme.Ocean, "Okyanus", "Themes/Colors.Ocean.xaml", true),
            [Theme.Nebula] = new ThemeOption(Theme.Nebula, "Bulutsu", "Themes/Colors.Nebula.xaml", true),

            [Theme.Light] = new ThemeOption(Theme.Light, "Açık", "Themes/Colors.Light.xaml", false),
            [Theme.Snow] = new ThemeOption(Theme.Snow, "Kar", "Themes/Colors.Snow.xaml", false),
            [Theme.Sky] = new ThemeOption(Theme.Sky, "Gökyüzü", "Themes/Colors.Sky.xaml", false),
            [Theme.Mint] = new ThemeOption(Theme.Mint, "Nane", "Themes/Colors.Mint.xaml", false),
            [Theme.Peach] = new ThemeOption(Theme.Peach, "Şeftali", "Themes/Colors.Peach.xaml", false)
        };

        public static IReadOnlyList<ThemeOption> AvailableThemes => ThemeOptions.Values.ToList();

        public static Theme CurrentTheme { get; private set; } = Theme.Dark;

        public static event EventHandler<Theme>? ThemeChanged;

        public static readonly DependencyProperty CurrentThemeProperty = DependencyProperty.RegisterAttached(
            "CurrentTheme",
            typeof(Theme),
            typeof(ThemeManager),
            new PropertyMetadata(Theme.Dark, OnCurrentThemeChanged));

        public static void SetCurrentTheme(DependencyObject element, Theme value)
        {
            element.SetValue(CurrentThemeProperty, value);
        }

        public static Theme GetCurrentTheme(DependencyObject element)
        {
            return (Theme)element.GetValue(CurrentThemeProperty);
        }

        public static ThemeOption GetOption(Theme theme)
        {
            return ThemeOptions.TryGetValue(theme, out var option)
                ? option
                : ThemeOptions[Theme.Dark];
        }

        public static Theme ParseTheme(string? input, Theme fallback = Theme.Dark)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return fallback;
            }

            if (Enum.TryParse(input, true, out Theme parsed))
            {
                return parsed;
            }

            var normalized = input.Trim();
            var match = ThemeOptions.Values.FirstOrDefault(o =>
                string.Equals(o.DisplayName, normalized, StringComparison.CurrentCultureIgnoreCase)
                || string.Equals(o.DisplayName, normalized, StringComparison.InvariantCultureIgnoreCase));

            return match?.Key ?? fallback;
        }

        public static void ApplyTheme(string? inputTheme, Theme fallback = Theme.Dark)
        {
            ApplyTheme(ParseTheme(inputTheme, fallback));
        }

        public static void ApplyTheme(Theme theme)
        {
            if (Application.Current == null)
            {
                return;
            }

            var option = GetOption(theme);
            var dictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (var dictionary in dictionaries.Where(IsThemeDictionary).ToList())
            {
                dictionaries.Remove(dictionary);
            }

            dictionaries.Add(new ResourceDictionary { Source = option.Source });
            RefreshThemeColors(option.Source);
            RefreshThemeBrushes();

            CurrentTheme = theme;
            ThemeChanged?.Invoke(null, theme);
        }

        private static void RefreshThemeBrushes()
        {
            if (Application.Current == null)
            {
                return;
            }

            foreach (var mapping in BrushColorMappings)
            {
                var color = TryGetColor(mapping.Value);
                if (!color.HasValue)
                {
                    continue;
                }

                if (Application.Current.Resources[mapping.Key] is SolidColorBrush existingBrush)
                {
                    if (existingBrush.IsFrozen)
                    {
                        Application.Current.Resources[mapping.Key] = new SolidColorBrush(color.Value);
                    }
                    else
                    {
                        existingBrush.Color = color.Value;
                    }
                }
                else
                {
                    Application.Current.Resources[mapping.Key] = new SolidColorBrush(color.Value);
                }
            }
        }

        private static void RefreshThemeColors(Uri themeSource)
        {
            if (Application.Current == null)
            {
                return;
            }

            var themeDictionary = new ResourceDictionary { Source = themeSource };
            foreach (var colorKey in ThemeColorKeys)
            {
                if (!themeDictionary.Contains(colorKey))
                {
                    continue;
                }

                if (themeDictionary[colorKey] is Color color)
                {
                    Application.Current.Resources[colorKey] = color;
                }
            }
        }

        private static Color? TryGetColor(string resourceKey)
        {
            if (Application.Current == null)
            {
                return null;
            }

            var value = Application.Current.TryFindResource(resourceKey);
            if (value is Color color)
            {
                return color;
            }

            if (value is SolidColorBrush brush)
            {
                return brush.Color;
            }

            return null;
        }

        private static void OnCurrentThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Theme theme)
            {
                ApplyTheme(theme);
            }
        }

        private static bool IsThemeDictionary(ResourceDictionary dictionary)
        {
            var source = dictionary?.Source?.OriginalString;
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            return source.IndexOf("Themes/Colors.", StringComparison.InvariantCultureIgnoreCase) >= 0
                && source.EndsWith(".xaml", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
