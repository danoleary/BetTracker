module BackBetPlacedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is decreased by transaction amount`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.PlaceBackBet =
        { Stake = Stake 100.99m; Odds = Odds 2.0m; EventDescription = EventDescription "some event"; BetId = (createNewBetId ())  } 
    let event = BackBetPlaced args

    let result = apply state event

    match result with
    | Bookie bookie -> Assert.Equal(Balance 0.01m, bookie.Balance)
    | _ -> failwith "failed"

[<Fact>]
let ``Bet is added to bets`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.PlaceBackBet =
        { Stake = Stake 100.99m; Odds = Odds 2.0m; EventDescription = EventDescription "some event"; BetId = (BetId (Guid.NewGuid ()))  } 
    let event = BackBetPlaced args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(args.BetId, bet.Id)
        Assert.Equal(args.Stake, bet.Stake)
        Assert.Equal(args.Odds, bet.Odds)
    | _ -> failwith "failed"