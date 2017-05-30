var target = Argument("target", "Default");
var noClean = Argument<bool>("no-clean", false);

var solutionFile = "./Configuration.Store.sln";

Task("Restore Nuget Packages")
    .Does(() => {
        var solution = File(solutionFile);
        NuGetRestore(solution.Path);
    });

Task("Build")
    .IsDependentOn("Restore Nuget Packages")
    .Does(() => {
        var targets = string.Format(
            "{0}Build",
            noClean ? string.Empty : "Clean;");
        
        DotNetBuild(solutionFile, settings =>
            settings.SetConfiguration("Release")
                .SetVerbosity(Cake.Core.Diagnostics.Verbosity.Normal)
                .WithTarget("Clean;Build")
                .WithProperty("TreatWarningsAsErrors","true"));
    });

Task("Run Unit Tests")
    .IsDependentOn("Build")
    .Does(() => {
        var settings = new FixieSettings{
            NUnitXml = "./TestResult.xml"
        };
        
        Fixie(
            "./tests/**/bin/Release/*.Tests.dll",
            settings);
    });

Task("Upload test results to AppVeyor")
    .IsDependentOn("Run Unit Tests")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .Does(() => {
        AppVeyor.UploadTestResults(
            File("./TestResult.xml").Path,
            AppVeyorTestResultsType.NUnit);
    });

Task("Default")
    .IsDependentOn("Run Unit Tests")
    .IsDependentOn("Upload test results to AppVeyor")
  .Does(() =>
{
  Information("Project built and tested");
});

RunTarget(target);