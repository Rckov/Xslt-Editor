using XsltEditor.Transform.Engines;
using XsltEditor.Transform.Enums;

namespace XsltEditor.Transform;

public static class XsltEngineFactory
{
	private static readonly Dictionary<EngineType, IXsltEngine> _engines = [];

	public static IXsltEngine Get(EngineType engineType)
	{
		if (!_engines.TryGetValue(engineType, out IXsltEngine? engine))
		{
			engine = engineType switch
			{
				EngineType.XslCompiledTransform => new XslCompiledEngine(),
				EngineType.Saxon => new SaxonEngine(),
				_ => throw new ArgumentOutOfRangeException(nameof(engineType))
			};

			_engines[engineType] = engine;
		}

		return engine;
	}

	public static void WarmupAll()
	{
		foreach (EngineType type in Enum.GetValues<EngineType>())
		{
			Get(type);
		}
	}
}