module DomainHelpers

open Domain

let onlyIfBookieExists state i =
    printf "%A" i
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some bookie -> bookie
    | None -> failwith "Bookie does not exist"

let onlyIfBalanceIsHighEnoughForWithdrawal (TransactionAmount amount) bookie  =
    if bookie.Balance >= Balance amount then
        bookie
    else failwith "Can't make withdrawl, balance not high enough"

let onlyIfBalanceIsHighEnoughForStake (Stake stake) bookie  =
    if bookie.Balance >= Balance stake then bookie else failwith "Can't place back bet"

let onlyIfThereIsAMatchingBet (betId: BetId) (bookie: Bookie) =
    if List.exists (fun (x: Bet) -> x.Id = betId) bookie.Bets then
        bookie
    else 
        printfn "Failing bet: %A" betId
        failwith "Can't find matching bet"

let subtractStakeFromBalance (Balance balance) (Stake stake) =
    Balance (balance - stake)

let subtractAmountFromBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance - amount)

let addWinningsToBalance (Balance balance) (Winnings winnings) =
    Balance (balance + winnings)

let addAmountToBalance (Balance balance) (TransactionAmount amount) =
    Balance (balance + amount)

let updateBookie bookies bookieId updateFunc =
    let bookie = bookies
                |> List.find (fun x -> x.Id = bookieId)
                |> updateFunc
    let otherBookies = bookies |> List.filter (fun x -> x.Id <> bookieId)
    bookie :: otherBookies

let calculateExposure (Stake stake) (Odds odds) =
    stake * (odds - 1m)