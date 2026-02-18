#r "nuget: Plotly.NET, 5.0.0"

#load "Units.fs"
#load "Signal.fs"
#load "Control.fs"
#load "Wav.fs"

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor
open Plotly.NET

let inline (+) (a: Signal) (b: Signal) =
    Signal.add a b
    
let inline (*) (a: Signal) (b: Signal) =
    Signal.mul a b

type Interp = float -> float -> float

type Phase =
    { Name: string
      Duration: Time
      Target: float
      Interpolate: float -> float }   

let def = [
    { Name = "A"
      Duration = 1.5<s>
      Target = 1.0
      Interpolate = id }
    
    { Name = "S"
      Duration = 1.0<s>
      Target = 0.8
      Interpolate = fun _ -> 1.0 }
      
    { Name = "R"
      Duration = 2.0<s>
      Target = 0.0
      Interpolate = id }
      
let buildEnvelope (phases: Phase list) (gate: Signal) : Signal =
    let trend = Control.trend gate

    // Mutable runtime state
    let mutable currentPhase = -1
    let mutable phaseStartTime = 0.0<s>
    let mutable phaseStartValue = 0.0

    fun t ->
        // 1. Handle gate events
        match trend t with
        | Rising ->
            currentPhase <- 0
            phaseStartTime <- t
            phaseStartValue <- 0.0

        | Falling ->
            // For now: ignore falling edges (AD-style behavior)
            ()

        | Stable -> ()

        // 2. If no active phase, output 0
        if currentPhase < 0 || currentPhase >= phases.Length then
            0.0
        else
            let phase = phases.[currentPhase]

            // 3. Compute normalized time within the phase
            let x = (t - phaseStartTime) / phase.Duration

            if x >= 1.0 then
                // Phase finished → move to next
                currentPhase <- currentPhase + 1
                phaseStartTime <- t
                phaseStartValue <- phase.Target
                phase.Target
            else
                // 4. Interpolate within the phase
                let shaped = phase.Interpolate x
                phaseStartValue + (phase.Target - phaseStartValue) * shaped

let gate = Control.between 1.0<s> 5.0<s>
