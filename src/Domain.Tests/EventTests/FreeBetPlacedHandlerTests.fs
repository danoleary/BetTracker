module FreeBetPlacedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance doesnt change after free bet placed event is applied`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.PlaceFreeBet =
        { Stake = Stake 100.99m; Odds = Odds 2.0m; BetId = createNewBetId ()  } 
    let event = FreeBetPlaced args

    let result = apply state event

    match result with
    | Bookie bookie ->
            Assert.Equal(Balance 101m, bookie.Balance)
    | _ -> failwith "failed"

[<Fact>]
let ``Bet is added to bets`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.PlaceFreeBet =
        { Stake = Stake 100.99m; Odds = Odds 2.0m; BetId = createNewBetId ()  } 
    let event = FreeBetPlaced args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(args.BetId, bet.Id)
        Assert.Equal(args.Stake, bet.Stake)
        Assert.Equal(args.Odds, bet.Odds)
    | _ -> failwith "failed"

