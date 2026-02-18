module Subtraktor.Control
    
// val rising : Signal -> (Time -> bool)
// val falling : Signal -> (Time -> bool)
// val gate : Time -> Time -> Signal // like Gate.between

open Subtraktor.Units
            
let isRisingEdge (s: Signal) =
    let mutable previous = 0.0
    fun t ->
        let current = s t
        let isRising = previous < 0.5 && current >= 0.5
        previous <- current
        isRising
    
let isFallingEdge (s: Signal) =
    let mutable previous = 0.0
    fun t ->
        let current = s t
        let isFalling = previous >= 0.5 && current < 0.5        
        previous <- current

let gate (t0: Time) (t1: Time) =
    ()