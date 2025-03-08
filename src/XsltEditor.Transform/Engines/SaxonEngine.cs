using System.Xml;

using XsltEditor.Transform.Engines.Base;

namespace XsltEditor.Transform.Engines;

internal class SaxonEngine : BaseEngine
{
    protected override string Transform(XmlReader xml, XmlReader schema, XmlUrlResolver? resolver = null)
    {
        throw new NotImplementedException("Saxon is not implemented");
    }
}