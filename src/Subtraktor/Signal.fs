module Subtraktor.Signal

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units

/// <summary>
/// Represents a <b>continuous-time audio signal</b>.
/// Given a time value in <b>seconds</b>, it samples amplitude.
/// </summary>
/// <remarks>
/// A <c>Signal</c> is pure and from a client perspective has
/// <b>no side effects</b>. For the same input time, it always returns the same
/// output value.
/// </remarks>
type Signal = float<s> -> float

let constant (value: float) : Signal =
    fun _ -> value
    
let silence : Signal = constant 0.0

let add (s1: Signal) (s2: Signal) : Signal =
    fun t -> s1 t + s2 t
    
let scale (k: float) (s: Signal) : Signal =
    fun t -> k * s t
    
let mix (s1: Signal) (s2: Signal) : Signal =
    scale 0.5 (add s1 s2)
    
/// <summary>
/// Applies a signal transformation in a pipeline-friendly way.
/// </summary>
/// <remarks>
/// The <c>apply</c> combinator is useful to make signal processing chains
/// read naturally from left to right. Instead of nesting function calls,
/// the <c>apply</c> operator allows transformations to be composed in a clear
/// linear style:
/// <example>
/// <code>
/// let signal =
///     |> apply (scale 0.5)
///     |> apply (add anotherSignal)
/// </code>
/// </example>
/// This improves readability without introducing new semantics.
/// Executing <c>apply f s</c> is equivalent to <c>f s</c>.
/// </remarks>
let apply (f: Signal -> Signal) (s: Signal) : Signal =
    f s   

/// <summary>
/// Converts a <b>continuous-time signal</b> into a <b>discrete buffer of audio samples</b>.
/// </summary>
/// <remarks>
/// <p>
/// <c>render</c> is the bridge between <b>Subtraktor’s mathematical signal
/// model</b> and real-world digital audio. A <c>Signal</c> represents an ideal,
/// continuous function of time; rendering evaluates that function at evenly
/// spaced time steps determined by the sample rate.
/// </p>
///
/// <p>
/// The resulting array contains <c>duration * sampleRate</c> samples, starting
/// at <c>t = 0</c> seconds. Each sample is produced by calling the signal with
/// a time value expressed in seconds. The function is pure: rendering the same
/// signal with the same parameters always yields the same output.
///</p>
///
/// <p>
/// Constraints:
/// <ul>
/// <li><c>sampleRate</c> must be a positive frequency in Hertz.</li>
/// <li><c>duration</c> must be non-negative.</li>
/// <li>No antialiasing or band-limiting is performed; callers are responsible
/// for ensuring the signal is suitable for discrete sampling.</li>
/// </ul>
/// </p>
/// </remarks>
let render
    (sampleRate: float<Hz>)
    (duration: float<s>)
    (signal: Signal) : float[] =
        let sampleCount = int (duration * sampleRate)
        let sample i = (float (i * 1<samples>) / sampleRate)
        Array.init sampleCount (fun i -> sample i |> signal)