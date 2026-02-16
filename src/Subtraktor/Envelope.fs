module Subtraktor.Envelope

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units
open Subtraktor.Signal

type Envelope = float<s> -> float

/// <summary>
/// Creates a simple attack–decay (AD) amplitude envelope.
/// </summary>
/// <remarks>
/// <para>
/// The envelope rises linearly from 0.0 to 1.0 over the specified
/// <paramref name="attack"/> time, then falls linearly from 1.0 to 0.0
/// over the specified <paramref name="decay"/> time.
/// </para>
/// <para>
/// After <c>attack + decay</c> seconds, the envelope outputs <c>0.0</c>
/// for all further times. This makes the AD envelope a one‑shot shape:
/// it does not sustain and produces zero beyond its duration.
/// </para>
/// <para>
/// When applied to a <c>Signal</c>, any portion of the rendered audio
/// beyond the envelope duration will be silent.
/// </para>
/// </remarks>
/// <param name="attack">
/// Time for the envelope to rise from 0.0 to 1.0.
/// </param>
/// <param name="decay">
/// Time for the envelope to fall from 1.0 back to 0.0.
/// </param>
/// <param name="t">
/// The time coordinate at which to evaluate the envelope.
/// </param>
/// <returns>
/// An envelope function mapping <c>Time</c> to amplitude in the range [0.0, 1.0].
/// </returns>
let ad (attack: Time) (decay: Time) : Envelope =
    fun (t: float<s>) ->
        if t < attack then
            float (t / attack)
        elif t < attack + decay then
            let dt = t - attack
            1.0 - float (dt / decay)
        else
            0.0
            
/// <summary>
/// Applies an amplitude envelope to a signal.
/// </summary>
/// <remarks>
/// <para>
/// The resulting signal is the pointwise product of the original <paramref name="signal"/>
/// and the provided <paramref name="envelope"/> function. This allows envelopes to shape the
/// amplitude of any signal over time, such as adding attack/decay characteristics or
/// other dynamic contours.
/// </para>
/// <para>
/// The envelope is evaluated at the same time coordinate as the signal. If the envelope
/// returns <c>0.0</c> for a given time, the output will be silent at that point. If the
/// envelope ends before the rendered duration, the remainder of the output will be silent.
/// </para>
/// <para>
/// This function is purely multiplicative and does not normalize, clamp, or otherwise
/// modify the signal beyond applying the envelope's amplitude.
/// </para>
/// </remarks>
/// <param name="envelope">
/// An envelope function mapping <c>Time</c> to amplitude in the
/// range [0.0, 1.0].
/// </param>
/// <param name="t">
/// The time coordinate at which to evaluate the envelope.
/// </param>
/// <param name="signal">
/// The signal to which the envelope will be applied.
/// </param>
/// <returns>
/// A new signal whose amplitude is shaped by the envelope.
/// </returns>
let withEnvelope (envelope: Signal) signal =
    fun t -> signal t * envelope t