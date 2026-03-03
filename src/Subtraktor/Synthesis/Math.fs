namespace Subtraktor.Synthesis

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units

/// Core audio mathematical functions for DSP operations
module Math =
    // ============================================================================
    // CONSTANTS
    // ============================================================================
    
    let A4_HZ = 440.0<Hz>
    
    let TWO_PI = 2.0 * Math.PI |> LanguagePrimitives.FloatWithMeasure<rad>
    
    // ============================================================================
    // TIME & SAMPLE CONVERSIONS
    // ============================================================================
    
    /// Convert a time in seconds to a sample count given a sample rate.
    let timeToSamples (sampleRate: int<1/s>) (timeSeconds: float<s>) : int =
        (timeSeconds * float sampleRate) |> int
    
    /// Convert a sample count to time in seconds given a sample rate.
    let samplesToTime (sampleRate: int<1/s>) (samples : int) : float<s> =
        float samples / float sampleRate
        |> LanguagePrimitives.FloatWithMeasure<s>
    
    /// Calculate the duration of one sample in seconds given a sample rate.
    let sampleDuration (sampleRate : int<Hz>) : float<s> =
        1.0<s> / float sampleRate
    
    // ============================================================================
    // FREQUENCY CONVERSIONS
    // ============================================================================
    
    /// Convert a MIDI note number to frequency in Hz.
    /// MIDI note 69 = A4 = 440 Hz (standard tuning).
    /// Each semitone is a factor of 2^(1/12) ≈ 1.0595...
    let midiNoteToFrequency (midiNote : int) : float<Hz> =
        let semitonesFromA4 = float (midiNote - 69)
        A4_HZ * (2.0 ** (semitonesFromA4 / 12.0))
    
    /// Convert frequency in Hz to a MIDI note number.
    /// Returns the nearest MIDI note (0-127).
    let frequencyToMidiNote (freq: float<Hz>) : int =
        let semitonesFromA4 = 12.0 * Math.Log2(freq / A4_HZ)
        (69 + (semitonesFromA4 |> Math.Round |> int))
        |> max 0
        |> min 127
    
    /// Convert frequency in Hz to an angular frequency (radians/sample).
    /// This is the phase increment per sample, used in oscillators.
    /// Formula: ω = 2π * f / sampleRate
    let frequencyToPhaseIncrement (sampleRate: int<1/s>) (freq : float<Hz>) =
        TWO_PI * freq / (float sampleRate |> LanguagePrimitives.FloatWithMeasure<1/s>)
    
    // ============================================================================
    // AMPLITUDE & DECIBELS
    // ============================================================================
    
    /// Convert amplitude from linear scale to decibels (dB).
    /// 0 dB = 1.0 linear amplitude
    /// Formula: dB = 20 * log10(amplitude)
    let linearToDecibels (linear : float) : float<dB> =
        if linear > 0.0 then
            20.0 * Math.Log10(linear)
            |> LanguagePrimitives.FloatWithMeasure<dB>
        else
            Double.NegativeInfinity
            |> LanguagePrimitives.FloatWithMeasure<dB>
    
    /// Convert amplitude from decibels (dB) to linear scale.
    /// Formula: linear = 10^(dB/20)
    let decibelsToLinear (decibels : float<dB>) : float =
        10.0 ** (decibels / 20.0<dB>)
    
    // ============================================================================
    // PHASE & RADIAN OPERATIONS
    // ============================================================================
    
    /// Normalize a phase value to the range [0, 2π).
    /// This keeps phase values bounded and prevents numerical issues.
    let normalizePhase (phase: float<rad>) : float<rad> =
        let wrapped = phase % TWO_PI
        if wrapped < 0.0<rad> then wrapped + TWO_PI else wrapped
    
    /// Wrap a phase value to [0, 1.0) range (useful when working with normalized phase).
    let wrapPhase (phase : float) : float =
        let wrapped = phase % 1.0
        if wrapped < 0.0 then wrapped + 1.0 else wrapped

