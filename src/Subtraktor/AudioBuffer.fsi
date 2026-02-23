namespace Subtraktor

open System

/// Interleaved samples, normalized to [-1.0f; 1.0f] for pure transforms.
type AudioBuffer =
    { Format: WaveFormat
      FrameCount: FrameCount
      Samples: float32[] }

[<RequireQualifiedAccess>]
module AudioBuffer =
    val create : 
        format: WaveFormat -> 
        frames: FrameCount -> 
        samples: float32[] -> 
        Result<AudioBuffer, ValidationError>
    
    val mapSamples : (float32 -> float32) -> AudioBuffer -> AudioBuffer
    val duration : AudioBuffer -> TimeSpan    