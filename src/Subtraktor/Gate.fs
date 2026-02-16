namespace Subtraktor

open Subtraktor.Units
        
type Gate = Time -> bool

module Gate =
    let onBetween (start: Time) (``end``: Time) : Gate =
        fun t -> t >= start && t < ``end``
            
    let alwaysOn : Gate = fun _ -> true