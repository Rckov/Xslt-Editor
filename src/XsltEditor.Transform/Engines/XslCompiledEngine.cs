using System.Text;
using System.Xml;
using System.Xml.Xsl;

using XsltEditor.Transform.Engines.Base;

namespace XsltEditor.Transform.Engines;

internal class XslCompiledEngine : BaseEngine
{
    private readonly XsltSettings _settings = new(true, true);

    private readonly StringBuilder _stringBuilder = new();
    private readonly XslCompiledTransform _transform = new(false);

    protected override string Transform(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
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