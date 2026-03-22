using XsltEditor.Models;
using XsltEditor.ViewModels;

namespace XsltEditor.Services.Abstractions;

internal interface IDocumentFactory
{
	DocumentViewModel Create(string name, DocumentType type);
}