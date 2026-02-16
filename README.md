# Subtraktor
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

It’s a synthesizer built with restraint, curiosity, and craftsmanship—one that 
grows slowly, intentionally, and with a clear sense of purpose.

## Philosophy
* Simplicity without compromise.
* Composability as a first-class idea.
* Underpromise, overdeliver.
* Minimalism with maximal impact.
* Craftsmanship over convenience.
* Transparency in design and implementation.

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