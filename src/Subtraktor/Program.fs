open System

open Subtraktor
open FSharp.Data.UnitSystems.SI.UnitSymbols

let example () =
    // let sampleRate = 44100.0
    //
    // let sine freq sampleRate : seq<float> =
    //     let phaseInc = 2.0 * Math.PI * freq / sampleRate
    //     let rec loop phase =
    //         seq {
    //             yield sin phase
    //             let nextPhase = (phase + phaseInc) % (2.0 * Math.PI) 
    //             yield! loop nextPhase
    //         }
    //     loop 0.0
    //   
    // let lfo = sine 6.0 sampleRate
    //
    // let modulated =
    //     let lfoSeq = lfo
    //     let rec loop lfoStream =
    //         seq {
    //             let freq = Seq.head lfoStream
    //             yield! (sine freq sampleRate |> Seq.take 1)
    //             yield! loop (Seq.skip 1 lfoStream)
    //         }            
    //     loop lfoSeq
    //     
    // let duration = 1.0
    // let sampleCount = int (sampleRate * duration)
    //
    // let samples =
    //     modulated
    //     |> Seq.take sampleCount
    //     |> Seq.toArray
    //     
    // samples
    // |> Wav.write "d:/temp/test.wav" (LanguagePrimitives.FloatWithMeasure<Hz>(sampleRate))

    
    
    
    ()
    
example () |> ignore