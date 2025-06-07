using System.IO;
using System.Reflection;

using XsltEditor.Services.Interfaces;

namespace XsltEditor.Services;

internal class ResourceOperationsService : IResourceOperationsService
{
    public Stream? GetResourceStream(string resourceName)
    {
        return Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
    }

    public async Task<string> ReadResourceAsStringAsync(string resourceName)
    {
        await using var stream = GetResourceStream(resourceName);

        if (stream is null)
        {
            throw new FileNotFoundException($"Resource '{resourceName}' not found.");
        }

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}