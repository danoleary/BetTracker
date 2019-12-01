module DomainHelpers

open Domain
open System

let onlyIfBookieExists state cmd =
    match state with
    | Bookie b -> Ok b
    | EmptyState -> Error BookieDoesNotExistError

let ifNotEmpty state evt func: State =
        match state with
        | Bookie bookie -> func bookie evt
        | EmptyState -> failwith "state is empty"

let onlyIfBalanceIsHighEnoughForWithdrawal (TransactionAmount amount) bookie  =
    let (Balance balance) = bookie.Balance
    if balance >= amount then
        Ok bookie
    else Error (BalanceNotHighEnoughError balance)

let onlyIfBalanceIsHighEnoughForStake (Stake stake)  bookie  =
    let (Balance balance) = bookie.Balance
    if balance >= stake
        then Ok bookie
        else Error (BalanceNotHighEnoughError balance)

let onlyIfThereIsAMatchingBet (betId: BetId) (bookie: Bookie) =
    if List.exists (fun (x: Bet) -> x.Id = betId) bookie.Bets then
        Ok bookie
    else 
        Error NoMatchingBetError

let subtractStakeFromBalance (Balance balance) (Stake stake) =
    Balance (balance - stake)

let subtractAmountFromBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance - amount)

let addWinningsToBalance (Balance balance) (Winnings winnings) =
    Balance (balance + winnings)

let addAmountToBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance + amount)

let addAmountToTotal (TransactionAmount total) (TransactionAmount amount) =
    TransactionAmount (total + amount)

let calculateExposure (Stake stake) (Odds odds) =
    Math.Round(stake * (odds - 1m), 2)