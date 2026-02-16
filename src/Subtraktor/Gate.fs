namespace Subtraktor

open Subtraktor.Units
        
type Gate = Time -> bool

module Gate =
    let between (start: Time) (``end``: Time) : Gate =
        fun t -> t >= start && t < ``end``
            
    let always : Gate = fun _ -> true