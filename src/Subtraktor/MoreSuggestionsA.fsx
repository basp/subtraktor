#r "nuget: Plotly.NET, 5.0.0"

open System
open Plotly.NET

type State = {
    Phase: float
}

let normalizedPhase x =
    if x >= 1.0 then x - 1.0 else x 

let sawGen sampleRate =
    fun frequency state ->
        let delta = frequency / sampleRate
        let phase' = normalizedPhase (state.Phase + delta)
        let y = (2.0 * state.Phase) - 1.0
        (y, { Phase = phase' })

let saw = sawGen 44100.0

