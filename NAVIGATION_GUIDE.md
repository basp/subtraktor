# Subtraktor Project Structure & Navigation Guide

## Quick Navigation

```
📂 d:\basp\subtraktor
│
├─ 🚀 START_HERE.md                  ← BEGIN HERE! (5 min read)
├─ 📋 PROGRESS_CHECKLIST.md          ← Track your learning
├─ 📝 SETUP_SUMMARY.md               ← What was set up
│
├─ 📂 docs/                          ← ALL DOCUMENTATION
│  ├─ README.md                      ← Master guide (15 min)
│  ├─ QUICK_START.md                 ← Fast start (5 min)
│  ├─ PHASE_0_FUNDAMENTALS.md        ← MAIN LEARNING GUIDE (45 min) ⭐
│  ├─ DSP_REFERENCE.md               ← Formulas & quick lookup
│  ├─ PHASE_1_PREVIEW.md             ← What's next
│  └─ SETUP_COMPLETE.md              ← Setup details
│
├─ 📂 src/Subtraktor/
│  │
│  ├─ 📂 Synthesis/                  ← OUR CODE (Phase 0)
│  │  ├─ Math.fsi                    ← Function signatures ✓
│  │  └─ Math.fs                     ← Implementations ✓
│  │
│  ├─ 📂 Explorations/               ← LEARNING SCRIPTS
│  │  ├─ 00-MathTests.fsx            ← 28 UNIT TESTS ✓ RUN THIS FIRST!
│  │  ├─ 01-Math.fsx                 ← Interactive exploration ✓
│  │  ├─ (02-Oscillators.fsx)        ← Phase 1 (coming soon)
│  │  └─ ...
│  │
│  ├─ Subtraktor.fsproj              ← Project file (updated ✓)
│  │
│  ├─ (Other modules - existing)
│  └─ bin/, obj/                     ← Build outputs
│
└─ subtraktor.slnx                   ← Solution file
```

---

## 🎯 Usage Flowchart

```
                    ┌─────────────────────┐
                    │  START_HERE.md      │
                    │  (5 minute read)    │
                    └──────────┬──────────┘
                               ↓
                    ┌─────────────────────┐
                    │   dotnet build      │
                    │  (rebuild solution) │
                    └──────────┬──────────┘
                               ↓
            ┌──────────────────────────────────────┐
            │  Run 00-MathTests.fsx                │
            │  (verify 28 tests pass) ✓            │
            └──────────────────────┬───────────────┘
                                   ↓
             ┌────────────────────────────────────┐
             │  Read PHASE_0_FUNDAMENTALS.md      │
             │  (deep dive learning - 45 min)     │
             └──────────┬─────────────────────────┘
                        ↓
          ┌─────────────────────────────────────┐
          │  Execute 01-Math.fsx interactively  │
          │  (hands-on exploration)             │
          └──────────┬────────────────────────┬─┘
                     ↓                        ↓
          ✅ PHASE 0  [Pass Checkpoint]   ❌ [Stuck?]
             COMPLETE                     [Review Docs]
                     ↓
          ┌─────────────────────────────────────┐
          │  Read PHASE_1_PREVIEW.md            │
          │  (understand what's next)           │
          └──────────┬────────────────────────┘
                     ↓
          ┌─────────────────────────────────────┐
          │  CREATE Phase 1 Code (Oscillators)  │
          │  (write tests, implement features)  │
          └─────────────────────────────────────┘
```

---

## 📚 Documentation by Purpose

### 🚀 Getting Started (Start Here!)
- **START_HERE.md** — Entry point, 5-minute overview
- **QUICK_START.md** — Fast setup instructions
- **SETUP_SUMMARY.md** — What was set up for you

### 🎓 Learning Phase 0 (Pick One)
- **PHASE_0_FUNDAMENTALS.md** ⭐ — RECOMMENDED
  - Deep explanation of all concepts
  - Multiple examples
  - "Why does this matter?"
  - Worked problems
  - ~45 minutes to read thoroughly

- **DSP_REFERENCE.md**
  - Quick formula lookup
  - Tables and constants
  - Use while coding/solving problems
  - Cross-reference from FUNDAMENTALS

### 👀 Future Phases
- **README.md** — Overview of all 6 phases
- **PHASE_1_PREVIEW.md** — What comes after Phase 0
- **PROGRESS_CHECKLIST.md** — Track progress through all phases

---

## 💻 How to Use This Project

### For Testing
```
1. Open: src/Subtraktor/Explorations/00-MathTests.fsx
2. Right-click → Execute in Interactive
3. Read results in F# Interactive panel
4. All 28 tests should pass ✓
```

### For Learning
```
1. Read docs/PHASE_0_FUNDAMENTALS.md (main learning)
2. Reference docs/DSP_REFERENCE.md (lookups)
3. Execute 01-Math.fsx sections (hands-on)
```

### For Exploration
```
1. Open: src/Subtraktor/Explorations/01-Math.fsx
2. Select a section (lines 1-50, 52-100, etc.)
3. Execute with Ctrl+Enter
4. Check console output
5. Modify code and re-execute
```

### For Reference
```
1. Open: docs/DSP_REFERENCE.md
2. Use Ctrl+F to find what you need
3. Copy formulas as needed
```

---

## 🔍 Finding Things

### "Where's the unit tests?"
→ `src/Subtraktor/Explorations/00-MathTests.fsx`

### "Where's the audio math code?"
→ `src/Subtraktor/Synthesis/Math.fs` (implementation)
→ `src/Subtraktor/Synthesis/Math.fsi` (signatures)

### "How do I learn about phase increments?"
→ `docs/PHASE_0_FUNDAMENTALS.md` → Section 3
→ `docs/DSP_REFERENCE.md` → Oscillators section

### "How do I know if I'm ready for Phase 1?"
→ `docs/PHASE_0_FUNDAMENTALS.md` → End: "Checkpoint Questions"
→ `PROGRESS_CHECKLIST.md` → Phase 0 section

### "How do I run tests?"
→ `QUICK_START.md` → Step 2

### "What's the big picture?"
→ `docs/README.md` (master overview)
→ `START_HERE.md` (quick overview)

### "I'm stuck, what do I do?"
→ Check the relevant guide's "Troubleshooting" section
→ Run `01-Math.fsx` to manually verify math
→ Review examples in tests

---

## 📊 Content Organization

### By Learning Phase

**Phase 0 (Complete) ✅**
- Code: `src/Subtraktor/Synthesis/Math.fs{,i}`
- Tests: `00-MathTests.fsx`
- Exploration: `01-Math.fsx`
- Main Doc: `docs/PHASE_0_FUNDAMENTALS.md`
- Reference: `docs/DSP_REFERENCE.md`

**Phase 1 (Ready to Start)**
- Preview: `docs/PHASE_1_PREVIEW.md`
- Guide: Build oscillators and waveforms
- Code: `src/Subtraktor/Synthesis/Oscillators.fs{,i}` (to create)
- Tests: `src/Subtraktor/Explorations/02-Oscillators.fsx` (to create)

**Phases 2-6 (Planned)**
- All outlined in `docs/README.md`
- Checklist in `PROGRESS_CHECKLIST.md`

### By Type

**Runnable Code**
- `src/Subtraktor/Synthesistis/Math.fs` — Production code
- `00-MathTests.fsx` — Unit tests
- `01-Math.fsx` — Interactive exploration

**Documentation**
- Guides: FUNDAMENTALS, PREVIEW, README
- Reference: DSP_REFERENCE
- Checklists: PROGRESS_CHECKLIST
- Getting Started: START_HERE, QUICK_START

**Configuration**
- `Subtraktor.fsproj` — Project file
- `subtraktor.slnx` — Solution file

---

## ⏱️ Time Allocation

### If You Have 30 Minutes
1. Run `00-MathTests.fsx` (5 min)
2. Read `QUICK_START.md` (5 min)
3. Start `01-Math.fsx` Section 1 (10 min)
4. Read quick concepts in `DSP_REFERENCE.md` (10 min)

### If You Have 1-2 Hours
1. Run `00-MathTests.fsx` (5 min)
2. Read `PHASE_0_FUNDAMENTALS.md` (45 min)
3. Execute `01-Math.fsx` sections (30 min)
4. Experiment with modifications (15 min)

### If You Have 3+ Hours
1. Complete Phase 0 learning (2 hours)
2. Read `PHASE_1_PREVIEW.md` (15 min)
3. Plan Phase 1 implementation (15 min)
4. Start writing Oscillators code (30 min+)

---

## 🎯 Success Indicators

### Phase 0 Completion
- [ ] `00-MathTests.fsx` passes (28/28) ✓
- [ ] Can read and understand `PHASE_0_FUNDAMENTALS.md`
- [ ] Can run `01-Math.fsx` and understand output
- [ ] Can answer checkpoint questions
- [ ] Can explain samples, MIDI, phase, and dB to someone else

### Ready for Phase 1
- [ ] All above complete
- [ ] `PROGRESS_CHECKLIST.md` Phase 0 marked done
- [ ] Feel comfortable with audio math
- [ ] Read `PHASE_1_PREVIEW.md`

---

## 🚀 Next Actions

### Right Now (5 minutes)
```
1. Read START_HERE.md
2. Run: dotnet build
3. Run: 00-MathTests.fsx
4. Verify 28 tests pass ✓
```

### Soon (1-2 hours)
```
1. Read PHASE_0_FUNDAMENTALS.md thoroughly
2. Run 01-Math.fsx section by section
3. Experiment with different values
4. Answer checkpoint questions
```

### When Ready (Whenever)
```
1. Check PROGRESS_CHECKLIST.md Phase 0
2. Read PHASE_1_PREVIEW.md
3. Create Synthesis/Oscillators.fsi
4. Write tests for oscillators
5. Implement oscillator functions
```

---

## 🗺️ The Full Journey (Phases 1-6)

```
Phase 0: Audio Math ✅
    ↓
Phase 1: Oscillators (Sine, Square, Sawtooth, Triangle)
    ↓
Phase 2: Envelopes (ADSR - Attack, Decay, Sustain, Release)
    ↓
Phase 3: Filters (Lowpass, Highpass)
    ↓
Phase 4: Modulation (AM, FM Synthesis)
    ↓
Phase 5: Voices (Polyphony, Voice Allocation)
    ↓
Phase 6: NAudio Integration (Real-Time Playback)
    ↓
✅ WORKING SOFTWARE SYNTHESIZER
```

**Estimated Total Time: 15-22 hours**

---

## 💡 Pro Tips

1. **Run tests frequently** — After any change, verify tests still pass
2. **Use interactive scripts** — Test ideas in `01-Math.fsx` before committing to code
3. **Keep the reference handy** — `DSP_REFERENCE.md` for quick lookups
4. **Track progress** — Use `PROGRESS_CHECKLIST.md` to stay motivated
5. **Experiment liberally** — Modify values, see what happens, learn from results
6. **Ask "why?"** — Every formula has a reason; understanding it beats memorization
7. **Go slowly** — Understanding > speed for learning projects

---

## 📖 Reading Order Recommendation

```
🏃 QUICK PATH (1-2 hours)
├─ START_HERE.md (5 min)
├─ Run tests (5 min)
├─ PHASE_0_FUNDAMENTALS.md (45 min)
├─ 01-Math.fsx exploration (30 min)
└─ Feel ready? → Start Phase 1

🏃‍♀️ THOROUGH PATH (3-4 hours)
├─ START_HERE.md (5 min)
├─ QUICK_START.md (5 min)
├─ Run tests (5 min)
├─ PHASE_0_FUNDAMENTALS.md (1 hour detailed read)
├─ 01-Math.fsx exploration (30 min)
├─ DSP_REFERENCE.md deep dive (30 min)
├─ Re-read tricky sections (15 min)
└─ PHASE_1_PREVIEW.md (15 min)

🏃🏃 COMPLETE PATH (5-6 hours)
├─ Everything above
├─ Solve additional problems
├─ Create own test cases
├─ Modify 01-Math.fsx extensively
└─ Prepare Phase 1 implementation
```

---

## ✨ Special Navigation Tips

### Markdown Navigation (In Supported Editors)
- Use Ctrl+Shift+O to see document outline
- Jump directly to sections
- Quick navigation between chapters

### Search Tips
- Ctrl+F in any file to find text
- Search for function names to find usage
- Search for keywords in reference docs

### Cross-References
- Links throughout docs point to related sections
- Use these to explore deeper
- Build connections between concepts

---

## 🎊 You're All Set!

Everything you need is here:
- ✅ Code (production-ready)
- ✅ Tests (comprehensive)
- ✅ Documentation (thorough)
- ✅ Learning path (structured)
- ✅ Interactive scripts (hands-on)

**Now go learn something awesome!** 🚀🎶

---

## Quick Reference Card

| I Want To... | File |
|---|---|
| Get started quickly | START_HERE.md |
| Understand Phase 0 deeply | PHASE_0_FUNDAMENTALS.md |
| Look up a formula | DSP_REFERENCE.md |
| See what's next | PHASE_1_PREVIEW.md |
| Run tests | 00-MathTests.fsx |
| Explore interactively | 01-Math.fsx |
| Track my progress | PROGRESS_CHECKLIST.md |
| Understand the full plan | README.md |

---

**Happy Learning!** 🎶

*Subtraktor: Test-Driven DSP Learning*
