using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Interfaces;

internal interface IXmlTransformService
{
    void CreateEngine(EngineType engineType);

    Task<string> TransformAsync(string xmlContent, string xslContent, string? rootPath);
}