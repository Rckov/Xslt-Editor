namespace XsltEditor.Services.Interfaces;

internal interface IXmlTransformService
{
    Task<string?> TransformAsync(string xml, string xsl, string? pathXSL);
}