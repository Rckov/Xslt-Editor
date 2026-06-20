using System.Xml;

namespace XsltEditor.Transform.Tools;

internal sealed class XsltUriResolver : XmlUrlResolver
{
    private Uri? _baseUri;

    public override Uri ResolveUri(Uri? baseUri, string? relativeUri)
    {
        return base.ResolveUri(_baseUri ?? baseUri, relativeUri);
    }

    public void SetBaseUri(string? uri)
    {
        _baseUri = uri is not null ? new Uri(uri) : null;
    }
}