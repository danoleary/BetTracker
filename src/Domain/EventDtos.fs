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
type EventDto(timestamp: DateTime) = 
    member x.Timestamp = timestamp
    member x.Type = Empty

type BookieAddedDto(timestamp, name: string) =
    inherit EventDto(timestamp)
    member x.Name = name
    member x.Type = BookieAdded
    
type DepositMadeDto(timestamp, amount: decimal) =
    inherit EventDto(timestamp)
    member x.Amount = amount
    member x.Type = DepositMade

type WithdrawalMadeDto(timestamp, amount: decimal) =
    inherit EventDto(timestamp)
    member x.Amount = amount
    member x.Type = WithdrawalMade

type BackBetPlacedDto(timestamp, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = BackBetPlaced

type FreeBetPlacedDto(timestamp, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = FreeBetPlaced

type LayBetPlacedDto(timestamp, betId: Guid, stake: decimal, odds: decimal, event: string) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds
    member x.Event = event
    member x.Type = LayBetPlaced

type BackBetSettledDto(timestamp, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Result = result
    member x.Type = BackBetSettled

type FreeBetSettledDto(timestamp, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Result = result
    member x.Type = FreeBetSettled

type LayBetSettledDto(timestamp, betId: Guid, result: BetResult) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Result = result
    member x.Type = LayBetSettled

type BackBetCashedOutDto(timestamp, betId: Guid, amount: decimal) =
    inherit EventDto(timestamp)
    member x.BetId = betId
    member x.Amount = amount
    member x.Type = BackBetCashedOut

type BonusCreditedDto(timestamp, amount: decimal) =
    inherit EventDto(timestamp)
    member x.Amount = amount
    member x.Type = BonusCredited