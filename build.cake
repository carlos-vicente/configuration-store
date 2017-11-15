#addin "Cake.Npm"
#addin "Cake.Yarn"
#addin "Cake.Gulp"

var target = Argument("target", "Default");
var noClean = Argument<bool>("no-clean", false);

var solutionFile = "./Configuration.Store.sln";
var webDirectory = "./src/Configuration.Store.Web/";

Task("Restore Nuget Packages")
    .Does(() => 
    {
        var solution = File(solutionFile);
        NuGetRestore(solution.Path);
    });

Task("Clean generated code")
    .WithCriteria(!noClean)
    .Does(() =>
    {
        var directoriesToClean = new List<DirectoryPath>();
        
        if(DirectoryExists("./src/Configuration.Store.Web/node_modules"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/node_modules"));
        
        if(DirectoryExists("./src/Configuration.Store.Web/Styles/materialize"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/Styles/materialize"));
        
        if(DirectoryExists("./src/Configuration.Store.Web/Styles/swagger"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/Styles/swagger"));
            
        if(DirectoryExists("./src/Configuration.Store.Web/Styles/app"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/Styles/app"));

        if(DirectoryExists("./src/Configuration.Store.Web/Scripts/app"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/Scripts/app")); 
            
        if(DirectoryExists("./src/Configuration.Store.Web/Scripts/lib"))
            directoriesToClean.Add(Directory("./src/Configuration.Store.Web/Scripts/lib"));
        
        CleanDirectories(directoriesToClean);
    });

Task("Restore Javascript Packages")
    .IsDependentOn("Clean generated code")
    .Does(() => 
    {
        var npmSettings = new NpmInstallSettings
        {
            WorkingDirectory = webDirectory,
            LogLevel = NpmLogLevel.Warn
        };

        NpmInstall(npmSettings);
        
        Yarn
            .FromPath(webDirectory)
            .Install(yarnSettings => 
            {
                yarnSettings.ToolPath = "./src/Configuration.Store.Web/node_modules/.bin/yarn.cmd";
            });
    });

Task("Build")
    .IsDependentOn("Restore Nuget Packages")
    .IsDependentOn("Restore Javascript Packages")
    .Does(() => 
    {
        // build javascript stuff
        Gulp
            .Local
            .Execute(gulpSettings => 
            {
                gulpSettings
                    .WithGulpFile(string.Format("{0}gulpfile.js", webDirectory));
                gulpSettings
                    .SetPathToGulpJs(string.Format("{0}node_modules/gulp/bin/gulp.js", webDirectory));
            });

        // build .net stuff
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
    .Does(() =>
    {
        var settings = new FixieSettings
        {
            NUnitXml = "./TestResult.xml"
        };
        
        var files = GetFiles("./tests/**/bin/Release/*.Tests.dll")
            .Where(f => !f.ToString().Contains("Common"));
        
        Fixie(files, settings);
    });

Task("Upload test results to AppVeyor")
    .IsDependentOn("Run Unit Tests")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
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