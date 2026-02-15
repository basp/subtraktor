module Subtraktor.Osc

open FSharp.Data.UnitSystems.SI.UnitSymbols

open System
open Subtraktor.Signal

let sine (freq: float<Hz>) : Signal =
    fun t -> sin (2.0 * Math.PI * float freq * float t)
    