using System.Runtime.Versioning;
using System.Xml;
using System.Xml.Linq;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Tools;

namespace XsltEditor.Services;

[SupportedOSPlatform("windows")]
internal class XmlTransformService : IXmlTransformService
{
    private readonly Transformer _transformer;
    private readonly XsltUriResolver _xsltUriResolver;

    public XmlTransformService()
    {
        _transformer = new Transformer();
        _xsltUriResolver = new XsltUriResolver();
    }

    public void Create(EngineType engineType)
    {
        _transformer.Create(engineType);
    }

    public async Task<string> TransformAsync(TextDocument xsl, TextDocument xml)
    {
        try
        {
            using var xslReader = GetReader(xsl.Content);
            using var xmlReader = GetReader(xml.Content);

            if (!string.IsNullOrWhiteSpace(xsl.FilePath))
            {
                _xsltUriResolver.SetBaseUri(xsl.FilePath);
            }

            return await _transformer.TransformAsync(xmlReader, xslReader, _xsltUriResolver);
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    private static XmlReader GetReader(string? xmlText)
    {
        if (string.IsNullOrWhiteSpace(xmlText))
        {
            throw new ArgumentNullException(nameof(xmlText));
        }

        return XElement.Parse(xmlText).CreateReader();
    }
}