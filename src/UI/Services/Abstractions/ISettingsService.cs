using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions;

internal interface ISettingsService
{
	Settings Settings { get; }

	void Save();
}