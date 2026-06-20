using Formatter.Services;
using XsltEditor.Sdk;
using XsltEditor.Sdk.Abstractions;

namespace Formatter;

public class Cmd : PluginBase
{
    public override string Name => "XML Formatter";
    public override string Description => "Formats XML and XSL documents with indentation";

    public override void Execute(IDocumentContext context)
    {
        var formatter = new FormatterPlugin();

        foreach (var doc in context.Documents)
        {
            doc.Content = formatter.Format(doc.Content);
        }
    }
}