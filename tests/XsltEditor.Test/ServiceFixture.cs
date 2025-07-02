using Microsoft.Extensions.DependencyInjection;

using XsltEditor.Services;
using XsltEditor.Services.Interfaces;
using XsltEditor.Transform;
using XsltEditor.Transform.Interfaces;

namespace XsltEditor.Test;

public class ServiceFixture : IDisposable
{
    public ServiceFixture()
    {
        SettingsPath = Path.GetTempFileName();

        var services = new ServiceCollection();

        services.AddTransient<ITransformer, Transformer>();
        services.AddTransient<ISettingsService>(_ => new SettingsService(SettingsPath));

        Services = services.BuildServiceProvider();
    }

    public string SettingsPath { get; }
    public IServiceProvider Services { get; }

    public void Dispose()
    {
        if (File.Exists(SettingsPath))
        {
            File.Delete(SettingsPath);
        }
    }
}