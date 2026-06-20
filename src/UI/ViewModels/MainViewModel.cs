using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using XsltEditor.Common.Attributes;
using XsltEditor.Extensions;
using XsltEditor.Models.Messages;
using XsltEditor.Sdk.Abstractions;
using XsltEditor.Sdk.Enums;
using XsltEditor.Services;
using XsltEditor.Services.Abstractions;
using XsltEditor.Transform.Enums;
using XsltEditor.Views;

namespace XsltEditor.ViewModels;

[Window(typeof(MainWindow))]
public partial class MainViewModel : ObservableObject
{
    private readonly DispatcherTimer _debounceTimer;
    private readonly ITransformService _transformService;
    private readonly IWindowService _windowService;

    [ObservableProperty] private DocumentViewModel? _activeDocument;
    [ObservableProperty] private string? _htmlContent;
    [ObservableProperty] private string? _xsltVersion;

    public MainViewModel(
        IDocumentFactory factory,
        IWindowService windowService,
        ISettingsService settingsService,
        ITransformService transformService,
        IPluginService pluginService,
        IMessenger messenger)
    {
        _windowService = windowService;
        _transformService = transformService;
        _xsltVersion = ToXsltVersion(settingsService.Settings.EngineType);

        _debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        _debounceTimer.Tick += OnDebounceTimerTick;

        Documents =
        [
            factory.Create("XSL", DocumentType.Xsl),
            factory.Create("XML", DocumentType.Xml)
        ];

        Plugins = pluginService.Plugins;

        foreach (var document in Documents)
        {
            document.PropertyChanged += OnDocumentPropertyChanged;
        }

        messenger.Register<EngineChangedMessage>(this, (_, m) => XsltVersion = ToXsltVersion(m.EngineType));
    }

    public ObservableCollection<DocumentViewModel> Documents { get; }
    public IReadOnlyList<IPlugin> Plugins { get; }
    public bool HasPlugins => Plugins.Count > 0;

    [RelayCommand]
    private void OpenSnippets()
    {
        _windowService.ShowWindow<SnippetViewModel>(dialog: true);
    }

    [RelayCommand]
    private void OpenSettings()
    {
        _windowService.ShowWindow<SettingsViewModel>(dialog: true);
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
            if (document is not null)
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
    private void ExecutePlugin(IPlugin plugin)
    {
        try
        {
            var context = new DocumentContext(Documents, HtmlContent);
            plugin.Execute(context);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Plugin Error");
        }
    }

    private void OnDocumentPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(DocumentViewModel.Content))
        {
            return;
        }

        _debounceTimer.Stop();
        _debounceTimer.Start();
    }

    private async void OnDebounceTimerTick(object? sender, EventArgs e)
    {
        _debounceTimer.Stop();

        try
        {
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

    private string ToXsltVersion(EngineType engine)
    {
        return engine is EngineType.Saxon ? "XSLT 3.0" : "XSLT 1.0";
    }
}