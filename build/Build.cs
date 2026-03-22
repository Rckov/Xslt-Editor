using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
	public static int Main() => Execute<Build>(x => x.Pack);

	[Parameter("Configuration — default Release")]
	readonly string Configuration = "Release";

	[Parameter("Runtime identifier")]
	readonly string Runtime = "win-x64";

	[Solution]
	readonly Solution Solution = null!;

	AbsolutePath SourceDirectory => RootDirectory / "src";
	AbsolutePath PublishDirectory => RootDirectory / "publish";
	AbsolutePath OutputDirectory => RootDirectory / "out";
	AbsolutePath WxsFile => RootDirectory / "build" / "Package.wxs";

	Target Clean => _ => _
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(d => d.DeleteDirectory());
			PublishDirectory.CreateOrCleanDirectory();
			OutputDirectory.CreateOrCleanDirectory();
		});

	Target Restore => _ => _
		.DependsOn(Clean)
		.Executes(() =>
		{
			DotNet("tool restore");
		});

	Target Publish => _ => _
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

	Target Pack => _ => _
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
				$"-out \"{OutputDirectory / "XsltEditor-Setup.msi"}\"")
				.AssertZeroExitCode();
		});
}
