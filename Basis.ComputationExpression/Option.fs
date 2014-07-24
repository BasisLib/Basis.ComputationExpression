namespace Basis.ComputationExpression

open System

type FlowControl = Break | Continue

module Option =
  module Simple =
    type OptionBuilder () =
      member this.Bind(x, f) = Option.bind f x
      member this.Return(x) = Some x
      member this.ReturnFrom(x: 'T option) = x

    let option = OptionBuilder ()

  module State =
    type OptionBuilder () =
      member this.Zero() = None, Continue
      member this.Return(x) = Some x, Break
      member this.ReturnFrom(x: 'T option) = x, Break
      member this.Bind(x: 'T option, f: 'T -> 'U option * FlowControl) = (Option.bind (f >> fst) x), Continue
      member this.Using(x: #IDisposable, f) =
        try f x
        finally match box x with null -> () | notNull -> x.Dispose()
      member this.Combine(m, cont) =
        match m with
        | x, Continue -> if Option.isSome x then x, Break else cont ()
        | other -> m
      member this.TryWith(f: unit -> 'T option * FlowControl, h) = try f () with e -> h e
      member this.TryFinally(f: unit -> 'T option * FlowControl, g) = try f () finally g ()
      member this.While(guard, f) =
        let isExit = ref false
        let res = ref None
        while guard () && not !isExit do
          match f () with
          | x, Break -> isExit := true; res := x
          | x, Continue -> if Option.isSome x then res := x
        if !isExit then !res, Break else !res, Continue
      member this.For(xs: #seq<'T>, f) =
        this.Using(
          xs.GetEnumerator(),
          fun itor -> this.While(itor.MoveNext, fun () -> f itor.Current))
      member this.Delay(f: 'T option * FlowControl) = f
      member this.Run(f: unit -> 'T option * FlowControl) = f () |> fst

    let option = OptionBuilder ()