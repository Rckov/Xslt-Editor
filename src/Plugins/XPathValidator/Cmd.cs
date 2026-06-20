using XPathValidator.ViewModels;
using XPathValidator.Views;
using XsltEditor.Sdk;
using XsltEditor.Sdk.Abstractions;
using XsltEditor.Sdk.Enums;
using XsltEditor.Sdk.Extensions;

namespace XPathValidator;

public class Cmd : PluginBase
{
    public override string Name => "XPath Validator";
    public override string Description => "Validates XPath expressions in XML document";

    public override void Execute(IDocumentContext context)
    {
        var document = context.GetDocument(DocumentType.Xml);
        if (document is null)
        {
            return;
        }

        var viewModel = new MainViewModel(document);

        var window = new MainWindow
        {
            DataContext = viewModel
        };

        window.Show();
    }
}