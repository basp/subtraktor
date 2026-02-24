#load @"..\Subtraktor\Synthesis\Math.fs"

/// Phase 0: Audio Mathematics Fundamentals - Interactive Exploration
/// 
/// This script helps us understand and verify the basic audio math we need for synthesis.
/// 
/// Key concepts to explore:
/// 1. Sample rates and time conversions - understanding how time maps to discrete samples
/// 2. MIDI and frequency conversions - standard note naming and pitch calculations  
/// 3. Phase and radian arithmetic - how oscillators step through waves
/// 4. Amplitude and decibels - loudness scales
/// 
/// Work through each section, run it, and verify the outputs make sense.

open Subtraktor

// ============================================================================
// SECTION 1: UNDERSTANDING SAMPLE RATE AND TIME
// ============================================================================
printfn "=== SECTION 1: Sample Rate & Time Conversions ==="
printfn ""

// Real-world audio is sampled at discrete intervals. Common rates:
// - CD quality: 44100 Hz (44,100 samples per second) 
// - Professional: 48000 Hz, 96000 Hz
// - High-res: 192000 Hz

let sampleRate = 44100  // CD quality

// Let's think about what this means:
// In 1 second of audio, we have exactly 44,100 samples
// Each sample represents one "snapshot" of the audio waveform

printfn "At sample rate: %d Hz" sampleRate
printfn "Duration of one sample: %.6f seconds (≈ %.2f microseconds)" 
    (sampleDuration sampleRate) 
    (sampleDuration sampleRate * 1_000_000.0)

// Exercise: How many samples in different durations?
let durations = [0.001; 0.01; 0.1; 1.0]
printfn "\nTime-to-samples conversions:"
for timeSeconds in durations do
    let samples = timeToSamples sampleRate timeSeconds
    printfn "  %.3f seconds = %d samples" timeSeconds samples

// Exercise: Convert back
printfn "\nVerify conversion works both ways:"
let testTime = 2.5
let samples = timeToSamples sampleRate testTime
let timeBack = samplesToTime sampleRate samples
printfn "  Start with: %.2f seconds → %d samples → %.2f seconds" testTime samples timeBack
printfn "  Matches: %b" (abs (timeBack - testTime) < 0.00001)

printfn ""

// ============================================================================
// SECTION 2: MIDI NOTES AND FREQUENCY
// ============================================================================
printfn "=== SECTION 2: MIDI Notes & Frequency Conversion ==="
printfn ""

// Musical notes are named and numbered. MIDI (Musical Instrument Digital Interface)
// assigns each note a number from 0 to 127.
// MIDI note 69 is A4 at 440 Hz - this is the standard tuning reference.

// The chromatic scale (all 12 semitones in an octave) is evenly spaced in logarithmic 
// frequency space. Each semitone multiplies frequency by 2^(1/12) ≈ 1.0595

printfn "Standard tuning reference: A4 = MIDI 69 = 440 Hz\n"

// Exercise: Convert some familiar notes to frequency
let notesToExplore = [
    ("C4", 60);
    ("A3", 57);
    ("A4", 69);
    ("A5", 81);
    ("C1", 12);
    ("C8", 108);
]

printfn "Converting MIDI note numbers to frequencies:"
for (name, midiNote) in notesToExplore do
    let freq = midiNoteToFrequency midiNote
    printfn "  %s (MIDI %d) → %.2f Hz" name midiNote freq

// Exercise: Verify the inverse conversion
printfn "\nVerify conversion both ways:"
for (name, midiNote) in notesToExplore do
    let freq = midiNoteToFrequency midiNote
    let midiBack = frequencyToMidiNote freq
    let match_str = if midiBack = midiNote then "✓" else "✗"
    printfn "  %s (MIDI %d) → %.2f Hz → MIDI %d %s" name midiNote freq midiBack match_str

// Exercise: Understand frequency relationships
printfn "\nFrequency relationships (important for music):"
let baseNote = 69  // A4
let baseFreq = midiNoteToFrequency baseNote
let octaveUp = midiNoteToFrequency (baseNote + 12)  // One octave = 12 semitones
let octaveDown = midiNoteToFrequency (baseNote - 12)
printfn "  A4 = %.2f Hz" baseFreq
printfn "  A5 (one octave up) = %.2f Hz (ratio: %.2f)" octaveUp (octaveUp / baseFreq)
printfn "  A3 (one octave down) = %.2f Hz (ratio: %.2f)" octaveDown (octaveDown / baseFreq)
printfn ""

// ============================================================================
// SECTION 3: PHASE AND OSCILLATOR STEPPING
// ============================================================================
printfn "=== SECTION 3: Phase Increment & Oscillator Stepping ==="
printfn ""

// To generate a sine wave, we need to step through the wave at regular intervals.
// Each sample, we advance a "phase" value. The amount we advance is the "phase increment".
// 
// Phase increment (in radians/sample) = 2π * frequency / sampleRate
// 
// Example: if we want a 440 Hz sine wave at 44100 Hz sampling:
//   - We complete 440 full cycles per second
//   - At 44100 samples/sec, that's 440/44100 ≈ 1 full cycle per 100 samples
//   - Each sample advances phase by: 2π * 440 / 44100 ≈ 0.0628 radians

let testFrequency = 440.0

let phaseIncrement = frequencyToPhaseIncrement sampleRate testFrequency
printfn "To generate %.0f Hz at %d Hz sampling:" testFrequency sampleRate
printfn "  Phase increment per sample: %.6f radians" phaseIncrement
printfn "  (in terms of 2π: %.4f * 2π)" (phaseIncrement / TWO_PI)

// Let's step through the first few samples and see the phase advance
printfn "\nPhase evolution for first 10 samples:"
let mutable phase = 0.0
for i in 0..9 do
    printf "  Sample %d: phase = %.6f rad (%.4f cycles) " i phase (phase / TWO_PI)
    
    // For a sine wave, the output would be:
    let output = sin phase
    printf "→ sin(phase) = %.6f\n" output
    
    phase <- phase + phaseIncrement

// Exercise: How many samples for one complete cycle?
let samplesPerCycle = TWO_PI / phaseIncrement |> int
printfn "\nFor %.0f Hz at %d Hz sampling:" testFrequency sampleRate
printfn "  Samples per complete cycle: %d" samplesPerCycle
printfn "  Frequency check: %d Hz / %d samples = %.2f Hz" sampleRate samplesPerCycle (float sampleRate / float samplesPerCycle)

printfn ""

// ============================================================================
// SECTION 4: AMPLITUDE AND DECIBELS
// ============================================================================
printfn "=== SECTION 4: Amplitude & Decibels ==="
printfn ""

// Loudness is tricky. Our ears perceive loudness logarithmically, so audio 
// engineers use a logarithmic scale called decibels (dB).
//
// 0 dB = 1.0 linear (reference level, "full scale")
// Each -3 dB ≈ half the power (≈ 0.707 linear amplitude)  
// Each -6 dB ≈ quarter the power (0.5 linear amplitude)
// Each -20 dB = 10x quieter

let amplitudesToTest = [1.0; 0.707; 0.5; 0.1; 0.01]

printfn "Linear amplitude to decibels:"
for amp in amplitudesToTest do
    let db = linearToDecibels amp
    printfn "  %.3f linear → %.2f dB" amp db

printfn "\nDecibels to linear amplitude:"
let dbToTest = [0.0; -3.0; -6.0; -12.0; -20.0]
for db in dbToTest do
    let amp = decibelsToLinear db
    printfn "  %.1f dB → %.6f linear" db amp

// Verify round-trip conversion
printfn "\nRound-trip verification:"
for amp in amplitudesToTest do
    let db = linearToDecibels amp
    let ampBack = decibelsToLinear db
    let error = abs (ampBack - amp)
    printfn "  %.3f → %.2f dB → %.6f (error: %.2e)" amp db ampBack error

printfn ""

// ============================================================================
// SUMMARY & NEXT STEPS
// ============================================================================
printfn "=== REFLECTION ==="
printfn ""
printfn "What we've understood:"
printfn "  ✓ Audio is sampled at fixed intervals (sample rate)"
printfn "  ✓ Time maps linearly to sample counts"
printfn "  ✓ MIDI notes have standard frequency mappings (12-TET tuning)"
printfn "  ✓ Oscillators step through phase at a rate determined by frequency & sample rate"
printfn "  ✓ Amplitude follows a logarithmic scale (decibels)"
printfn ""
printfn "Next: We'll use these foundations to build simple waveform generators (oscillators)!"
