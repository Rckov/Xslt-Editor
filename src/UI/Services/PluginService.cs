using System.IO;
using System.Reflection;
using System.Runtime.Loader;

using XsltEditor.Sdk.Abstractions;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.Services;

internal sealed class PluginService : IPluginService, IDisposable
{
	private static readonly string PluginsPath
		= Path.Combine(AppContext.BaseDirectory, "plugins");

	private readonly List<AssemblyLoadContext> _contexts = [];

	public PluginService()
	{
		Plugins = LoadPlugins();
	}

	public IReadOnlyList<IPlugin> Plugins { get; }

	public void Dispose()
	{
		foreach (var plugin in Plugins)
		{
			plugin.Dispose();
		}

		foreach (var context in _contexts)
		{
			context.Unload();
		}
	}

	private List<IPlugin> LoadPlugins()
	{
		if (!Directory.Exists(PluginsPath))
		{
			Directory.CreateDirectory(PluginsPath);
		}

		var plugins = new List<IPlugin>();

		foreach (var dllPath in Directory.EnumerateFiles(PluginsPath, "*.dll"))
		{
			var plugin = TryLoadPlugin(dllPath);
			if (plugin is not null)
			{
				plugins.Add(plugin);
			}
		}

		return plugins;
	}

	private IPlugin? TryLoadPlugin(string dllPath)
	{
		var context = new AssemblyLoadContext(Path.GetFileNameWithoutExtension(dllPath), true);

		try
		{
			var assembly = context.LoadFromAssemblyPath(Path.GetFullPath(dllPath));
			var pluginTypes = GetPluginTypes(assembly);
			var plugin = CreatePlugin(pluginTypes);

			if (plugin is not null)
			{
				_contexts.Add(context);
				return plugin;
			}
		}
		catch
		{
			// ignore
		}

		context.Unload();
		return null;
	}

	private IPlugin? CreatePlugin(IEnumerable<Type> pluginTypes)
	{
		foreach (var type in pluginTypes)
		{
			if (Activator.CreateInstance(type) is IPlugin plugin)
			{
				return plugin;
			}
		}

		return null;
	}

	private IEnumerable<Type> GetPluginTypes(Assembly assembly)
	{
		foreach (var type in assembly.GetTypes())
		{
			if (type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IPlugin)))
			{
				yield return type;
			}
		}
	}
}
