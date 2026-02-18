module Subtraktor.Control
    
open Subtraktor
open Subtraktor.Units
            
type Edge =
    | Rising
    | Falling
    | Stable
        
let direction (s: Signal) =
    let mutable previous = 0.0
    fun t ->
        let current = s t
        let edge =
            if previous < 0.5 && current >= 0.5 then Rising
            elif previous >= 0.5 && current < 0.5 then Falling
            else Stable
        previous <- current
        edge

let isRising s t =
    match direction s t with
    | Rising -> true
    | _ -> false
    
let isFalling s t =
    match direction s t with
    | Falling -> true
    | _ -> false
    