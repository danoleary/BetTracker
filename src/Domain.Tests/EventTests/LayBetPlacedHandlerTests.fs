module LayBetPlacedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is decreased by exposure amount`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.01m)
    let args: CmdArgs.PlaceLayBet =
        { Id = id; Stake = Stake 100m; Odds = Odds 2.0m; BetId = (createNewBetId ())  } 
    let event = LayBetPlaced args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    Assert.Equal(Balance 0.01m, bookie.Balance)

[<Fact>]
let ``Bet is added to bets`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 101m)
    let args: CmdArgs.PlaceLayBet =
        { Id = id; Stake = Stake 100.99m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ()))  } 
    let event = LayBetPlaced args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    let bet = List.exactlyOne bookie.Bets
    Assert.Equal(args.BetId, bet.Id)
    Assert.Equal(args.Stake, bet.Stake)
    Assert.Equal(args.Odds, bet.Odds)