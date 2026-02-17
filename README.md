# ᛋ ᚢ ᛒ ᛏ ᚱ ᚨ ᚲ ᛏ ᛟ ᚱ
Cold wave. Warm tone.

## Overview
Subtraktor is a minimalist, function‑first subtractive synthesizer built in 
idiomatic F#. It’s designed as a learning project and a craft project in equal 
measure: a place where clear concepts, composable functions, and thoughtful 
engineering come together to form a small but expressive sound engine.

At its core, Subtraktor models the classic subtractive signal path — oscillator, 
filter, envelope, and renderer — using simple, predictable building blocks. Each
component is implemented as a composable function that maps directly to familiar 
synthesis concepts, allowing complex behaviour to emerge from small, 
well‑understood parts.

The public API behaves as pure and transparent, even when internal 
implementation details use mutation for performance or clarity. The goal is 
not to avoid pragmatism, but to expose a clean, functional surface that 
encourages experimentation and understanding.

Subtraktor aims for depth rather than breadth. Instead of accumulating 
features, it focuses on refining the essentials: improving the quality, 
clarity, and expressiveness of the core signal path. Every addition is 
deliberate, every line of code considered, and every abstraction justified.

It’s a synthesizer built with restraint, curiosity, and craftsmanship — one 
that grows slowly, intentionally, and with a clear sense of purpose.

### Why?
Subtractive synthesis is one of the clearest ways to understand sound design.
Subtraktor aims to make that clarity visible in code.

## Philosophy
* **Simplicity without compromise**.
    <br/>Every concept is reduced to its essence — no hidden behavior, no magic, no unnecessary abstraction.

* **Composability as a first‑class idea**. 
    <br/>Small, pure building blocks combine into rich structures. The user shapes the sound, not the framework.

* **Minimalism with maximal impact.**
    <br/>A tiny API that punches far above its weight. Less code, more expression.

* **Transparency in design and implementation.**
    <br/>No smoothing, no auto‑correction, no silent processing. What you hear is exactly what you built.

* **Craftsmanship over convenience.**
    <br/>Raw waveforms, honest discontinuities, and early‑chip character are embraced, not hidden.

* **The client decides the polish.**
    <br/>Subtraktor outputs raw, unfiltered signals by default. Cleanliness is opt‑in, never imposed.

* **Imperfections are part of the instrument.**
    <br/>Aliasing, clicks, and edge cases are not flaws — they’re expressive tools, just like in classic trackers and sound chips.

* **Underpromise, overdeliver.**
    <br/>A small surface area with surprising depth. No bloat, no feature creep, no noise.

Subtraktor is built on the belief that small, pure functions can model complex 
sound behavior. Every abstraction must earn its place. Every function must be 
easy to reason about.

## Examples
### Basic sine wave
```fsharp
open Subtraktor.Signal
open Subtraktor.Osc

let osc = sine 440.0<Hz>

let samples =
    osc
    |> render 44100.0<Hz> 1.0<s>
```

### Combinators
```fsharp
open Subtraktor.Signal
open Subtraktor.Osc

// Two oscillators: a fundamental and a quieter fifth
let fundamental = sine 220.0<Hz>
let fifth       = sine 330.0<Hz> |> scale 0.5

// Combine them into a simple harmonic tone
let tone =
    fundamental
    |> add fifth
    |> scale 0.8   // overall gain

// Render one second of audio at 44.1 kHz
let samples =
    tone
    |> render 44100.0<Hz> 1.0<s>
```

### Envelope
```fsharp
let e3 = sine 164.81<Hz>
let g3 = sine 196.0<Hz>

add e3 g3
// (2s attack, 3s decay)
|> withEnvelope (ad 2.0<s> 3.0<s>) 
|> render 44100.0<Hz> 5.0<s>
|> writeWav "some/path/ad.wav" 44100.0<Hz>
```

### Visualization
Subtraktor itself doesn't depend on any external libraries but it does offer
some visualization help in the `Viz` module. Below is an example that uses [Plotly.NET](https://plotly.net/) in a 
script file. This example assumes the script contains 
`#r "nuget: Plotly.NET, 5.0.0"` (or similar) and omits all the `#load` instructions for brevity.
```fsharp
// Sample rate.
let sr = 44100.0<Hz>
// Duration of the signal.
let dur = 5.0<s>    
// Use a very low frequency saw for visualization purposes.
let gatedSignal = Osc.saw 2.0<Hz> |> Env.apply env
// The shape of the envelope.
let envData = env |> Viz.sample sr dur    
// The trigger signals of the gate.
let gateData =
    let plot t =
        match gate t with
        | true -> 1.0
        | _ -> 0.0
    plot |> Viz.sample sr dur
// The actual signal, confined to the envelope.
let sigData = gatedSignal |> Viz.sample sr dur
Chart.combine [
    Chart.Line envData
    Chart.Line gateData
    Chart.Line sigData
]
|> Chart.show
```

## Roadmap
Subtraktor will grow slowly and intentionally. Each milestone focuses on depth, 
clarity, and refinement rather than feature accumulation. This roadmap is a 
guide, not a contract — it will evolve as the project does.

### 1. Core Signal Path (Vertical Slice)
* Implement a basic oscillator (sine, saw, square)
* Add a simple filter (low‑pass to start)
* Introduce an ADSR envelope
* Render audio to a buffer or file

**Goal:** a minimal but complete subtractive chain.

### 2. Functional Architecture
* Define a clean, composable public API
* Ensure all public functions behave as pure
* Introduce internal mutation only where it improves clarity or performance
* Establish conventions for composing and binding DSP functions

**Goal:** a functional surface with pragmatic internals.

### 3. Refinement & Depth
* Improve oscillator quality (band‑limiting, anti‑aliasing)
* Enhance filter behavior (resonance, stability, character)
* Refine envelope curves and timing accuracy
* Add tests and visualizations for DSP components

**Goal:** deepen the core instead of widening the feature set.

### 4. Transparency & Craftsmanship
* Document design decisions and trade‑offs
* Add diagrams or flow descriptions for the signal path
* Keep the codebase small, readable, and intentional
* Remove or simplify anything that adds unnecessary complexity

**Goal:** a codebase that teaches as much as it performs.

### 5. Optional Future Exploration
These are not promises — just possible directions if they align with the 
philosophy:
* Additional oscillator types
* Additional filter modes
* Modulation routing
* A minimal UI or REPL‑based patching interface

**Goal:** only expand when it strengthens the core experience.