
type Octave = int

type PitchClass =
    | Cff
    | Cf
    | C
    | Cs
    | Css
    | Dff
    | Df
    | D
    | Ds
    | Dss
    // ...
    
type Pitch = PitchClass * Octave

type Dur =
    | Whole
    | Half
    | Quarter
    | Eighth
    | Sixteenth
    | Custom of float

type Primitive<'a> =
    | Note of Dur * 'a
    | Rest of Dur

type Control =
    | Temp of float

// type Music<'a> =
//     | Prim of Primitive<'a>
//     | Seq of Music<'a> * Music<'a>
//     | Par of Music<'a> * Music<'a>
//     | Modify of Control * Music<'a>

type Music<'a> =
    | Prim of Primitive<'a>
    | Seq of Music<'a> list
    | Par of Music<'a> list
    | Modify of Control * Music<'a>
    
let note d p = Note(d, p)

let rest d = Rest(d)

let cff o d = note d (Cff, o)
let cf o d = note d (Cf, o)
let c o d = note d (C, o)
let cs o d = note d (Cs, o)
let css o d = note d (Css, o)
let dff o d = note d (Dff, o)
let df o d = note d (Df, o)
let d o d = note d (D, o)
let ds o d = note d (Ds, o)
let dss o d = note d (Dss, o)

let bn = Custom 2.0
let wn = Whole
let hn = Half
let qn = Quarter
let en = Eighth
let sn = Sixteenth

let test () =
    let a = Rational.create 2 4
    let b = Rational.create -3 6
    let c = Rational.create 3 -6
    let d = Rational.create -3 -6
    [a, b, c, d]
    
test ()
