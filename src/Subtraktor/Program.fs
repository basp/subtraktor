open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor
open Subtraktor.Signal
open Subtraktor.Osc
open Subtraktor.Wav
open Subtraktor.Envelope

let lowPianoC = sine 32.7<Hz>
let lowBassGuitarE = sine 41.2<Hz>
let lowNoteOn5StringBass = sine 30.9<Hz>
let osc = sine 200.0<Hz>

let c3 = sine 130.81<Hz>
let d3 = sine 146.83<Hz>
let e3 = sine 164.81<Hz>
let f3 = sine 174.61<Hz>
let g3 = sine 196.0<Hz>

// c3 |> render 44100.0<Hz> 5.0<s> |> writeWav "d:/temp/c3.wav" 44100.0<Hz>
// d3 |> render 44100.0<Hz> 5.0<s> |> writeWav "d:/temp/d3.wav" 44100.0<Hz>
// e3 |> render 44100.0<Hz> 5.0<s> |> writeWav "d:/temp/e3.wav" 44100.0<Hz>
// f3 |> render 44100.0<Hz> 5.0<s> |> writeWav "d:/temp/f3.wav" 44100.0<Hz>
// g3 |> render 44100.0<Hz> 5.0<s> |> writeWav "d:/temp/g3.wav" 44100.0<Hz>

add e3 g3
|> withEnvelope (ad 2.0<s> 3.0<s>)
|> render 44100.0<Hz> 5.0<s>
|> writeWav "d:/temp/ad.wav" 44100.0<Hz>
