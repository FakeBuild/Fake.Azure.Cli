#load ".fake/build.fsx/intellisense.fsx"
open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open BlackFox.Fake

let clean = BuildTask.create "Clean" [] {
    !! "**/bin"
    ++ "**/obj"
    |> Shell.cleanDirs 
}

let build = BuildTask.create "Build" [clean.IfNeeded] {
    !! "**/*.*proj"
    |> Seq.iter (DotNet.build id)
}

let _all = BuildTask.createEmpty "All" [clean; build]

BuildTask.runOrDefault _all