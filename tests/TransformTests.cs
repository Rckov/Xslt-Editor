using XsltEditor.Transform;
using XsltEditor.Transform.Enums;

using Xunit;

namespace XsltEditor.Tests;

public class TransformTests
{
	private static readonly string _xsl1_0 = File.ReadAllText("Resources/XSL1_0.xsl");
	private static readonly string _xsl3_0 = File.ReadAllText("Resources/XSL3_0.xsl");

	private static readonly string _xml = File.ReadAllText("Resources/XML/TestResource.xml");

	[Fact]
	public async Task Test0_Compiled()
	{
		var engine = XsltEngineFactory.Get(EngineType.XslCompiledTransform);

		var result = await engine.TransformAsync(_xml, _xsl1_0);

		Assert.False(string.IsNullOrWhiteSpace(result));
		Assert.Contains("<p>My Book</p>", result);
		Assert.Contains("Author: John Doe", result);
	}

	[Fact]
	public async Task Test0_Saxon()
	{
		var engine = XsltEngineFactory.Get(EngineType.Saxon);

		var result = await engine.TransformAsync(_xml, _xsl3_0);

		Assert.False(string.IsNullOrWhiteSpace(result));
		Assert.Contains("MY BOOK", result);
		Assert.Contains("Author: John Doe", result);
		Assert.Contains("Books (1)", result);
	}
}
