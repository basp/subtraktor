# Subtraktor: Test-Driven Synthesizer Learning Project

> A functional DSP learning journey in F#, building a software synthesizer from first principles.

## Vision

**Subtraktor** is a minimalist synthesizer project designed for learning. We're building it step-by-step using test-driven development, starting from pure math and advancing to real-time audio playback.

### Goals

✅ **Learn DSP deeply** — understand algorithms, not just use them  
✅ **Write clear, functional code** — explicit, composable, testable  
✅ **Build something real** — a working synthesizer with actual sound  
✅ **Use TDD** — tests drive design and catch bugs early  
✅ **Go slowly** — explanation > speed

### Anti-Goals

❌ Feature creep (we pick specific synth style and do it well)  
❌ Premature optimization (clarity first, optimize later if needed)  
❌ Hiding complexity (explicit is better than implicit)  
❌ Rushing (steady wins the race)  

---

## How to Use This Project

### Directory Structure

```
subtraktor/
├─ src/
│  └─ Subtraktor/
│     ├─ Synthesis/
│     │  ├─ Math.fsi          # Phase 0: Audio math signatures
│     │  └─ Math.fs           # Phase 0: Audio math implementation
│     ├─ Explorations/
│     │  ├─ 00-MathTests.fsx  # Phase 0: Unit tests
│     │  ├─ 01-Math.fsx       # Phase 0: Interactive exploration
│     │  ├─ 02-Oscillators.fsx    # Phase 1: Coming soon
│     │  ├─ 03-Envelopes.fsx      # Phase 2: Coming soon
│     │  └─ ...
│     └─ ... (other modules)
└─ docs/
   ├─ PHASE_0_FUNDAMENTALS.md    # Detailed Phase 0 guide
   └─ PHASES.md                   # Overview of all phases
```

### Running Tests

**In Rider:**
1. Open `Explorations/00-MathTests.fsx`
2. Right-click → **Execute in Interactive**
3. Watch results in F# Interactive console

**Command line:**
```cmd
cd src\Subtraktor
dotnet fsi Explorations/00-MathTests.fsx
```

### Interactive Exploration

Each phase has an exploration script (e.g., `01-Math.fsx`) for hands-on learning:

1. Open the `.fsx` script in Rider
2. Execute sections one at a time (Ctrl+Enter)
3. Modify values and re-execute
4. Build intuition by experimenting

---

## The Learning Phases

### Phase 0: Audio Mathematics Fundamentals ✅ (You are here!)

**What you'll learn:**
- Sample rates and time conversions
- MIDI notes and frequency relationships
- Phase accumulation and oscillator stepping
- Amplitude and decibels

**What we build:**
- `Synthesis.Math` — pure audio math functions
- Unit tests for all conversions
- Interactive exploration script

**Typical time:** 1-2 hours of reading + experimentation

**Success criteria:**
- All tests pass ✓
- You can explain why sample rate matters
- You can convert MIDI notes to frequencies mentally
- You understand phase accumulation

---

### Phase 1: Waveform Generators & Oscillators

**What you'll learn:**
- Generating periodic waveforms (sine, square, sawtooth, triangle)
- State management for stateful oscillators
- Phase continuity and aliasing
- Signal amplitude and mixing

**What we build:**
- `Synthesis.Oscillators` — pure oscillator functions
- Stateful oscillator wrapper for real-time use
- Tests for frequency accuracy, waveform shape
- Interactive playback script

**Key insight:** Oscillators are the "sound source" of any synthesizer. Everything else filters, modulates, or envelopes the oscillator output.

---

### Phase 2: Envelope Generators (ADSR)

**What you'll learn:**
- Attack, Decay, Sustain, Release curves
- State machines for envelopes
- Time-based parameter control
- Envelope shaping and smoothing

**What we build:**
- `Synthesis.Envelopes` — ADSR envelope generator
- Tests for correct timing and curve shapes
- Interactive envelope visualizer

**Key insight:** Envelopes shape how sounds evolve over time. Without them, all notes sound the same (constant amplitude).

---

### Phase 3: Time-Domain Filters

**What you'll learn:**
- First-order filters (lowpass, highpass)
- Cutoff frequency and resonance
- Filter state and stability
- Building more complex filters from simple ones

**What we build:**
- `Synthesis.Filters` — simple resonant filters
- Tests for frequency response
- Interactive filter sweep explorer

**Key insight:** Filters are how subtractive synthesis works. You start with a buzzy waveform and filter it to remove high frequencies, making it mellow.

---

### Phase 4: Modulation & FM Synthesis

**What you'll learn:**
- Amplitude modulation (AM) and frequency modulation (FM)
- Modulator routing and composition
- FM synthesis fundamentals
- Creating complex timbres from simple components

**What we build:**
- `Synthesis.Modulation` — AM/FM composition functions
- FM oscillator pairs (carrier + modulator)
- Tests for modulation depth and rate

**Key insight:** Modulation is how we create movement and complexity. A static oscillator is boring; add modulation and you get life.

---

### Phase 5: Voice Architecture & Polyphony

**What you'll learn:**
- Voice allocation (which note plays on which oscillator?)
- MIDI note triggering
- Parameter control (pitch, volume, filter cutoff)
- Mixing multiple voices together

**What we build:**
- `Synthesis.Voice` — a complete voice (osc + envelope + filter)
- `Synthesis.VoicePool` — allocate/manage multiple voices
- Polyphonic note handling

**Key insight:** A "synthesizer voice" bundles together all the DSP components (oscillators, envelopes, filters). Multiple voices allow playing chords.

---

### Phase 6: NAudio Integration & Real-Time Playback

**What you'll learn:**
- NAudio ISampleProvider interface
- Audio threading and safety
- Device enumeration and selection
- Writing/reading audio files

**What we build:**
- `Interop.NAudioBridge` — NAudio integration layer
- Real-time audio playback
- File export

**Key insight:** This is where our pure DSP algorithms meet the real world. NAudio handles the messy details of communicating with audio hardware.

---

## Test-Driven Development (TDD)

### The Process

For each feature, we follow this cycle:

1. **Write a test** — What should this function do? Write a test that fails initially.
2. **Write minimal code** — Make the test pass with the simplest implementation.
3. **Verify correctness** — Run all tests. Do they pass?
4. **Refactor if needed** — Clean up code while keeping tests green.
5. **Add edge cases** — Test boundary conditions, error cases.

### Example: Testing a 440 Hz Oscillator

```fsharp
// Phase 1, Test: oscillator generates correct frequency
let [<Test>] ``440 Hz sine oscillator generates correct frequency`` () =
    let sampleRate = 44100
    let frequency = 440.0
    let durationSeconds = 0.1
    
    // Generate 0.1 seconds of 440 Hz sine
    let samples = generateSineWave sampleRate frequency durationSeconds
    
    // Verify the output is periodic
    // Count zero crossings (should be frequency * duration * 2)
    let zeroCrossings = countZeroCrossings samples
    let expectedCrossings = int (frequency * durationSeconds * 2.0)
    
    Assert.AreEqual(expectedCrossings, zeroCrossings)
```

### Why TDD?

- **Specification:** Tests *define* what the code should do
- **Confidence:** Green tests = the code works
- **Regression detection:** Future changes break tests immediately
- **Design:** Writing tests first forces you to think about the API
- **Documentation:** Tests show how to *use* the code

---

## Code Style & Philosophy

### Principles

1. **Pure Functions First**
   - DSP logic should be pure (same input → same output)
   - Immutable data by default
   - Side effects (I/O, mutable state) isolated at boundaries

2. **Composition Over Inheritance**
   - Build complex audio chains from simple functions
   - `signal |> filter |> envelope |> output`
   - Avoid deep class hierarchies

3. **Explicit Over Implicit**
   - Function signatures are clear about what they do
   - Parameter types prevent invalid inputs
   - No hidden state or magic

4. **Simple Over Clever**
   - Easy-to-understand code beats clever one-liners
   - Name things clearly
   - Add comments explaining the *why*, not the *what*

5. **Performance Matters (Eventually)**
   - Start with clarity
   - Profile hotspots (oscillators, filters running at sample rate)
   - Optimize only what's measurably slow
   - Document performance trade-offs

### Acceptable Compromises

- **Mutable state in oscillators** — Tracking phase per sample requires efficiency; local mutation is OK
- **Imperative loops** — Sample generation runs at 44.1k Hz; efficiency matters
- **Array pooling** — Large audio buffers; reuse them to reduce GC pressure
- **Custom types** — Sometimes a discriminated union or record is clearer than a function

The rule: **Practicality beats purity.**

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- JetBrains Rider (recommended) or Visual Studio Code with Ionide
- Basic F# knowledge (loops, recursion, pattern matching)
- Curiosity about audio/DSP

### Setup

1. Clone this repo
2. Open `subtraktor.slnx` in Rider
3. Rebuild the solution (`Ctrl+Shift+B`)
4. Open `Explorations/00-MathTests.fsx`
5. Execute in F# Interactive

You should see test results like:
```
=== TIME & SAMPLE CONVERSIONS ===
  ✓ sampleDuration at 44100 Hz = 1/44100
  ✓ timeToSamples and samplesToTime are inverses
  ✓ 1 second at 44100 Hz = 44100 samples
  ...
╔════════════════════════════════════════╗
║     TEST RESULTS                       ║
║  Passed: 28                             ║
║  Failed: 0                              ║
╚════════════════════════════════════════╝
```

### Next Steps

1. **Read `docs/PHASE_0_FUNDAMENTALS.md`** — Deep dive into audio math
2. **Run `00-MathTests.fsx`** — Verify all tests pass
3. **Explore `01-Math.fsx`** — Experiment interactively
4. **Experiment** — Modify values, make predictions, verify them
5. **Reflect** — Write down what you learned in each section

---

## Learning Resources

### Within This Project

- **`docs/PHASE_0_FUNDAMENTALS.md`** — Comprehensive Phase 0 guide
- **`Explorations/00-MathTests.fsx`** — Executable test cases and examples
- **`Explorations/01-Math.fsx`** — Interactive exploration
- **Source code comments** — Implementation insights

### External Resources

1. **"Think DSP" by Allen Downey**
   - Free online: https://github.com/AllenDowney/ThinkDSP
   - Python, but excellent DSP fundamentals
   - Highly recommended for visual understanding

2. **"The Audio Programming Book"**
   - Comprehensive DSP reference
   - C++ examples, concepts translate to all languages

3. **3Blue1Brown on YouTube**
   - Fourier transforms and signal processing
   - Visual, intuitive explanations

4. **STMicroelectronics DSP Library Documentation**
   - Reference implementations of standard filters
   - Stability analysis and fixed-point considerations

5. **Designing Audio Effect Plugins in C++**
   - Advanced synthesis and effects techniques
   - Object-oriented approach (different from our functional style, but good reference)

---

## Troubleshooting

### Tests Won't Run

**Problem:** "could not load or find Synthesis.Math"

**Solution:**
1. Rebuild solution (`Ctrl+Shift+B`)
2. Ensure `Math.fsi` and `Math.fs` are in `src/Subtraktor/Synthesis/`
3. Check `.fsproj` includes these files in compile order

### Tests Fail

**Problem:** "expectedValue ≠ actualValue"

**Solution:**
1. Check tolerance — floating-point arithmetic isn't exact
2. Verify the formula in `Math.fs` matches the expected math
3. Run `01-Math.fsx` and manually check the calculation
4. Add debug output to see intermediate values

### F# Interactive Crashes

**Problem:** Script times out or crashes

**Solution:**
1. Restart F# Interactive (Shift+Alt+Enter in Rider)
2. Check for infinite loops in modified code
3. Reduce loop sizes for testing (don't generate hours of audio)

---

## Contributing to This Project

Since this is a learning project, feel free to:

- ✅ Add more test cases
- ✅ Add comments explaining concepts
- ✅ Experiment with alternative implementations
- ✅ Create variation scripts (e.g., "04-Filters-Advanced.fsx")
- ✅ Document tricky DSP math

Avoid:

- ❌ Jumping ahead (finish Phase N before starting Phase N+1)
- ❌ Skipping tests (tests ensure you understand)
- ❌ Removing explanatory comments (clarity is the goal)

---

## The Long View

By the end of this project, you'll have:

1. **Deep understanding of DSP** — why algorithms work, not just how to use them
2. **Working synthesizer** — that actually produces sound
3. **Functional audio code** — reusable DSP building blocks
4. **Production-ready patterns** — how to test, document, and refactor audio code
5. **Foundation for next steps** — FM synthesis, sampling, effects, polyphonic sequencing, etc.

Plus, you'll have built something cool. 🎶

---

## Quick Reference

| Phase | Topic | Output | Time |
|-------|-------|--------|------|
| 0 | Audio Math | `Synthesis.Math` | 1-2h |
| 1 | Oscillators | `Synthesis.Oscillators` | 2-3h |
| 2 | Envelopes | `Synthesis.Envelopes` | 2-3h |
| 3 | Filters | `Synthesis.Filters` | 2-3h |
| 4 | Modulation | `Synthesis.Modulation` | 3-4h |
| 5 | Voices | `Synthesis.Voice` | 2-3h |
| 6 | NAudio | `Interop.NAudioBridge` | 2-3h |

**Total:** ~15-22 hours for a complete, working synthesizer

---

Happy learning! 🚀

Start with Phase 0, run those tests, and let's build something amazing together.

Questions? Check `docs/PHASE_0_FUNDAMENTALS.md` for detailed explanations.
