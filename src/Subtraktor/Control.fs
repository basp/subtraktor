module Subtraktor.Control
    
open Subtraktor
open Subtraktor.Units
            
type Trend =
    | Rising
    | Falling
    | Stable
        
let trend (s: Signal) =
    let mutable previous = 0.0
    fun t ->
        let current = s t
        let edge =
            if previous < 0.5 && current >= 0.5 then Rising
            elif previous >= 0.5 && current < 0.5 then Falling
            else Stable
        previous <- current
        edge

let toSignal (trend: Trend) : Signal =
    match trend with
    | Rising -> fun _ -> 1.0
    | Falling -> fun _ -> -1.0
    | Stable -> fun _ -> 0.0

let isRising s t =
    match trend s t with
    | Rising -> true
    | _ -> false
    
let isFalling s t =
    match trend s t with
    | Falling -> true
    | _ -> false
    
let between (up: Time) (down: Time) : Signal =
    fun t ->
        if t >= up && t < down then 1.0
        else 0.0
