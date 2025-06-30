using System.IO;
using System.Xml;

using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Interfaces;
using XsltEditor.Transform.Tools;

namespace XsltEditor.Services;

internal class XmlTransformService : IXmlTransformService
{
    private readonly ITransformer _transformer;
    private readonly XmlReaderSettings _readerSettings = new()
    {
        Async = true,
        DtdProcessing = DtdProcessing.Prohibit
    };

    private readonly XsltUriResolver _xsltUriResolver = new();

    public XmlTransformService(ITransformer transformer)
    {
        _transformer = transformer;

        // (#b) move to service/settings
        _transformer.Create(EngineType.XslCompiledTransform);
    }

    public async Task<string?> TransformAsync(string xml, string xsl, string? pathXSL)
    {
        try
        {
            using var xmlReader = GetReader(xml);
            using var xslReader = GetReader(xsl);

            if (!string.IsNullOrWhiteSpace(pathXSL))
            {
                _xsltUriResolver.SetBaseUri(pathXSL);
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