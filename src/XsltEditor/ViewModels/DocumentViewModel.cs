using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;

using XsltEditor.Models.Messages;
using XsltEditor.Services.Implementation;
using XsltEditor.Services.Interfaces;
using XsltEditor.ViewModels.Base;
using XsltEditor.Views.UserControls;

namespace XsltEditor.ViewModels;

[SupportedOSPlatform("windows")]
public class DocumentViewModel : BaseViewModel, IDisposable
{
    private readonly Dictionary<ThemeType, string> _highlightingPaths = [];
    private readonly IMessenger _messenger;

    public DocumentViewModel(IMessenger messenger, ICompletionDataService? completionDataService)
    {
        _messenger = messenger;
        _messenger.Subscribe<ThemeMessage>(OnThemeChanged);
        _messenger.Subscribe<CaretLineMessage>(OnScrollToLine);

        AddHighlighting(ThemeType.Dark, "XsltEditor.Resources.Highlighting.DarkMode.xshd");
        AddHighlighting(ThemeType.Light, "XsltEditor.Resources.Highlighting.LightMode.xshd");

        if (completionDataService != null)
        {
            CompletionData = completionDataService.LoadCompletionData();
        }
    }

    public string? Name
    {
        get;
        init => Set(ref field, value);
    }

    public string? FilePath
    {
        get;
        private set => Set(ref field, value);
    }

    public string? Text
    {
        get;
        set => Set(ref field, value);
    }

    public bool? IsDirty
    {
        get;
        set => Set(ref field, value);
    }

    public bool IsReadOnly
    {
        get;
        set => Set(ref field, value);
    }

    public int Line
    {
        get;
        set => Set(ref field, value);
    }

    public int Column
    {
        get;
        set => Set(ref field, value);
    }

    public Encoding? Encoding
    {
        get;
        set => Set(ref field, value);
    }

    public IHighlightingDefinition? Highlighting
    {
        get;
        private set => Set(ref field, value);
    }

    public IList<CompletionData>? CompletionData { get; private set; }

    public void Dispose()
    {
        _messenger.Unsubscribe<CaretLineMessage>(OnScrollToLine);
        GC.SuppressFinalize(this);
    }

    public async Task OpenDocument(string path)
    {
        try
        {
            await using var fileStream = new FileStream(path, FileMode.Open);
            using var streamReader = new StreamReader(fileStream, true);

            FilePath = path;
            Encoding = streamReader.CurrentEncoding;

            Text = await streamReader.ReadToEndAsync();
            LogInfo($"File opened successfully: {path}");
        }
        catch (Exception ex)
        {
            LogError($"Error opening file: {path}", ex);
            throw;
        }
    }

    public async Task SaveDocument(string path)
    {
        try
        {
            await using var fileStream = new FileStream(path, FileMode.Create);
            await using var streamWriter = new StreamWriter(fileStream);

            await streamWriter.WriteAsync(Text);

            if (string.IsNullOrEmpty(FilePath))
            {
                FilePath = path;
                Encoding = streamWriter.Encoding;
            }

            IsDirty = false;
            LogInfo($"File saved successfully: {path}");
        }
        catch (Exception ex)
        {
            LogError($"Error saving file: {path}", ex);
            throw;
        }
    }

    private void OnThemeChanged(ThemeMessage message)
    {
        var highlighting = _highlightingPaths[message.ThemeType];
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(highlighting);

        if (stream == null)
        {
            return;
        }

        using var reader = XmlReader.Create(stream);
        Highlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
    }

    private void OnScrollToLine(CaretLineMessage message)
    {
        Line = message.Line;
    }

    private void AddHighlighting(ThemeType type, string path)
    {
        _highlightingPaths.Add(type, path);
    }

    protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName is nameof(Text))
        {
            IsDirty = true;
        }
    }
}