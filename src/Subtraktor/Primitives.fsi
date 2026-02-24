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
    val create : int -> Result<SampleRate, ValidationError>
    val value : SampleRate -> int

[<RequireQualifiedAccess>]
module ChannelCount =
    val create : int -> Result<ChannelCount, ValidationError>
    val value : ChannelCount -> int

[<RequireQualifiedAccess>]
module FrameCount =
    val create : int -> Result<FrameCount, ValidationError>
    val value : FrameCount -> int
