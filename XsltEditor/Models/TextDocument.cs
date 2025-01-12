using System.IO;
using System.Text;

using XsltEditor.Models.Base;

namespace XsltEditor.Models;

public class TextDocument : ObservableObject
{
    public TextDocument(string extension)
    {
        Extension = extension;
    }

    public string? FilePath { get; private set; }
    public string Extension { get; private set; }

    public string? Content
    {
        get;
        set => Set(ref field, value);
    }

    public bool HasChanged
    {
        get;
        set => Set(ref field, value);
    }

    public Encoding? Encoding
    {
        get;
        private set => Set(ref field, value);
    }

    public async Task LoadContent(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        await using var fileStream = new FileStream(filePath, FileMode.Open);
        using var streamReader = new StreamReader(fileStream, true);

        FilePath = filePath;
        Encoding = streamReader.CurrentEncoding;

        Content = await streamReader.ReadToEndAsync();
    }

    public async Task SaveContent(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        await using var fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
        await using var streamWriter = new StreamWriter(fileStream);

        await streamWriter.WriteAsync(Content);

        if (string.IsNullOrEmpty(FilePath))
        {
            FilePath = filePath;
            Encoding = streamWriter.Encoding;
        }

        HasChanged = false;
    }
}