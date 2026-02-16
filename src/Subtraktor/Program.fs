open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Signal
open Subtraktor.Osc
open Subtraktor.Wav
open Subtraktor.Envelope
open Subtraktor.Filter

// let c3 = sine 130.81<Hz>
// let d3 = sine 146.83<Hz>
// let e3 = sine 164.81<Hz>
// let f3 = sine 174.61<Hz>
// let g3 = sine 196.0<Hz>

let c3 = sine 130.81<Hz>
let d3 = saw 146.83<Hz>
let f3 = square 174.61<Hz>

add c3 f3
|> apply (add d3)
|> withEnvelope (ad 2.0<s> 3.0<s>)
|> apply (lowpass 170.5<Hz> 44100.0<Hz>)
|> render 44100.0<Hz> 5.0<s>
|> writeWav "d:/temp/ad.wav" 44100.0<Hz>
