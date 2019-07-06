module SettleFreeBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``no events raised if there is no matching free bet`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let settleFreeBet: CmdArgs.SettleFreeBet =
        { Result = Win; BetId = (BetId (Guid.NewGuid())) } 
    let command = SettleFreeBet settleFreeBet

    let result = execute state command

    match result with
    | Error (NoMatchingBetError _) -> Assert.True(true)
    | _ -> failwith "execution didnt error"

[<Fact>]
let ``free bet settled raised if there is a matching back bet`` () =
    let bookieId = createNewBookieId ()
    let betId = createNewBetId ()
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let settleFreeBet: CmdArgs.SettleFreeBet =
        { Result = Win; BetId = betId } 
    let command = SettleFreeBet settleFreeBet

    let result = execute state command

    match result with
    | Ok (FreeBetSettled args) ->
        Assert.True(args.BetId = settleFreeBet.BetId)
        Assert.True(args.Result = settleFreeBet.Result)
    | _ -> failwith "incorrect event"