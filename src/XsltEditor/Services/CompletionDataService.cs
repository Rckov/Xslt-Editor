using System.Text.Json;

using XsltEditor.Models;
using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class CompletionDataService : ICompletionDataService
{
    private readonly IFileOperationsService _fileService;
    private readonly IResourceOperationsService _resourceService;

    private readonly string _filePath;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public CompletionDataService(IFileOperationsService fileService, IResourceOperationsService resourceService)
    {
        _fileService = fileService;
        _resourceService = resourceService;

        _filePath = _fileService.GetPath("completions.json");
    }

    public IList<CompletionData> Data { get; }

    public Task LoadCompletionData()
    {
        throw new NotImplementedException();
    }

    public Task SaveCompletionData()
    {
        throw new NotImplementedException();
    }

    public void Add(CompletionData data)
    {
        throw new NotImplementedException();
    }

    public void Remove(CompletionData data)
    {
        throw new NotImplementedException();
    }

    private void RestoreDefaultCompletionData()
    {
    }
}