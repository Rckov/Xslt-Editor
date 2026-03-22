using System.Collections.Frozen;

using XsltEditor.Models;
using XsltEditor.Services.Abstractions.Themes;

namespace XsltEditor.Services.Themes;

internal class ThemeProvider : IThemeProvider
{
	private static readonly FrozenDictionary<ThemeType, ThemeDescriptor> _themes =
		new Dictionary<ThemeType, ThemeDescriptor>
		{
			[ThemeType.Default] = new(ThemeType.Default,
				"pack://application:,,,/Resources/Themes/Brushes/DefaultBrushes.xaml",
				"pack://application:,,,/Resources/Themes/DefaultHighlighting.xshd"),

			[ThemeType.Dark] = new(ThemeType.Dark,
				"pack://application:,,,/Resources/Themes/Brushes/DarkBrushes.xaml",
				"pack://application:,,,/Resources/Themes/DarkHighlighting.xshd")
		}.ToFrozenDictionary();

	public ThemeDescriptor Get(ThemeType type)
	{
		return _themes.TryGetValue(type, out ThemeDescriptor? descriptor)
			? descriptor
			: throw new InvalidOperationException($"Theme '{type}' is not registered.");
	}
}