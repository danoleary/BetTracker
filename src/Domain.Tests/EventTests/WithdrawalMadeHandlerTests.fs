module WithdrawalMadeHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is decreased by transaction amount after withdrawl made event is applied`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.MakeWithdrawal =
        { Id = id; Transaction = { Timestamp = DateTime.Now; Amount = TransactionAmount 100.0m }}
    let event = WithdrawalMade args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    Assert.Equal(Balance 1m, bookie.Balance)