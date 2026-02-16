module Subtraktor.Envelope

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units
open Subtraktor.Signal

let ad (attack: Time) (decay: Time)=
    fun (t: float<s>) ->
        if t < attack then
            float (t / attack)
        elif t < attack + decay then
            let dt = t - attack
            1.0 - float (dt / decay)
        else
            0.0
            
let withEnvelope (envelope: Signal) signal =
    fun t -> signal t * envelope t