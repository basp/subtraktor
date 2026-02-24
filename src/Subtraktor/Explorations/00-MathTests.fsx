#load @"..\Synthesis\Math.fs"

open System
open Subtraktor.Synthesis.Math

/// Phase 0: Audio Mathematics Unit Tests
/// 
/// This module contains unit tests for fundamental DSP math operations.
/// We use simple assertion functions rather than a test framework initially.
/// 
/// Tests drive the development of the Math module and verify correctness
/// of time/frequency conversions, MIDI operations, and decibel calculations.

// ============================================================================
// TEST INFRASTRUCTURE
// ============================================================================

let mutable testsPassed = 0
let mutable testsFailed = 0

/// Asserts that a boolean condition is true
let inline assert_true (condition : bool) (testName : string) : unit =
    if condition then
        testsPassed <- testsPassed + 1
        printf "  ✓ %s\n" testName
    else
        testsFailed <- testsFailed + 1
        printf "  ✗ FAILED: %s\n" testName

/// Asserts that two floats are approximately equal (within tolerance)
let inline assert_approx_equal (expected : float) (actual : float) (tolerance : float) (testName : string) : unit =
    let diff = abs (expected - actual)
    if diff <= tolerance then
        testsPassed <- testsPassed + 1
        printf "  ✓ %s (diff: %.2e)\n" testName diff
    else
        testsFailed <- testsFailed + 1
        printf "  ✗ FAILED: %s - expected %.6f, got %.6f (diff: %.2e)\n" testName expected actual diff

/// Asserts that two integers are equal
let inline assert_equal (expected : int) (actual : int) (testName : string) : unit =
    if expected = actual then
        testsPassed <- testsPassed + 1
        printf "  ✓ %s\n" testName
    else
        testsFailed <- testsFailed + 1
        printf "  ✗ FAILED: %s - expected %d, got %d\n" testName expected actual

let run_test_suite (suiteName : string) : unit =
    printf "\n=== %s ===\n" suiteName

let report_results () =
    printf "\n"
    printf "╔════════════════════════════════════════╗\n"
    printf "║     TEST RESULTS                       ║\n"
    printf "║  Passed: %d                             ║\n" testsPassed
    printf "║  Failed: %d                             ║\n" testsFailed
    printf "╚════════════════════════════════════════╝\n"

// ============================================================================
// TEST SUITE 1: TIME & SAMPLE CONVERSIONS
// ============================================================================

run_test_suite "TIME & SAMPLE CONVERSIONS"

// Test: sampleDuration matches inverse of sampleRate
let sampleRate_44100 = 44100
let sampleDur = sampleDuration sampleRate_44100
assert_approx_equal (1.0 / float sampleRate_44100) sampleDur 1e-10 
    "sampleDuration at 44100 Hz = 1/44100"

// Test: timeToSamples and samplesToTime are inverses
let testTime = 2.5
let samples = timeToSamples sampleRate_44100 testTime
let timeBack = samplesToTime sampleRate_44100 samples
assert_approx_equal testTime timeBack 1e-6
    "timeToSamples and samplesToTime are inverses"

// Test: 1 second = sample rate
assert_equal sampleRate_44100 (timeToSamples sampleRate_44100 1.0)
    "1 second at 44100 Hz = 44100 samples"

// Test: specific time conversions
assert_equal 22050 (timeToSamples sampleRate_44100 0.5)
    "0.5 seconds at 44100 Hz = 22050 samples"

assert_equal 441 (timeToSamples sampleRate_44100 0.01)
    "0.01 seconds at 44100 Hz = 441 samples"

// Test: samplesToTime correctness
assert_approx_equal 1.0 (samplesToTime sampleRate_44100 44100) 1e-10
    "44100 samples at 44100 Hz = 1.0 second"

assert_approx_equal 0.5 (samplesToTime sampleRate_44100 22050) 1e-10
    "22050 samples at 44100 Hz = 0.5 second"

// ============================================================================
// TEST SUITE 2: FREQUENCY CONVERSIONS (MIDI & Hz)
// ============================================================================

run_test_suite "FREQUENCY CONVERSIONS"

// Test: A4 reference pitch
let a4_freq = midiNoteToFrequency 69
assert_approx_equal 440.0 a4_freq 1e-10
    "MIDI note 69 (A4) = 440 Hz"

// Test: Octaves are 2x or 0.5x frequency
let a5_freq = midiNoteToFrequency 81   // A5 = A4 + 12 semitones (1 octave)
assert_approx_equal (440.0 * 2.0) a5_freq 1e-6
    "A5 (MIDI 81) = 2 * A4 frequency"

let a3_freq = midiNoteToFrequency 57   // A3 = A4 - 12 semitones (1 octave)
assert_approx_equal (440.0 / 2.0) a3_freq 1e-6
    "A3 (MIDI 57) = A4 / 2 frequency"

// Test: C notes across different octaves
let c4_freq = midiNoteToFrequency 60
let c5_freq = midiNoteToFrequency 72
assert_approx_equal (c4_freq * 2.0) c5_freq 1e-6
    "C5 frequency = 2 * C4 frequency"

// Test: Semitone ratio is 2^(1/12)
let semitone_ratio = (midiNoteToFrequency 70) / (midiNoteToFrequency 69)
let expected_ratio = 2.0 ** (1.0 / 12.0)
assert_approx_equal expected_ratio semitone_ratio 1e-10
    "Semitone frequency ratio = 2^(1/12)"

// Test: frequency to MIDI note conversion (round trip)
let testFrequencies = [220.0; 440.0; 880.0; 110.0]
for freq in testFrequencies do
    let midiNote = frequencyToMidiNote freq
    let freqBack = midiNoteToFrequency midiNote
    // Tolerance is larger here because we're rounding to nearest MIDI note
    assert_approx_equal freq freqBack 5.0  // ±5 Hz tolerance for rounding
        (sprintf "Frequency round-trip: %.2f Hz → MIDI %d → %.2f Hz" freq midiNote freqBack)

// Test: MIDI boundaries
assert_equal 0 (frequencyToMidiNote 0.1)
    "Very low frequency clamps to MIDI 0"

assert_equal 127 (frequencyToMidiNote 20000.0)
    "Very high frequency clamps to MIDI 127"

// ============================================================================
// TEST SUITE 3: PHASE INCREMENT (for oscillators)
// ============================================================================

run_test_suite "PHASE INCREMENT & OSCILLATOR STEPPING"

// Test: phase increment for 440 Hz at 44100 Hz
let phaseInc_440 = frequencyToPhaseIncrement sampleRate_44100 440.0
let expected_phase_inc = (2.0 * Math.PI * 440.0) / float sampleRate_44100
assert_approx_equal expected_phase_inc phaseInc_440 1e-12
    "Phase increment for 440 Hz at 44100 Hz"

// Test: 0 Hz should give 0 phase increment
let phaseInc_zero = frequencyToPhaseIncrement sampleRate_44100 0.0
assert_approx_equal 0.0 phaseInc_zero 1e-12
    "Phase increment for 0 Hz = 0"

// Test: Phase increment at different sample rates
let phaseInc_48k = frequencyToPhaseIncrement 48000 440.0
let phaseInc_44k = frequencyToPhaseIncrement 44100 440.0
assert_true (phaseInc_48k < phaseInc_44k)
    "Higher sample rate → smaller phase increment for same frequency"

// ============================================================================
// TEST SUITE 4: PHASE NORMALIZATION
// ============================================================================

run_test_suite "PHASE NORMALIZATION"

// Test: normalizePhase keeps values in [0, 2π)
let phase1 = normalizePhase (Math.PI * 3.0)
assert_true (phase1 >= 0.0 && phase1 < TWO_PI)
    "normalizePhase keeps result in [0, 2π)"

// Test: normalizePhase of 0 is 0
assert_approx_equal 0.0 (normalizePhase 0.0) 1e-12
    "normalizePhase(0) = 0"

// Test: normalizePhase of 2π is 0
assert_approx_equal 0.0 (normalizePhase TWO_PI) 1e-12
    "normalizePhase(2π) = 0"

// Test: normalizePhase of negative value wraps correctly
let negPhase = normalizePhase (-Math.PI)
assert_approx_equal Math.PI negPhase 1e-12
    "normalizePhase(-π) = π"

// Test: wrapPhase for [0, 1.0) range
let wrapped1 = wrapPhase 1.5
assert_true (wrapped1 >= 0.0 && wrapped1 < 1.0)
    "wrapPhase keeps result in [0, 1.0)"

let wrapped2 = wrapPhase -0.5
assert_approx_equal 0.5 wrapped2 1e-12
    "wrapPhase(-0.5) = 0.5"

// ============================================================================
// TEST SUITE 5: AMPLITUDE & DECIBELS
// ============================================================================

run_test_suite "AMPLITUDE & DECIBELS"

// Test: 1.0 linear = 0 dB
assert_approx_equal 0.0 (linearToDecibels 1.0) 1e-10
    "1.0 linear amplitude = 0 dB"

// Test: decibels to linear is inverse of linear to decibels
let testAmplitudes = [0.1; 0.5; 1.0; 2.0]
for amp in testAmplitudes do
    let db = linearToDecibels amp
    let ampBack = decibelsToLinear db
    assert_approx_equal amp ampBack 1e-10
        (sprintf "Amplitude round-trip: %.2f → %.2f dB → %.2f" amp db ampBack)

// Test: specific dB values
assert_approx_equal 0.5 (decibelsToLinear -6.0) 1e-6
    "−6 dB ≈ 0.5 linear amplitude"

assert_approx_equal 0.1 (decibelsToLinear -20.0) 1e-6
    "−20 dB = 0.1 linear amplitude"

// Test: −3 dB ≈ 0.707 (reciprocal of sqrt(2))
assert_approx_equal (1.0 / Math.Sqrt(2.0)) (decibelsToLinear -3.0) 1e-6
    "−3 dB ≈ 1/√2"

// ============================================================================
// FINAL REPORT
// ============================================================================

report_results ()

if testsFailed = 0 then
    printfn "🎉 All tests passed! Ready for Phase 1.\n"
else
    printfn "⚠️  Some tests failed. Fix issues before proceeding.\n"
