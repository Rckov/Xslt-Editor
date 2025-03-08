using System.IO;
using System.Xml;

using XsltEditor.Services.Interfaces;
using XsltEditor.Transform;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Tools;

namespace XsltEditor.Services.Implementation;

internal sealed class XmlTransformService : IXmlTransformService
{
    private readonly Transformer _transformer;
    private readonly XsltUriResolver _xsltUriResolver;

    private readonly XmlReaderSettings _readerSettings;

    public XmlTransformService()
    {
        _transformer = new();
        _xsltUriResolver = new();

        _readerSettings = new XmlReaderSettings
        {
            Async = true,
            DtdProcessing = DtdProcessing.Prohibit,
        };
    }

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
        catch (XmlException ex)
        {
            return $"Error XML: {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Error transformation: {ex.Message}";
        }
    }

    private XmlReader GetReader(string xmlText)
    {
        return XmlReader.Create(new StringReader(xmlText), _readerSettings);
    }
}