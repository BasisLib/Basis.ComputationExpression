module OptionTest

open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit

[<TestFixture>]
module SimpleTest =
  open Basis.ComputationExpression.Option.Simple

  [<Test>]
  let ``return x >>= f =  f x`` () =
    let f x = if x = 0 then None else Some (1000 / x)
    check (fun x -> option.Bind(option.Return(x), f) = f x)
    check (fun x -> option { let! y = option.Return(x) in return! f y } = f x)

  [<Test>]
  let ``m >>= return  =  m`` () =
    check (fun m -> option.Bind(m, option.Return) = m)
    check (fun m -> option { let! x = m in return x } = m)

  [<Test>]
  let ``(m >>= f) >>= g  =  m >>= (fun x -> f x >>= g)`` () =
    let f x = if x = 0 then None else Some (1000 / x)
    let g x = if x < 10 then None else Some (x / 10)
    check (fun m -> option.Bind(option.Bind(m, f), g) = option.Bind(m, fun x -> option.Bind(f x, g)))
    check (fun m ->
      option {
        let! y =
          option {
            let! x = m
            return! f x
          }
        return! g y
      } = option {
        let! x = m
        let! y = f x
        return! g y
      }
    )