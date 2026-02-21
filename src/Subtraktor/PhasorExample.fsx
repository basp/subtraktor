#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Control.fs"
#load "Wav.fs"

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Plotly.NET
open Subtraktor

let phasor sampleRate =    
    let dt = 1.0 / sampleRate
    let rec loop acc = seq {
        yield acc
        let next = (acc + dt) % 1.0
        yield! loop next
    }
    loop 0.0

let example () =
    let sampleRate = 4410.0
    let duration = 3.0
    let numberOfSamples = int (sampleRate * duration)        

    let sine freq =
        fun phase ->
            sin (2.0 * Math.PI * freq * phase)    

    let lfo = sine 1.0

    let osc =
        fun phase ->
            let freq = (sin (2.0 * Math.PI * phase)) * 40.0
            let radians = 2.0 * Math.PI * phase
            let freq = (lfo phase) * 40.0
            sine freq phase
    
    let samples =
        phasor sampleRate
        |> Seq.take numberOfSamples
    
    let charts = Chart.combine [
        samples
        |> Seq.map lfo
        |> Seq.mapi (fun i x -> (float i, x))
        |> Array.ofSeq
        |> Chart.Line

        samples
        |> Seq.map osc
        |> Seq.mapi (fun i x -> (float i, x))
        |> Array.ofSeq
        |> Chart.Line
    ]

    charts |> Chart.show
    // |> Wav.write "d:/temp/test.wav" (LanguagePrimitives.FloatWithMeasure<Hz>(sampleRate))

    ()
    
example ()