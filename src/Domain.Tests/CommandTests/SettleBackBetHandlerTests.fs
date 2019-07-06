module SettleBackBetHandlerTests

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
    let settleBackBet: CmdArgs.SettleBackBet =
        { Result = Win; BetId = (BetId (Guid.NewGuid())) } 
    let command = SettleBackBet settleBackBet

    let result = execute state command

    match result with
    | Error (NoMatchingBetError _) -> Assert.True(true)
    | _ -> failwith "execution didnt error"

[<Fact>]
let ``back bet settled raised if there is a matching back bet`` () =
    let bookieId = createNewBookieId ()
    let betId = createNewBetId ()
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let settleBackBet: CmdArgs.SettleBackBet =
        { Result = Win; BetId = betId } 
    let command = SettleBackBet settleBackBet

    let result = execute state command

    match result with
    | Ok (BackBetSettled args) ->
        Assert.True(args.BetId = settleBackBet.BetId)
        Assert.True(args.Result = settleBackBet.Result)
    | _ -> failwith "incorrect event"