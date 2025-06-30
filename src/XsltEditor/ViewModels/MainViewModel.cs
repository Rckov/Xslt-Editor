using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using XsltEditor.Common.Attributes;
using XsltEditor.Extensions;
using XsltEditor.Models.Enums;
using XsltEditor.Services.Interfaces;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[Window(typeof(MainWindow))]
internal partial class MainViewModel : ObservableObject
{
    private const int DebounceMilliseconds = 500;

    private readonly ISettingsService _settingsService;
    private readonly IThemeService _themeService;
    private readonly IXmlTransformService _transformService;
    private readonly IWindowService _windowService;

    [ObservableProperty] private string? _htmlContent;
    [ObservableProperty] private DocumentViewModel? _activeDocument;

    private DispatcherTimer? _debounceTimer;

    public MainViewModel(
        IWindowService windowService,
        IThemeService themeService,
        ISettingsService settingsService,
        IXmlTransformService transformService)
    {
        _windowService = windowService;
        _themeService = themeService;
        _settingsService = settingsService;
        _transformService = transformService;

        Documents =
        [
            CreateDocument("XSL", DocumentType.Xsl),
            CreateDocument("XML", DocumentType.Xml)
        ];

        InitializeSettings();
        InitializeDebounceTimer();
    }

    public ObservableCollection<DocumentViewModel> Documents { get; }

    [RelayCommand]
    private void OpenCompletionView()
    {
        _windowService.ShowDialog<CompletionViewModel>();
    }

    [RelayCommand]
    private async Task SaveDocument()
    {
        if (ActiveDocument is null)
        {
            return;
        }

        try
        {
            await ActiveDocument.SaveDocumentCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error Saving the Document");
        }
    }

    [RelayCommand]
    private async Task OpenDocument(DocumentType documentType)
    {
        try
        {
            var document = Documents.GetDocument(documentType);
            if (document != null)
            {
                await document.OpenDocumentCommand.ExecuteAsync(null);
                ActiveDocument = document;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error Opening the Document");
        }
    }

    [RelayCommand]
    private void ChangeTheme()
    {
        _themeService.SwitchTheme();

        _settingsService.Settings.Theme = _themeService.CurrentThemeType;
        _settingsService.SaveSettings(_settingsService.Settings);
    }

    [RelayCommand]
    private void ExitApplication()
    {
        Environment.Exit(0);
    }

    private void InitializeSettings()
    {
        _themeService.SetTheme(_settingsService.Settings.Theme);
    }

    private DocumentViewModel CreateDocument(string name, DocumentType documentType)
    {
        // (#a) move to factory
        var document = new DocumentViewModel(
            App.Services.GetRequiredService<IMessenger>(),
            App.Services.GetRequiredService<IFileDialogService>(),
            App.Services.GetRequiredService<IDocumentStorageService>(),
            App.Services.GetRequiredService<IWindowService>(),
            App.Services.GetRequiredService<ICompletionDataService>())
        {
            Id = Guid.NewGuid(),
            Name = name,
            DocumentType = documentType
        };

        AttachDocumentEvents(document);
        return document;
    }

    private void AttachDocumentEvents(DocumentViewModel document)
    {
        document.PropertyChanged += Document_PropertyChanged;
    }

    private void InitializeDebounceTimer()
    {
        _debounceTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(DebounceMilliseconds)
        };
        _debounceTimer.Tick += OnDebounceTimerTick;
    }

    private void Document_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(DocumentViewModel.Content) || _debounceTimer == null)
        {
            return;
        }

        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private async void OnDebounceTimerTick(object? sender, EventArgs e)
    {
        try
        {
            _debounceTimer?.Stop();

            var xsl = Documents.GetDocument(DocumentType.Xsl);
            var xml = Documents.GetDocument(DocumentType.Xml);

            if (string.IsNullOrWhiteSpace(xsl?.Content) || string.IsNullOrWhiteSpace(xml?.Content))
            {
                HtmlContent = string.Empty;
                return;
            }

            HtmlContent = await _transformService.TransformAsync(xml.Content, xsl.Content, xsl.FilePath);
        }
        catch
        {
            HtmlContent = "Error transformation.";
        }
    }
}
