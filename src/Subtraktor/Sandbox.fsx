#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Gate.fs"
#load "Osc.fs"
#load "Wav.fs"
#load "Env.fs"
#load "Filter.fs"
#load "Viz.fs"

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor

open Plotly.NET

let gate =
    Gate.between 0.0<s> 3.0<s>   

let env =
    Env.``asr`` gate {
        Attack = 2.0<s>
        Release = 1.0<s>
    }

let example1 () =
    let data =
        env
        |> Viz.sample 44100.0<Hz> 5.0<s>
        
    data
    |> Chart.Line
    |> Chart.show

let example2 () =
    let gatedSignal = Env.apply env (Osc.saw 2.0<Hz>)
    let sr = 44100.0<Hz>
    let dur = 5.0<s>
    let envData = Viz.sample sr dur env
    let gateData =
        Viz.sample sr dur (fun t -> if gate t then 1.0 else 0.0)
    let sigData = Viz.sample sr dur gatedSignal
    Chart.combine [
        Chart.Line envData
        Chart.Line gateData
        Chart.Line sigData
    ]
    |> Chart.show
    
example2 ()