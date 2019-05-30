module BackBetSettledHandler

open Domain

type Winnings = Winnings of decimal

let private calculateWinnings stake odds =
    let x = match stake with | Stake stake -> stake
    let y = match odds with | Odds odds -> odds
    Winnings (x * y)

let private addWinningsToBalance balance winnings =
    let x = match balance with | Balance balance -> balance
    let y = match winnings with | Winnings winnings -> winnings
    Balance (x + y)

let applyBackBetSettled state (backBetSettled: CmdArgs.SettleBackBet) =
    let bookie: Bookie =
            state.Bookies
            |> List.find (fun x -> x.Id = backBetSettled.Id)
    let bet: Bet = bookie.Bets
                |> List.find (fun x -> x.Id = backBetSettled.BetId)
                |> (fun t -> { t with Settled = Settled })
    let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> backBetSettled.Id)
    let otherBets = List.filter (fun (x: Bet) -> x.Id <> bet.Id) bookie.Bets
    match backBetSettled.Result with
    | Win -> 
        let updatedBookie =
            { bookie with
                Bets = bet :: otherBets;
                Balance = addWinningsToBalance bookie.Balance (calculateWinnings bet.Stake bet.Odds) }
        { state with Bookies = updatedBookie :: otherBookies }
    | Lose -> 
        let updatedBookie = { bookie with Bets = bet :: otherBets;}
        { state with Bookies = updatedBookie :: otherBookies }