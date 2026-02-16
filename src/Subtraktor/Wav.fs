module Subtraktor.Wav

open System.IO
open Subtraktor.Units

let encodePcm16alt (samples: float[]) : byte[] =
    failwith "Not implemented"

let encodePcm16 (samples: float[]) : byte[] =
    let clamp x =
        if x > 1.0 then 1.0
        else if x < -1.0 then -1.0
        else x
        
    let to16bit x =
        let s = clamp x
        int16 (s * 32767.0)
        
    let toPcmBytes sample =
        let low = byte (sample &&& 0x00FFs)
        let high = byte ((sample >>> 8) &&& 0x00FFs)
        (low, high)
        
    let pcm = Array.zeroCreate<byte> (samples.Length * 2)
    
    samples
    |> Array.iteri (fun i sample ->
            let low, high =
                sample
                |> to16bit
                |> toPcmBytes
            pcm[2 * i] <- low
            pcm[2 * i + 1] <- high)
    
    pcm
    
let buildHeader (sampleRate: SampleRate) (dataSize: int) : byte[] =
    let fmtHeaderSize = 16
    let format = 1 // PCM
    let channels = 1
    let bitsPerSample = 16
    let blockAlign = channels * bitsPerSample / 8
    let byteRate = (int sampleRate) * blockAlign
    
    // RIFF/WAVE headers are typically 44 bytes long. 
    let header = Array.zeroCreate<byte> 44
    
    let writeAscii offset (s: string) =
        for i = 0 to s.Length - 1 do
            header[offset + i] <- byte s[i]
    
    let writeLE32 offset (i: int) =
        header[offset + 0] <- byte ((i >>> 0) &&& 0xFF)
        header[offset + 1] <- byte ((i >>> 8) &&& 0xFF)
        header[offset + 2] <- byte ((i >>> 16) &&& 0xFF)
        header[offset + 3] <- byte ((i >>> 24) &&& 0xFF)
    
    let writeLE16 offset (i: int) =
        header[offset + 0] <- byte ((i >>> 0) &&& 0xFF)
        header[offset + 1] <- byte ((i >>> 8) &&& 0xFF)
    
    // RIFF chunk descriptor (12 bytes)
    //
    // 0–3: "RIFF" — identifies the file as a RIFF container.
    // 4–7: Chunk size = 36 + dataSize.
    //      This is the number of bytes following this field.
    //      (WAV files always use 36 + dataSize for PCM.)
    // 8–11: "WAVE" — identifies the RIFF payload as WAVE audio.
    writeAscii 0 "RIFF"
    writeLE32 4 (36 + dataSize)
    writeAscii 8 "WAVE"
    
    // fmt subchunk (24 bytes)
    //
    // 12–15 : "fmt "            — identifies the format subchunk.
    // 16–19 : 16                — size of the PCM format data.
    // 20–21 : 1                 — audio format = 1 (PCM, uncompressed).
    // 22–23 : 1                 — number of channels (mono).
    // 24–27 : sampleRate        — samples per second.
    // 28–31 : byteRate          — sampleRate * channels * bitsPerSample/8.
    // 32–33 : blockAlign        — channels * bitsPerSample/8.
    // 34–35 : bitsPerSample     — 16 bits per sample.
    writeAscii 12 "fmt "
    writeLE32 16 fmtHeaderSize          // PCM header size
    writeLE16 20 format                 // audio format = 1 (PCM)
    writeLE16 22 channels               // num channels = 1 (mono)
    writeLE32 24 (int sampleRate)       // sample rate
    writeLE32 28 byteRate               // byte rate (mono, 16-bit)
    writeLE16 32 blockAlign             // block align
    writeLE16 34 bitsPerSample          // bits per sample

    // data subchunk (8 bytes)
    //
    // 36–39 : "data"          — identifies the beginning of the audio data.
    // 40–43 : dataSize        — number of bytes of PCM sample data.
    //                           For 16‑bit mono: numSamples * 2.
    //                           The PCM stream begins immediately after this field.
    writeAscii 36 "data"
    writeLE32 40 dataSize

    header
    
let writeWav path rate samples =
    let pcm = encodePcm16 samples
    let header = buildHeader rate pcm.Length
    use stream = new FileStream(path, FileMode.Create)
    stream.Write(header, 0, header.Length)
    stream.Write(pcm, 0, pcm.Length)
