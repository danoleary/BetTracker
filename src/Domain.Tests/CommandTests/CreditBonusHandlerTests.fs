module CreditBonusHandlerTests

open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Bonus credited event is raised if bookie exists`` () =
    let state: State = bookieCreatedState (createNewBookieId () )
    let creditBonus: CmdArgs.CreditBonus  = { Amount = TransactionAmount 10m }
    let command = CreditBonus creditBonus

    let result: Event = execute state command

    match result with
    | BonusCredited args -> Assert.True(args.Amount = creditBonus.Amount)
    | _ -> failwith "incorrect event"