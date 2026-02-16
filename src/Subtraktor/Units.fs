module Subtraktor.Units

open FSharp.Data.UnitSystems.SI.UnitSymbols

[<Measure>] type samples

type Time = float<s>

type Frequency = float<Hz>

type SampleRate = float<Hz>
