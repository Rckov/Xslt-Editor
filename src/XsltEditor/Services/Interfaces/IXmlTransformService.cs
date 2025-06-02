using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Interfaces;

internal interface IXmlTransformService
{
    void CreateEngine(EngineType engineType);

    Task<string> TransformAsync(string xml, string xsl, string? rootPath);
}