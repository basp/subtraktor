namespace Subtraktor

open Subtraktor.Numerics
    
module Interpreter =     
    type Octave = int

    type Dur = Rational

    type Time = Rational

    type PitchClass =
        | C
        | Cs
        | D
        | Ds
        | E
        | F
        | Fs
        | G
        | Gs
        | A
        | As
        | B
        
    type Pitch = PitchClass * Octave
        
    type MidiEvent =
        | NoteOn of Pitch * byte
        | NoteOff of Pitch * byte

    type Events = seq<Time * MidiEvent>

    type NormalizedEvents = seq<Time * MidiEvent>

    type Primitive<'a> =
        | Note of Dur * 'a
        | Rest of Dur

    module Primitive =    
        let defaultVelocity = 64uy    
        
        let duration (x: Primitive<_>) =
            match x with
            | Note(dur, _) -> dur
            | Rest(dur) -> dur
            
        let interpret (startTime: Time) (x: Primitive<Pitch>) =
            match x with
            | Note (dur, pitch) ->
                [
                    (startTime, NoteOn(pitch, defaultVelocity))
                    (startTime + dur, NoteOff(pitch, defaultVelocity))
                ]
            // Rest produces no events.
            | Rest _ -> []        

    // NOTE:
    // Stub for now, we'll flush this out later.
    type Control =
        | Temp of float
        // TODO:
        // * Velocity/dynamics
        // * Articulation
        // * Instrument/channel
        // * Selection
        // * etc.

    type Music<'a> =
        | Prim of Primitive<'a>
        | Seq of Music<'a> list
        | Par of Music<'a> list
        | Modify of Control * Music<'a>

    let note d p = Note(d, p)

    let rest d = Rest(d)

    let c oct dur =
        note dur (C, oct)

    let ``c♯`` oct dur =
        note dur (Cs, oct)

    let d oct dur =
        note dur (D, oct)

    let ``d♯`` oct dur =
        note dur (Ds, oct)

    let e oct dur =
        note dur (E, oct)

    let f oct dur =
        note dur (F, oct)

    let ``f♯`` oct dur =
        note dur (Fs, oct)

    let g oct dur =
        note dur (G, oct)

    let ``g♯`` oct dur =
        note dur (Gs, oct)

    let a oct dur =
        note dur (A, oct)

    let ``a♯`` oct dur =
        note dur (As, oct)

    let b oct dur =
        note dur (B, oct)

    let bn = Rational.FromInt32(2)
    let wn = Rational.FromInt32(1)
    let hn = Rational.Create(1, 2)
    let qn = Rational.Create(1, 4)
    let en = Rational.Create(1, 8)
    let sn = Rational.Create(1, 16)
    let tn = Rational.Create(1, 32)
    let sfn = Rational.Create(1, 64)

    let sfnr<'a> = sfn |> rest

    module Music =
        let rec interpret (startTime: Time) (music: Music<Pitch>) : Events =
            seq {
                match music with
                | Prim prim ->
                    yield! prim
                           |> Primitive.interpret startTime
                           |> Seq.ofList
                | Seq pieces ->
                    let mutable currentTime = startTime
                    for piece in pieces do
                        yield! piece
                               |> interpret currentTime
                        currentTime <- currentTime + duration piece
                | Par pieces ->
                    for piece in pieces do
                        yield! piece
                               |> interpret startTime
                | Modify (_ctrl, piece) ->
                    // We'll handle control modifications later.
                    yield! piece |> interpret startTime
            }    
        and duration (music: Music<_>) =
            match music with
            | Prim prim -> prim |> Primitive.duration
            | Seq pieces -> pieces |> Seq.sumBy duration
            | Par pieces ->
                match pieces with
                | [] -> Rational.ofInt 0
                | xs -> xs |> List.map duration |> List.max
            | Modify (_, m) -> duration m

        let normalize (_events: seq<Time * MidiEvent>) : seq<Time * MidiEvent> =
            failwith "Not yet implemented."     

