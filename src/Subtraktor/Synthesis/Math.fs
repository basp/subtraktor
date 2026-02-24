namespace Subtraktor.Synthesis

open System

/// Core audio mathematical functions for DSP operations
module Math =
    // ============================================================================
    // CONSTANTS
    // ============================================================================
    
    let A4_HZ = 440.0    
    let TWO_PI = 2.0 * Math.PI
    
    // ============================================================================
    // TIME & SAMPLE CONVERSIONS
    // ============================================================================
    
    /// Convert a time in seconds to a sample count given a sample rate.
    let timeToSamples (sampleRate : int) (timeSeconds : float) : int =
        (timeSeconds * float sampleRate) |> int
    
    /// Convert a sample count to time in seconds given a sample rate.
    let samplesToTime (sampleRate : int) (samples : int) : float =
        float samples / float sampleRate
    
    /// Calculate the duration of one sample in seconds given a sample rate.
    let sampleDuration (sampleRate : int) : float =
        1.0 / float sampleRate
    
    // ============================================================================
    // FREQUENCY CONVERSIONS
    // ============================================================================
    
    /// Convert a MIDI note number to frequency in Hz.
    /// MIDI note 69 = A4 = 440 Hz (standard tuning).
    /// Each semitone is a factor of 2^(1/12) ≈ 1.0595...
    let midiNoteToFrequency (midiNote : int) : float =
        let semitonesFromA4 = float (midiNote - 69)
        A4_HZ * (2.0 ** (semitonesFromA4 / 12.0))
    
    /// Convert frequency in Hz to a MIDI note number.
    /// Returns the nearest MIDI note (0-127).
    let frequencyToMidiNote (frequencyHz : float) : int =
        let semitonesFromA4 = 12.0 * Math.Log2(frequencyHz / A4_HZ)
        (69 + (semitonesFromA4 |> Math.Round |> int))
        |> max 0 |> min 127
    
    /// Convert frequency in Hz to an angular frequency (radians/sample).
    /// This is the phase increment per sample, used in oscillators.
    /// Formula: ω = 2π * f / sampleRate
    let frequencyToPhaseIncrement (sampleRate : int) (frequencyHz : float) : float =
        TWO_PI * frequencyHz / float sampleRate
    
    // ============================================================================
    // AMPLITUDE & DECIBELS
    // ============================================================================
    
    /// Convert amplitude from linear scale to decibels (dB).
    /// 0 dB = 1.0 linear amplitude
    /// Formula: dB = 20 * log10(amplitude)
    let linearToDecibels (linear : float) : float =
        if linear > 0.0 then
            20.0 * Math.Log10(linear)
        else
            Double.NegativeInfinity
    
    /// Convert amplitude from decibels (dB) to linear scale.
    /// Formula: linear = 10^(dB/20)
    let decibelsToLinear (decibels : float) : float =
        10.0 ** (decibels / 20.0)
    
    // ============================================================================
    // PHASE & RADIAN OPERATIONS
    // ============================================================================
    
    /// Normalize a phase value to the range [0, 2π).
    /// This keeps phase values bounded and prevents numerical issues.
    let normalizePhase (phase : float) : float =
        let wrapped = phase % TWO_PI
        if wrapped < 0.0 then wrapped + TWO_PI else wrapped
    
    /// Wrap a phase value to [0, 1.0) range (useful when working with normalized phase).
    let wrapPhase (phase : float) : float =
        let wrapped = phase % 1.0
        if wrapped < 0.0 then wrapped + 1.0 else wrapped

