# Quick Start Guide

## Your First Test Run

### Step 1: Rebuild the Solution

In **JetBrains Rider**:
- Press `Ctrl+Shift+B` to rebuild
- Wait for "Build completed successfully"

Or from **Command Line**:
```cmd
cd d:\basp\subtraktor
dotnet build
```

### Step 2: Run Phase 0 Tests

Open `src/Subtraktor/Explorations/00-MathTests.fsx` in Rider.

**To run the tests:**
1. Click anywhere in the file
2. Right-click → **Execute in Interactive** (or press Ctrl+Alt+Enter)
3. Wait for the F# Interactive console to show results

**Expected output:**
```
=== TIME & SAMPLE CONVERSIONS ===
  ✓ sampleDuration at 44100 Hz = 1/44100
  ✓ timeToSamples and samplesToTime are inverses
  ... (many more tests)

╔════════════════════════════════════════╗
║     TEST RESULTS                       ║
║  Passed: 28                             ║
║  Failed: 0                              ║
╚════════════════════════════════════════╝

🎉 All tests passed! Ready for Phase 1.
```

If you see failures, check:
1. Did the build succeed? (Rebuild if needed)
2. Are all files in the right directories?
3. Check `docs/PHASE_0_FUNDAMENTALS.md` Troubleshooting section

### Step 3: Explore Interactively

Open `src/Subtraktor/Explorations/01-Math.fsx` and:

1. **Execute Section 1** (sample rate conversions)
   - Position cursor in the section
   - Press `Ctrl+Enter`
   - Check the console output

2. **Execute Section 2** (MIDI and frequency)
   - Try modifying the `notesToExplore` list
   - Re-execute
   - See how different notes map to frequencies

3. **Modify and experiment** — that's the whole point!

### Troubleshooting

**Q: "The type 'Subtraktor.Synthesis.Math' is not defined"**
- Solution: Rebuild the solution first (`Ctrl+Shift+B`)

**Q: Script times out or hangs**
- Solution: Press `Shift+Alt+Enter` in Rider to restart F# Interactive

**Q: Tests pass but I don't understand why**
- Solution: Open `01-Math.fsx` and run the interactive exploration for that section

---

## What's in This Package

```
Phase 0 Files:
├─ src/Subtraktor/Synthesis/
│  ├─ Math.fsi                  (Function signatures)
│  └─ Math.fs                   (Implementations)
│
├─ src/Subtraktor/Explorations/
│  ├─ 00-MathTests.fsx          (Unit tests - RUN THIS FIRST)
│  └─ 01-Math.fsx               (Interactive exploration)
│
└─ docs/
   ├─ README.md                 (This learning guide)
   ├─ PHASE_0_FUNDAMENTALS.md   (Deep dive into audio math)
   └─ QUICK_START.md            (This file)
```

---

## Learning Path

1. **This file** — Quick start (you are here)
2. **Run `00-MathTests.fsx`** — Verify everything works
3. **Read `docs/PHASE_0_FUNDAMENTALS.md`** — Learn the concepts
4. **Execute `01-Math.fsx`** — Hands-on exploration
5. **Experiments** — Modify code, predict results, verify
6. **Move to Phase 1** — When comfortable with Phase 0

---

## Key Concepts (TL;DR)

### Sample Rate
- Audio is discrete samples, not continuous
- 44100 Hz = 44,100 samples per second
- Each sample = 1/44100 seconds ≈ 0.0000227 seconds

### MIDI to Frequency
- MIDI note 69 = A4 = 440 Hz
- Each 12 semitones = 1 octave = 2× frequency
- Formula: `freq = 440 × 2^((note - 69) / 12)`

### Phase Accumulation (Oscillators)
- `phase += 2π × frequency / sampleRate` per sample
- When phase ≥ 2π, we've completed one cycle
- Output: `sin(phase)` for sine wave, etc.

### Decibels
- 0 dB = 1.0 linear (full scale)
- −3 dB ≈ 0.707 linear
- −6 dB = 0.5 linear
- Formula: `dB = 20 × log₁₀(linear)`

---

## Next Steps After Phase 0

Once all tests pass and you understand the concepts:

1. **Start Phase 1** — Oscillators
   - Generate sine, square, sawtooth waves
   - Verify pitch accuracy
   - Build intuition about aliasing

2. **Keep going** — Follow phases 1-6
   - Each adds new capability
   - Tests verify correctness
   - You'll have a complete synthesizer

---

## Tips

- 📝 Run tests after *any* code change
- 🔬 Experiment in `01-Math.fsx` first before writing new modules
- 📚 Read comments in the code — they explain the *why*
- 🎯 Don't rush — understanding beats speed
- 🐛 Tests are your friend — they find bugs early

---

Good luck! Welcome to DSP learning with Subtraktor. 🎶

Start with `00-MathTests.fsx` now!
