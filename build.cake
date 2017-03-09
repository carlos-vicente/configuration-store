var target = Argument("target", "Default");

var solutionFile = "./Configuration.Store.sln";

Task("Restore Nuget Packages")
    .Does(() => {
        var solution = File(solutionFile);
        NuGetRestore(solution.Path);
    });

Task("Build")
    .IsDependentOn("Restore Nuget Packages")
    .Does(() => {
        DotNetBuild(solutionFile, settings =>
            settings.SetConfiguration("Release")
                .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Normal)
                .WithTarget("Clean;Build")
                .WithProperty("TreatWarningsAsErrors","true"));
    });

Task("Default")
    .IsDependentOn("Build")
  .Does(() =>
{
  Information("Project built");
});

RunTarget(target);