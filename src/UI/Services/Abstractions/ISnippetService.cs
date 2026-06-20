using XsltEditor.Models;

namespace XsltEditor.Services.Abstractions;

public interface ISnippetService
{
    IReadOnlyList<SnippetData> Data { get; }

    void Save();

    void Add(SnippetData item);

    void Remove(SnippetData item);
}