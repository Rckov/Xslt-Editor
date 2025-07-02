using Microsoft.Extensions.DependencyInjection;

using System.Xml;

using XsltEditor.Transform.Enums;
using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Test.Services;

public class TransformerTest(ServiceFixture fixture) : IClassFixture<ServiceFixture>
{
    private readonly ITransformer _transformer = fixture.Services.GetRequiredService<ITransformer>();

    [Fact]
    public async Task Test0()
    {
        // Arrange
        _transformer.Create(EngineType.XslCompiledTransform);

        using var xml = XmlReader.Create("Resources/TestResource.xml");
        using var xsl = XmlReader.Create("Resources/TestResource.xsl");

        // Act
        var result = await _transformer.TransformAsync(xml, xsl);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(result));
        Assert.Contains("<p>My Book</p>", result);
        Assert.Contains("Author: John Doe", result);
    }
}