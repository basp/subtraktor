namespace Subtraktor

open System

type DeviceId = DeviceId of string

type Source =
    | FromFile of path: string
    | FromDevice of deviceId: DeviceId

type Sink =
    | ToFile of path: string
    | ToDevice of deviceId: DeviceId

type Transform =
    | Gain of gain: float32
    | NormalizePeak of targetPeak: float32
    | Trim of start: TimeSpan * duration: TimeSpan
    | FadeIn of TimeSpan
    | FadeOut of TimeSpan
    | Resample of SampleRate
    | ToMono
    | ToStereo

type Pipeline =
    { Source: Source
      Transforms: Transform list
      Sink: Sink }

module Pipeline =
    let create source sink =
        { Source = source
          Transforms = []
          Sink = sink }
    
    let addTransform transform pipeline =
       { pipeline with
            Transforms = transform :: pipeline.Transforms } 