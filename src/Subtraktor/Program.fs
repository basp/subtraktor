open FSharp.Data.UnitSystems.SI.UnitSymbols

open Subtraktor

let example () =
    let osc1 = Signal.saw 55.0<Hz>
    let osc2 = Signal.saw 110.0<Hz> |> Signal.scale 0.7
    let osc3 = Signal.square 220.0<Hz> |> Signal.scale 0.5
    let osc4 = Signal.triangle 440.0<Hz> |> Signal.scale 0.2
    
    let mixed =
        osc1
        |> Signal.mix osc2
        |> Signal.mix osc3
        |> Signal.mix osc4
    
    let render rate samples =
        samples
        |> Signal.sample rate 5.0<s>
        |> Array.map snd
        |> Wav.write "d:/temp/441.wav" rate

    mixed
    |> Signal.scale 0.73
    |> render 44100.0<Hz>
 
example ()