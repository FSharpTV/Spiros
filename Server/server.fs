module FSharp.TV.Server
open Akka.Actor;
open Akka.Configuration;
open System;
open FSharp.TV.Spirograph
open FSharp.TV.Types

//type SpiroActor() as g =
//    inherit ReceiveActor()
//    do printfn "Starting Actor"
//    do g.Receive<obj>(fun (spiro:(Turtle->Turtle) list) -> FSharp.TV.SunFlower.buildSpiro spiro)
System.IO.Directory.SetCurrentDirectory __SOURCE_DIRECTORY__

let handleCmd (s:Spiro) = 
    let cmd = (int s.Cmd)
    printfn "Msg Recieved %A" cmd
    let r = FSharp.TV.SunFlower.build cmd
    r

type SpiroActor() as g =
    inherit ReceiveActor()
    do printfn "Starting Actor"
//    do g.Receive<_>(fun (cmd) -> handleCmd cmd) |> ignore
    do g.Receive<_>(fun (cmd:Spiro) -> handleCmd cmd) |> ignore

[<EntryPoint>]
let main argv =
    let config = ConfigurationFactory.ParseString(@"
        akka {
            actor {
                provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
            }

            remote {
                helios.tcp {
                    port = 8081
                    hostname = localhost
                }
            }
        }
        ")
    let system = ActorSystem.Create("MyServer", config)
    let spirog = system.ActorOf<SpiroActor> "spirog"
    let name = System.Console.ReadLine()
    System.Console.ReadLine() |> ignore
    0 // return an integer exit code