using System.Xml;

namespace XsltEditor.Transform.Interfaces;

internal interface ITransform
{
    Task<string> TransformAsync(XmlReader xml, XmlReader schema, XsltUriResolver? resolver = null);
}