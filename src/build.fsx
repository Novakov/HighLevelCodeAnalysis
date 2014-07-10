#r "packages/FAKE.3.2.0/tools/FakeLib.dll"
open Fake

let buildDir = "..\\build" |> FullName
let binaries = buildDir @@ "binaries"
let tests = buildDir @@ "tests"


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

    let setParams (defaults:NUnitParams) = { defaults with
                                                ErrorLevel = DontFailBuild
                                                Framework = "4.0"
                                                ShowLabels = false
                                                WorkingDir = tests
                                                OutputFile = tests @@ "TestResult.xml"
    
    }

    !! (binaries @@ "Tests.dll" )
    |> NUnit setParams
    |> DoNothing
)

Target "Default" DoNothing

"Clean"
    ==> "Build"
    ==> "Tests"
    ==> "Default"


RunTargetOrDefault "Default"