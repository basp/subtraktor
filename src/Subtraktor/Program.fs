open System
open System.IO
open System.Runtime.InteropServices
open System.Text

[<Literal>]
let SampleRate = 44100

let writeWavMono16 (path: string) (sampleRate: int) (samples: int16[]) =
    let numChannels = 1s
    let bitsPerSample = 16s
    let blockAlign = int16 (int numChannels * int bitsPerSample / 8)
    let byteRate = sampleRate * int blockAlign
    let dataSizeBytes = samples.Length * sizeof<int16>
    let riffSize = 36 + dataSizeBytes
    
    use fs = new FileStream(path, FileMode.Create)
    use bw = new BinaryWriter(fs, Encoding.ASCII)
    
    let writeAscii (s: string) = bw.Write(Encoding.ASCII.GetBytes(s))
    
    writeAscii "RIFF"
    bw.Write(riffSize)
    writeAscii "WAVE"
    
    writeAscii "fmt "
    bw.Write(16)
    bw.Write(int16 1)
    bw.Write(numChannels)
    bw.Write(sampleRate)
    bw.Write(byteRate)
    bw.Write(blockAlign)
    bw.Write(bitsPerSample)
    
    writeAscii "data"
    bw.Write(dataSizeBytes)
    bw.Write(MemoryMarshal.AsBytes(samples.AsSpan()))
    
let secondsToSamples (sampleRate: int) (seconds: float) =
    int (Math.Round(seconds * float sampleRate))

let floatToPcm16 (x: float) =
    let xClamped = max -1.0 (min 1.0 x)
    int16 (Math.Round(xClamped * 32767.0))

[<EntryPoint>]
let main _argv =
    let durationSec = 2.0
    let n = secondsToSamples SampleRate durationSec
    
    let freqHz = 440.0
    let amplitude = 0.2
    
    let samples : int16[] =
        Array.init n (fun i ->
            let t = float i / float SampleRate
            let x = amplitude * Math.Sin(2.0 * Math.PI * freqHz * t)
            floatToPcm16 x)
        
    writeWavMono16 "some/path/sine.wav" SampleRate samples    
    0
