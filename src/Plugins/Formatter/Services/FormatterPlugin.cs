using System.Xml;
using System.Xml.Linq;

namespace Formatter.Services;

internal class FormatterPlugin
{
    public string? Format(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return content;
        }

        try
        {
            var doc = XDocument.Parse(content);

            using var writer = new StringWriter();
            using var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = doc.Declaration is null
            });

            doc.WriteTo(xmlWriter);
            xmlWriter.Flush();

            return writer.ToString();
        }
        catch
        {
            return content;
        }
    }
}