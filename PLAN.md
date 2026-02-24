A functional wrapper over NAudio for F# could improve:
* Ergonomics: hide mutable/disposable plumbing behind composable functions.
* Safety: model invalid states with types (Result, discriminated unions) instead of runtime surprises.
* Readability: build audio pipelines with clear, flat transformations.
* Testability: isolate effects (I/O, playback devices) behind small interfaces.

A practical design would be:
* Core pure domain (formats, buffers, graph description).
* Effectful interpreter layer (NAudio interop, device/file I/O).
* Resource-safe API (computation expressions / helper combinators for IDisposable).
* Typed errors instead of exceptions where possible.
* Small, focused modules (Input, Decode, Transform, Mix, Output).
```
subtraktor/
├─ src/
│  └─ Subtraktor/
│     ├─ Subtraktor.fsproj
│     ├─ PublicApi/
│     │  ├─ AudioPrimitives.fsi
│     │  ├─ AudioPrimitives.fs
│     │  ├─ AudioBuffer.fsi
│     │  ├─ AudioBuffer.fs
│     │  ├─ Pipeline.fsi
│     │  └─ Pipeline.fs
│     ├─ Processing/
│     │  ├─ Gain.fs
│     │  ├─ Normalize.fs
│     │  ├─ Resample.fs
│     │  └─ Mix.fs
│     ├─ IO/
│     │  ├─ Decode.fs
│     │  ├─ Encode.fs
│     │  ├─ Devices.fs
│     │  └─ Runtime.fs
│     ├─ Interop/
│     │  ├─ NAudioConverters.fs
│     │  └─ NAudioRuntime.fs
│     └─ AssemblyInfo.fs
├─ tests/
│  └─ Subtraktor.Tests/
│     ├─ Subtraktor.Tests.fsproj
│     ├─ PublicApiTests.fs
│     ├─ ProcessingTests.fs
│     └─ IoTests.fs
└─ README.md
```
* .fsi only for stable public API
* Domain split by responsibility: `Processing`, `IO`, `Interop`.
* NAudio kept isolated in `Interop`, so pure API stays clean.
* Tests mirror production modules.

## Tools we might use
* NAudio
* ManagedBass
* NWaves
* [thinkdsp](https://github.com/AllenDowney/ThinkDSP)