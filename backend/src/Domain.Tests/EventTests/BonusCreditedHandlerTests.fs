module BonusCreditedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is increased by bonus amount after event is applied`` () =
    let state: State = bookieCreatedState (createNewBookieId () )
    let creditBonus: CmdArgs.CreditBonus  = { Amount = TransactionAmount 10m }
    let event = BonusCredited creditBonus

    let result = apply state event

    match result with
    | Bookie bookie ->
            let (TransactionAmount amount) = creditBonus.Amount
            let (Balance balance) = bookie.Balance
            Assert.Equal(amount, balance)
    | _ -> failwith "failed"
    