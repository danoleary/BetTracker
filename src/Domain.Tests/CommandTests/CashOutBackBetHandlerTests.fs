module CashOutBackBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``no events raised if there is no matching back bet`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.00m)
    let cashOutBackBet: CmdArgs.CashOutBackBet =
        { CashOutAmount = CashOutAmount 97.0m; BetId = (BetId (Guid.NewGuid())) } 
    let command = CashOutBackBet cashOutBackBet

    let methodCall = (fun () -> (execute state command) |> ignore)

    Assert.Throws<Exception>(methodCall)

[<Fact>]
let ``back bet cashed out raised if there is a matching back bet`` () =
    let bookieId = createNewBookieId ()
    let betId = createNewBetId ()
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let cashOutBackBet: CmdArgs.CashOutBackBet =
        { CashOutAmount = CashOutAmount 45.0m; BetId = betId } 
    let command = CashOutBackBet cashOutBackBet

    let result: Event = execute state command

    match result with
    | BackBetCashedOut args ->
        Assert.True(args.BetId = cashOutBackBet.BetId)
        Assert.True(args.CashOutAmount = cashOutBackBet.CashOutAmount)
    | _ -> failwith "incorrect event"