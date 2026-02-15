module Subtraktor.Tests.OscTests

open Xunit
open Subtraktor.Osc

let approxEqual x y =
    let closeEnough = 1e-6
    abs (x - y) < closeEnough   

let approxZero x = approxEqual x 0.0

let approxOne x = approxEqual x 1.0

[<Fact>]
let ``sine at t=0 is always 0`` () =
    let s = sine 440.0
    Assert.Equal(0.0, s 0.0)

[<Fact>]
let ``sine reaches peak at quarter period`` () =
    let freq = 440.0
    let s = sine freq
    let tQuarter = 1.0 / (4.0 * freq)
    Assert.True(approxEqual (s tQuarter) 1.0)

[<Fact>]
let ``sine is periodic`` () =
    let freq = 440.0
    let s = sine freq
    let period = 1.0 / freq
    Assert.True(approxZero (abs (s 0.1 - s (0.1 + period))))
