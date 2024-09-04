using System.Xml;

namespace XsltEditor.Transform;

public class XsltUriResolver : XmlUrlResolver
{
    public XsltUriResolver()
    { }

    public XsltUriResolver(Uri baseUri) => BaseUri = baseUri;

    public XsltUriResolver(string baseUri) => BaseUri = new Uri(baseUri);

    public Uri? BaseUri { get; set; }

    public override object? GetEntity(Uri absoluteUri, string? role, Type? ofObjectToReturn)
    {
        return base.GetEntity(absoluteUri, role, ofObjectToReturn);
    }

    public override Uri ResolveUri(Uri? baseUri, string? relativeUri)
    {
        return base.ResolveUri(BaseUri ?? baseUri, relativeUri);
    }

    public void SetBaseUri(string uri)
    {
        if (BaseUri is null || BaseUri.OriginalString != uri)
        {
            BaseUri = new Uri(uri);
        }
    }
}