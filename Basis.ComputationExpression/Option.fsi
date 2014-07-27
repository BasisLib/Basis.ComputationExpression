namespace Basis.ComputationExpression

open System

type FlowControl

module Option =
  module Simple =
    type OptionBuilder = class
      new: unit -> OptionBuilder
      member Bind: 'T option * ('T -> 'U option) -> 'U option
      member Return: 'T -> 'T option
      member ReturnFrom: 'T option -> 'T option
    end

    val option: OptionBuilder

  module State =
    type OptionBuilder = class
      new: unit -> OptionBuilder
      member Zero: unit -> 'T option * FlowControl
      member Return: 'T -> 'T option * FlowControl
      member ReturnFrom: 'T option -> 'T option * FlowControl
      member Bind: 'T option * ('T -> 'U option * FlowControl) -> 'U option * FlowControl
      member Using<'T, 'U when 'T :> IDisposable> : 'T * ('T -> 'U) -> 'U
      member Combine: ('T option * FlowControl) * (unit -> 'T option * FlowControl) -> 'T option * FlowControl
      member TryWith: (unit -> 'T option * FlowControl) * (exn -> 'T option * FlowControl) -> 'T option * FlowControl
      member TryFinally: (unit -> 'T option * FlowControl) * (unit -> unit) -> 'T option * FlowControl
      member While: (unit -> bool) * (unit -> 'T option * FlowControl) -> 'T option * FlowControl
      member For: #seq<'T> * ('T -> 'U option * FlowControl) -> 'U option * FlowControl
      member Delay: (unit -> 'T option * FlowControl) -> (unit -> 'T option * FlowControl)
      member Run: (unit -> 'T option * FlowControl) -> 'T option
    end

    val option: OptionBuilder