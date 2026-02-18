#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Wav.fs"

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor
open Plotly.NET

let inline (+) (a: Signal) (b: Signal) =
    Signal.add a b
    
let inline (*) (a: Signal) (b: Signal) =
    Signal.mul a b

