# Phase 1 Preview: Waveform Generators & Oscillators

> This document previews Phase 1. Start here once Phase 0 is complete and you're comfortable with audio math fundamentals.

---

## What Phase 1 Is About

Phase 1 builds on Phase 0's math foundation to create **oscillators** — functions that generate periodic waveforms (sine, square, sawtooth, triangle).

### Key Question
*"Given a frequency and sample rate, how do I generate a sine wave that actually sounds like that pitch?"*

**Answer:** Use the phase accumulator from Phase 0 to step through the waveform, one sample at a time.

---

## What You'll Learn

### 1. Oscillator Function Signatures
```fsharp
// Pure oscillator generators (no internal state)
val sineWave : sampleRate:int → frequency:float → duration:float → float array

// Stateful oscillator (wraps pure math for real-time use)
type Oscillator = {
    mutable phase : float
    frequency : float
    sampleRate : int
}
val nextSample : Oscillator → float
```

### 2. Waveform Shapes
- **Sine**: Pure, smooth, warm
- **Square**: Bright, hollow, retro synth character
- **Sawtooth**: Harsh, buzzy, rich harmonics (great for subtractive synth)
- **Triangle**: Middle ground (between sine and square)

### 3. Frequency Accuracy Testing
```fsharp
// How do we verify a 440 Hz oscillator actually produces 440 Hz?
// Option 1: Count zero crossings
// Option 2: Spectral analysis (FFT-style)
// Option 3: Measure period of one cycle
```

### 4. Phase Continuity
```fsharp
// Problem: If we generate audio in chunks, phase must continue smoothly
// between chunks. Otherwise we get clicks/pops.

// Solution: Keep oscillator state across calls
let mutable phase = 0.0
let generate_chunk duration =
    let samples = []
    for i in 0..duration do
        samples.Add(sin(phase))
        phase <- phase + phaseIncrement
        if phase >= TWO_PI then phase <- phase - TWO_PI
    samples
```

---

## Test-Driven Development for Phase 1

### Test 1: Sine Oscillator at 440 Hz
```fsharp
let [<Test>] ``440 Hz sine oscillator`` () =
    // Generate 1 second at 44100 Hz (44100 samples)
    let samples = sineWave 44100 440.0 1.0
    
    // Verify: Should have ~440 complete cycles
    let zeroCrossings = countZeroCrossings samples
    let expectedCycles = 440
    
    Assert.AreEqual(expectedCycles * 2, zeroCrossings)
    // Each cycle has 2 zero crossings
```

### Test 2: Phase Wrapping (No Numerical Drift)
```fsharp
let [<Test>] ``oscillator phase wraps correctly`` () =
    // Generate 10 seconds of audio
    let osc = Oscillator { 
        phase = 0.0
        frequency = 440.0
        sampleRate = 44100
    }
    
    for i in 0..441000 do  // 10 seconds of samples
        let sample = nextSample osc
        // Phase should stay in [0, 2π)
        Assert.IsTrue(osc.phase >= 0.0 && osc.phase < TWO_PI)
```

### Test 3: Waveform Shapes
```fsharp
let [<Test>] ``sine wave oscillates correctly`` () =
    let samples = sineWave 44100 1.0 1.0  // 1 Hz = 1 cycle/sec
    
    // Find the peak (should be at ~quarter cycle)
    let peak = samples |> Array.max
    Assert.IsTrue(peak > 0.99, "Should reach ≈1.0")

let [<Test>] ``square wave has discontinuities`` () =
    let samples = squareWave 44100 440.0 0.1
    
    // Find discontinuities (large changes between samples)
    let discontinuities = ref 0
    for i in 1..samples.Length-1 do
        let diff = abs(samples.[i] - samples.[i-1])
        if diff > 0.5 then incr discontinuities
    
    // Square wave should have clear transitions
    Assert.IsTrue(!discontinuities > 100)
```

### Test 4: Frequency Relationships
```fsharp
let [<Test>] ``octave relationship`` () =
    // A5 should have half the period of A4 (or same freq, different samples per cycle)
    let a4 = sineWave 44100 440.0 0.1
    let a5 = sineWave 44100 880.0 0.1
    
    let cycles_a4 = countCycles a4
    let cycles_a5 = countCycles a5
    
    // In the same duration, A5 completes twice as many cycles
    Assert.AreEqual(cycles_a4 * 2, cycles_a5)
```

---

## Implementation Strategy

### Step 1: Pure Oscillator Functions
```fsharp
module Oscillators =
    // Pure functions (deterministic, no state)
    let sineWave sampleRate frequency duration =
        let phaseInc = frequencyToPhaseIncrement sampleRate frequency
        let numSamples = timeToSamples sampleRate duration
        
        let mutable phase = 0.0
        Array.init numSamples (fun _ ->
            let sample = sin(phase)
            phase <- phase + phaseInc
            if phase >= TWO_PI then phase <- normalizePhase phase
            sample
        )
    
    let squareWave sampleRate frequency duration =
        // Similar to sine, but: sample = if sin(phase) >= 0 then 1.0 else -1.0
        
    let sawtoothWave sampleRate frequency duration =
        // Similar to sine, but: sample = 2.0 * (phase / TWO_PI - 0.5)
        
    let triangleWave sampleRate frequency duration =
        // Piecewise linear between -1 and 1
```

### Step 2: Stateful Oscillator Type
```fsharp
type Oscillator = {
    mutable phase : float
    frequency : float
    sampleRate : int
}

let createOscillator sampleRate frequency = {
    phase = 0.0
    frequency = frequency
    sampleRate = sampleRate
}

let nextSample (osc : Oscillator) : float =
    let phaseInc = frequencyToPhaseIncrement osc.sampleRate osc.frequency
    let sample = sin(osc.phase)
    osc.phase <- normalizePhase (osc.phase + phaseInc)
    sample

let nextSamples (osc : Oscillator) (count : int) : float array =
    Array.init count (fun _ -> nextSample osc)
```

### Step 3: Waveform Selector
```fsharp
type WaveformType = 
    | Sine 
    | Square 
    | Sawtooth 
    | Triangle

let createWaveOscillator sampleRate frequency waveformType =
    // Return a function: unit → float that generates next sample
    match waveformType with
    | Sine -> ... 
    | Square -> ...
    | Sawtooth -> ...
    | Triangle -> ...
```

---

## What to Explore

### Interactive Script Sections
1. **Generate a sine wave** and plot it (if you have a visualization library)
2. **Listen to 440 Hz, 880 Hz, 220 Hz** to hear octave relationships
3. **Compare waveforms** — hear the difference between sine, square, sawtooth
4. **Verify frequency** — count cycles, measure period
5. **Test phase continuity** — generate audio in chunks, verify no clicks

### Hands-On Experiments
```fsharp
// 01-Math.fsx-style exploration:
let sampleRate = 44100

// Generate 1 second of 440 Hz sine
let sine440 = sineWave sampleRate 440.0 1.0

// Count zero crossings
let zeroCrossings = countZeroCrossings sine440
// Should be ≈880 (440 cycles × 2 crossings/cycle)

// Listen to it (if you have NAudio hooked up)
playAudio sine440 sampleRate

// Try different frequencies
let sine880 = sineWave sampleRate 880.0 1.0
playAudio sine880 sampleRate  // Should sound one octave higher

// Try different waveforms
let square440 = squareWave sampleRate 440.0 1.0
playAudio square440 sampleRate  // Brighter, more "synthetic"
```

---

## Success Criteria for Phase 1

- ✅ All oscillator tests pass (sine, square, sawtooth, triangle)
- ✅ Frequency accuracy verified (zero-crossing count, period measurement)
- ✅ Phase wrapping confirmed (no numerical drift, stays in [0, 2π))
- ✅ Stateful oscillator works without clicks between chunks
- ✅ You can generate and listen to different pitches
- ✅ You can hear the difference between waveform types
- ✅ Code is clear and well-documented

---

## Concepts You'll Use from Phase 0

| Phase 0 Function | Phase 1 Use |
|------------------|-----------|
| `frequencyToPhaseIncrement` | Core of every oscillator |
| `normalizePhase` | Keep phase bounded |
| `timeToSamples` | Know how many samples to generate |
| `midiNoteToFrequency` | Convert MIDI notes to frequencies |
| `samplesToTime` | Measure duration of cycles |

---

## Common Pitfalls (Learn From Others!)

### Pitfall 1: Phase Wrapping in Wrong Place
```fsharp
// ❌ WRONG: Wrapping each sample individually (slow)
phase <- phase + phaseInc
if phase >= TWO_PI then phase := phase - TWO_PI

// ✅ BETTER: Use normalizePhase periodically
// (or let overflow happen, normalize every N samples)
```

### Pitfall 2: Forgetting to Wrap Between Chunks
```fsharp
// ❌ WRONG: Resetting phase to 0 between chunks → clicks/pops
chunk1 = generateChunk()
phase = 0.0  // BUG! Lost continuity
chunk2 = generateChunk()

// ✅ RIGHT: Continue phase across chunks
chunk1 = generateChunk()  // phase ends at ~3.5
chunk2 = generateChunk()  // phase starts at ~3.5, continues
```

### Pitfall 3: Not Normalizing Phase
```fsharp
// ❌ WRONG: Phase grows unbounded → numerical issues after hours
// Phase might reach 1e20 radians (floating-point precision lost)

// ✅ RIGHT: Keep phase in [0, 2π) to maintain precision
phase <- normalizePhase(phase + phaseInc)
```

### Pitfall 4: Using Linear Amplitude
```fsharp
// ❌ Possible Issue: Linear amplitude controls feel harsh
// -3dB should feel "half as loud", but linear doesn't match perception

// ✅ BETTER: For volume knobs, use dB scaling
// (Phase 1 is just generation, but good to know)
```

---

## Tips for Phase 1

1. **Start simple**: Implement sine first, then generalize to other waveforms
2. **Test frequently**: After each oscillator type, run tests
3. **Verify by ear**: Generate 440 Hz sine and listen. Does it sound right?
4. **Check zero crossings**: Easy way to verify frequency without FFT
5. **Keep phase bounded**: Normalize frequently to avoid numerical creep
6. **Document edge cases**: What happens at 0 Hz? Very high frequencies?

---

## Next: Phase 2 Preview

Once Phase 1 is solid, Phase 2 introduces **envelope generators (ADSR)**:

```
OSC → ENVELOPE → OUTPUT
  ↓      ↓
[gen] [adsr]
```

Instead of constant amplitude, the envelope shapes how sound evolves:
- **Attack**: Ramp from 0 to peak
- **Decay**: Fall from peak to sustain
- **Sustain**: Hold at steady level
- **Release**: Fall to 0 when note ends

This makes sounds feel "alive" instead of mechanical.

---

## Checklist: Ready for Phase 1?

- [ ] Phase 0 tests all pass (28/28)
- [ ] You understand sample rates and time conversion
- [ ] You understand phase accumulation (from Phase 0 exploration)
- [ ] You can explain MIDI-to-frequency conversion
- [ ] You understand why phase needs wrapping
- [ ] You can predict how 880 Hz will differ from 440 Hz

If checked, you're ready! Start Phase 1 implementation. 🚀

---

*Subtraktor: Phase 1 Preview*

See you when Phase 0 is complete!
