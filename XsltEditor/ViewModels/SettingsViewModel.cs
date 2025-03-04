using System.Collections.ObjectModel;
using System.Windows.Input;
using XsltEditor.Infrastructure;
using XsltEditor.Models.Base;
using XsltEditor.Services;

namespace XsltEditor.ViewModels;

public class SettingsViewModel : ObservableObject
{
    private ThemeType _selectedTheme;

    public ObservableCollection<ThemeType> Themes { get; } = new()
    {
        ThemeType.Light,
        ThemeType.Dark
    };

    public ThemeType SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (Set(ref _selectedTheme, value))
            {
                ThemeManager.Apply(value);
            }
        }
    }

    public Action? CloseWindow { get; set; }

    public string? FontFamily
    {
        get;
        set => Set(ref field, value);
    }

    public int FontSize
    {
        get;
        set => Set(ref field, value);
    }

    public bool ShowLineNumbers
    {
        get;
        set => Set(ref field, value);
    }

    public bool ConvertTabsToSpaces
    {
        get;
        set => Set(ref field, value);
    }

    public bool EnableEmailHyperlinks
    {
        get;
        set => Set(ref field, value);
    }

    public bool EnableHyperlinks
    {
        get;
        set => Set(ref field, value);
    }

    public bool EnableCodeFolding
    {
        get;
        set => Set(ref field, value);
    }

    public int TabSize
    {
        get;
        set => Set(ref field, value);
    }

    public int IndentSize
    {
        get;
        set => Set(ref field, value);
    }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public SettingsViewModel()
    {
        _selectedTheme = ThemeManager.CurrentTheme;
        
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(Cancel);
    }

    private void Save(object? parameter)
    {
        // TODO: Save settings
        CloseWindow?.Invoke();
    }

    private void Cancel(object? parameter)
    {
        CloseWindow?.Invoke();
    }
} 