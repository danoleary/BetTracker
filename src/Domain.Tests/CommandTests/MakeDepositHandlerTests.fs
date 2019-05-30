module MakeDepositHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``No event is raised if make deposit is handled before add bookie`` () =
    let state: State = { Bookies = [] }
    let args: CmdArgs.MakeDeposit =
        { Id = createNewBookieId (); Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount 100.0m } } 
    let command = MakeDeposit args

    let methodCall = (fun () -> (execute state command) |> ignore)

    Assert.Throws<Exception>(methodCall)

[<Fact>]
let ``Deposit made is raised if deposit is made after bookie created`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let makeDeposit: CmdArgs.MakeDeposit =
        { Id = id; Transaction = { Timestamp = DateTime.UtcNow; Amount = TransactionAmount 100.0m } } 
    let command = MakeDeposit makeDeposit

    let result: Event = execute state command

    match result with
    | DepositMade args -> Assert.True(args.Transaction = makeDeposit.Transaction)
    | _ -> failwith "incorrect event"