module Subtraktor.Lfo

open Subtraktor.Units

let sine (freq: Frequency) : Signal =
    Osc.sine freq

// let triangle (freq: Frequency) : Signal =
//     Osc.triangle freq
    
let square (freq: Frequency) : Signal =
    Osc.square freq
    
let saw (freq: Frequency) : Signal =
    Osc.saw freq