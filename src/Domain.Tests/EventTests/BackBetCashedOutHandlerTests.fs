module BackBetCashedOutHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is increased by cashout amount and bet state is cashed out`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.CashOutBackBet =
        { Id = bookieId; BetId = betId; CashOutAmount = CashOutAmount 45.0m } 
    let event = BackBetCashedOut args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    let bet = List.exactlyOne bookie.Bets
    Assert.Equal(Balance 45m, bookie.Balance)
    Assert.Equal(CashedOut, bet.State)