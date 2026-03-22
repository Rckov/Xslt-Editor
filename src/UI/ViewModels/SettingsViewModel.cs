using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using XsltEditor.Common.Attributes;
using XsltEditor.Models;
using XsltEditor.Models.Messages;
using XsltEditor.Services.Abstractions;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.Transform.Enums;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[Window(typeof(SettingsWindow))]
internal partial class SettingsViewModel(
	ISettingsService settingsService,
	IThemeService themeService,
	IMessenger messenger) : ObservableObject
{
	private readonly ThemeType _originalTheme = settingsService.Settings.ThemeType;

	[ObservableProperty] private ThemeType _selectedTheme = settingsService.Settings.ThemeType;
	[ObservableProperty] private EngineType _selectedEngine = settingsService.Settings.EngineType;

	public ThemeType[] Themes { get; } = Enum.GetValues<ThemeType>();
	public EngineType[] Engines { get; } = Enum.GetValues<EngineType>();

	partial void OnSelectedThemeChanged(ThemeType value)
	{
		themeService.SetTheme(value);
	}

	[RelayCommand]
	private void Save()
	{
		settingsService.Settings.ThemeType = SelectedTheme;
		settingsService.Settings.EngineType = SelectedEngine;
		settingsService.Save();

		messenger.Send(new EngineChangedMessage(SelectedEngine));
	}

	[RelayCommand]
	private void Cancel()
	{
		themeService.SetTheme(_originalTheme);
	}
}