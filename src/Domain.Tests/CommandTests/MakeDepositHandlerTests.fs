module MakeDepositHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``No event is raised if make deposit is handled before add bookie`` () =
    let state: State = EmptyState
    let args: CmdArgs.MakeDeposit =
        { Transaction = TransactionAmount 100.0m }
    let command = MakeDeposit args

    let result = execute state command

    match result with
    | Error BookieDoesNotExistError -> Assert.True(true)
    | _ -> failwith "execution didnt error"

[<Fact>]
let ``Deposit made is raised if deposit is made after bookie created`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let makeDeposit: CmdArgs.MakeDeposit =
        { Transaction = TransactionAmount 100.0m } 
    let command = MakeDeposit makeDeposit

    let result = execute state command

    match result with
    | Ok (DepositMade args) -> Assert.True(args.Transaction = makeDeposit.Transaction)
    | _ -> failwith "incorrect event"