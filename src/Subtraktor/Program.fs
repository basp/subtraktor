open FSharp.Data.UnitSystems.SI.UnitSymbols

open Subtraktor

// let c3 = sine 130.81<Hz>
// let d3 = sine 146.83<Hz>
// let e3 = sine 164.81<Hz>
// let f3 = sine 174.61<Hz>
// let g3 = sine 196.0<Hz>

// let c3 = sine 130.81<Hz>
// let d3 = saw 146.83<Hz>
// let f3 = square 174.61<Hz>
//
// add c3 f3
// |> apply (add d3)
// |> withEnvelope (ad 2.0<s> 3.0<s>)
// |> apply (lowpass 170.5<Hz> 44100.0<Hz>)
// |> render 44100.0<Hz> 5.0<s>
// |> writeWav "d:/temp/ad.wav" 44100.0<Hz>

// let gate =
//     Gate.between 0.0<s> 3.0<s>
//
// let env =
//     Env.``asr`` gate { Attack = 2.0<s>; Release = 2.0<s> }
//
// let signal =
//     Osc.saw 440.0<Hz>
//
// signal
// |> Env.apply env
// |> Signal.render 44100.0<Hz> 5.0<s>
// |> Wav.write "d:/temp/asr.wav" 44100.0<Hz>

let gate = Gate.between 1.0<s> 4.0<s>

let env =
    Env.``asr`` gate {
        Attack = 2.0<s>
        Release = 1.0<s>
    }

let diagnosis3 () =
    let g1 = gate 3.5<s>
    let g2 = gate 2.0<s>
    let g3 = gate 1.0001<s>

    printfn "Gate at 3.5 = %A" g1
    printfn "Gate at 2.0 = %A" g2
    printfn "Gate at 1.0001 = %A" g3
    
diagnosis3 ()