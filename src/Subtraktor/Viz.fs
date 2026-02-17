module Subtraktor.Viz

open Subtraktor.Units
open Subtraktor.Signal

let sample (sr: SampleRate) (duration: Time) (signal: Signal) =
    let dt = 1.0 / sr
    let samples = int (duration / dt)
    Array.init samples (fun i ->
        let t = float i * dt
        (t, signal t))