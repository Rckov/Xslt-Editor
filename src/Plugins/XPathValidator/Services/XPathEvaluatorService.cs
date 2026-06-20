using System.Xml.Linq;
using System.Xml.XPath;
using XPathValidator.Models;

namespace XPathValidator.Services;

public class XPathEvaluatorService
{
    private readonly XDocument _document;
    private readonly XPathNamespaceResolver _namespaceResolver;

    private XPathEvaluatorService(XDocument document, XPathNamespaceResolver namespaceResolver)
    {
        _document = document;
        _namespaceResolver = namespaceResolver;
    }

    public static XPathEvaluatorService? Create(string? xmlContent)
    {
        if (string.IsNullOrWhiteSpace(xmlContent))
        {
            return null;
        }

        var document = XDocument.Parse(xmlContent);
        var namespaceResolver = new XPathNamespaceResolver(document);

        return new XPathEvaluatorService(document, namespaceResolver);
    }

    public IEnumerable<ExpressionResult> Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            yield break;
        }

        var results = _document.XPathEvaluate(expression, _namespaceResolver.Manager);

        if (results is IEnumerable<object> enumerable)
        {
            foreach (var item in enumerable)
            {
                yield return item switch
                {
                    XElement element => FormatElement(element),
                    XAttribute attribute => FormatAttribute(attribute),
                    _ => new ExpressionResult("Result", item.ToString(), "Value")
                };
            }
        }
        else if (results is not null)
        {
            yield return new ExpressionResult("Result", results.ToString(), "Value");
        }
    }

    private ExpressionResult FormatElement(XElement element)
    {
        var name = GetQualifiedName(element.Name);
        return new ExpressionResult(name, element.Value, "Element");
    }

    private ExpressionResult FormatAttribute(XAttribute attribute)
    {
        var name = $"@{GetQualifiedName(attribute.Name)}";
        return new ExpressionResult(name, attribute.Value, "Attribute");
    }

    private string GetQualifiedName(XName name)
    {
        var prefix = _namespaceResolver.GetPrefix(name.Namespace);
        return string.IsNullOrEmpty(prefix)
            ? name.LocalName
            : $"{prefix}:{name.LocalName}";
    }
}