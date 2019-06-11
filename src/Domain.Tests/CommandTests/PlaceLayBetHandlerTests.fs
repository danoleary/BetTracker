module PlaceLayBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Lay bet placed is raised if the total balance is greater than the exposure`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 100.00m)
    let placeLayBet: CmdArgs.PlaceLayBet =
        { Stake = Stake 100m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ())) } 
    let command = PlaceLayBet placeLayBet

    let result: Event = execute state command

    match result with
    | LayBetPlaced args ->
        Assert.True(args.BetId = placeLayBet.BetId)
        Assert.True(args.Odds = placeLayBet.Odds)
        Assert.True(args.Stake = placeLayBet.Stake)
    | _ -> failwith "incorrect event"

[<Fact>]
let ``No events are raised if the total balance is less than the exposure`` () =
    let id = createNewBookieId ()
    let state = depositMadeState id (TransactionAmount 99.99m)
    let placeLayBet: CmdArgs.PlaceLayBet =
        { Stake = Stake 100.0m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ()))  } 
    let command = PlaceLayBet placeLayBet

    let methodCall = (fun () -> (execute state command) |> ignore)

    Assert.Throws<Exception>(methodCall)