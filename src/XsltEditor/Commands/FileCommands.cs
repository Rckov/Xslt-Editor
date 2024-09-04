using System.Diagnostics;
using System.IO;

using XsltEditor.Commands.Base;
using XsltEditor.Models;

namespace XsltEditor.Commands;

internal class SaveRelayCommand : BaseCommand
{
    public override void Execute(object? parameter)
    {
        if (parameter is FileDocument document)
        {
            document.Save();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        if (parameter is FileDocument document)
        {
            return document.HasChanged;
        }

        return false;
    }
}

internal class OpenFileRelayCommand : BaseCommand
{
    public override void Execute(object? parameter)
    {
        if (parameter is FileDocument document)
        {
            document.Open();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }
}

internal class OpenFolderRelayCommand : BaseCommand
{
    public override void Execute(object? parameter)
    {
        if (parameter is FileDocument document)
        {
            if (File.Exists(document.FileName))
            {
                using var prc = Process.Start("explorer.exe", "/select, \"" + document.FileName + "\"");
            }
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return true;
    }
}