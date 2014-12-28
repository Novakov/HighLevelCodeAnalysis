#r "packages/FAKE.3.2.0/tools/FakeLib.dll"
open Fake
open Fake.OpenCoverHelper

let buildDir = "..\\build" |> FullName
let binaries = buildDir @@ "binaries"
let tests = buildDir @@ "tests"

let packagesDir = "packages" |> FullName

let nunitParameters (defaults:NUnitParams) = { defaults with
                                                ErrorLevel = DontFailBuild
                                                Framework = "4.0"
                                                ShowLabels = false
                                                WorkingDir = tests
                                                OutputFile = tests @@ "TestResult.xml"
                                                DisableShadowCopy = true    
}

Target "Clean" (fun _ ->
    CleanDir buildDir
)

Target "Build" (fun _ ->   
    let setParams defaults = { defaults with
                                Verbosity = Some(MSBuildVerbosity.Minimal)
                                Targets = ["Build"]                                
                                Properties = [
                                             "OutputPath", binaries |> trimSeparator
                                ]                                
    }

    build setParams "HighLevelCodeAnalysis.sln" |> DoNothing
)

Target "Tests" (fun _ ->   
    CreateDir tests

    !! (binaries @@ "Tests.dll" )
    |> NUnit nunitParameters
    |> DoNothing
)

Target "TestsWithCoverage" (fun _ ->
    CreateDir tests

    let setParams (defaults:OpenCoverParams) = { defaults with
                                                    ExePath = FileSystem.findToolInSubPath "OpenCover.Console.exe" packagesDir
                                                    Output = tests @@ "coverage.xml"
                                                    Register = RegisterUser
                                                    TestRunnerExePath = FileSystem.findToolInSubPath "nunit-console.exe" packagesDir
    }
    
    let targetArgs = NUnitCommon.buildNUnitdArgs (nunitParameters NUnitDefaults) !! (binaries @@ "Tests.dll" )

    OpenCover setParams targetArgs
)

Target "Default" DoNothing

"Clean"
    ==> "Build"
    =?> ("Tests", not (hasBuildParam "Coverage"))
    =?> ("TestsWithCoverage", hasBuildParam "Coverage")
    ==> "Default"


RunTargetOrDefault "Default"