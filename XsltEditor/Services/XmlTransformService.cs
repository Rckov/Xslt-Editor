using System.Runtime.Versioning;
using System.Xml;
using System.Xml.Linq;

using XsltEditor.Services.Interfaces;
using XsltEditor.Transform;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Tools;

namespace XsltEditor.Services;

[SupportedOSPlatform("windows")]
internal class XmlTransformService : IXmlTransformService
{
    private readonly Transformer _transformer = new();
    private readonly XsltUriResolver _xsltUriResolver = new();

    public void Create(EngineType engineType)
    {
        _transformer.Create(engineType);
    }

    public async Task<string> TransformAsync(string xsl, string xml, string? rootPath = null)
    {
        try
        {
            using var xslReader = GetReader(xsl);
            using var xmlReader = GetReader(xml);

            if (!string.IsNullOrWhiteSpace(rootPath))
            {
                _xsltUriResolver.SetBaseUri(rootPath);
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