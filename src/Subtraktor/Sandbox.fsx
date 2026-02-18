#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Wav.fs"

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor

open Plotly.NET

let osc1 = Signal.sine 3.60<Hz>
let osc2 = Signal.sine 2.70<Hz>
let mixed = Signal.mix osc1 osc2

        
let inline (+) (a: Signal) (b: Signal) =
    Signal.add a b
    
let inline (*) (a: Signal) (b: Signal) =
    Signal.mul a b

let c = osc1 + osc2

let scaled =
    Signal.mix osc1 osc2
    |> Signal.scale 0.6

scaled
|> Signal.sample 44100.0<Hz> 5.0<s>
|> Chart.Line
|> Chart.show
