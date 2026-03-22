namespace XsltEditor.Transform;

public interface IXsltEngine
{
	Task<string> TransformAsync(string xml, string xsl, string? baseUri = null);
}