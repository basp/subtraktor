namespace Subtraktor

type SampleRate = private SampleRate of int
type ChannelCount = private ChannelCount of int
type FrameCount = private FrameCount of int

type SampleFormat =
    | Pcm16
    | Pcm24
    | Pcm32
    | Float32

type WaveFormat =
    { SampleRate: SampleRate
      ChannelCount: ChannelCount
      SampleFormat: SampleFormat }

type ValidationError =
    | InvalidSampleRate of int
    | InvalidChannelCount of int
    | InvalidFrameCount of int
    | InvalidGain of float32
    | EmptyBuffer
    | MismatchedFormat of expected: WaveFormat * actual: WaveFormat

[<RequireQualifiedAccess>]
module SampleRate =
    let create x : Result<SampleRate, ValidationError> =
        Ok (SampleRate x)

    let value (SampleRate x) = x

[<RequireQualifiedAccess>]
module ChannelCount =
    let create x : Result<ChannelCount, ValidationError> =
        Ok (ChannelCount x)

    let value (ChannelCount x) = x

[<RequireQualifiedAccess>]
module FrameCount =
    let create x : Result<FrameCount, ValidationError> =
        Ok (FrameCount x)

    let value (FrameCount x) = x
