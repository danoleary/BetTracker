module LayBetSettledHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Balance is increased by stake plus exposure bet state is settled if result is won`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = layBetPlacedState bookieId betId (TransactionAmount 100.01m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.SettleLayBet =
        { BetId = betId; Result = Win } 
    let event = LayBetSettled args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(Balance 150.01m, bookie.Balance)
        Assert.Equal(Settled, bet.State)
    | _ -> failwith "failed"
    
[<Fact>]
let ``Balance doesnt change and bet state is settled if result is lost`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = layBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.SettleLayBet =
        { BetId = betId; Result = Lose } 
    let event = LayBetSettled args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(Balance 0m, bookie.Balance)
        Assert.Equal(Settled, bet.State)
    | _ -> failwith "failed"
    