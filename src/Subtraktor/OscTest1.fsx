#r "nuget: Plotly.NET, 5.0.0"

open System
open Plotly.NET

let sampleRate = 1000.0

let duration = 2.0

let twoPi = 2.0 * Math.PI

let private nextPhase freq sampleRate prevPhase =
    let incr = freq / sampleRate
    let p = prevPhase + incr
    p - floor p    

let saw=
    let mutable phase = 0.0
    fun freq ->
        phase <- nextPhase freq sampleRate phase
        (2.0 * phase) - 1.0
        
let sine =
    let mutable phase = 0.0
    fun freq ->
        phase <- nextPhase freq sampleRate phase
        sin (twoPi * phase)
        
let square =
    let mutable phase = 0.0
    fun freq ->
        phase <- nextPhase freq sampleRate phase
        if phase < 0.5 then 1.0 else -1.0
        
let n = round (duration * sampleRate) |> int
        
let xs =
    Array.init n (fun i -> float i / sampleRate)

let sineOutput1 =
    xs
    |> Array.map (fun _ -> 1.0 * sine 1.0)
    |> Array.map (fun y -> Math.Round(y, 3))

let sineOutput2 =
    sineOutput1
    |> Array.map (fun y -> 0.5 * sine y)
    |> Array.map (fun y -> y - 0.25)
    |> Array.map (fun y -> Math.Round(y, 3))

let sawOutput =
    xs
    |> Array.map (fun _ -> saw 3.0)
    |> Array.map (fun y -> Math.Round(y, 3))
    
let squareOutput =
    xs
    |> Array.map (fun _ -> square 1.5)
    |> Array.map (fun y -> Math.Round(y, 3))
    
let squareOutput2 =
    xs
    |> Array.map (fun _ -> square 1.25)
    |> Array.map (fun y -> Math.Round(y, 3))
    
Chart.combine [
    Array.zip xs sineOutput1 |> Chart.Line
    Array.zip xs sineOutput2 |> Chart.Line
    Array.zip xs sawOutput |> Chart.Line
    Array.zip xs squareOutput |> Chart.Line
    Array.zip xs squareOutput2 |> Chart.Line
]
|> Chart.show

// The first sine wave is 6 Hz with amplitude of 40, which
// means the signal has a range (-40, 40). This signal is then
// being added to the constant value of 440. This addition
// operation causes this value to go up 40 to 480 and down 40 to 400.
// This value feeds into the frequency argument of another sine wave
// with amplitude of 0.3.
// 
// 6 40 sine 440 + 0.3 sine
