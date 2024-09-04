using System.Text;
using System.Xml;
using System.Xml.Xsl;

using XsltEditor.Transform.Xslt.Base;

namespace XsltEditor.Transform.Xslt;

internal class XslCompiledTransformer : BaseTransformer
{
    private readonly XsltSettings _settings;
    private readonly XslCompiledTransform _transform;

    private readonly StringBuilder _stringBuilder = new();

    public XslCompiledTransformer()
    {
        _settings = new(true, true);
        _transform = new(false);
    }

    public override string Transform(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null)
    {
        _stringBuilder.Clear();

        try
        {
            using var xmlWriter = XmlWriter.Create(_stringBuilder, _transform.OutputSettings);

            _transform.Load(schema, _settings, resolver);
            _transform.Transform(xml, null, xmlWriter, resolver);
        }
        catch (Exception ex) when (ex.InnerException is null)
        {
            _stringBuilder.AppendLine(ex.Message);
        }
        catch (Exception ex) when (ex.InnerException is not null)
        {
            _stringBuilder.AppendLine($"{ex.Message}\n{ex.InnerException.Message}");
        }

        return _stringBuilder.ToString();
    }
}