# Subtraktor Session Notes

## Session: 24-25 Feb 2026

### Current Phase
**Phase 0: Audio Mathematics Fundamentals** - In Progress

### What We Accomplished

#### 1. Fixed Missing Math Module
- Restored `src/Subtraktor/Synthesis/Math.fs` and `Math.fsi`
- Files are properly included in `Subtraktor.fsproj`
- Module lives in `Subtraktor.Synthesis` namespace
- Contains 18 pure DSP math functions:
  - Time/sample conversions
  - MIDI ↔ frequency conversions
  - Phase increment calculations
  - Amplitude ↔ decibel conversions
  - Phase normalization/wrapping

#### 2. Tests Running
- `00-MathTests.fsx` is executable in Rider
- Tests load the Math module successfully
- Most tests pass, but some dB tests needed tolerance adjustments

#### 3. Concepts Reviewed
- **Phase wrapping functions**: Discussed `normalizePhase` (for radians `[0, 2π)`) vs `wrapPhase` (for normalized `[0, 1.0)`)
- **Negative modulo behavior**: Why both functions check `wrapped < 0.0` (F#'s `%` operator keeps sign of dividend)
- **Decibel accuracy**: Identified that −6 dB ≠ exactly 0.5, and −3 dB ≠ exactly 1/√2

### Open Issues

#### 1. dB Test Tolerance Issues
The following tests required larger tolerances (1e-2, 1e-3) instead of 1e-6:

```fsharp
// Test: specific dB values
assert_approx_equal 0.5 (decibelsToLinear -6.0) 1e-2
    "−6 dB ≈ 0.5 linear amplitude"

// Test: −3 dB ≈ 0.707 (reciprocal of sqrt(2))
assert_approx_equal (1.0 / Math.Sqrt(2.0)) (decibelsToLinear -3.0) 1e-3
    "−3 dB ≈ 1/√2"
```

**Reason**: The expected values are approximations:
- −6 dB = `10^(-6/20) ≈ 0.501187` (not exactly 0.5)
- −3 dB = `10^(-3/20) ≈ 0.707945` (not exactly 1/√2 = 0.707107)

**Options to fix**:
- Update expected values to exact formula results
- Update dB inputs to match the target linear values (e.g., use −6.0206 dB for 0.5)
- Keep approximations but document why larger tolerance is needed

### Current File State

#### Core Implementation
```
src/Subtraktor/Synthesis/
├── Math.fsi (95 lines) - Function signatures with XML docs
└── Math.fs (87 lines) - Pure implementations
```

#### Tests & Exploration
```
src/Subtraktor/Explorations/
├── 00-MathTests.fsx - 28 unit tests (running, needs dB test fixes)
└── 01-Math.fsx - Interactive exploration script
```

#### Documentation
```
docs/
├── PHASE_0_FUNDAMENTALS.md - Deep learning guide
├── QUICK_START.md - 5-minute overview
├── DSP_REFERENCE.md - Formula reference
└── ... (other phase docs)
```

### Progress Tracking
- Updated `PROGRESS_CHECKLIST.md` to record Phase 0 start date (24 Feb 2026)

### What's Next

#### Immediate (When You Resume)
1. **Decision**: How to handle dB test tolerance issues
   - Option A: Update expected values to exact results
   - Option B: Update dB inputs to match linear targets
   - Option C: Document approximations and keep larger tolerances

2. **Continue Phase 0 Exploration**
   - Run remaining sections of `01-Math.fsx`
   - Answer checkpoint questions in `PHASE_0_FUNDAMENTALS.md`
   - Verify understanding of all core concepts

#### After Phase 0 Complete
3. **Move to Phase 1: Oscillators**
   - Read `PHASE_1_PREVIEW.md`
   - Implement waveform generators (sine, square, sawtooth, triangle)
   - Create stateful oscillator types
   - Write comprehensive tests

### Key Files to Remember
- `Math.fs` / `Math.fsi` - Core DSP math (working)
- `00-MathTests.fsx` - Test suite (needs minor fixes)
- `01-Math.fsx` - Interactive exploration (ready to use)
- `PROGRESS_CHECKLIST.md` - Track learning progress
- `PHASE_0_FUNDAMENTALS.md` - Main learning guide

### Questions Explored This Session
1. Why do `normalizePhase` and `wrapPhase` check for negative values?
   - **Answer**: F#'s `%` operator preserves sign, so negative inputs need correction
2. What's the difference between `normalizePhase` and `wrapPhase`?
   - **Answer**: Different representations - radians [0, 2π) vs normalized [0, 1.0)
3. Why did dB tests need larger tolerances?
   - **Answer**: Expected values were approximations, not exact formula results

### Session Summary
✅ Math module restored and working  
✅ Tests running successfully  
✅ Core concepts reviewed and understood  
⚠️ Minor test accuracy issues identified (dB tests)  
📖 Ready to continue Phase 0 learning  

---

**Status**: Phase 0 in progress, ready to resume anytime.  
**Last Updated**: 25 Feb 2026
