using System.Xml;

using XsltEditor.Transform.Engines;
using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Transform;

public class Transformer : ITransformer
{
    private IEngine? _engine;

    public void Create(EngineType engineType)
    {
        _engine = engineType switch
        {
            //EngineType.Saxon => new SaxonEngine(),
            EngineType.XslCompiledTransform => new XslCompiledEngine(),
            _ => throw new NotImplementedException()
        };
    }

    public Task<string> TransformAsync(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
    {
        if (_engine is null)
        {
            throw new NotImplementedException(nameof(_engine));
        }

        return _engine.TransformAsync(xml, schema, resolver);
    }
}