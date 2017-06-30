#addin "Cake.Npm"
#addin "Cake.Yarn"
#addin "Cake.Gulp"


var target = Argument("target", "Default");
var noClean = Argument<bool>("no-clean", false);

var solutionFile = "./Configuration.Store.sln";

Task("Restore Nuget Packages")
    .Does(() => {
        var solution = File(solutionFile);
        NuGetRestore(solution.Path);
    });

Task("Restore Javascript Packages")
    .Does(() => {
        var webDirectory = "./src/Configuration.Store.Web/";
        
        var npmSettings = new NpmInstallSettings
        {
            WorkingDirectory = webDirectory,
            LogLevel = NpmLogLevel.Warn
        };

        NpmInstall(npmSettings);
        
        Yarn
            .FromPath(webDirectory)
            .Install(yarnSettings => {
                yarnSettings.ToolPath = "./src/Configuration.Store.Web/node_modules/.bin/yarn.cmd";
            });
        
        Gulp
            .Local
            .Execute(gulpSettings => {
                gulpSettings.WithGulpFile(string.Format("{0}gulpfile.js", webDirectory));
                gulpSettings.SetPathToGulpJs(string.Format(
                    "{0}node_modules/gulp/bin/gulp.js",
                    webDirectory));
            });
    });

Task("Build")
    .IsDependentOn("Restore Nuget Packages")
    .IsDependentOn("Restore Javascript Packages")
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
        
        var files = GetFiles("./tests/**/bin/Release/*.Tests.dll")
            .Where(f => !f.ToString().Contains("Common"));
        
        Fixie(files, settings);
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