module Subtraktor.Tests.AsrTests

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Xunit
open Subtraktor
open Utils

let gate = Gate.between 1.0<s> 4.0<s>

let env =
    Env.``asr`` gate {
        Attack = 2.0<s>
        Release = 1.0<s>
    }

[<Fact>]
let ``before gate, envelope is zero`` () =
    let v = env 0.5<s>
    
    Assert.True(approxEqual 0.0 v)

[<Fact>]
let ``attack phase ramps from 0 to 1`` () =
    let v1 = env 1.0<s>
    let v2 = env 2.0<s>
    let v3 = env 3.0<s>
    
    Assert.True (approxEqual 0.0 v1)
    Assert.True (approxEqual 0.5 v2)
    Assert.True (approxEqual 1.0 v3)

[<Fact>]
let ``sustain phase holds at 1`` () =
    // Trigger gate to ensure that the release phase starts.
    env 1.0<s> |> ignore
    env 3.0<s> |> ignore
    
    let v = env 3.5<s>
    Assert.Equal(1.0, v, 1e-6)
