using System.Xml;

using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Interfaces;

using XsltEditor.Transform.Xslt;

namespace XsltEditor.Transform;

public class Transformer
{
    private ITransform? _transform;

    public void Create(EngineType engineType)
    {
        _transform = engineType switch
        {
            EngineType.Saxon => new SaxonTransformer(),
            EngineType.XslCompiledTransform => new XslCompiledTransformer(),
            _ => throw new NotImplementedException(),
        };
    }

    public Task<string> TransformAsync(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null)
    {
        if (_transform is null)
        {
            throw new NotImplementedException(nameof(_transform));
        }

        return _transform.TransformAsync(xml, schema, resolver);
    }
}