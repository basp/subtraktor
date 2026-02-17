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

let gate =
    Gate.between 0.0<s> 3.0<s>
    
let env =
    Env.``asr`` gate {
        Attack = 2.0<s>
        Release = 1.0<s>
    }
    
let data =
    env
    |> Viz.sample 44100.0<Hz> 5.0<s>
    
open Plotly.NET

data
|> Chart.Line
|> Chart.show