module DepositMadeHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

let private amountAndBalanceAreEqual balance amount =
    let x = match balance with | Balance balance -> balance
    let y = match amount with | TransactionAmount amount -> amount
    x = y

[<Fact>]
let ``Balance is increased by transaction amount after deposit made event is applied`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let args: CmdArgs.MakeDeposit =
        { Transaction = { Timestamp = DateTime.Now; Amount = TransactionAmount 100.0m }}
    let event = DepositMade args

    let result = apply state event

    match result with
    | Bookie bookie ->
            Assert.True(amountAndBalanceAreEqual bookie.Balance args.Transaction.Amount)
    | _ -> failwith "failed"