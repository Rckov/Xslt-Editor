using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions;

public interface ISettingsService
{
    Settings Settings { get; }

    void Save();
}