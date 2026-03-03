module Subtraktor.Numerics

open System
open System.Numerics

[<Struct; CustomComparison; CustomEquality>]
type Rational =
    val private n: bigint
    val private d: bigint
    
    private new (num, den) =
        { n = num
          d = den }
    
    static let normalize (n: bigint) (d: bigint) =
        if d.IsZero then raise (DivideByZeroException())
        if n.IsZero then 0I, 1I
        else
            let sign = if d.Sign < 0 then -1I else 1I
            // Sign normalized numerator and denominator.
            let sn = n * sign
            let sd = d * sign
            let g = BigInteger.GreatestCommonDivisor(BigInteger.Abs sn, sd)
            // Normalized numerator and denominator.
            sn / g, sd / g            
    
    static member Create(num: int, den: int) =
        Rational.Create(bigint num, bigint den)
    
    static member Create(num: bigint, den: bigint) =
        let nn, dd = normalize num den
        Rational(nn, dd)
    
    static member FromInt32(x: int) = Rational.Create(x, 1)
    
    member x.Numerator = x.n
    member x.Denominator = x.d

    static member Zero = Rational(0I, 1I)
    static member One = Rational(1I, 1I)
    
    static member (+) (a: Rational, b: Rational) =
        Rational.Create(a.n * b.d + b.n * a.d, a.d * b.d)
    
    static member (-) (a: Rational, b: Rational) =
        Rational.Create(a.n * b.d - b.n * a.d, a.d * b.d)
    
    static member (*) (a: Rational, b: Rational) =
        Rational.Create(a.n * b.n, a.d * b.d)
        
    static member (/) (a: Rational, b: Rational) =
        if b.n.IsZero then raise (DivideByZeroException())
        Rational.Create(a.n * b.d, a.d * b.n)
    
    override x.Equals(obj) =
        match obj with
        | :? Rational as y -> (x :> IEquatable<Rational>).Equals(y)
        | _ -> false
        
    override x.GetHashCode() =
        HashCode.Combine(x.n, x.d)
        
    interface IEquatable<Rational> with
        member x.Equals(y: Rational) = x.n = y.n && x.d = y.d

    interface IComparable<Rational> with
        member x.CompareTo(y: Rational) =
            compare (x.n * y.d) (y.n * x.d)
            
    interface IComparable with
        member x.CompareTo(obj) =
            match obj with
            | null -> 1
            | :? Rational as y -> (x :> IComparable<Rational>).CompareTo(y)
            | _ -> invalidArg (nameof obj) "Argument must be Rational."
         
[<RequireQualifiedAccess>]
module Rational =
    let numerator (r: Rational) = r.Numerator
    let denominator (r: Rational) = r.Denominator
    let ofInt x = Rational.FromInt32(x)    
    let zero = Rational.Zero
    let one = Rational.One    
    let add (a: Rational) (b: Rational) = a + b
    let sub (a: Rational) (b: Rational) = a - b    
    let mul (a: Rational) (b: Rational) = a * b
    let div (a: Rational) (b: Rational) = a / b