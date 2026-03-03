#load "./Numerics/Rational.fs"
#load "./Interpreter.fs"

open Subtraktor.Numerics
open Subtraktor.Interpreter

let test () =
    let dMinor = Par [
        d 4 wn |> Prim
        f 4 wn |> Prim
        a 4 wn |> Prim
    ]
    
    let gMajor = Par [
        g 4 wn |> Prim
        b 4 wn |> Prim
        d 5 wn |> Prim   
    ]
    
    let cMajor = Par [
        c 4 bn |> Prim
        e 4 bn |> Prim
        g 4 bn |> Prim
    ]
    
    let music = Seq [ dMinor; gMajor; cMajor ]    
    
    music
    |> Music.interpret Rational.Zero
    |> Seq.toList    
    
test ()
