using Microsoft.Extensions.DependencyInjection;

using XsltEditor.Sdk.Enums;
using XsltEditor.Services.Abstractions;
using XsltEditor.Services.Abstractions.Themes;
using XsltEditor.ViewModels;

namespace XsltEditor.Services;

internal class DocumentFactory(
	IServiceProvider services,
	IThemeService themeService,
	ISnippetService snippetService) : IDocumentFactory
{
	public DocumentViewModel Create(string name, DocumentType type)
	{
		DocumentViewModel vm = services.GetRequiredService<DocumentViewModel>();
		vm.Name = name;
		vm.DocumentType = type;
		vm.Highlighting = themeService.Highlighting;
		vm.CompletionData = snippetService.Data;
		return vm;
	}
}
