module LayBetSettledHandler

open Domain
open DomainHelpers

let private calculateWinnings (Stake stake) odds =
    let exposure = calculateExposure (Stake stake) odds
    Winnings (exposure + stake)

let private settle (evt: CmdArgs.SettleLayBet) bookie  =
    let bet: Bet = bookie.Bets
                |> List.find (fun x -> x.Id = evt.BetId)
                |> (fun t -> { t with State = Settled })
    let otherBets = List.filter (fun (x: Bet) -> x.Id <> bet.Id) bookie.Bets

    match evt.Result with
    | Win -> 
        { bookie with
                Bets = bet :: otherBets;
                Balance = addWinningsToBalance bookie.Balance (calculateWinnings bet.Stake bet.Odds) }
    | Lose -> 
        { bookie with Bets = bet :: otherBets;}

let applyLayBetSettled state (evt: CmdArgs.SettleLayBet) =
    { state with Bookies = (updateBookie state.Bookies evt.Id (settle evt)) }