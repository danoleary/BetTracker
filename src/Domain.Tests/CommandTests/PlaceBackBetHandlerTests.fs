module PlaceBackBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Back bet placed is raised if the total balance is greater than the stake`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.00m)
    let placeBackBet: CmdArgs.PlaceBackBet =
        { Stake = Stake 100m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ())) } 
    let command = PlaceBackBet placeBackBet

    let result: Event = execute state command

    match result with
    | BackBetPlaced args ->
        Assert.True(args.BetId = placeBackBet.BetId)
        Assert.True(args.Odds = placeBackBet.Odds)
        Assert.True(args.Stake = placeBackBet.Stake)
    | _ -> failwith "incorrect event"

[<Fact>]
let ``No events are raised if the total balance is less than the stake`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.00m)
    let placeBackBet: CmdArgs.PlaceBackBet =
        { Stake = Stake 100.01m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ()))  } 
    let command = PlaceBackBet placeBackBet

    let methodCall = (fun () -> (execute state command) |> ignore)

    Assert.Throws<Exception>(methodCall)