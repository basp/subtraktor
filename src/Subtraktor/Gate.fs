namespace Subtraktor

open Subtraktor.Signal
open Subtraktor.Units
        
type Gate = Time -> bool

module Gate =
    let between (start: Time) (``end``: Time) : Gate =
        fun t -> t >= start && t < ``end``
            
    let always : Gate = fun _ -> true
    
    let never : Gate = fun _ -> false
    
    let invert (g: Gate) : Gate = not << g
    
    let ``and`` (a: Gate) (b: Gate) : Gate =
        fun t-> a t && b t
        
    let ``or`` (a: Gate) (b: Gate) : Gate =
        fun t-> a t || b t
        
    let pulse (period: Time) (width: Time) : Gate =
        fun t -> (t % period) < width
        
    let apply (gate: Gate) (signal: Signal) : Signal =
        fun t -> if gate t then signal t else 0.0
        