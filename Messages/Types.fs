module FSharp.TV.Types
//type Greet =
//    { Who: string }

type Greet(who:string) =
    member x.Who = who

type Spiro(cmd:int) =
    member public x.Cmd = cmd