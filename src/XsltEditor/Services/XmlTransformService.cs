using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class XmlTransformService : IXmlTransformService
{
    public Task<string?> TransformAsync(string xml, string xsl, string? pathXSL)
    {
        throw new NotImplementedException();
    }
}