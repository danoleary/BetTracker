module TestHelpers

open System
open Domain
open Aggregate
open Domain.CmdArgs

let bookieCreatedState id =
    apply EmptyState (BookieAdded { BookieId = id; Name = "Some bookie" })

let depositMadeState id amount =
    let state = bookieCreatedState id
    let event = DepositMade { Transaction = { Timestamp = DateTime.UtcNow; Amount = amount } }
    apply state event

let backBetPlacedState bookieId betId deposit stake odds =
    let state = depositMadeState bookieId deposit
    let event = BackBetPlaced { Odds = odds; Stake = stake; BetId = betId }
    apply state event

let layBetPlacedState bookieId betId deposit stake odds =
    let state = depositMadeState bookieId deposit
    let event = LayBetPlaced { Odds = odds; Stake = stake; BetId = betId }
    apply state event

let createNewBookieId () = BookieId (Guid.NewGuid ())

let createNewBetId () = BetId (Guid.NewGuid ())
 


