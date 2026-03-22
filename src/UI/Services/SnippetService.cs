using System.IO;
using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Abstractions;

namespace XsltEditor.Services;

internal class SnippetService : ISnippetService
{
	private static readonly string FilePath =
		Path.Combine(AppContext.BaseDirectory, "XsltEditor.Snippets.json");

	private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

	private readonly List<SnippetData> _data = Load();

	public IReadOnlyList<SnippetData> Data => _data;

	public void Save()
	{
		var json = JsonSerializer.Serialize(_data, JsonOptions);
		File.WriteAllText(FilePath, json);
	}

	public void Add(SnippetData item)
	{
		if (!_data.Contains(item))
		{
			_data.Add(item);
		}
	}

	public void Remove(SnippetData item)
	{
		_data.Remove(item);
	}

	private static List<SnippetData> Load()
	{
		if (!File.Exists(FilePath))
		{
			return [];
		}

		try
		{
			var json = File.ReadAllText(FilePath);
			return JsonSerializer.Deserialize<List<SnippetData>>(json, JsonOptions) ?? [];
		}
		catch
		{
			return [];
		}
	}
}