using System.Xml;

using XsltEditor.Transform.Enums;

namespace XsltEditor.Transform.Interfaces;

public interface ITransformer
{
    void Create(EngineType engineType);

    Task<string> TransformAsync(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null);
}