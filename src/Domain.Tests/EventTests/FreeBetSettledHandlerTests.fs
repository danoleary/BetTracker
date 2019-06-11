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
    let args: CmdArgs.SettleFreeBet = { BetId = betId; Result = Win } 
    let event = FreeBetSettled args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(Balance 50m, bookie.Balance)
        Assert.Equal(Settled, bet.State)
    | _ -> failwith "failed"

[<Fact>]
let ``Balance doesnt change and bet state is settled if result is lost`` () =
    let bookieId = createNewBookieId ()
    let betId = BetId (Guid.NewGuid ())
    let state = backBetPlacedState bookieId betId (TransactionAmount 50m) (Stake 50m) (Odds 2m)
    let args: CmdArgs.SettleFreeBet = { BetId = betId; Result = Lose } 
    let event = FreeBetSettled args

    let result = apply state event

    match result with
    | Bookie bookie ->
        let bet = List.exactlyOne bookie.Bets
        Assert.Equal(Settled, bet.State)
        Assert.Equal(Balance 0m, bookie.Balance)
    | _ -> failwith "failed"
    