namespace Subtraktor.Synthesis

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units
open Subtraktor.Synthesis.Math

type Oscillator = {
    mutable phase: float<rad>
    frequency: float<Hz>
    sampleRate: int<1/s>
}


//
// module Oscillator =
//     let sine (osc: Oscillator) : float =
//         let incr =
//             osc.frequency
//             |> frequencyToPhaseIncrement osc.sampleRate
//
//         0.0        
//     
//     let square (osc: Oscillator) : float<Hz> = 0.0<Hz>
//     
//     let saw (osc: Oscillator) : float = 0.0
//     
//     let triangle (osc: Oscillator) : float = 0.0
//     
//     let nextSample (osc: Oscillator) : float =
//         failwith "Not implemented"

