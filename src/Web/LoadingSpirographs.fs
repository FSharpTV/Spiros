module ``Loading spirographs``

open System
open System.IO
open System.Reflection

let rootPath =
  Assembly.GetExecutingAssembly().CodeBase
  |> fun s -> (Uri s).AbsolutePath
  |> Path.GetDirectoryName

open System
open Suave
open Suave.Types
open Suave.Web
open Suave.Http
open Suave.Http.Successful
open Suave.Http.Applicatives
open Suave.Http.RequestErrors
open Suave.DotLiquid
open Suave.Utils // for ThreadSafeRandom

DotLiquid.setTemplatesDir (__SOURCE_DIRECTORY__ + "/templates")

type Image =
  { href : string
    title : string }

let staticPictures =
  choose [
    // this will first serve matching spiro pictures
    ``Browsing for spiros``.servedSpiros

    // falling through to serving the index page:
    page "image-of-the-day.liquid" { href = "/Experiment.png"; title = "The best spirographs ever" }
  ]

//let spiroOfTheDay =
//
//  let spirosFolder =
//    Path.Combine(rootPath, "spiros")
//
//  let (files : string []), (fileToDescription : Map<string, string>) =
//    File.ReadAllLines(Path.Combine(spirosFolder, "descriptions.tsv"))
//    |> Array.map (fun s -> s.Split('\t'))
//    // transform the array of two items into a F# tuple; we know its
//    // two-values-shape, so ignore the warning:
//    |> Array.map (fun [| fileName; description |] -> fileName, description)
//    // do something of the sequence of FileName * Description
//    |> fun values -> 
//      values |> Array.map fst, // make an array of all file names (not paths)
//      Map.ofSeq values // make a mapping between file name and their descriptions
//
//  choose [
//    ``Browsing for spiros``.servedSpiros
//
//    // needs to be Lazy, so we use warbler:
//    warbler (fun _ ->
//      // remember; in Suave, you may have multiple concurrent requests going:
//      let index = ThreadSafeRandom.next 0 files.Length
//
//      // first find the image for the found index:
//      let imageFile = files.[index]
//
//      // the find the description for that file:
//      let title = fileToDescription.[imageFile]
//
//      // serve the page with this image:
//      page "image-of-the-day.liquid" { href = sprintf "/%s" imageFile; title = title }
//    )
//  ]

let main argv =
  startWebServer defaultConfig staticPictures
  0
