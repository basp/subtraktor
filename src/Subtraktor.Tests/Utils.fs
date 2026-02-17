module Subtraktor.Tests.Utils

let approxEqual x y =    
    let closeEnough = 1e-6
    abs (x - y) < closeEnough   

let approxZero x = approxEqual x 0.0

let approxOne x = approxEqual x 1.0

