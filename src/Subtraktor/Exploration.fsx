#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Gate.fs"
#load "Viz.fs"

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor
open Subtraktor.Units
open Plotly.NET

type ADSR = {
    Attack: Time
    Decay: Time
    Sustain: float
    Release: Time
}

type State =
    | Idle
    | Attack
    | Decay
    | Sustain
    | Release

let envelope (gate: Gate) (adsr: ADSR) : Signal =
    let mutable state = Idle
    let mutable lastGate = false
    let mutable attackStart = 0.0<s>
    let mutable decayStart = 0.0<s>
    let mutable releaseStart = 0.0<s>
    
    fun t ->
        let newGate = gate t
        match lastGate, newGate with
        // Rising edge.
        | false, true ->
            state <- Attack
            attackStart <- t
        // Falling edge.
        | true, false ->
            state <- Release
            releaseStart <- t
        // Idle.
        | _ -> ()
        
        lastGate <- newGate
        
        match state with
        | Idle -> 0.0
        | Attack ->
            let x = (t - attackStart) / adsr.Attack
            if x >= 1.0 then
                decayStart <- t
                state <- Decay
                1.0
            else x
        | Decay ->
            let x = 1.0 - (t - decayStart) / adsr.Decay
            if x <= adsr.Sustain then
                state <- Sustain
                adsr.Sustain
            else x
        | Sustain -> adsr.Sustain
        | Release ->
            let x = adsr.Sustain - (t - releaseStart) / adsr.Release
            if x <= 0.0 then
                state <- Idle
                0.0
            else x

let ``basic example`` () =
    let cfg =
        { Attack = 2.0<s>
          Decay = 1.5<s>
          Sustain = 0.5
          Release = 1.0<s> }
        
    let gate = Gate.between 1.0<s> 6.0<s>
    
    let adsr = envelope gate cfg
    
    adsr
    |> Viz.sample 44100.0<Hz> 10.0<s>
    |> Chart.Line
    |> Chart.show

``basic example`` ()