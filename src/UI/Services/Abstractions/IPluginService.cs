using XsltEditor.Sdk.Abstractions;

namespace XsltEditor.Services.Abstractions;

public interface IPluginService
{
    IReadOnlyList<IPlugin> Plugins { get; }
}