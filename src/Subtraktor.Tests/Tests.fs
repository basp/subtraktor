module Tests

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Xunit
open Subtraktor.Synthesis.Math

let ``44.1KHz`` = 44100

[<Fact>]
let ``Sample duration matches inverse of sample rate`` () =
    let actual = sampleDuration ``44.1KHz``
    let expected = 1.0 / float ``44.1KHz``
    Assert.Equal(expected, actual, 1e-10)

[<Fact>]
let ``Time to samples is inverse of samples to time`` () =
    let expected = 2.5
    let samples = timeToSamples ``44.1KHz`` expected
    let actual = samplesToTime ``44.1KHz`` samples
    Assert.Equal(expected, actual, 1e-10)

[<Fact>]
let ``1 second equals sample rate`` () =
    let actual = timeToSamples ``44.1KHz`` 1.0
    Assert.Equal(``44.1KHz``, actual)

[<Fact>]
let ``0.5 seconds at 44100 Hz equals 22050 samples`` () =
    let actual = timeToSamples ``44.1KHz`` 0.5
    Assert.Equal(22050, actual)
    
[<Fact>]
let ``0.01 seconds at 44100 Hz equals 441 samples`` () =
    let actual = timeToSamples ``44.1KHz`` 0.01
    Assert.Equal(441, actual)
    
[<Fact>]
let ``44100 samples at 44100 Hz equals 1 second`` () =
    let actual = samplesToTime ``44.1KHz`` 44100
    Assert.Equal(1.0, actual)
    
[<Fact>]
let ``22050 samples at 44100 Hz equals 0.5 second`` () =
    let actual = samplesToTime ``44.1KHz`` 22050
    Assert.Equal(0.5, actual)

[<Fact>]
let ``A4 reference frequency is 440 KHz`` () =
    Assert.Equal(440.0, A4_HZ)

[<Fact>]
let ``A5 is double A4 frequency`` () =
    let a5 = midiNoteToFrequency 81
    Assert.Equal(880.0, a5)
    
[<Fact>]
let ``A3 is half A4 frequency`` () =
    let a3 = midiNoteToFrequency 57
    Assert.Equal(220.0, a3)
    
[<Fact>]
let ``C notes across different octaves`` () =
    let c4 = midiNoteToFrequency 60
    let c5 = midiNoteToFrequency 72
    let expected = c4 * 2.0
    Assert.Equal(expected, c5)

[<Fact>]
let ``Semitone ratio is 2^(1/12)`` () =
    let a4 = midiNoteToFrequency 69
    let aSharp4 = midiNoteToFrequency 70
    let actual = aSharp4 / a4
    let expected = 2.0 ** (1.0 / 12.0)
    Assert.Equal(expected, actual)

[<Fact>]
let ``Frequency to MIDI note conversion (round trip)`` () =
    let expected = [220.0; 440.0; 880.0; 110.0]
    let notes = expected |> List.map frequencyToMidiNote
    let actual = notes |> List.map midiNoteToFrequency
    Assert.Equal<float list>(expected, actual)
    
[<Fact>]
let ``Very low frequency clamps to MIDI 0`` () =
    let actual = frequencyToMidiNote 0.1
    Assert.Equal(0, actual)

[<Fact>]    
let ``Very high frequency clamps to MIDI 127`` () =
    let actual = frequencyToMidiNote 20000.0
    Assert.Equal(127, actual)

[<Fact>]   
let ``Phase increment for 440 Hz frequency at 44.1 KHz sample rate`` () =
    let actual = frequencyToPhaseIncrement ``44.1KHz`` 440.0
    let expected = (2.0 * Math.PI * 440.0) / float ``44.1KHz``
    Assert.Equal(expected, actual)

[<Fact>]
let ``0 Hz frequency should give phase increment of 0`` () =
    let actual = frequencyToPhaseIncrement ``44.1KHz`` 0.0
    Assert.Equal(0.0, actual)
  
[<Fact>]
let ``Phase increment at different sample rates`` () =
    let phase48k = frequencyToPhaseIncrement 48000 440.0
    let phase44k = frequencyToPhaseIncrement 44100 440.0
    Assert.True(phase48k < phase44k)
    
[<Fact>]
let ``Normalized phase keeps values in [0, 2π)`` () =
    let phase = normalizePhase (3.0 * Math.PI)
    Assert.True(phase >= 0.0 && phase < TWO_PI)

[<Fact>]
let ``Normalized phase of 0 is 0`` () =
    let actual = normalizePhase 0.0
    Assert.Equal(0.0, actual)

[<Fact>]
let ``Normalized phase of 2π is 0`` () =
    let actual = normalizePhase TWO_PI
    Assert.Equal(0.0, actual)

[<Fact>]
let ``Normalized phase of negative value wraps correctly`` () =
    let actual = normalizePhase (-Math.PI)
    Assert.Equal(Math.PI, actual)

[<Fact>]
let ``Wrapped phase wraps correctly`` () =    
    let actual = wrapPhase 1.5
    Assert.Equal(0.5, actual)

[<Fact>]    
let ``Wrapped phase of negative values wraps correctly`` () =
    let actual = wrapPhase -0.5
    Assert.Equal(0.5, actual)
    
[<Fact>]
let ``1.0 linear is 0 dB`` () =
    let actual = linearToDecibels 1.0
    Assert.Equal(0.0, actual)

[<Fact>]
let ``Decibels to linear is inverse of linear to decibels`` () =
    let expected = [0.1; 0.5; 1.0; 2.0]
    let levels = expected |> List.map linearToDecibels
    let actual = levels |> List.map decibelsToLinear
    Assert.Equal<float list>(expected, actual)

[<Fact>]
let ``Specific dB values`` () =
    let tests = [-6.0; -20.0; -3.0]
    let expected = [0.501187; 0.1; 0.707945]
    let actual = tests |> List.map decibelsToLinear
    Assert.Equal(expected.Length, actual.Length)
    expected
    |> List.zip actual
    |> List.iter (fun (e, a) -> Assert.Equal(e, a, 1e-6))
