module Subtraktor.Tests.EnvTests

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Xunit
open Subtraktor.Env

[<Fact>]
let ``Attack starts at 0`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(0.0, env 0.0<s>)

[<Fact>]
let ``Attack reaches 1 at attack time`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(1.0, env 2.0<s>)

[<Fact>]
let ``Mid attack is linear`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(0.5, env 1.0<s>)

[<Fact>]
let ``Decay reaches 0 at end`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(0.0, env 5.0<s>)

[<Fact>]
let ``Mid decay is linear`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(0.5, env 3.5<s>)

[<Fact>]
let ``Envelope is silent after attack plus decay`` () =
    let env = ad 2.0<s> 3.0<s>
    Assert.Equal(0.0, env 6.0<s>)

[<Fact>]
let ``Envelope never exceeds 1 or drops below 0`` () =
    let env = ad 2.0<s> 3.0<s>
    let samples =
        [ 0.0<s>; 0.5<s>; 1.0<s>; 2.0<s>; 3.0<s>; 4.0<s>; 5.0<s>; 6.0<s> ]
        |> List.map env

    for v in samples do
        Assert.InRange(v, 0.0, 1.0)


