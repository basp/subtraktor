module Subtractor.Tests.SignalTests

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Xunit
open Subtraktor.Signal

[<Fact>]
let ``constant returns the same value for any time`` () =
    let s = constant 0.42
    Assert.Equal(0.42, s 0.0<s>)
    Assert.Equal(0.42, s 1.0<s>)
    Assert.Equal(0.42, s 123.456<s>)

[<Fact>]
let ``add combines two signals sample-wise`` () =
    let s1 = constant 0.3
    let s2 = constant 0.2
    let s = add s1 s2
    Assert.Equal(0.5, s 0.0<s>)

[<Fact>]
let ``scale multiplies a signal`` () =
    let s = scale 2.0 (constant 0.25)
    Assert.Equal(0.5, s 0.0<s>)

[<Fact>]
let ``mix averages two signals`` () =
    let s1 = constant 1.0
    let s2 = constant -1.0
    let s = mix s1 s2
    Assert.Equal(0.0, s 0.0<s>)

[<Fact>]
let ``apply behaves the same as direct function application`` () =
    let double s = scale 2.0 s
    let s = constant 0.3

    let direct = double s
    let viaApply = s |> apply double

    Assert.Equal(direct 1.0<s>, viaApply 1.0<s>)

[<Fact>]
let ``apply enables left-to-right signal flow`` () =
    // A simple chain: scale → add constant → scale again
    let chain =
        constant 1.0
        |> apply (scale 2.0)            // 1.0 * 2.0 = 2.0
        |> apply (add (constant 3.0))   // 2.0 + 3.0 = 5.0
        |> apply (scale 0.5)            // 5.0 * 0.5 = 2.5

    Assert.Equal(2.5, chain 0.0<s>)

[<Fact>]
let ``apply makes higher-order composition readable`` () =
    // A reusable transformation pipe line
    let transform =
        apply (scale 3.0)
        >> apply (add (constant -1.0))

    let s = constant 2.0 |> transform

    // (2.0 * 3.0) - 1.0 = 5.0
    Assert.Equal(5.0, s 0.0<s>)

[<Fact>]
let ``apply clarifies intent when chaining multiple transformations`` () =
    let s =
        constant 0.1
        |> apply (scale 10.0)
        |> apply (scale 0.5)
        |> apply (add (constant 1.0))

    // ((0.1 * 10) * 0.5) + 1 = 1.5
    Assert.Equal(1.5, s 0.0<s>)
