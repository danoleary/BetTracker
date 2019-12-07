module SettleLayBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``no events raised if there is no matching lay bet`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let settleLayBet: CmdArgs.SettleLayBet =
        { Result = Win; BetId = (BetId (Guid.NewGuid())) } 
    let command = SettleLayBet settleLayBet

    let result = execute state command

    match result with
    | Error (NoMatchingBetError _) -> Assert.True(true)
    | _ -> failwith "execution didnt error"

[<Fact>]
let ``lay bet settled raised if there is a matching lay bet`` () =
    let bookieId = createNewBookieId ()
    let betId = createNewBetId ()
    let state = layBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let settleLayBet: CmdArgs.SettleLayBet =
        { Result = Win; BetId = betId } 
    let command = SettleLayBet settleLayBet

    let result = execute state command

    match result with
    | Ok (LayBetSettled args) ->
        Assert.True(args.BetId = settleLayBet.BetId)
        Assert.True(args.Result = settleLayBet.Result)
    | _ -> failwith "incorrect event"