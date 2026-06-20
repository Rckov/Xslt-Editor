using XsltEditor.Sdk.Abstractions;

namespace XsltEditor.Sdk;

public abstract class PluginBase : IPlugin
{
    public abstract string Name { get; }
    public abstract string Description { get; }

    public abstract void Execute(IDocumentContext context);

    public virtual void Dispose()
    {
    }
}