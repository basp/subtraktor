namespace Subtraktor.Synthesis

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units

/// Audio Mathematics Fundamentals
/// 
/// This module provides basic mathematical operations needed for DSP and synthesis:
/// - Sample rate conversions and time calculations
/// - Frequency calculations and conversions
/// - Phase and radian operations
/// - Amplitude and decibel conversions
/// 
/// All functions are pure and work with SI units and standard audio conventions:
/// - Time in seconds (float)
/// - Frequency in Hz (float)
/// - Amplitude in linear scale [0.0..1.0] or as needed
/// - Sample rates in Hz (int)

/// Core audio mathematical functions for DSP operations
module Math =
    
    // ============================================================================
    // CONSTANTS
    // ============================================================================
    
    /// Standard concert pitch A4 in Hz (440 Hz)
    val A4_HZ : float<Hz>
    
    /// Two pi, commonly used for phase and radian calculations
    val TWO_PI : float<rad>
    
    // ============================================================================
    // TIME & SAMPLE CONVERSIONS
    // ============================================================================
    
    /// Convert a time in seconds to a sample count given a sample rate.
    /// 
    /// Example: at 44100 Hz, 1.0 second = 44100 samples
    val timeToSamples : sampleRate: int<Hz> -> timeSeconds:float<s> -> int
    
    /// Convert a sample count to time in seconds given a sample rate.
    /// 
    /// Example: at 44100 Hz, 44100 samples = 1.0 second
    val samplesToTime : sampleRate: int<Hz> -> samples:int -> float<s>
    
    /// Calculate the duration of one sample in seconds given a sample rate.
    /// 
    /// Example: at 44100 Hz, one sample duration ≈ 0.0000227 seconds
    val sampleDuration : sampleRate: int<Hz> -> float<s>
    
    // ============================================================================
    // FREQUENCY CONVERSIONS
    // ============================================================================
    
    /// Convert a MIDI note number to frequency in Hz.
    /// MIDI note 69 = A4 = 440 Hz (standard tuning).
    /// Each semitone multiplies frequency by 2^(1/12).
    val midiNoteToFrequency : midiNote:int -> float<Hz>
    
    /// Convert frequency in Hz to a MIDI note number.
    /// Returns the nearest MIDI note (0-127).
    val frequencyToMidiNote : freq:float<Hz> -> int
    
    /// Convert frequency in Hz to an angular frequency (radians/sample).
    /// This is used internally for oscillator phase calculations.
    /// 
    /// Formula: ω = 2π * f / sampleRate
    val frequencyToPhaseIncrement : sampleRate:int<Hz> -> freq:float<Hz> -> float<rad>
    
    // ============================================================================
    // AMPLITUDE & DECIBELS
    // ============================================================================
    
    /// Convert amplitude from linear scale to decibels (dB).
    /// 
    /// Note: 0 dB = 1.0 linear amplitude
    /// Negative dB values represent quieter amplitudes
    /// Formula: dB = 20 * log10(amplitude)
    val linearToDecibels : linear: float -> float<dB>
    
    /// Convert amplitude from decibels (dB) to linear scale.
    /// 
    /// Formula: linear = 10^(dB/20)
    val decibelsToLinear : decibels: float<dB> -> float
    
    // ============================================================================
    // PHASE & RADIAN OPERATIONS
    // ============================================================================
    
    /// Normalize a phase value to the range [0, 2π).
    /// This keeps phase values bounded and prevents numerical issues.
    val normalizePhase : phase: float<rad> -> float<rad>
    
    /// Wrap a phase value to [0, 1.0) range (useful when working with normalized phase).
    val wrapPhase : phase: float -> float

