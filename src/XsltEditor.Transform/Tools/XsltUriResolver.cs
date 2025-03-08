using System.Xml;

namespace XsltEditor.Transform.Tools;

public class XsltUriResolver : XmlUrlResolver
{
    private Uri? BaseUri { get; set; }

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