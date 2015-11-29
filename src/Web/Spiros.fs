module ``Browsing for spiros``

open System
open System.IO
open System.Reflection

let rootPath =
  Assembly.GetExecutingAssembly().CodeBase
  |> fun s -> (Uri s).AbsolutePath
  |> Path.GetDirectoryName

open Suave
open Suave.Types
open Suave.Http
open Suave.Web
open Suave.Http.Files

let filesInHome : WebPart =
  Files.browseHome

// you can easily read the list of files in your browser:
let spiroDir = Path.Combine(rootPath, "spiros")

// printfn "Spiro dir: %s" spiroDir
let allSpiros = Files.dir spiroDir

// or you can 'jump into' the 'spiros' folder and serve from there:
let servedSpiros = Files.browse spiroDir

let main argv =
  startWebServer defaultConfig servedSpiros
  0
