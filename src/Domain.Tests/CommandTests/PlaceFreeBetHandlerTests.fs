module PlaceFreeBetHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Free bet placed is raised`` () =
    let id = createNewBookieId ()
    let state = bookieCreatedState id
    let placeFreeBet: CmdArgs.PlaceFreeBet =
        { Stake = Stake 100m; Odds = Odds 2.0m; BetId = (BetId (Guid.NewGuid ()))  } 
    let command = PlaceFreeBet placeFreeBet

    let result = execute state command

    match result with
    | Ok (FreeBetPlaced args) ->
        Assert.True(args.BetId = placeFreeBet.BetId)
        Assert.True(args.Odds = placeFreeBet.Odds)
        Assert.True(args.Stake = placeFreeBet.Stake)
    | _ -> failwith "incorrect event"