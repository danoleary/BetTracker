module Dtos

open System
open Domain

[<AbstractClass>]
type CommandDto(aggregateId: Guid, timestamp: DateTime) = 
    member x.AggregateId = aggregateId
    member x.Timestamp = timestamp

type AddBookieDto(aggregateId, timestamp, name: string) =
    inherit CommandDto(aggregateId, timestamp)
    member x.Name = name
    
type MakeDepositDto(aggregateId, timestamp, amount: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.Amount = amount

type MakeWithdrawalDto(aggregateId, timestamp, amount: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.Amount = amount

type PlaceBackBetDto(aggregateId, timestamp, betId: Guid, stake: decimal, odds: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds

type PlaceFreeBetDto(aggregateId, timestamp, betId: Guid, stake: decimal, odds: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds

type PlaceLayBetDto(aggregateId, timestamp, betId: Guid, stake: decimal, odds: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Stake = stake
    member x.Odds = odds

type SettleBackBetDto(aggregateId, timestamp, betId: Guid, result: BetResult) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Result = result

type SettleFreeBetDto(aggregateId, timestamp, betId: Guid, result: BetResult) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Result = result

type SettleLayBetDto(aggregateId, timestamp, betId: Guid, result: BetResult) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Result = result

type CashOutBackBetDto(aggregateId, timestamp, betId: Guid, amount: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.BetId = betId
    member x.Amount = amount

type CreditBonusDto(aggregateId, timestamp, amount: decimal) =
    inherit CommandDto(aggregateId, timestamp)
    member x.Amount = amount