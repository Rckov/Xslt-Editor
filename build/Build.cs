using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

internal class Build : NukeBuild
{
	public static int Main() => Execute<Build>(x => x.Pack);

	[Parameter("Configuration — default Release")]
	private readonly string Configuration = "Release";

	[Parameter("Runtime identifier")]
	private readonly string Runtime = "win-x64";

	[Solution]
	private readonly Solution Solution = null!;

	private AbsolutePath SourceDirectory => RootDirectory / "src";
	private AbsolutePath PublishDirectory => RootDirectory / "publish";
	private AbsolutePath OutputDirectory => RootDirectory / "out";
	private AbsolutePath WxsFile => RootDirectory / "build" / "Package.wxs";

	private Target Clean => _ => _
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(d => d.DeleteDirectory());
			PublishDirectory.CreateOrCleanDirectory();
			OutputDirectory.CreateOrCleanDirectory();
		});

	private Target Restore => _ => _
		.DependsOn(Clean)
		.Executes(() => DotNet("tool restore"));

	private Target Publish => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetPublish(s => s
				.SetProject(Solution.GetProject("XsltEditor"))
				.SetConfiguration(Configuration)
				.SetRuntime(Runtime)
				.SetSelfContained(true)
				.SetPublishSingleFile(false)
				.SetOutput(PublishDirectory));
		});

	private Target Pack => _ => _
		.DependsOn(Publish)
		.Executes(() =>
		{
			ProcessTasks.StartProcess(
				"dotnet",
				$"wix build \"{WxsFile}\" " +
				$"-arch x64 " +
				$"-ext WixToolset.UI.wixext " +
				$"-d PublishDir={PublishDirectory}\\ " +
				$"-d SourceDir={SourceDirectory}\\ " +
				$"-d BuildDir={RootDirectory / "build"}\\ " +
				$"-out \"{OutputDirectory / "XsltEditor-Setup.msi"}\"")
				.AssertZeroExitCode();
		});
}
