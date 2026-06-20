namespace XsltEditor.Sdk.Abstractions;

public interface IPlugin : IDisposable
{
    string Name { get; }
    string Description { get; }

    void Execute(IDocumentContext context);
}