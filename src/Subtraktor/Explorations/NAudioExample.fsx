#r "nuget: NAudio"

open NAudio.Wave

type SampleProvider() =
    interface ISampleProvider with
        member this.WaveFormat =
            WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)
        member this.Read(buffer, offset, count) =
            0

let sampleProvider () =
    { new ISampleProvider with
        member this.Read(buffer, offset, count) =
            0
        member this.WaveFormat=
            WaveFormat.CreateIeeeFloatWaveFormat(44100, 2) }