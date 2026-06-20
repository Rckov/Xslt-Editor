using XPathValidator.Services;
using Xunit;

namespace XsltEditor.Plugins.Tests;

public class XPathValidatorTests
{
    [Fact]
    public void Test_1()
    {
        var xml = """
                  <root>
                  	<item>value1</item>
                  	<item>value2</item>
                  </root>
                  """;
        var evaluator = XPathEvaluatorService.Create(xml);

        var results = evaluator!.Evaluate("//item").ToList();

        Assert.Equal(2, results.Count);
        Assert.Equal("item", results[0].Name);
        Assert.Equal("value1", results[0].Content);
    }

    [Fact]
    public void Test_2()
    {
        var xml = """
                  <root xmlns:meta="http://example.com/meta">
                  	<meta:Service>TestService</meta:Service>
                  </root>
                  """;
        var evaluator = XPathEvaluatorService.Create(xml);

        var results = evaluator!.Evaluate("//meta:Service").ToList();

        Assert.Single(results);
        Assert.Equal("meta:Service", results[0].Name);
        Assert.Equal("TestService", results[0].Content);
    }
}