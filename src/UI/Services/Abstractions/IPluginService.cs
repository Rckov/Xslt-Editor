using XsltEditor.Sdk.Abstractions;

namespace XsltEditor.Services.Abstractions;

internal interface IPluginService
{
	IReadOnlyList<IPlugin> Plugins { get; }
}
