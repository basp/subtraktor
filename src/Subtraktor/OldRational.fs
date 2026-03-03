module Subtraktor.OldRational

open System

[<CustomEquality; CustomComparison>]
type Rational =
    { Num : int
      Den : int }
    
    override this.GetHashCode() =
        HashCode.Combine(this.Num, this.Den)
        
    interface IComparable<Rational> with
        member this.CompareTo other =
            let left = this.Num * other.Den
            let right = other.Num * this.Den
            if left < right then -1
            elif left > right then 1
            else 0
            
    interface IComparable with
        member this.CompareTo obj =
            match obj with
            | :? Rational as other ->
                (this :> IComparable<Rational>).CompareTo(other)
            | _ -> invalidArg (nameof(obj)) "Argument must of type Rational."
    
    interface IEquatable<Rational> with
        member this.Equals(other) =
            false             
    
    override this.Equals(obj) =
        match obj with
        | :? Rational as other ->
            (this :> IEquatable<Rational>).Equals(other)
        | _ -> false

module Rational =
    let private gcd a b =
        let rec loop a b =
            if b = 0 then abs a
            else loop b (a % b)
        loop a b

    let private normalize n d =
        if d = 0 then invalidArg (nameof(d)) "Denominator cannot be zero."
        
        let n, d =
            if d < 0 then -n, -d
            else n, d
            
        let g = gcd n d
        { Num = n / g; Den = d / g }
        
    let create n d = normalize n d    
    let zero = create 0 1
    let one = create 1 1    
    let ofInt n = create n 1
    
    let add a b =
        let n = a.Num * b.Den + b.Num * a.Den
        let d = a.Den * b.Den
        normalize n d
        
    let subtract a b =
        let n = a.Num * b.Den - b.Num * a.Den
        let d = a.Den * b.Den
        normalize n d
        
    let multiply a b =
        let n = a.Num * b.Num
        let d = a.Den * b.Den
        normalize n d
        
    let divide a b =
        if b.Num = 0 then invalidArg (nameof(b)) "Cannot divide by zero."
        let n = a.Num * b.Den
        let d = a.Den * b.Num
        normalize n d
        
    let equals a b =
        a.Num = b.Num &&
        a.Den = b.Den   
        
    let compare a b =
        let left = a.Num * b.Den
        let right = b.Num * a.Den
        if left < right then -1
        elif left > right then 1
        else 0

    let (+) a b = add a b
    let (-) a b = subtract a b
    let (*) a b = multiply a b
    let (/) a b = divide a b            