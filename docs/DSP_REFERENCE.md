# DSP Concepts Reference

Quick reference for DSP concepts used in Subtraktor. Use this to refresh your memory on key formulas and relationships.

## Audio Fundamentals

### Sample Rate
| Concept | Value | Notes |
|---------|-------|-------|
| CD Quality | 44,100 Hz | Standard (44.1 kHz) |
| Professional | 48,000 Hz | Video/film standard |
| High-Resolution | 96,000 Hz, 192,000 Hz | Overkill for most (human hearing ~20 kHz) |
| Sample Duration (44.1k) | ~22.7 μs | `1 / 44,100` |
| Nyquist Frequency (44.1k) | ~22,050 Hz | Max representable frequency = `sampleRate / 2` |

### Time ↔ Samples Conversion
```fsharp
samples = timeSeconds × sampleRate
timeSeconds = samples / sampleRate

// Example: 44100 Hz
2.5 seconds → 2.5 × 44100 = 110,250 samples
```

---

## Frequency & Pitch

### MIDI Note Numbers
| Note | MIDI | Frequency (Hz) | Notes |
|------|------|----------------|-------|
| C1 | 12 | 32.70 | Lowest standard note |
| C4 | 60 | 261.63 | "Middle C" |
| A3 | 57 | 220.00 | A3 (reference) |
| A4 | 69 | 440.00 | **Standard tuning** |
| A5 | 81 | 880.00 | One octave above A4 |
| C8 | 108 | 4186.01 | Highest standard note |

### MIDI to Frequency
```fsharp
// Formula:
f = 440 × 2^((n - 69) / 12)

// Example: MIDI 60 (C4)
f = 440 × 2^((60 - 69) / 12)
f = 440 × 2^(-9/12)
f = 440 × 0.5946...
f ≈ 261.63 Hz
```

### Frequency to MIDI
```fsharp
// Formula:
n = 69 + 12 × log₂(f / 440)

// Example: 880 Hz (A5)
n = 69 + 12 × log₂(880 / 440)
n = 69 + 12 × log₂(2)
n = 69 + 12 × 1
n = 81  ✓
```

### Semitone Relationships
| Semitones | Multiplier | Example |
|-----------|-----------|---------|
| 0 | 1× | Same pitch |
| 1 | 2^(1/12) ≈ 1.0595 | One half-step up |
| 12 | 2× | One octave up |
| 24 | 4× | Two octaves up |
| −12 | 0.5× | One octave down |

---

## Oscillators & Phase

### Phase Increment (Per Sample)
```fsharp
// Formula:
ω = 2π × f / Fs

where:
  ω = phase increment (radians/sample)
  f = frequency (Hz)
  Fs = sample rate (Hz)
  2π ≈ 6.28318...

// Example: 440 Hz at 44100 Hz
ω = 2π × 440 / 44100
ω ≈ 0.0628 radians/sample
```

### Cycle Count
```fsharp
// Samples per complete cycle (one full 2π):
cycleLength = 2π / ω = Fs / f

// Example: 440 Hz at 44100 Hz
cycleLength = 44100 / 440 = 100.23 samples
// ~100 samples = one 440 Hz cycle
```

### Phase Accumulation (Pseudocode)
```
phase = 0
for each sample:
    output = sin(phase)     // or square(phase), sawtooth(phase), etc.
    phase += ω              // advance phase
    if phase >= 2π:
        phase -= 2π         // wrap to [0, 2π)
```

### Normalized Phase [0, 1)
Alternative to radians; sometimes clearer:
```fsharp
// Convert to normalized phase (0 to 1 = one cycle):
normalizedPhase = phase / (2π)

// Example: phase = π radians
normalizedPhase = π / (2π) = 0.5  // halfway through cycle
```

---

## Amplitude & Loudness

### Decibel Scale
| dB | Linear | Perception | Notes |
|----|--------|-----------|-------|
| 0 | 1.0 | Reference | Full scale |
| −3 | 0.707 | −3 dB ≈ half power | `1 / √2` |
| −6 | 0.500 | Quarter power | −6 dB = "half as loud" |
| −12 | 0.251 | Much quieter | −12 dB = "very quiet" |
| −20 | 0.100 | Very quiet | −20 dB = "10× quieter" |
| −40 | 0.010 | Nearly silent | −40 dB = "100× quieter" |

### Linear to Decibels
```fsharp
// Formula:
dB = 20 × log₁₀(linear)

// Examples:
1.0 linear    → 0 dB
0.5 linear    → −6 dB
0.1 linear    → −20 dB
0.0316 linear → −40 dB
```

### Decibels to Linear
```fsharp
// Formula:
linear = 10^(dB / 20)

// Examples:
0 dB   → 1.0 linear
−6 dB  → 0.5 linear
−20 dB → 0.1 linear
−40 dB → 0.01 linear
```

### Key Perception Facts
- Ears perceive loudness **logarithmically** (not linearly)
- Doubling the power (amplitude × √2) doesn't sound like "twice as loud"
- Halving the power doesn't sound like "half as loud"
- −3 dB ≈ "half the power" (but doesn't sound half as loud)
- −10 dB ≈ "sounds about half as loud" (subjectively)

---

## Waveforms

### Common Oscillator Shapes

#### Sine Wave
```
Range: [−1, 1]
Formula: y = sin(2πt) where t ∈ [0, 1]
Character: Pure, smooth, warm
Harmonics: Only fundamental (no overtones)
```

#### Square Wave
```
Range: [−1, 1]
Formula: y = sign(sin(2πt))
Character: Bright, hollow, synthetic
Harmonics: Odd harmonics (1, 3, 5, 7, ...)
Duty Cycle: Often 50/50 (equal on/off)
```

#### Sawtooth Wave
```
Range: [−1, 1]
Formula: y = 2(t − floor(t + 0.5))
Character: Harsh, buzzy, "complex"
Harmonics: All harmonics (1, 2, 3, 4, ...)
Uses: Subtractive synth source
```

#### Triangle Wave
```
Range: [−1, 1]
Formula: Piecewise linear (ramps up, then down)
Character: Between sine and square
Harmonics: Odd harmonics (1, 3, 5, ...) but gentler than square
```

---

## Filters

### Low-Pass Filter
- **Purpose**: Attenuates high frequencies
- **Effect**: Makes sound "mellow", removes brightness
- **Use Case**: Subtractive synthesis (remove high harmonics)
- **Parameter**: Cutoff frequency (Hz) — frequencies above this are reduced

### High-Pass Filter
- **Purpose**: Attenuates low frequencies
- **Effect**: Removes bass, makes sound "thin"
- **Use Case**: Removing rumble, creating air/presence
- **Parameter**: Cutoff frequency (Hz)

### Resonance / Q
- **Adds a peak** at the cutoff frequency
- **Effect**: Emphasizes the cutoff region
- **Parameter**: Q or resonance (0 to 10+)
- **High Q**: Sharp peak, oscillates (can self-resonate)
- **Low Q**: Smooth rolloff

---

## Envelopes

### ADSR (Attack, Decay, Sustain, Release)
| Stage | Effect | Duration | Example |
|-------|--------|----------|---------|
| **Attack** | Ramps from 0 to peak | 0 ms to 1 sec | 10 ms for snappy |
| **Decay** | Falls from peak to sustain | 0 ms to 1 sec | 100 ms for punch |
| **Sustain** | Holds steady level | Indefinite | 0.7 (70% amplitude) |
| **Release** | Falls to 0 after note off | 0 ms to 2 sec | 300 ms for tail |

### Envelope Shaping
- **Linear**: Simple ramps (fast, but can sound artificial)
- **Exponential**: Curves (more natural, mimics real instruments)
- **Curves**: Custom shapes (sine, power functions)

---

## Common Synthesis Techniques

### Subtractive Synthesis
```
OSC → FILTER → ENVELOPE → OUTPUT
  ↓      ↑         ↓
[saw] [lpf]  [envelope]
```
Process:
1. Start with rich oscillator (sawtooth)
2. Filter to remove harmonics (lowpass with envelope)
3. Shape with envelope generator (ADSR)
4. Result: "warm", "analog" character

### Frequency Modulation (FM)
```
CARRIER OSC ← MODULATOR OSC
   ↓ (frequency controlled by mod)
OUTPUT
```
Process:
1. Modulator oscillator varies pitch of carrier
2. Creates sidebands (new frequencies)
3. Result: Complex, "metallic" character (with right ratio)

### Amplitude Modulation (AM)
```
CARRIER OSC × MODULATOR OSC
   ↓ (amplitude controlled by mod)
OUTPUT
```
Process:
1. Modulator oscillator varies volume of carrier
2. Result: "tremolo" effect, amplitude shaping

---

## Numerical Precision

### Floating-Point (Float32, Float64)
| Type | Bits | Range | Precision | Audio Use |
|------|------|-------|-----------|-----------|
| Float32 | 32 | ±3.4e38 | ~7 digits | DSP operations |
| Float64 | 64 | ±1.8e308 | ~15 digits | Audio processing, calculations |

### Why Tolerance Matters
```fsharp
// Floating-point arithmetic isn't exact:
0.1 + 0.2 ≠ 0.3  // Might be 0.30000000000000004

// When testing, use tolerance:
assert_approx_equal 0.3 (0.1 + 0.2) 1e-10 "sum"
// Pass if: |0.3 - actual| <= 1e-10
```

### Common Tolerances
- `1e-10` — Very strict (math operations)
- `1e-6` — Moderate (DSP calculations)
- `1e-3` — Loose (perceptual tests)
- `5.0` — Very loose (rounding to MIDI notes)

---

## Constants

```fsharp
A4_HZ = 440.0
TWO_PI = 6.28318530717958647692...
PI = 3.14159265358979323846...
SQRT2 = 1.41421356237309504880...
LOG10_2 = 0.30102999566398119521...
```

---

## Useful Conversions

### dB ↔ Linear Amplitude
```fsharp
// −3 dB (convenient reference):
linear = 10^(−3 / 20) = 0.7079...  // 1/√2

// −6 dB:
linear = 10^(−6 / 20) = 0.5

// −20 dB:
linear = 10^(−20 / 20) = 0.1
```

### Frequency ↔ Period
```fsharp
// Period = 1 / Frequency
frequency = 440 Hz → period = 1/440 ≈ 0.00227 seconds (2.27 ms)

// Frequency = 1 / Period
period = 0.01 seconds → frequency = 1/0.01 = 100 Hz
```

### Cents (Fine Pitch Adjustment)
```fsharp
// 1 semitone = 100 cents
// 1 cent = 1/100 semitone = 2^(1/1200)

// Frequency offset by N cents:
f_new = f_original × 2^(cents / 1200)

// Example: +50 cents (half semitone) above A4:
440 × 2^(50/1200) ≈ 466.16 Hz
```

---

## Quick Calculations

### Samples Needed for Duration
```
samples = seconds × sampleRate

1 second at 44100 Hz = 44,100 samples
0.1 second = 4,410 samples
0.01 second (10 ms) = 441 samples
```

### Frequency Range for Samples
```
If you want ~1000 samples per cycle:
frequency = sampleRate / 1000 = 44100 / 1000 = 44.1 Hz

For 100 samples per cycle:
frequency = 441 Hz

For 10 samples per cycle:
frequency = 4410 Hz
```

### MIDI Note at Given Frequency
```
Closest MIDI note to 1000 Hz:
n = 69 + 12 × log₂(1000 / 440)
n = 69 + 12 × 1.1856
n = 69 + 14.2
n ≈ 83 (B5)
```

---

## When to Use What

### Use Sine When...
- You want the "purest" sound (one frequency, no harmonics)
- You're building FM synthesis (clean modulation)
- You're testing oscillators (simple reference)

### Use Sawtooth When...
- You want a bright, buzzy source for subtractive synthesis
- You're starting FM synthesis with complex timbre
- You want all harmonics present for filtering

### Use Square When...
- You want a "hollow" synth sound (limited harmonics)
- You want something between sine and sawtooth
- Classic 8-bit video game aesthetic

### Use Filters When...
- You want to reduce harshness (low-pass)
- You want to remove rumble (high-pass)
- You want "expression" in a note (cutoff modulation)

---

## Debugging Checklist

- [ ] Is oscillator generating at the right frequency? (Test pitch)
- [ ] Is phase wrapping correctly? (No continuous growth)
- [ ] Is waveform shape correct? (Sine smooth? Square corners?)
- [ ] Is amplitude correct? (Not clipping, not too quiet)
- [ ] Are floating-point comparisons using tolerance?
- [ ] Are edge cases handled? (0 Hz, extreme frequencies, silence)
- [ ] Is CPU usage reasonable? (Profile hotspots)

---

## Further Reading

- **ThinkDSP** — Free online, visual approach
- **Audio Programming Book** — Comprehensive reference
- **DSP Wikipedia** — Mathematical foundations
- **3Blue1Brown Videos** — Intuitive visual explanations

---

*Quick Reference for Subtraktor DSP Learning Project*

Last updated: Phase 0 (Audio Math Fundamentals)
