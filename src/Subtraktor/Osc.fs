module Subtraktor.Osc

open System
open Subtraktor.Signal

let sine (freq: float) : Signal =
    fun t -> sin (2.0 * Math.PI * freq * t)
    