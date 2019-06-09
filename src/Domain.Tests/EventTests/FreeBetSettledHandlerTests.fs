module FreeBetSettledHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is increased by stake times odds and bet state is settled if result is won`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.SettleFreeBet =
        { Id = bookieId; BetId = betId; Result = Win } 
    let event = FreeBetSettled args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    let bet = List.exactlyOne bookie.Bets
    Assert.Equal(Balance 50m, bookie.Balance)
    Assert.Equal(Settled, bet.State)
    

[<Fact>]
let ``Balance doesnt change and bet state is settled if result is lost`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.SettleFreeBet =
        { Id = bookieId; BetId = betId; Result = Lose } 
    let event = FreeBetSettled args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    let bet = List.exactlyOne bookie.Bets
    Assert.Equal(Balance 0m, bookie.Balance)
    Assert.Equal(Settled, bet.State)