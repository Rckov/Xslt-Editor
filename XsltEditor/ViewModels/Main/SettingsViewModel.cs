using System.Collections.ObjectModel;

using XsltEditor.Models.Base;
using XsltEditor.Services;

namespace XsltEditor.ViewModels.Main;

public class SettingsViewModel : ObservableObject
{
    public SettingsViewModel()
    {
        FillThemes();
    }

    public ObservableCollection<ThemeType> Themes { get; } = [];
    public ObservableCollection<int> FontSize { get; } = [8, 10, 12, 14, 16, 18, 20];

    public ThemeType SelectedTheme
    {
        get => ThemeManager.CurrentTheme;
        set
        {
            if (Set(ref field, value))
            {
                ThemeManager.Apply(value);
            }
        }
    }

    private void FillThemes()
    {
        foreach (ThemeType theme in Enum.GetValues(typeof(ThemeType)))
        {
            Themes.Add(theme);
        }
    }

}