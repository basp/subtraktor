module Subtraktor.Signal

type Signal = float -> float

let constant (value: float) : Signal =
    fun _ -> value
    
let silence : Signal = constant 0.0

let add (s1: Signal) (s2: Signal) : Signal =
    fun t -> s1 t + s2 t
    
let scale (k: float) (s: Signal) : Signal =
    fun t -> k * s t
    
let mix (s1: Signal) (s2: Signal) : Signal =
    scale 0.5 (add s1 s2)
    
let apply (f: Signal -> Signal) (s: Signal) : Signal =
    f s