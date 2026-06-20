using System.Text;
using System.Xml;
using System.Xml.Xsl;
using XsltEditor.Transform.Tools;

namespace XsltEditor.Transform.Engines;

internal sealed class XslCompiledEngine : IXsltEngine
{
    private readonly XmlReaderSettings _readerSettings = new()
    {
        DtdProcessing = DtdProcessing.Prohibit
    };

    private readonly XsltUriResolver _resolver = new();
    private readonly XslCompiledTransform _transform = new();

    private readonly XmlWriterSettings _writerSettings = new()
    {
        Indent = false,
        OmitXmlDeclaration = true,
        Encoding = Encoding.UTF8,
        ConformanceLevel = ConformanceLevel.Fragment
    };

    private readonly XsltSettings _xsltSettings = new(true, true);

    public Task<string> TransformAsync(string xml, string xsl, string? baseUri = null)
    {
        return Task.Run(() => Transform(xml, xsl, baseUri));
    }

    private string Transform(string xml, string xsl, string? baseUri)
    {
        _resolver.SetBaseUri(baseUri);

        try
        {
            using var xslReader = XmlReader.Create(new StringReader(xsl), _readerSettings);
            using var xmlReader = XmlReader.Create(new StringReader(xml), _readerSettings);

            var output = new StringBuilder();
            using var writer = XmlWriter.Create(output, _writerSettings);

            _transform.Load(xslReader, _xsltSettings, _resolver);
            _transform.Transform(xmlReader, null, writer, _resolver);

            return output.ToString();
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            if (ex.InnerException is not null)
            {
                message += Environment.NewLine + ex.InnerException.Message;
            }

            return message;
        }
    }
}