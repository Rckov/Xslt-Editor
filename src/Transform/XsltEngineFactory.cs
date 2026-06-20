using XsltEditor.Transform.Engines;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Transform;

public static class XsltEngineFactory
{
    private static readonly Dictionary<EngineType, IXsltEngine> Engines = [];

    public static IXsltEngine Get(EngineType engineType)
    {
        if (Engines.TryGetValue(engineType, out var engine))
        {
            return engine;
        }

        engine = engineType switch
        {
            EngineType.XslCompiledTransform => new XslCompiledEngine(),
            EngineType.Saxon => new SaxonEngine(),
            _ => throw new ArgumentOutOfRangeException(nameof(engineType))
        };

        Engines[engineType] = engine;
        return engine;
    }

    public static void WarmupAll()
    {
        foreach (var type in Enum.GetValues<EngineType>())
        {
            Get(type);
        }
    }
}