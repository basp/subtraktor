<!-- PHASE 0: AUDIO MATHEMATICS FUNDAMENTALS -->
# Phase 0: Audio Mathematics Fundamentals

## Overview

Welcome to **Phase 0**, the foundation of our DSP learning journey! This phase establishes the mathematical concepts and helper functions that every audio DSP algorithm needs.

### What We're Building

- **`Synthesis.Math` module** — Pure functions for audio mathematics
- **`00-MathTests.fsx`** — Test-driven verification of our implementations
- **`01-Math.fsx`** — Interactive exploration script

### Why Start Here?

Before we can build oscillators, envelope generators, or filters, we need to understand:

1. **How audio is sampled** — time maps to discrete sample indices
2. **How pitch is specified** — MIDI notes and frequency conversions
3. **How oscillators advance** — phase increments and angular frequency
4. **How loudness is measured** — decibels and linear amplitude

These concepts are fundamental to every subsequent phase.

---

## Key Concepts

### 1. Sample Rate and Time

**The Problem:** Audio is analog (continuous in time), but computers work with discrete samples.

**The Solution:** Sample the analog signal at regular intervals. Standard sample rates:
- **44100 Hz** (CD quality) — 44,100 samples per second
- **48000 Hz** (professional) — used in video, film
- **96000 Hz** (high-res) — more detail, larger files

**Time ↔ Samples:**
- At 44100 Hz, 1 second = 44,100 samples
- The duration of one sample = 1/44100 ≈ 0.0000227 seconds (≈ 22.7 microseconds)
- To convert: `samples = timeSeconds × sampleRate`

```fsharp
// 2.5 seconds at 44100 Hz = ?
let samples = 2.5 * 44100.0 = 110250.0 samples
```

**Why This Matters:** When generating audio, we work sample-by-sample. We need fast, accurate time-to-sample conversions.

### 2. MIDI Notes and Frequency

**The Problem:** Musicians think in note names (C4, A4, etc.). But oscillators work with frequency (Hz).

**The Solution:** MIDI (Musical Instrument Digital Interface) numbers:
- Each note gets a number: 0–127
- **MIDI 69 = A4 = 440 Hz** (standard concert pitch)
- Semitone = 12 equal divisions per octave
- **Each semitone multiplies frequency by 2^(1/12) ≈ 1.0595**

**Key Relationships:**
- One octave = 12 semitones = frequency × 2
  - A4 = 440 Hz
  - A5 = 880 Hz (one octave higher)
  - A3 = 220 Hz (one octave lower)

- C4 = MIDI 60
- C5 = MIDI 72 (one octave = 12 semitones)

```fsharp
// A4 is MIDI 69, 440 Hz
// How many Hz is E5?
let e5_midi = 76  // E5 is 7 semitones above A4
let e5_freq = 440.0 * (2.0 ** (7.0 / 12.0))  // ≈ 659 Hz
```

**Why This Matters:** When a user plays a MIDI note on a keyboard, we convert it to frequency to tell our oscillators what pitch to play.

### 3. Phase and Oscillator Stepping

**The Problem:** How do we generate a 440 Hz sine wave at 44100 Hz sampling?

**The Solution:** Use a **phase accumulator** — a counter that tracks our position in the waveform.

**Phase Increment (ω):**
- `ω = 2π × frequency / sampleRate`
- Example: 440 Hz at 44100 Hz → ω ≈ 0.0628 radians per sample
- At this rate, we complete one full cycle (2π radians) in 100 samples

**How It Works:**
```
Sample 0: phase = 0.0 rad  →  sin(0) = 0.0
Sample 1: phase += ω        →  sin(ω) ≈ 0.063
Sample 2: phase += ω        →  sin(2ω) ≈ 0.125
...
Sample 100: phase ≈ 2π      →  sin(2π) ≈ 0.0  (cycle complete)
```

**Why This Matters:** This is the core algorithm for generating any periodic waveform — sine, square, sawtooth, etc.

### 4. Amplitude and Decibels

**The Problem:** Our ears perceive loudness **logarithmically**, not linearly.

- Doubling the power doesn't sound like twice as loud
- Halving power sounds like a smaller decrease than doubling

**The Solution:** Use **decibels (dB)** — a logarithmic scale.

**Key Facts:**
- **0 dB = 1.0 linear** (reference/"full scale")
- **−3 dB ≈ 0.707 linear** (half the power)
- **−6 dB = 0.5 linear** (quarter the power)
- **−20 dB = 0.1 linear** (10× quieter)

**Formula:**
- Linear → dB: `dB = 20 × log₁₀(amplitude)`
- dB → Linear: `amplitude = 10^(dB/20)`

**Why This Matters:** When users adjust volume with a knob, we use dB for smoother perceptual loudness changes.

---

## Test-Driven Development (TDD)

### How We Test

We've created `00-MathTests.fsx` with simple test functions:

```fsharp
assert_equal expected actual "test name"  // For integers
assert_approx_equal expected actual tolerance "test name"  // For floats
assert_true condition "test name"  // For booleans
```

### Running Tests

1. **In JetBrains Rider:**
   - Open `00-MathTests.fsx`
   - Right-click → **Execute in Interactive** (or press Alt+Enter)
   - Watch the test output in the F# Interactive console

2. **From Command Line:**
   ```cmd
   cd d:\basp\subtraktor\src\Subtraktor
   dotnet fsi Explorations/00-MathTests.fsx
   ```

### What Happens

The test suite will:
1. Load the `Math.fs` module
2. Run assertions on each function
3. Count passes and failures
4. Report results with a summary

### Example Test Output

```
=== TIME & SAMPLE CONVERSIONS ===
  ✓ sampleDuration at 44100 Hz = 1/44100
  ✓ timeToSamples and samplesToTime are inverses
  ...
╔════════════════════════════════════════╗
║     TEST RESULTS                       ║
║  Passed: 28                             ║
║  Failed: 0                              ║
╚════════════════════════════════════════╝
```

---

## Interactive Exploration with `01-Math.fsx`

The script `01-Math.fsx` lets you **explore concepts interactively**:

1. Open it in Rider
2. Execute sections one at a time (Ctrl+Enter on each section)
3. Check the console output
4. Modify values and re-execute to test your understanding

### Try These Experiments

1. **Change sample rate:**
   ```fsharp
   let sampleRate = 48000  // or 96000, 192000
   // Re-run section 1 to see how time/sample conversions change
   ```

2. **Explore different MIDI notes:**
   ```fsharp
   let notesToExplore = [
       ("D4", 62);
       ("G4", 67);
       ("B4", 71);
   ]
   // Run section 2 to see their frequencies
   ```

3. **Understand phase stepping:**
   - Change `testFrequency` to 880 Hz (one octave higher)
   - See how phase increments faster
   - Notice cycles complete in half as many samples

4. **Decibel experiments:**
   ```fsharp
   // What amplitude feels "half as loud"?
   // Try −3 dB vs −6 dB vs −20 dB
   ```

---

## Understanding the Code Structure

### Synthesis.Math Module (`Math.fsi` / `Math.fs`)

**Signature file (`Math.fsi`):**
- Declares function signatures (names, types)
- Includes documentation comments
- This is the **public contract**

**Implementation file (`Math.fs`):**
- Contains actual function implementations
- Uses SI units (seconds, Hz, radians)
- All functions are **pure** (no side effects)

### Function Categories

1. **Time & Samples:** Convert between continuous time and discrete sample indices
2. **Frequency:** MIDI ↔ Hz conversions, phase increments
3. **Amplitude:** Linear ↔ Decibel conversions
4. **Phase:** Normalization for numerical stability

All functions follow these principles:
- ✅ Pure (same inputs → same outputs)
- ✅ Documented (explain what they do)
- ✅ Tested (unit tests verify correctness)
- ✅ Simple (do one thing, do it well)

---

## Next Steps (Phase 1)

Once you're comfortable with the math, we'll build:

**Phase 1: Waveform Generators & Oscillators**
- Simple sine wave oscillator
- Square, sawtooth, triangle waves
- Phase tracking for stateful generation
- Pure function composition for signal chains

What we'll test:
- ✓ Correct frequency output (verify pitch)
- ✓ Correct waveform shape
- ✓ Amplitude scaling
- ✓ Clean phase wrapping (no numerical creep)

---

## Tips for Learning

1. **Don't skip the math** — DSP is fundamentally mathematical. Understanding *why* these formulas work makes everything else clearer.

2. **Use the interactive script** — Running code and seeing results is 10× better than reading about it.

3. **Play with numbers** — Try different frequencies, sample rates, note numbers. Build intuition.

4. **Verify round-trips** — If you convert A → B → A, do you get back what you started with? If not, something's wrong.

5. **Ask "why?"** — For each formula, ask: Why 2π? Why 2^(1/12)? Why 20×log₁₀? Understanding the "why" beats memorization.

6. **Run tests frequently** — After any change, run tests. Catch bugs early.

---

## Resources for Deeper Learning

If you want to go deeper:

1. **"Think DSP" by Allen Downey**
   - Free online: https://github.com/AllenDowney/ThinkDSP
   - Excellent for audio fundamentals with Python (easily translates to F#)

2. **"The Audio Programming Book"**
   - Comprehensive coverage of DSP algorithms
   - C examples, but concepts apply everywhere

3. **Wikipedia + 3Blue1Brown Videos**
   - Fourier transforms, signal processing concepts
   - Visual, intuitive explanations

---

## Checkpoint: Are You Ready?

Before moving to Phase 1, verify:

- [ ] You can run `00-MathTests.fsx` and see all tests pass
- [ ] You can execute `01-Math.fsx` and understand the output
- [ ] You can modify test values and predict results correctly
- [ ] You understand why sample rate matters
- [ ] You understand MIDI-to-frequency conversion
- [ ] You understand phase accumulation in oscillators
- [ ] You understand the dB scale

If all checkboxes are ✓, you're ready for Phase 1! 🚀
