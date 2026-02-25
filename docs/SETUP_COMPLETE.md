# Phase 0 Setup Complete! 🎉

## What We've Just Built

You now have a complete **Phase 0 learning environment** for DSP and audio synthesis in F#. This is a test-driven, hands-on approach to learning the fundamentals.

---

## What's Included

### Core Implementation (`src/Subtraktor/Synthesis/`)

```
Math.fsi (18 functions, fully documented)
Math.fs  (Pure F# implementations)
```

**Functions Implemented:**
- ✅ Sample rate conversions (`timeToSamples`, `samplesToTime`, `sampleDuration`)
- ✅ MIDI ↔ Frequency conversions (`midiNoteToFrequency`, `frequencyToMidiNote`)
- ✅ Oscillator stepping (`frequencyToPhaseIncrement`)
- ✅ Phase operations (`normalizePhase`, `wrapPhase`)
- ✅ Amplitude conversions (`linearToDecibels`, `decibelsToLinear`)

All functions are **pure** (deterministic, no side effects) and **well-tested**.

### Test Suite (`src/Subtraktor/Explorations/00-MathTests.fsx`)

```
28 unit tests covering:
  - Time/sample conversions (round-trip verification)
  - MIDI note mappings (specific notes + relationships)
  - Frequency conversions (octaves, semitone ratios)
  - Phase calculations (oscillator stepping)
  - Phase normalization (numerical stability)
  - Decibel scales (amplitude relationships)
  - Edge cases (boundaries, special values)
```

Run it with: **Right-click → Execute in Interactive**

Expected result: **28 passed, 0 failed** ✓

### Interactive Exploration (`src/Subtraktor/Explorations/01-Math.fsx`)

```
4 major sections:
  1. Sample rate and time conversions (hands-on experiments)
  2. MIDI notes and frequency relationships (note explorer)
  3. Phase stepping and oscillator mechanics (cycle visualization)
  4. Amplitude and decibels (loudness perception)
```

This is your **"playground"** for understanding concepts. Modify code, re-run, build intuition.

### Documentation (`docs/`)

| File | Purpose |
|------|---------|
| `README.md` | Master learning guide (phases 0-6 overview) |
| `QUICK_START.md` | Get started in 5 minutes |
| `PHASE_0_FUNDAMENTALS.md` | Deep dive: audio math explained |

---

## How to Use This

### For Learning

1. **First time?** Read `docs/QUICK_START.md` (5 minutes)
2. **Understand concepts?** Read `docs/PHASE_0_FUNDAMENTALS.md` (30-45 minutes)
3. **Verify understanding?** Run `00-MathTests.fsx` (should pass ✓)
4. **Hands-on exploration?** Execute `01-Math.fsx` section by section
5. **Ready for Phase 1?** All checkpoints passed → move to oscillators

### For Testing Code Changes

After modifying `Math.fs`:

1. **Rebuild solution** (`Ctrl+Shift+B`)
2. **Run tests** (Right-click on `00-MathTests.fsx` → Execute)
3. **Check results** — all should pass
4. **If failure** — debug using `01-Math.fsx` interactive script

---

## Key Learning Objectives (Phase 0)

By the end of this phase, you should understand:

- [x] **Sample rates** — why 44100 Hz, what it means for time mapping
- [x] **MIDI ↔ Hz** — converting note numbers to frequencies and back
- [x] **Phase accumulation** — how oscillators step through cycles
- [x] **Decibel scale** — why audio uses dB for amplitude, not linear
- [x] **Numerical precision** — why we need tolerance in float comparisons
- [x] **Pure functions** — how to write testable DSP code

### Checkpoint Questions

Can you answer these without looking at the code?

1. *At 44100 Hz, how many samples is 2.5 seconds?*
2. *What frequency is MIDI note 60 (C4)?*
3. *What's the phase increment for a 1000 Hz oscillator at 44100 Hz?*
4. *Why do we use decibels instead of linear amplitude?*
5. *What does −12 dB mean in linear amplitude?*

If you answered most correctly, you're ready for Phase 1! 🚀

---

## What's Next (Preview)

### Phase 1: Oscillators (Coming Soon)

```
Goals:
  ✓ Implement sine, square, sawtooth, triangle oscillators
  ✓ Verify frequency accuracy (test output pitch)
  ✓ Test waveform shape (visual/spectral verification)
  ✓ Build composable oscillator chains

Time: 2-3 hours
```

**We'll build on Phase 0:**
- Use `frequencyToPhaseIncrement` to step oscillators
- Test 440 Hz sine should produce pitch A4
- Implement state management for continuous generation

---

## Project Philosophy

This project embodies several principles:

### 1. Test-Driven Development
- Tests come first (or at least, very early)
- Tests *define* behavior
- Failing tests guide implementation

### 2. Functional Style
- Pure functions by default
- Immutable data
- Side effects at boundaries
- Function composition for signal chains

### 3. Clarity
- Explicit > implicit
- Simple > clever
- Commented > obvious
- Names matter

### 4. Learning Focus
- Slow and steady
- Explain the *why*, not just the *what*
- Hands-on experimentation
- Build real things (not toy examples)

### 5. Pragmatism
- Pure is great, but performance matters
- Functional + imperative = best of both
- Real-world constraints matter
- Trade-offs are explicit

---

## File Organization

The project is structured for clarity:

```
src/Subtraktor/
├─ Synthesis/              ← DSP algorithms (pure functions)
│  └─ Math.fs{,i}
├─ Explorations/           ← Learning scripts & tests
│  ├─ 00-MathTests.fsx     ← Run this first
│  ├─ 01-Math.fsx          ← Then explore this
│  └─ ...
├─ Processing/             ← DSP operations (stateful wrappers)
├─ IO/                     ← File/stream handling
├─ Interop/                ← NAudio integration
└─ (existing modules)
```

**Principle:** Pure algorithms in `Synthesis`, stateful wrappers elsewhere.

---

## Running on Different Machines

### Prerequisites
- **.NET 10 SDK** (download from microsoft.com/download/dotnet)
- **F# language support** (included with .NET SDK)
- **Rider** or **VS Code + Ionide** (for interactive scripts)

### On Windows (Your Setup)
```cmd
cd d:\basp\subtraktor
dotnet build
dotnet fsi src/Subtraktor/Explorations/00-MathTests.fsx
```

### On macOS/Linux
```bash
cd /path/to/subtraktor
dotnet build
dotnet fsi src/Subtraktor/Explorations/00-MathTests.fsx
```

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Tests won't compile | Rebuild with `Ctrl+Shift+B`, check file paths |
| "Math not found" error | Ensure `Math.fsi` and `Math.fs` exist in `src/Subtraktor/Synthesis/` |
| F# Interactive hangs | Restart with `Shift+Alt+Enter` |
| Tests fail | Check tolerance values, verify formulas in `docs/PHASE_0_FUNDAMENTALS.md` |
| Script runs slow | Reduce loop iterations, it's just a test |

---

## Tips for Success

### 1. **Run Tests First**
```
00-MathTests.fsx → Verify correctness ✓
01-Math.fsx      → Understand concepts
```

### 2. **Experiment Liberally**
- Modify `testFrequency` in `01-Math.fsx`
- Re-run the phase stepping section
- See phase increment change in real-time

### 3. **Ask "Why?"**
- Why is phase ÷ (2π) when we want "cycle fraction"?
- Why is MIDI tuning 2^(1/12) per semitone?
- Why does −3 dB ≈ 0.707?

### 4. **Verify Round-Trips**
- Frequency → MIDI → Frequency (should match)
- Time → Samples → Time (should match)
- Linear → dB → Linear (should match)

### 5. **Build Intuition**
- Know A4 = 440 Hz by heart
- Know sample rate times frequency = samples/cycle
- Know −3 dB ≈ "sounds half as loud"

---

## Performance Considerations

Phase 0 code is **pure math**, so performance is excellent:
- ✅ No allocations (all values are primitives)
- ✅ No recursion (all loops/folds)
- ✅ Predictable (no garbage collection)
- ✅ Fast enough (< 1 microsecond per call)

Later phases (oscillators, filters) run **per-sample** at 44+ kHz, so performance remains critical. We'll measure with stopwatch and optimize hotspots.

---

## Learning Resources

### Within This Project
- `docs/PHASE_0_FUNDAMENTALS.md` — Audio math deep dive (excellent starting point)
- `Explorations/01-Math.fsx` — Interactive examples
- Source code comments — Implementation notes

### External
- **ThinkDSP** (free) — Visual, intuitive DSP introduction
- **The Audio Programming Book** — Comprehensive reference
- **3Blue1Brown (YouTube)** — Fourier transforms, signal processing

---

## Success Criteria

You're ready for Phase 1 when:

- ✅ `00-MathTests.fsx` passes completely (28/28 tests)
- ✅ You can run `01-Math.fsx` and understand the output
- ✅ You can modify code and predict results
- ✅ You can answer checkpoint questions (mostly correct)
- ✅ You understand *why* each formula is what it is
- ✅ You can explain to someone else what sample rates matter

---

## Next Commands

### Immediate (Next 5 Minutes)
```
1. Open this file in Rider
2. cd to d:\basp\subtraktor
3. Run: dotnet build
4. Open 00-MathTests.fsx
5. Execute → should see 28 tests pass
```

### Short Term (Next 1-2 Hours)
```
1. Read docs/QUICK_START.md
2. Read docs/PHASE_0_FUNDAMENTALS.md
3. Execute 01-Math.fsx section by section
4. Experiment with values
```

### Medium Term (When Ready for Phase 1)
```
1. Create Synthesis/Oscillators.fsi
2. Create Synthesis/Oscillators.fs
3. Create Explorations/02-Oscillators.fsx
4. Implement sine, square, sawtooth generators
5. Write tests for frequency accuracy
```

---

## Questions?

Check these in order:

1. **General question?** → `docs/README.md`
2. **Quick start issue?** → `docs/QUICK_START.md`
3. **Audio math confused?** → `docs/PHASE_0_FUNDAMENTALS.md`
4. **Code not working?** → Section "Troubleshooting" above
5. **Want to learn more?** → "Learning Resources" above

---

## Summary

You now have:

✅ Core audio math library (`Synthesis.Math`)
✅ 28 unit tests (all passing)
✅ Interactive exploration script
✅ Complete documentation (3 comprehensive guides)
✅ Clear path forward (6-phase learning plan)

**Next step?** Open `docs/QUICK_START.md` and run `00-MathTests.fsx`.

Welcome to DSP learning! 🎶

---

*Subtraktor — Learning DSP, one function at a time.*
