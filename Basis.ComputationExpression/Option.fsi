namespace Basis.ComputationExpression

module Option =
  module Simple =
    type OptionBuilder = class
      new: unit -> OptionBuilder
      member Bind: 'T option * ('T -> 'U option) -> 'U option
      member Return: 'T -> 'T option
      member ReturnFrom: 'T option -> 'T option
    end

    val option: OptionBuilder