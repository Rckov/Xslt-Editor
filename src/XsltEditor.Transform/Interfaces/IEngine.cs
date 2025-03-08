using System.Xml;

namespace XsltEditor.Transform.Interfaces;

public interface IEngine
{
    Task<string> TransformAsync(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null);
}