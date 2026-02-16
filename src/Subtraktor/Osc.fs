module Subtraktor.Osc

open FSharp.Data.UnitSystems.SI.UnitSymbols

open System
open Subtraktor.Signal

/// <summary>
/// Generates a pure sine‑wave oscillator at the given frequency.
/// </summary>
/// <remarks>
/// <para>
/// A sine oscillator produces the simplest possible periodic waveform: a
/// smooth, single‑frequency tone with no harmonics. It is the mathematical
/// foundation of all subtractive synthesis, and serves as a building block for
/// more complex signals.
/// </para>
/// <para>
/// The oscillator is defined as:
/// <c>sin(2π · freq · t)</c>, where <c>freq</c> is expressed in Hertz and
/// <c>t</c> in seconds. The function is pure and time‑aligned: evaluating it
/// at the same time always yields the same amplitude.
/// </para>
/// </remarks>
let sine (freq: float<Hz>) : Signal =
    fun t -> sin (2.0 * Math.PI * float freq * float t)
    