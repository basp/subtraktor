#r "nuget: Plotly.NET, 5.0.0"

open System
open FSharp.Data.UnitSystems.SI.UnitSymbols
open Plotly.NET

let duration = 1.0

let framerate = 1000.0

let freq = 4.0
 
let pi2 = 2.0 * Math.PI 
 
let amp = 1.0

let offset : float = 0.0

let ts =
    let n =
        round (duration * framerate)
        |> int
    Array.init n (fun i -> float i / framerate) 
 
let phase t f offset =
    (pi2 * f * t + offset)
 
let sine =
    let y x = amp * sin x
    ts
    |> Array.map (fun t -> phase t freq offset)
    |> Array.map y
    
let square =
    sine
    |> Array.map sign
    
let saw =
    let y x = phase x freq offset
    ts
    |> Array.map y
    
let sinusoid a t ω ϕ =
    // ω = 2πf
    a * sin (ω * t + ϕ)  
    
let phasor (framerate : float<1/s>) =    
    let dt = 1.0 / framerate
    let rec loop acc = seq {
        yield acc
        let next = (acc + dt) % 1.0<s>
        yield! loop next
    }
    loop 0.0    
    
Chart.combine [
    Array.zip ts saw |> Chart.Line
    Array.zip ts sine |> Chart.Line
    Array.zip ts square |> Chart.Line ]
|> Chart.show

// Angular velocity with measurements example.
type [<Measure>] rad

let example () =
    let twoPi = 2.0<rad> * Math.PI
    let f = 1.0<1/s>
    twoPi * f