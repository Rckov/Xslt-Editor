using System.Text;
using javax.xml.transform.stream;
using net.liberty_development.SaxonHE12s9apiExtensions;
using net.sf.saxon.lib;
using net.sf.saxon.s9api;
using StringReader = java.io.StringReader;

namespace XsltEditor.Transform.Engines;

internal sealed class SaxonEngine : IXsltEngine
{
    private static readonly Processor Processor = new(false);

    private readonly XsltCompiler _compiler = Processor.newXsltCompiler();
    private readonly DocumentBuilder _documentBuilder = Processor.newDocumentBuilder();
    private readonly StringBuilder _errors = new();

    private string? _cachedXml;
    private XdmNode? _cachedXmlNode;

    public SaxonEngine()
    {
        _compiler.setErrorReporter(new CollectingErrorReporter(_errors));
    }

    public Task<string> TransformAsync(string xml, string xsl, string? baseUri = null)
    {
        return Task.Run(() => Transform(xml, xsl, baseUri));
    }

    private string Transform(string xml, string xsl, string? baseUri)
    {
        var executable = CompileXsl(xsl, baseUri);
        if (executable is null)
        {
            return _errors.ToString();
        }

        try
        {
            var source = GetOrParseXml(xml);

            using var writer = new StringWriter();
            var serializer = Processor.NewSerializer(writer);

            var transformer = executable.load30();
            transformer.setGlobalContextItem(source);
            transformer.applyTemplates(source, serializer);

            return writer.ToString();
        }
        catch (SaxonApiException ex)
        {
            return ex.getMessage() ?? ex.Message;
        }
    }

    private XdmNode GetOrParseXml(string xml)
    {
        if (_cachedXmlNode is not null && _cachedXml == xml)
        {
            return _cachedXmlNode;
        }

        var source = new StreamSource(new StringReader(xml));
        _cachedXmlNode = _documentBuilder.build(source);
        _cachedXml = xml;
        return _cachedXmlNode;
    }

    private XsltExecutable? CompileXsl(string xsl, string? baseUri)
    {
        _errors.Clear();

        try
        {
            var source = new StreamSource(new StringReader(xsl));
            if (baseUri is not null)
            {
                source.setSystemId(baseUri);
            }

            return _compiler.compile(source);
        }
        catch (SaxonApiException)
        {
            return null;
        }
    }

    private sealed class CollectingErrorReporter(StringBuilder errors) : ErrorReporter
    {
        public void report(XmlProcessingError error)
        {
            var loc = error.getLocation();
            var line = loc?.getLineNumber() ?? -1;
            var col = loc?.getColumnNumber() ?? -1;

            if (errors.Length > 0)
            {
                errors.AppendLine();
            }

            errors.Append($"[Line {line}, Col {col}] {error.getMessage()}");
        }
    }
}