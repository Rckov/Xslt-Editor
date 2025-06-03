using XsltEditor.Services.Interfaces;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services;

internal class XmlTransformService : IXmlTransformService
{
    public void CreateEngine(EngineType engineType)
    {
    }

    public async Task<string> TransformAsync(string xml, string xsl, string? rootPath)
    {
        await Task.CompletedTask;
        return string.Empty;
    }
}