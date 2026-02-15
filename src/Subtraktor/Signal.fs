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

/// <summary>
/// Creates a signal that always returns the same amplitude, regardless of
/// time.
/// </summary>
/// <remarks>
/// <c>constant</c> is useful for constructing time‑invariant signals such as
/// offsets, biases, or control values. It reinforces the idea that a
/// <c>Signal</c> is simply a function of time: even if the time parameter is
/// ignored, the result is still a valid signal in the <b>Subtraktor</b> model.
/// </remarks>
let constant (value: float) : Signal =
    fun _ -> value
    
/// <summary>
/// A signal that always produces zero amplitude.
/// </summary>
/// <remarks>
/// <c>silence</c> is the additive identity for signals: adding it to any other
/// signal leaves the original unchanged. It is useful as a neutral element
/// when building signals incrementally or conditionally.
/// </remarks>    
let silence : Signal = constant 0.0

/// <summary>
/// Combines two signals by adding their amplitudes pointwise.
/// </summary>
/// <remarks>
/// <p>
/// <c>add</c> models the physical superposition of sound waves: at any moment
/// in time, the resulting amplitude is the sum of the individual amplitudes.
/// This operation is pure, time-aligned, and does not introduce phase shifts
/// or distortion.
/// </p>
/// 
/// <p>
/// Because addition is associative and commutative, complex signals can be
/// built from simpler ones in a predictable way. Callers are responsible for
/// ensuring the resulting amplitude stays within a desired range (e.g., to
/// avoid clipping).
/// </p>
/// </remarks>
let add (s1: Signal) (s2: Signal) : Signal =
    fun t -> s1 t + s2 t
    
/// <summary>
/// Multiplies a signal’s amplitude by a constant factor.
/// </summary>
/// <remarks>
/// <c>scale</c> adjusts the overall loudness of a signal without altering its
/// shape, frequency content, or phase. This is the simplest form of gain-control
/// A scale factor greater than 1 amplifies the signal; a factor between 0 and 1
/// attenuates it. Negative values invert the waveform, which can be musically
/// meaningful in some contexts.
/// </remarks>    
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