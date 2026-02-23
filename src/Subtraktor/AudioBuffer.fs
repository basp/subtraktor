namespace Subtraktor

open System

/// Interleaved samples, normalized to [-1.0f; 1.0f] for pure transforms.
type AudioBuffer =
    { Format: WaveFormat
      FrameCount: FrameCount
      Samples: float32[] }

[<RequireQualifiedAccess>]
module AudioBuffer =
    let create
        (format: WaveFormat)
        (frames: FrameCount)
        (samples: float32[]) : Result<AudioBuffer, ValidationError> =
        failwith "Not implemented"
        
    let mapSamples
        (f: float32 -> float32)
        (buffer: AudioBuffer) : AudioBuffer =
        failwith "Not implemented"
        
    let duration (buffer: AudioBuffer) : TimeSpan =
        failwith "Not implemented"
