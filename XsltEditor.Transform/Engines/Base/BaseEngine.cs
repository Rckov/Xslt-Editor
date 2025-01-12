using System.Xml;

using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Transform.Engines.Base;

internal abstract class BaseEngine : IEngine
{
    public Task<string> TransformAsync(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
    {
        return Task.Run(() => Transform(xml, schema, resolver));
    }

    protected abstract string Transform(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null);
}