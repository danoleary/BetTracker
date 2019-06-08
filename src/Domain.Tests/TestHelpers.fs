module TestHelpers

open System
open Domain
open Aggregate
open Domain.CmdArgs

let bookieCreatedState id =
    let state: State = { Bookies = [] }
    let event = BookieAdded { Id = id; Name = "Some bookie" }
    apply state event

let depositMadeState id amount =
    let state = bookieCreatedState id
    let event = DepositMade { Id = id; Transaction = { Timestamp = DateTime.UtcNow; Amount = amount } }
    apply state event

let backBetPlacedState bookieId betId deposit stake odds =
    let state = depositMadeState bookieId deposit
    let event = BackBetPlaced { Id = bookieId; Odds = odds; Stake = stake; BetId = betId }
    apply state event

let layBetPlacedState bookieId betId deposit stake odds =
    let state = depositMadeState bookieId deposit
    let event = LayBetPlaced { Id = bookieId; Odds = odds; Stake = stake; BetId = betId }
    apply state event

let createNewBookieId () = BookieId (Guid.NewGuid ())

let createNewBetId () = BetId (Guid.NewGuid ())
 


