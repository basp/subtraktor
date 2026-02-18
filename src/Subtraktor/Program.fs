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

let patch1 () =
    let gate = Gate.between 0.0<s> 3.5<s>

    let env =
        Env.``asr`` gate {
            Attack = 1.5<s>
            Release = 1.5<s>
        }
        
    let osc = Osc.saw 110.0<Hz>

    let patch = Signal.mul env osc

    patch
    |> Signal.render 44100.0<Hz> 5.0<s>
    |> Wav.write "d:/temp/patch1.wav" 44100.0<Hz>
    
let patch2 () =
    let gate = Gate.between 0.0<s> 4.0<s>
    
    let env =
        Env.``asr`` gate {
            Attack = 1.5<s>
            Release = 1.5<s>
        }
        
    let osc1 = Osc.saw 220.0<Hz>
    let osc2 = Osc.saw 221.5<Hz>
    
    let patch = Signal.mul (Signal.add osc1 osc2) env
    
    patch
    |> Signal.render 44100.0<Hz> 5.0<s>
    |> Wav.write "d:/temp/patch2.wav" 44100.0<Hz>    

let patch3 () =
    let lfo =
        Lfo.sine 5.0<Hz>
        |> Signal.scale 3.0

    let gate = Gate.between 0.0<s> 4.0<s>
          
    let env =
        Env.``asr`` gate {
            Attack = 1.5<s>
            Release = 1.5<s>
        }     
        
    let patch =
        Osc.saw 440.0<Hz>
        |> Signal.add lfo
        |> Env.apply env

    patch
    |> Signal.render 44100.0<Hz> 5.0<s>
    |> Wav.write "d:/temp/patch3.wav" 44100.0<Hz>
   