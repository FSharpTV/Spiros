module FSharp.TV.SunFlower

open System.Drawing
open FSharp.TV.Spirograph

let build (cmd:int) =
    let cmdsStripe = 
        [ 
          moveTo (900.0,500.0)
          curve5th 89.0 5
          penColor Color.Black 
          turn -9.0 
          curve15th 100.0 26
          penColor Color.Goldenrod 
          turn 70.0 
          curve5th 530.0 5
          penColor Color.Goldenrod       
          turn 70.0 
          curve5th 60.0 cmd
          penColor Color.Goldenrod 
          turn 10.0 
          curve3rd 101.0 19
          penColor Color.LightSeaGreen 
          turn 275.0 ]

    let cmdsGen = 
        [] 
        |> Seq.unfold (fun save -> Some(save, cmdsStripe)) 
        |> Seq.collect id

    let innerCmds =
        cmdsGen 
        |> Seq.take 19500
        |> Seq.toList

    let appendSaveCmdTo iCmd =
        let revd = (saveAs "sample") :: (iCmd |> List.rev)
        revd |> List.rev

    let cmds = appendSaveCmdTo innerCmds

    let draw()  =
        Seq.fold 
            (fun s f -> f s) 
            (newTurtle())
            cmds |> ignore
    draw()
