using XsltEditor.Models;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Interfaces;

public interface IXmlTransformService
{
    void Create(EngineType engineType);

    Task<string> TransformAsync(TextDocument xml, TextDocument xsl);
}