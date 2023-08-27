#addin nuget:?package=Cake.Coverlet&version=3.0.4
#tool dotnet:?package=dotnet-reportgenerator-globaltool&version=5.1.24

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
const string TEST_COVERAGE_OUTPUT_DIR = "coverage";
Task("Clean")
    .Does(() => {
 
    if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
    {
      Information("Nothing to clean GitHubActions.");
    }
    else
    {
        DotNetClean("./PayBolt.sln");
    }
});

Task("Restore")
    .IsDependentOn("Clean")
    .Description("Restoring the solution dependencies")
    .Does(() => {
    
    Information("Restoring the solution dependencies");
      var settings =  new DotNetRestoreSettings
        {
          Verbosity = DotNetVerbosity.Minimal,
          Sources = new [] { "https://api.nuget.org/v3/index.json" }
        };
   GetFiles("./**/**/*.csproj").ToList().ForEach(project => {
       Information($"Restoring {project.ToString()}");
       DotNetRestore(project.ToString(), settings);
     });
  

});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
     var buildSettings = new DotNetBuildSettings {
                        Configuration = configuration,
                       };
     GetFiles("./**/**/*.csproj").ToList().ForEach(project => {
         Information($"Building {project.ToString()}");
         DotNetBuild(project.ToString(),buildSettings);
     });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
       
       var testSettings = new DotNetTestSettings  {
                 Configuration = configuration,
                 NoBuild = true,
       };
        var coverageOutput = Directory(TEST_COVERAGE_OUTPUT_DIR);             
     
       GetFiles("./tests/**/*.csproj").ToList().ForEach(project => {
          Information($"Testing Project : {project.ToString()}");
            
          var codeCoverageOutputName = $"{project.GetFilenameWithoutExtension()}.cobertura.xml";
          var coverletSettings = new CoverletSettings {
              CollectCoverage = true,
               CoverletOutputFormat = CoverletOutputFormat.cobertura,
               CoverletOutputDirectory =  coverageOutput,
               CoverletOutputName =codeCoverageOutputName,
               ArgumentCustomization = args => args.Append($"--logger trx")
          };
                  
          Information($"Running Tests : { project.ToString()}");
          DotNetTest(project.ToString(), testSettings, coverletSettings );        
        });
     
      Information($"Directory Path : { coverageOutput.ToString()}");
          
      var glob = new GlobPattern($"./{ coverageOutput}/*.cobertura.xml");
         
      Information($"Glob Pattern : { glob.ToString()}");
      var outputDirectory = Directory("./coverage/reports");
     
      var reportSettings = new ReportGeneratorSettings
      {
         ArgumentCustomization = args => args.Append($"-reportTypes:Html;Cobertura")
      };
         
      ReportGenerator(glob, outputDirectory, reportSettings);
          
      if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
      {
        var summaryDirectory = Directory("./coverage");
        var summarySettings = new ReportGeneratorSettings
        {
           ArgumentCustomization = args => args.Append($"-reportTypes:Html;MarkdownSummaryGithub")
        };
        ReportGenerator(glob, summaryDirectory, summarySettings);
      }
});



Task("Default")
       .IsDependentOn("Clean")
       .IsDependentOn("Restore")
       .IsDependentOn("Build")
       .IsDependentOn("Test");

RunTarget(target);