using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.Services;

internal class SettingsService : ISettingsService
{
	private static readonly string _filePath =
		Path.Combine(AppContext.BaseDirectory, "XsltEditor.settings.json");

	private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

	public Settings Settings { get; } = Load();

	public void Save()
	{
		var json = JsonSerializer.Serialize(Settings, _jsonOptions);
		File.WriteAllText(_filePath, json);
	}

	private static Settings Load()
	{
		if (!File.Exists(_filePath))
		{
			return new Settings();
		}

		try
		{
			var json = File.ReadAllText(_filePath);
			return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
		}
		catch
		{
			return new Settings();
		}
	}
}