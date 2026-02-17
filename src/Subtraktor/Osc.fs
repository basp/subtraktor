module Subtraktor.Osc

open System
open Subtraktor.Units

/// <summary>
/// Generates a pure sine‑wave oscillator at the given frequency.
/// </summary>
/// <remarks>
/// <para>
/// A sine oscillator produces the simplest possible periodic waveform: a
/// smooth, single‑frequency tone with no harmonics. It is the mathematical
/// foundation of all subtractive synthesis and serves as a building block for
/// more complex signals.
/// </para>
/// <para>
/// The oscillator is defined as:
/// <c>sin(2π · freq · t)</c>, where <c>freq</c> is expressed in Hertz and
/// <c>t</c> in seconds. The function is pure and time‑aligned: evaluating it
/// at the same time always yields the same amplitude.
/// </para>
/// </remarks>
let sine (freq: Frequency) : Signal =
    fun (t: Time) ->
        sin (2.0 * Math.PI * float freq * float t)
    
let saw (freq: Frequency) : Signal =
    fun (t: Time) ->
        let phase = float freq * float t
        2.0 * (phase - floor phase) - 1.0
        
let square (freq: Frequency) : Signal =
    fun (t: Time) ->
        let phase = float freq * float t
        if (phase - floor phase) < 0.5 then 1.0
        else -1.0
        
let private phase (freq: Frequency) (t: Time) =
    let f = float freq
    let x = (float t * f) % 1.0
    if x < 0.0 then x + 1.0 else x
    
let triangle (freq: Frequency) : Signal =
    fun t ->
        let x = phase freq t
        2.0 * abs (2.0 * x - 1.0) - 1.0