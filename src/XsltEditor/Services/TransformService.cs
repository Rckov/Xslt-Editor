using System.Xml;
using System.Xml.Linq;

using XsltEditor.Models;
using XsltEditor.Transform;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services;

internal class TransformService
{
    private readonly Transformer _transformer;
    private readonly XsltUriResolver _xsltUriResolver;

    public TransformService()
    {
        _transformer = new Transformer();
        _transformer.Create(EngineType.XslCompiledTransform);

        _xsltUriResolver = new XsltUriResolver();
    }

    public async Task<string?> TransformAsync(FileDocument xml, FileDocument xsl)
    {
        try
        {
            using var xmlReader = GetReader(xml.Text);
            using var xslReader = GetReader(xsl.Text);

            if (!string.IsNullOrWhiteSpace(xsl.FileName))
            {
                _xsltUriResolver.SetBaseUri(xsl.FileName);
            }

            return await _transformer.TransformAsync(xmlReader, xslReader, _xsltUriResolver);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private XmlReader GetReader(string? xmlText)
    {
        if (string.IsNullOrWhiteSpace(xmlText))
        {
            throw new ArgumentNullException(nameof(xmlText));
        }

        return XElement.Parse(xmlText).CreateReader();
    }
}