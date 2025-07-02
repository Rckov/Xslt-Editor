using System.Text;
using System.Xml;
using System.Xml.Xsl;

using XsltEditor.Transform.Engines.Base;

namespace XsltEditor.Transform.Engines;

internal class XslCompiledEngine : BaseEngine
{
    private readonly StringBuilder _stringBuilder = new();
    private readonly XslCompiledTransform _compiledTransform = new(true);

    private readonly XsltSettings _settings = new(true, true);
    private readonly XmlWriterSettings _writerSettings;

    public XslCompiledEngine()
    {
        _writerSettings = new XmlWriterSettings
        {
            Indent = false,
            OmitXmlDeclaration = true,
            Encoding = Encoding.UTF8,
            ConformanceLevel = ConformanceLevel.Fragment,
            Async = false
        };
    }

    protected override string Transform(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
    {
        try
        {
            using var xmlWriter = XmlWriter.Create(_stringBuilder, _writerSettings);
            _compiledTransform.Load(schema, _settings, resolver);
            _compiledTransform.Transform(xml, null, xmlWriter, resolver);
        }
        catch (Exception ex)
        {
            _stringBuilder.AppendLine(ex.Message);
            if (ex.InnerException != null)
            {
                _stringBuilder.AppendLine(ex.InnerException.Message);
            }
        }

        return _stringBuilder.ToString();
    }
}