module Subtraktor.Filter

open System
open Units

let lowpass (cutoff: Frequency) (sampleRate: SampleRate) (signal: Signal) : Signal =
    // Right now, filters ignore the fact that Signal is a function of
    // continuous time, while the filter operates in discrete time. That’s fine —
    // Subtraktor’s philosophy embraces rawness.
    let computeCoefficient () =
        let x = float cutoff / float sampleRate
        1.0 - exp (-2.0 * Math.PI * x)    
    let mutable yPrev = 0.0
    let a = computeCoefficient ()
    fun t ->
        let x = signal t
        let y = yPrev + a * (x - yPrev)
        yPrev <- y
        y