using XsltEditor.Transform.Enums;

namespace XsltEditor.Services.Abstractions;

internal interface ITransformService
{
	void SetEngine(EngineType engineType);

	Task WarmupAsync();

	Task<string> TransformAsync(string xml, string xsl, string? baseUri = null);
}