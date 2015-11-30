open Newtonsoft.Json
open Suave.Http
open Suave.Http.Applicatives
open Suave.Http.Successful
open Suave.Types
open Suave.Utils
open Suave.Web
open System
open Suave.Http.Files
open Messages
open System.Drawing
open Plotting
open System.IO
open System.Collections.Concurrent

let plotters = ConcurrentDictionary<Guid, MailboxProcessor<TurtleCommand>>()

let plotterAgent plotter = 
  MailboxProcessor.Start <| fun mbx -> 
    let rec loop plotter = 
      async { 
        let! msg = mbx.Receive()
        printfn "%A" msg
        try 
            let plotter = 
              match msg with
              | Move position -> { plotter with Position = position }
              | Draw x -> Plotting.move x plotter
              | Turn x -> Plotting.turn x plotter
              | Polygon(x, y) -> Plotting.polygon x y plotter
              | Color(r, g, b) -> { plotter with Color = Color.FromArgb(r |> int, g |> int, b |> int) }
            plotter.Bitmap.Save plotter.Name
            return! loop plotter
        with _ ->
            return! loop plotter
      }
    loop plotter

let handler request = 
  let json = request.rawForm |> UTF8.toString
  let msg = JsonConvert.DeserializeObject<Request> json
  let response = 
    match msg with
    | Ping -> Pong
    | Register name ->
      if sprintf "%s.bmp" name |> File.Exists then RegistrationFailed
      else
      let plotter = 
        { Plotter.Bitmap = new Bitmap(800, 800)
          Position = 400, 400
          Color = Color.White
          Direction = 0.
          Name = sprintf "%s.bmp" name }
      plotter.Bitmap.Save plotter.Name
      let token = Guid.NewGuid()
      let agent = plotterAgent plotter
      plotters.TryAdd(token, agent) |> ignore
      Registered token
    | TurtleCommand(token, command) -> 
      let success, agent = plotters.TryGetValue token
      match success with
      | false -> UnknownToken token
      | _ -> 
        agent.Post command
        TurtleCommandExecuted
  JsonConvert.SerializeObject response |> OK

let getFile = sprintf "%s.bmp" >> file

let getFiles _ = 
  Directory.EnumerateFiles(Environment.CurrentDirectory, "*.bmp")
  |> Seq.map Path.GetFileNameWithoutExtension
  |> Seq.map (fun x -> sprintf "<div><a href='/images/%s'>%s</a></div><img src=\"/images/%s\"/>" x x x)
  |> Seq.reduce (+)
  |> sprintf "<html><head><meta http-equiv=\"refresh\" content=\"5\"></head>%s</html>"
  |> OK

let routes = 
  choose [ pathScan "/images/%s" getFile
           path "/images" >>= request getFiles
           path "/" >>= request handler ]

[<EntryPoint>]
let main _ = 
  startWebServer { defaultConfig with bindings = [ HttpBinding.mk' HTTP "0.0.0.0" 8083 ] } routes
  0
