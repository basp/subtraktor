module Subtraktor.Env

open FSharp.Data.UnitSystems.SI.UnitSymbols
open Subtraktor.Units

/// <summary>
/// Creates a simple attack–decay (AD) amplitude envelope.
/// </summary>
/// <remarks>
/// <para>
/// The envelope rises linearly from 0.0 to 1.0 over the specified
/// <paramref name="attack"/> time, then falls linearly from 1.0 to 0.0
/// over the specified <paramref name="decay"/> time.
/// </para>
/// <para>
/// After <c>attack + decay</c> seconds, the envelope outputs <c>0.0</c>
/// for all further times. This makes the AD envelope a one‑shot shape:
/// it does not sustain and produces zero beyond its duration.
/// </para>
/// <para>
/// When applied to a <c>Signal</c>, any portion of the rendered audio
/// beyond the envelope duration will be silent.
/// </para>
/// </remarks>
/// <param name="attack">
/// Time for the envelope to rise from 0.0 to 1.0.
/// </param>
/// <param name="decay">
/// Time for the envelope to fall from 1.0 back to 0.0.
/// </param>
/// <param name="t">
/// The time coordinate at which to evaluate the envelope.
/// </param>
/// <returns>
/// An envelope function mapping <c>Time</c> to amplitude in the range [0.0, 1.0].
/// </returns>
let ad (attack: Time) (decay: Time) : Signal =
    fun (t: float<s>) ->
        if t < attack then
            float (t / attack)
        elif t < attack + decay then
            let dt = t - attack
            1.0 - float (dt / decay)
        else
            0.0
                
type AsrState =
    | Idle
    | Attack
    | Sustain
    | Release
    
type Asr =
    { Attack : Time
      Release : Time }
    
let ``asr`` (gate: Gate) (env: Asr) : Signal =
    let mutable state = Idle
    let mutable lastGate = false
    let mutable attackStart = 0.0<s>
    let mutable releaseStart = 0.0<s>
    
    fun t ->
        let newGate = gate t
        match lastGate, newGate with
        | false, true ->
            state <- Attack
            attackStart <- t
        | true, false ->
            state <- Release
            releaseStart <- t
        | _ -> ()
        
        lastGate <- newGate
        
        match state with
        | Idle -> 0.0
        | Attack ->
            let x = (t - attackStart) / env.Attack
            if x >= 1.0 then
                state <- Sustain
                1.0
            else x
        | Sustain -> 1.0
        | Release ->
            let x = 1.0 - (t - releaseStart) / env.Release
            if x <= 0.0 then
                state <- Idle
                0.0
            else x
