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

// Sets up two "triggers" - one at the start of the envelope and one 3s after.
let gate =
    Gate.between 0.0<s> 3.0<s>   

// Sets up a simple ASR envelope using the gate defined above.
// Attack phase duration is 2s, release phase duration is 1s.
let env =
    Env.``asr`` gate {
        Attack = 2.0<s>
        Release = 1.0<s>
    }

// Visualizes the envelope.
let example1 () =
    let data =
        env
        |> Viz.sample 44100.0<Hz> 5.0<s>
        
    data
    |> Chart.Line
    |> Chart.show

// Visualizes the envelope, gate and the signal.
let example2 () =
    // Sample rate.
    let sr = 44100.0<Hz>
    // Duration of the signal.
    let dur = 5.0<s>    
    // Use a very low frequency saw for visualization purposes.
    let gatedSignal = Osc.saw 2.0<Hz> |> Env.apply env
    // The shape of the envelope.
    let envData = env |> Viz.sample sr dur    
    // The trigger signals of the gate.
    let gateData =
        let plot t =
            match gate t with
            | true -> 1.0
            | _ -> 0.0
        plot |> Viz.sample sr dur
    // The actual signal, confined to the envelope.
    let sigData = gatedSignal |> Viz.sample sr dur
    Chart.combine [
        Chart.Line envData
        Chart.Line gateData
        Chart.Line sigData
    ]
    |> Chart.show
    
example2 ()