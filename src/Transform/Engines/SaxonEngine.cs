using net.liberty_development.SaxonHE12s9apiExtensions;
using net.sf.saxon.lib;
using net.sf.saxon.s9api;

using System.Text;

namespace XsltEditor.Transform.Engines;

internal sealed class SaxonEngine : IXsltEngine
{
	private static readonly Processor _processor = new(false);

	private readonly XsltCompiler _compiler = _processor.newXsltCompiler();
	private readonly DocumentBuilder _documentBuilder = _processor.newDocumentBuilder();
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
		XsltExecutable? executable = CompileXsl(xsl, baseUri);
		if (executable is null)
		{
			return _errors.ToString();
		}

		try
		{
			XdmNode source = GetOrParseXml(xml);

			using var writer = new StringWriter();
			Serializer serializer = _processor.NewSerializer(writer);

			Xslt30Transformer transformer = executable.load30();
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

		var source = new javax.xml.transform.stream.StreamSource(new java.io.StringReader(xml));
		_cachedXmlNode = _documentBuilder.build(source);
		_cachedXml = xml;
		return _cachedXmlNode;
	}

	private XsltExecutable? CompileXsl(string xsl, string? baseUri)
	{
		_errors.Clear();

		try
		{
			var source = new javax.xml.transform.stream.StreamSource(new java.io.StringReader(xsl));
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
			Location? loc = error.getLocation();
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