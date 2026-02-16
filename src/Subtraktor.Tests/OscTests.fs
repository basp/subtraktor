module Subtraktor.Tests.OscTests

open Xunit
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Osc
open Subtraktor.Tests.Utils

[<Fact>]
let ``sine at t=0 is always 0`` () =
    let s = sine 440.0<Hz>
    Assert.Equal(0.0, s 0.0<s>)

[<Fact>]
let ``sine reaches peak at quarter period`` () =
    let freq = 440.0<Hz>
    let s = sine freq
    let tQuarter = 1.0 / (4.0 * freq)
    Assert.True(approxEqual (s tQuarter) 1.0)

[<Fact>]
let ``sine is periodic`` () =
    let freq = 440.0<Hz>
    let s = sine freq
    let period = 1.0 / freq
    Assert.True(approxZero (abs (s 0.1<s> - s (0.1<s> + period))))
