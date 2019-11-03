module EventDtos

open System
open Domain

type EventType =
    | Empty
    | BookieAdded
    | DepositMade
    | WithdrawalMade
    | BackBetPlaced
    | BackBetSettled
    | FreeBetPlaced
    | FreBetSettled
    | LayBetPlaced
    | LayBetSettled
    | BackBetCashedOut
    | BonusCredited

[<AbstractClass>]
type EventDto(timestamp: DateTime, bookieId: BookieId) =
    member x.BookieId = bookieId
    member x.Timestamp = timestamp
    member x.Type = Empty

type BookieAddedDto(timestamp, bookieId, name: string) =
    inherit EventDto(timestamp, bookieId)
    member x.Name = name
    member x.Type = BookieAdded

type DepositMadeDto(timestamp, bookieId, amount: decimal) =
    inherit EventDto(timestamp, bookieId)
    member x.Amount = amount
    member x.Type = DepositMade

type WithdrawalMadeDto(timestamp, bookieId, amount: decimal) =
    inherit EventDto(timestamp, bookieId)
    member x.Amount = amount
    member x.Type = WithdrawalMade

type BackBetPlacedDto(timestamp, bookieId, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = BackBetPlaced

type FreeBetPlacedDto(timestamp, bookieId, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = FreeBetPlaced

type LayBetPlacedDto(timestamp, bookieId, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = LayBetPlaced

type BackBetSettledDto(timestamp, bookieId, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Result = result
    member x.Type = BackBetSettled

type FreeBetSettledDto(timestamp, bookieId, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Result = result
    member x.Type = FreeBetSettled

type LayBetSettledDto(timestamp, bookieId, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Result = result
    member x.Type = LayBetSettled

type BackBetCashedOutDto(timestamp, bookieId, betId: Guid, amount: decimal) =
    inherit EventDto(timestamp, bookieId)
    member x.BetId = betId
    member x.Amount = amount
    member x.Type = BackBetCashedOut

type BonusCreditedDto(timestamp, bookieId, amount: decimal) =
    inherit EventDto(timestamp, bookieId)
    member x.Amount = amount
    member x.Type = BonusCredited