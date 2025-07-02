using Microsoft.Extensions.DependencyInjection;

using XsltEditor.Models.Enums;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Test.Services;

public class SettingsServiceTests(ServiceFixture fixture) : IClassFixture<ServiceFixture>
{
    private readonly ISettingsService _settingsService = fixture.Services.GetRequiredService<ISettingsService>();

    [Fact]
    public void Test0()
    {
        // Arrange
        if (File.Exists(fixture.SettingsPath))
        {
            File.Delete(fixture.SettingsPath);
        }

        // Act
        _settingsService.LoadSettings();

        // Assert
        Assert.NotNull(_settingsService.Settings);
        Assert.Equal(ThemeType.Dark, _settingsService.Settings.Theme);
    }

    [Fact]
    public void Test1()
    {
        // Arrange
        _settingsService.LoadSettings();
        _settingsService.Settings.Theme = ThemeType.Dark;

        // Act
        _settingsService.SaveSettings(_settingsService.Settings);
        _settingsService.LoadSettings();

        // Assert
        Assert.Equal(ThemeType.Dark, _settingsService.Settings.Theme);
    }
}