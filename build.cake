#addin nuget:?package=Cake.Coverlet&version=3.0.4
#addin nuget:?package=Cake.AzureDevOps&version=3.0.0
#tool dotnet:?package=dotnet-reportgenerator-globaltool&version=5.1.19

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
const string TEST_COVERAGE_OUTPUT_DIR = "coverage";
Task("Clean")
    .Does(() => {
 
    if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
    {
      Information("Nothing to clean on Azure Pipelines.");
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
         
      Information($"globpattern : { glob.ToString()}");
      var outputDirectory = Directory("./coverage/reports");
     
      var reportSettings = new ReportGeneratorSettings
      {
         ArgumentCustomization = args => args.Append($"-reportTypes:HtmlInline_AzurePipelines;Cobertura")
      };
         
      ReportGenerator(glob, outputDirectory, reportSettings);
          
      if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
      {
        var coverageFile = $"coverage/reports/Cobertura.xml";
        var coverageData = new AzurePipelinesPublishCodeCoverageData
        {
          CodeCoverageTool = AzurePipelinesCodeCoverageToolType.Cobertura,
          SummaryFileLocation = coverageFile,
          ReportDirectory = "coverage/reports"
        };
        Information($"Publishing Test Coverage : {coverageFile}");
        BuildSystem.AzurePipelines.Commands.PublishCodeCoverage(coverageData);
      }
});



Task("Default")
       .IsDependentOn("Clean")
       .IsDependentOn("Restore")
       .IsDependentOn("Build")
       .IsDependentOn("Test");

RunTarget(target);