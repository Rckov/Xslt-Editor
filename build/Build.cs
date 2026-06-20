using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

namespace Build;

public sealed class Build : NukeBuild
{
    private AbsolutePath OutputDirectory => RootDirectory / "out";
    private AbsolutePath AppOutputDirectory => OutputDirectory / "app";
    private AbsolutePath PluginsOutputDirectory => OutputDirectory / "plugins";
    private AbsolutePath SourceDirectory => RootDirectory / "src";
    private AbsolutePath PluginsDirectory => SourceDirectory / "Plugins";
    private AbsolutePath AppProject => SourceDirectory / "UI" / "XsltEditor.csproj";
    private IEnumerable<AbsolutePath> PluginProjects => FindProjects(PluginsDirectory);

    private Target Test
    {
        get
        {
            return d => d
                .Executes(() =>
                {
                    DotNetTasks.DotNetTest(settings => settings
                        .SetProjectFile(RootDirectory / "XsltEditor.slnx"));
                });
        }
    }

    private Target PublishApp
    {
        get
        {
            return d => d
                .Produces(AppOutputDirectory / "XSLT Editor.exe")
                .Executes(() =>
                {
                    PublishProject(AppProject, AppOutputDirectory, settings => settings
                        .SetSelfContained(true)
                        .SetRuntime("win-x64"));
                });
        }
    }

    private Target PublishPlugins
    {
        get
        {
            return d => d
                .Executes(() =>
                {
                    foreach (var project in PluginProjects)
                    {
                        PublishProject(project, PluginsOutputDirectory / project.Parent!.Name);
                    }
                });
        }
    }

    private Target Default => d => d.DependsOn(Test, PublishApp, PublishPlugins);

    public static int Main()
    {
        return Execute<Build>(x => x.Default);
    }

    private static IEnumerable<AbsolutePath> FindProjects(AbsolutePath directory)
    {
        return directory.GlobFiles("**/*.csproj");
    }

    private static void PublishProject(
        AbsolutePath project,
        AbsolutePath output,
        Func<DotNetPublishSettings, DotNetPublishSettings>? configure = null)
    {
        DotNetTasks.DotNetPublish(settings =>
        {
            var publishSettings = settings
                .SetProject(project)
                .SetOutput(output);

            return configure?.Invoke(publishSettings) ?? publishSettings;
        });
    }
}