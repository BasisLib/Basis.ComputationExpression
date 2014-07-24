namespace Basis.ComputationExpression

module Option =
  module Simple =
    type OptionBuilder () =
      member this.Bind(x, f) = Option.bind f x
      member this.Return(x) = Some x
      member this.ReturnFrom(x: 'T option) = x

    let option = OptionBuilder ()