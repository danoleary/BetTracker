module DomainHelpers

open Domain

let onlyIfBookieExists state i =
    match state with
    | Bookie b -> b
    | EmptyState -> failwith "Bookie does not exist"

let ifNotEmpty state evt func: State =
        match state with
        | Bookie bookie -> func bookie evt
        | EmptyState -> failwith "state is empty"

let onlyIfBalanceIsHighEnoughForWithdrawal (TransactionAmount amount) bookie  =
    if bookie.Balance >= Balance amount then
        bookie
    else failwith "Can't make withdrawl, balance not high enough"

let onlyIfBalanceIsHighEnoughForStake (Stake stake) (BetId betId) bookie  =
    
    if bookie.Balance >= Balance stake
        then bookie
        else failwith "Can't place back bet"

let onlyIfThereIsAMatchingBet (betId: BetId) (bookie: Bookie) =
    if List.exists (fun (x: Bet) -> x.Id = betId) bookie.Bets then
        bookie
    else 
        failwith "Can't find matching bet"

let subtractStakeFromBalance (Balance balance) (Stake stake) =
    Balance (balance - stake)

let subtractAmountFromBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance - amount)

let addWinningsToBalance (Balance balance) (Winnings winnings) =
    Balance (balance + winnings)

let addAmountToBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance + amount)

let calculateExposure (Stake stake) (Odds odds) =
    stake * (odds - 1m)