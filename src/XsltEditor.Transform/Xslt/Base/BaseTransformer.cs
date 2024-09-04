using System.Xml;

using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Transform.Xslt.Base;

internal abstract class BaseTransformer : ITransform
{
    public abstract string Transform(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null);

    public Task<string> TransformAsync(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null)
    {
        return Task.Run(() => Transform(xml, schema, resolver));
    }
}