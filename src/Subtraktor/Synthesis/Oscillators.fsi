namespace Subtraktor.Synthesis

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units

type Oscillator = {
    mutable phase: float<rad>
    frequency: float<Hz>
    sampleRate: int<1/s>
}

// module Oscillator =
//     val sine:  int<1/s> -> float<Hz> -> float<s> -> float array
//     
//     val square: int<1/s> -> float<Hz> -> float<s> -> float array
//     
//     val saw: int<1/s> -> float<Hz> -> float<s> -> float array        
//     
//     val triangle: int<1/s> -> float<Hz> -> float<s> -> float array
//     
//     val nextSample: Oscillator -> float
