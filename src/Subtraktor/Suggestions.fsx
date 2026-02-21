#r "nuget: Plotly.NET, 5.0.0"

open System
open Plotly.NET

let twoPi = 2.0 * Math.PI

let sampleRate = 1000.0

type OscState = { Phase: float }

let step state freq sampleRate =
    let next = state.Phase + (freq / sampleRate)
    { state with
        Phase =
            if next >= 1.0 then next - 1.0
            else next }
    
let osc1 = { Phase = 0.0 }
let osc2 = { Phase = 0.0 }

let nextSine sampleRate =
    fun freq state ->
        let y = sin (state.Phase * twoPi)
        (y, step state freq sampleRate)
        
let nextSaw sampleRate =
    fun freq state ->
        let y = 2.0 * state.Phase - 1.0
        (y, step state freq sampleRate)
        
