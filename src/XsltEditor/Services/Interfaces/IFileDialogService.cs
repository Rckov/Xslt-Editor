namespace XsltEditor.Services.Interfaces;

internal interface IFileDialogService
{
    string? OpenFileDialog(string title, params string[] extensions);

    string? OpenSaveDialog(string title, params string[] extensions);
}