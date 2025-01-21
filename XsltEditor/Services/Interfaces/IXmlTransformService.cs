using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Interfaces;

public interface IXmlTransformService
{
    void Create(EngineType engineType);

    Task<string> TransformAsync(string xml, string xsl, string? rootPath = null);
}