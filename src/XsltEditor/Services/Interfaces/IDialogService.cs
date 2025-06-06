namespace XsltEditor.Services.Interfaces;

internal interface IDialogService
{
    string? ShowOpenFileDialog(string title, params string[]? allowedExtensions);

    string? ShowSaveFileDialog(string title, params string[]? allowedExtensions);
}