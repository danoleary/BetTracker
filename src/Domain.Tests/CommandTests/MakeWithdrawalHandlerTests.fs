module MakeWithdrawalHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Withdrawal made is raised if withdrawal is the total balance is equal to the withdrawal amount`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100m)
    let makeWithdrawal: CmdArgs.MakeWithdrawal =
        { Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount 100m } } 
    let command = MakeWithdrawal makeWithdrawal

    let result: Event = execute state command

    match result with
    | WithdrawalMade args -> Assert.True(args.Transaction = makeWithdrawal.Transaction)
    | _ -> failwith "incorrect event"

[<Fact>]
let ``Withdrawal made is raised if withdrawal is the total balance is greater than the withdrawal amount`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100m)
    let makeWithdrawal: CmdArgs.MakeWithdrawal =
        { Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount 99.99m } } 
    let command = MakeWithdrawal makeWithdrawal

    let result: Event = execute state command

    match result with
    | WithdrawalMade args -> Assert.True(args.Transaction = makeWithdrawal.Transaction)
    | _ -> failwith "incorrect event"

[<Fact>]
let ``No events are raised if withdrawal is the total balance is less than the withdrawal amount`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.00m)
    let makeWithdrawal: CmdArgs.MakeWithdrawal =
        { Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount 100.01m } } 
    let command = MakeWithdrawal makeWithdrawal

    let methodCall = (fun () -> (execute state command) |> ignore)

    Assert.Throws<Exception>(methodCall)