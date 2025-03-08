using System.Xml;

using XsltEditor.Transform.Engines;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Transform;

public class Transformer
{
    public IEngine? Engine { get; private set; }

    public void Create(EngineType engineType)
    {
        Engine = engineType switch
        {
            //EngineType.Saxon => new SaxonEngine(),
            EngineType.XslCompiledTransform => new XslCompiledEngine(),
            _ => throw new NotImplementedException()
        };
    }

    public Task<string> TransformAsync(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
    {
        if (Engine is null)
        {
            throw new NotImplementedException(nameof(Engine));
        }

        return Engine.TransformAsync(xml, schema, resolver);
    }
}