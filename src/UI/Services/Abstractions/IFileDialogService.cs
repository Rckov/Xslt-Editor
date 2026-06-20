namespace XsltEditor.Services.Abstractions;

public interface IFileDialogService
{
    string? OpenFileDialog(string title, params string[] extensions);

    string? OpenSaveDialog(string title, params string[] extensions);
}