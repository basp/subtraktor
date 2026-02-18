namespace Subtraktor

open System
open Subtraktor.Units

type Signal = Time -> float

module Signal =
    let map (f: float -> float) (s: Signal) : Signal =
        fun t -> f (s t)
        
    let apply (f: Signal -> Signal) (s: Signal) : Signal =
        f s   

    let scale (k: float) (s: Signal) : Signal =
        fun t -> k * s t   

    let bias (b: float) (s: Signal) : Signal =
        fun t -> s t + b    
    
    let add (a: Signal) (b: Signal) : Signal =
        fun t -> a t + b t
    
    let mul (a: Signal) (b: Signal) : Signal =
        fun t -> a t * b t    
                   
    let mix (a: Signal) (b: Signal) : Signal =
        scale 0.5 (add a b)

    let constant (c: float) : Signal =
        fun _ -> c

    let zero = constant 0.0

    let one = constant 1.0
        
    let sine (freq: Frequency) : Signal =
        fun t -> sin (2.0 * Math.PI * freq * t)

    let saw (freq: Frequency) : Signal =
        fun t ->
            let phase = freq * t
            2.0 * (phase - floor phase) - 1.0

    let square (freq: Frequency) : Signal =
        fun (t: Time) ->
            let phase = freq * t
            if (phase - floor phase) < 0.5 then 1.0
            else -1.0    
    
    let triangle (freq: Frequency) : Signal =
        fun t ->
            let phase =
                let x = (freq * t) % 1.0 
                if x < 0.0 then x + 1.0
                else x            
            2.0 * abs (2.0 * phase - 1.0) - 1.0
            
    let sample (rate: SampleRate) (duration: Time) (signal: Signal) =
        let dt = 1.0 / rate
        let samples = int (duration / dt)
        Array.init samples (fun i ->
            let t = float i * dt
            (t, signal t))
