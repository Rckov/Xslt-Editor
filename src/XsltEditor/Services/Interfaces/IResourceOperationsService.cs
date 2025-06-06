using System.IO;

namespace XsltEditor.Services.Interfaces;

internal interface IResourceOperationsService
{
    Stream? GetResourceStream(string resourceName);

    Task<string> ReadResourceAsStringAsync(string resourceName);
}