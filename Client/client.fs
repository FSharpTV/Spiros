open Akka.Actor
open Akka.Configuration
open System
open System.Drawing
open FSharp.TV.Spirograph
open FSharp.TV.Server
open FSharp.TV.Types

[<EntryPoint>]
let main argv =
    let config = ConfigurationFactory.ParseString(@"
        akka {
            actor {
                provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
            }
            remote {
                helios.tcp {
                    port = 0 # bound to a dynamic port assigned by the OS
                    hostname = localhost
                }
            }
        }")

    Console.WriteLine("Enter a number between 20 and 50")
    let spiroCmd = Console.ReadLine();

    use system = ActorSystem.Create("MyClient", config)

    let spirographer = system.ActorSelection("akka.tcp://MyServer@localhost:8081/user/spirog")

    let msg = Spiro(int spiroCmd)
    spirographer.Tell msg
    Console.ReadLine() |> ignore
    0