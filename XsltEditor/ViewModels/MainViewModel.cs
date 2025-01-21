using System.ComponentModel;
using System.Runtime.Versioning;
using System.Windows.Input;

using XsltEditor.Core;
using XsltEditor.Models.Base;
using XsltEditor.Services.Interfaces;
using XsltEditor.Tools;
using XsltEditor.Transform.Enums;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class MainViewModel : ObservableObject
{
    private readonly IWindowService _windowService;
    private readonly IXmlTransformService _transformService;

    public MainViewModel(IWindowService windowService, IXmlTransformService transformService)
    {
        _windowService = windowService;

        _transformService = transformService;
        _transformService.Create(EngineType.XslCompiledTransform);

        InitCommands();
        SubscribeEvents();
    }

    public string? HtmlText
    {
        get;
        set => Set(ref field, value);
    }

    public DocumentViewModel XslDocument { get; } = new(".xsl");
    public DocumentViewModel XmlDocument { get; } = new(".xml");

    public ICommand? OpenFileCommand { get; private set; }
    public ICommand? SwitchThemeCommand { get; private set; }
    public ICommand? OpenCompletionWindowCommand { get; private set; }

    private void InitCommands()
    {
        SwitchThemeCommand = new RelayCommand(SwitchTheme);
        OpenCompletionWindowCommand = new RelayCommand(OpenCompletionWindow);
    }

    private void SubscribeEvents()
    {
        XslDocument.PropertyChanged += OnTextPropertyChanged;
        XmlDocument.PropertyChanged += OnTextPropertyChanged;
    }

    private void SwitchTheme(object? obj)
    {
        var curTheme = ThemeManager.CurrentTheme;
        var newTheme = curTheme == ThemeType.Dark ? ThemeType.Light : ThemeType.Dark;

        ThemeManager.Apply(newTheme);
    }

    private void OpenCompletionWindow(object? parameter)
    {
        _windowService.ShowDialogWindow<CompletionView>();
    }

    private async void OnTextPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Content))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(XslDocument.Content) || string.IsNullOrWhiteSpace(XmlDocument.Content))
        {
            HtmlText = string.Empty;
            return;
        }

        HtmlText = await _transformService.TransformAsync(XslDocument.Content, XmlDocument.Content, XslDocument.FilePath);
    }
}