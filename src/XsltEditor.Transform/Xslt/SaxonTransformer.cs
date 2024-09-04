using System.Xml;

using XsltEditor.Transform.Xslt.Base;

namespace XsltEditor.Transform.Xslt;

internal class SaxonTransformer : BaseTransformer
{
    public override string Transform(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null)
    {
        throw new NotImplementedException();
    }
}