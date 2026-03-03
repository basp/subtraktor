module Subtraktor.Tests

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Xunit
open Subtraktor.Units
open Subtraktor.Synthesis.Math

let ``44.1KHz`` = 44100<Hz>

[<Fact>]
let ``Sample duration matches inverse of sample rate`` () =
    let actual = sampleDuration ``44.1KHz`` |> float
    let expected = 1.0 / float ``44.1KHz`` |> float
    Assert.Equal(expected, actual, 1e-10)

[<Fact>]
let ``Time to samples is inverse of samples to time`` () =
    let expected = 2.5<s>
    let samples = timeToSamples ``44.1KHz`` expected
    let actual = samplesToTime ``44.1KHz`` samples
    Assert.Equal(float expected, float actual, 1e-10)

[<Fact>]
let ``1 second equals sample rate`` () =
    let actual = timeToSamples ``44.1KHz`` 1.0<s>
    Assert.Equal(int ``44.1KHz``, actual)

[<Fact>]
let ``0.5 seconds at 44100 Hz equals 22050 samples`` () =
    let actual = timeToSamples ``44.1KHz`` 0.5<s>
    Assert.Equal(22050, actual)
    
[<Fact>]
let ``0.01 seconds at 44100 Hz equals 441 samples`` () =
    let actual = timeToSamples ``44.1KHz`` 0.01<s>
    Assert.Equal(441, actual)
    
[<Fact>]
let ``44100 samples at 44100 Hz equals 1 second`` () =
    let actual = samplesToTime ``44.1KHz`` 44100
    Assert.Equal(1.0<s>, actual)
    
[<Fact>]
let ``22050 samples at 44100 Hz equals 0.5 second`` () =
    let actual = samplesToTime ``44.1KHz`` 22050
    Assert.Equal(0.5<s>, actual)

[<Fact>]
let ``A4 reference frequency is 440 KHz`` () =
    Assert.Equal(440.0<Hz>, A4_HZ)

[<Fact>]
let ``A5 is double A4 frequency`` () =
    let a5 = midiNoteToFrequency 81
    Assert.Equal(880.0<Hz>, a5)
    
[<Fact>]
let ``A3 is half A4 frequency`` () =
    let a3 = midiNoteToFrequency 57
    Assert.Equal(220.0<Hz>, a3)
    
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
    let expected = [220.0<Hz>; 440.0<Hz>; 880.0<Hz>; 110.0<Hz>]
    let notes =
        expected
        |> List.map frequencyToMidiNote
    let actual =
        notes
        |> List.map midiNoteToFrequency
        |> List.map float
    Assert.Equal<float list>(expected |> List.map float, actual)
    
[<Fact>]
let ``Very low frequency clamps to MIDI 0`` () =
    let actual = frequencyToMidiNote 0.1<Hz>
    Assert.Equal(0, actual)

[<Fact>]    
let ``Very high frequency clamps to MIDI 127`` () =
    let actual = frequencyToMidiNote 20000.0<Hz>
    Assert.Equal(127, actual)

[<Fact>]   
let ``Phase increment for 440 Hz frequency at 44.1 KHz sample rate`` () =
    let actual = frequencyToPhaseIncrement ``44.1KHz`` 440.0<Hz>
    let expected =
        (2.0 * Math.PI * 440.0) / float ``44.1KHz``
        |> LanguagePrimitives.FloatWithMeasure<rad>
    Assert.Equal(expected, actual)

[<Fact>]
let ``0 Hz frequency should give phase increment of 0`` () =
    let actual = frequencyToPhaseIncrement ``44.1KHz`` 0.0<Hz>
    Assert.Equal(0.0<rad>, actual)
  
[<Fact>]
let ``Phase increment at different sample rates`` () =
    let phase48k = frequencyToPhaseIncrement 48000<Hz> 440.0<Hz>
    let phase44k = frequencyToPhaseIncrement 44100<Hz> 440.0<Hz>
    Assert.True(phase48k < phase44k)
    
[<Fact>]
let ``Normalized phase keeps values in [0, 2π)`` () =
    let phase = normalizePhase (3.0<rad> * Math.PI)
    Assert.True(phase >= 0.0<rad> && phase < TWO_PI)

[<Fact>]
let ``Normalized phase of 0 is 0`` () =
    let actual = normalizePhase 0.0<rad>
    Assert.Equal(0.0<rad>, actual)

[<Fact>]
let ``Normalized phase of 2π is 0`` () =
    let actual = normalizePhase TWO_PI
    Assert.Equal(0.0<rad>, actual)

[<Fact>]
let ``Normalized phase of negative value wraps correctly`` () =
    let angle = -Math.PI |> LanguagePrimitives.FloatWithMeasure<rad>
    let actual = normalizePhase angle
    let expected = Math.PI |> LanguagePrimitives.FloatWithMeasure<rad>
    Assert.Equal(expected, actual)

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
    Assert.Equal(0.0<dB>, actual)

[<Fact>]
let ``Decibels to linear is inverse of linear to decibels`` () =
    let expected = [0.1; 0.5; 1.0; 2.0]
    let levels = expected |> List.map linearToDecibels
    let actual = levels |> List.map decibelsToLinear
    Assert.Equal<float list>(expected, actual)

[<Fact>]
let ``Specific dB values`` () =
    let tests = [-6.0<dB>; -20.0<dB>; -3.0<dB>]
    let expected = [0.501187; 0.1; 0.707945]
    let actual = tests |> List.map decibelsToLinear
    Assert.Equal(expected.Length, actual.Length)
    expected
    |> List.zip actual
    |> List.iter (fun (e, a) -> Assert.Equal(e, a, 1e-6))

module ``Testing rational numbers`` =
    open Subtraktor.Numerics

    [<Fact>]
    let ``Test create and normalization`` () =
        let tests = [
            (Rational.Create(2, 4), Rational.Create(1, 2))
            (Rational.Create(-3, 6), Rational.Create(-1, 2))
            (Rational.Create(3, -6), Rational.Create(-1, 2))
            (Rational.Create(-3, -6), Rational.Create(1, 2))        
        ]

        tests|> List.iter Assert.Equal

    let r (n: int) (d: int) = Rational.Create(n, d)
    
    [<Fact>]
    let ``Adding zero to a rational leaves it unchanged`` () =
        let rat = r 3 4
        let result = Rational.add rat Rational.zero
        Assert.Equal(rat, result)
        
    [<Fact>]
    let ``Adding two identical rationals doubles the numerator`` () =
        let rat = r 1 3
        let result = Rational.add rat rat
        Assert.Equal(r 2 3, result)

    [<Fact>]
    let ``Adding rationals with same denominator`` () =
        let a = r 1 4
        let b = r 1 4
        let result = Rational.add a b
        Assert.Equal(r 1 2, result)

    [<Fact>]
    let ``Adding rationals with different denominators`` () =
        let a = r 1 2
        let b = r 1 3
        let result = Rational.add a b
        Assert.Equal(r 5 6, result)

    [<Fact>]
    let ``Added results are automatically normalized`` () =
        let a = r 1 2
        let b = r 1 2
        let result = Rational.add a b
        Assert.Equal(r 1 1, result)

    [<Fact>]
    let ``Adding negative and positive rationals`` () =
        let a = r 3 4
        let b = r -1 4
        let result = Rational.add a b
        Assert.Equal(r 1 2, result)

    [<Fact>]
    let ``Adding two negative rationals`` () =
        let a = r -1 3
        let b = r -1 6
        let result = Rational.add a b
        Assert.Equal(r -1 2, result)
        
    [<Fact>]
    let ``Adding with cancellation of common factors`` () =
        let a = r 1 4
        let b = r 1 6
        let result = Rational.add a b
        Assert.Equal(r 5 12, result)

    [<Fact>]
    let ``Subtracting zero leaves rational unchanged`` () =
        let rat = r 5 7
        let result = Rational.sub rat Rational.zero
        Assert.Equal(rat, result)
        
    [<Fact>]
    let ``Subtracting a rational from itself gives zero`` () =
        let rat = r 3 5
        let result = Rational.sub rat rat
        Assert.Equal(Rational.zero, result)
        
    [<Fact>]
    let ``Subtracting rationals with same denominator`` () =
        let a = r 3 4
        let b = r 1 4
        let result = Rational.sub a b
        Assert.Equal(r 1 2, result)

    [<Fact>]
    let ``Subtracting rationals with different denominators`` () =
        let a = r 1 2
        let b = r 1 3
        let result = Rational.sub a b
        Assert.Equal(r 1 6, result)
        
    [<Fact>]
    let ``Subtracting results in negative rational`` () =
        let a = r 1 4
        let b = r 3 4
        let result = Rational.sub a b
        Assert.Equal(r -1 2, result)

    [<Fact>]
    let ``Subtracting negative from positive`` () =
        let a = r 3 4
        let b = r -1 4
        let result = Rational.sub a b
        Assert.Equal(r 1 1, result)

    [<Fact>]        
    let ``Subtracting two negative rationals`` () =
        let a = r -1 3
        let b = r -1 6
        let result = Rational.sub a b
        Assert.Equal(r -1 6, result)
        
    [<Fact>]
    let ``Addition is commutative`` () =
        let a = r 2 5
        let b = r 3 7
        let r1 = Rational.add a b
        let r2 = Rational.add b a
        Assert.Equal(r1, r2)
        
    [<Fact>]
    let ``Subtraction is not commutative`` () =
        let a = r 2 5
        let b = r 3 7
        let r1 = Rational.sub a b
        let r2 = Rational.sub b a
        Assert.Equal(r1.Numerator, -r2.Numerator)
        Assert.Equal(r1.Denominator, r2.Denominator)
        
    [<Fact>]
    let ``Multiplying by zero gives zero`` () =
        let rat = r 3 4
        let result = Rational.mul rat Rational.zero
        Assert.Equal(Rational.zero, result)

    [<Fact>]
    let ``Multiplying by one leaves rational unchanged`` () =
        let rat = r 3 4
        let result = Rational.mul rat Rational.one
        Assert.Equal(rat, result)
        
    [<Fact>]
    let ``Multiplying two identical rationals`` () =
        let rat = r 2 3
        let result = Rational.mul rat rat
        Assert.Equal(r 4 9, result)
        
    [<Fact>]
    let ``Multiplying rations with different denominators`` () =
        let a = r 2 3
        let b = r 3 4
        let result = Rational.mul a b
        Assert.Equal(r 1 2, result)

    [<Fact>]
    let ``Multiplied results are automatically normalized`` () =
        let a = r 2 4
        let b = r 2 4
        let result = Rational.mul a b
        Assert.Equal(r 1 4, result)
        
    [<Fact>]
    let ``Multiplying negative and positive rationals`` () =
        let a = r 2 3
        let b = r -3 4
        let result = Rational.mul a b
        Assert.Equal(r -1 2, result)
        
    [<Fact>]
    let ``Multiplying two negative rationals`` () =
        let a = r -2 3
        let b = r -3 4
        let result = Rational.mul a b
        Assert.Equal(r 1 2, result)
        
    [<Fact>]
    let ``Multiplying with cancellation of common factors`` () =
        let a = r 3 4
        let b = r 4 5
        let result = Rational.mul a b
        Assert.Equal(r 3 5, result)
        
    [<Fact>]
    let ``Multiplication is commutative`` () =
        let a = r 2 5
        let b = r 3 7
        let r1 = Rational.mul a b
        let r2 = Rational.mul b a
        Assert.Equal(r1, r2)
        
    [<Fact>]
    let ``Dividing by one leaves rational unchanged`` () =
        let rat = r 5 7
        let result = Rational.div rat Rational.one
        Assert.Equal(rat, result)
        
    [<Fact>]
    let ``Dividing a rational by itself gives one`` () =
        let rat = r 3 5
        let result = Rational.div rat rat
        Assert.Equal(Rational.one, result)
        
    [<Fact>]
    let ``Dividing zero by a non-zero rational gives zero`` () =
        let rat = r 3 5
        let result = Rational.div Rational.zero rat
        Assert.Equal(Rational.zero, result)
    
    [<Fact>]    
    let ``Dividing by zero throws exception`` () =
        let rat = r 3 5
        Assert.Throws<DivideByZeroException>(fun () ->
            Rational.div rat Rational.zero
            |> ignore)
    
    [<Fact>]
    let ``Diving rations with different denominators`` () =
        let a = r 2 3
        let b = r 3 4
        let result = Rational.div a b
        Assert.Equal(r 8 9, result)
    
    [<Fact>]
    let ``Division results are automatically normalized`` () =
        let a = r 2 4
        let b = r 1 2
        let result = Rational.div a b
        Assert.Equal(r 1 1, result)
        
    [<Fact>]
    let ``Dividing negative and positive rationals`` () =
        let a = r 2 3
        let b = r -3 4
        let result = Rational.div a b
        Assert.Equal(r -8 9, result)
        
    [<Fact>]
    let ``Dividing two negative rationals`` () =
        let a = r -2 3
        let b = r -3 4
        let result = Rational.div a b
        Assert.Equal(r 8 9, result)
        
    [<Fact>]
    let ``Division is not commutative`` () =
        let a = r 2 5
        let b = r 3 7
        let r1 = Rational.div a b
        let r2 = Rational.div b a
        Assert.Equal(r1.Numerator, r2.Denominator)
        Assert.Equal(r1.Denominator, r2.Numerator)
        
    [<Fact>]
    let ``Multiplication and division are inverse operations`` () =
        let a = r 2 5
        let b = r 3 7
        let product = Rational.mul a b
        let result = Rational.div product b
        Assert.Equal(a, result)
        
    [<Fact>]
    let ``Hashing equivalent rationals`` () =
        let a = r 2 4
        let b = r 1 2
        let c = r 1 3
        Assert.Equal(a.GetHashCode(), b.GetHashCode())
        Assert.NotEqual(a.GetHashCode(), c.GetHashCode())

    [<Fact>]
    let ``Comparison between equivalent rationals`` () =
        let a = r 2 4
        let b = r 1 2
        let c = r 1 3
        Assert.Equal(0, compare a b)
        Assert.Equal(1, compare a c)
        Assert.Equal(-1, compare c b)
        
    [<Fact>]
    let ``Equality between equivalent rationals`` () =
        let a = r 2 4
        let b = r 1 2
        let c = r 1 3
        Assert.Equal(a, b)
        Assert.NotEqual(a, c)
      
module ``Event stream normalization`` =
    open Subtraktor.Interpreter
    open Subtraktor.Numerics

    let private t (n: int) (d: int) = Rational.Create(n, d)

    let private findTime
        (label: string)
        (matches: MidiEvent -> bool)
        (events: (Time * MidiEvent) list) : Rational =
        events
        |> List.tryPick (fun (t, e) ->
            if matches e then Some t
            else None)
        |> function
            | Some t -> t
            | None -> failwithf $"Expected event not found: %s{label}"

    let private assertOnset
        (label: string)
        (expectedTime: Time)
        (matches: MidiEvent -> bool)
        (events: (Time * MidiEvent) list) : unit =
        let actualTime = findTime label matches events
        Assert.Equal(expectedTime, actualTime)

    let private isNoteOn pitch octave =
        function
        | NoteOn((p, o), _) -> p = pitch && o = octave
        | _ -> false
    
    let private isNoteOff pitch octave =
        function
        | NoteOff((p, o), _) -> p = pitch && o = octave
        | _ -> false
    
    let private isNoteOnC4 = isNoteOn C 4
        
    let private isNoteOffC4 = isNoteOff C 4
        
    let private isNoteOnE4 = isNoteOn E 4
        
    let private isNoteOnG4 = isNoteOn G 4

    [<Fact>]
    let ``Seq boundary trigger`` () =
        // C4 quarter note, then E4 quarter note.
        // Expect E4 NoteOn exactly at C4 NoteOff (1/4).        
        let music =
            Seq [
                Prim (note qn (C, 4))
                Prim (note qn (E, 4))
            ]
            
        let events =
            music
            |> Music.interpret (Rational.ofInt 0)
            |> Seq.toList
            
        Assert.Equal(4, events.Length)
        
        let cOffTime =
            events
            |> List.pick (fun (t, e) ->
                if isNoteOffC4 e then Some t
                else None)
       
        let eOnTime =
            events
            |> List.pick (fun (t, e) ->
                if isNoteOnE4 e then Some t
                else None)
            
        Assert.Equal(t 1 4, cOffTime)
        Assert.Equal(t 1 4, eOnTime)

    [<Fact>]
    let ``Parallel chord onset`` () =
        // C major triad as parallel quarter notes
        // Expect all NoteOn events at start time 0.
        let music =
            Par [
                Prim (note qn (C, 4))
                Prim (note qn (E, 4))
                Prim (note qn (G, 4))
            ]
        
        let events =
            music
            |> Music.interpret (Rational.ofInt 0)
            |> Seq.toList
            
        let onsets =
            events
            |> List.choose (fun (t, e) ->
                match e with
                | NoteOn _ -> Some (t, e)
                | _ -> None)
                 
        Assert.Equal(3, onsets.Length)
        Assert.All(onsets, fun (t, _e) -> Assert.Equal(Rational.ofInt 0, t))
        
    [<Fact>]
    let ``Nested Seq+Par handoff`` () =
        // First: parallel dyad C4+E4 quarter
        // Then: sequential handoff to G4 quarter
        // Expect G4 NoteOn at 1/4 (max duration of first Par block).        
        let music =
            Seq [
                Par [
                    Prim (note qn (C, 4))
                    Prim (note qn (E, 4))
                ]
                Prim (note qn (G, 4))
            ]
        
        let events =
            music
            |> Music.interpret (Rational.ofInt 0)
            |> Seq.toList
        
        let gOnTime =
            events
            |> List.pick (fun (t, e) -> if isNoteOnG4 e then Some t else None)
            
        Assert.Equal (t 1 4, gOnTime)
        
