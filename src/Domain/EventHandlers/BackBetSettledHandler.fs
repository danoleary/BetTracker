module BackBetSettledHandler

open Domain
open DomainHelpers

let private calculateWinnings (Stake stake) (Odds odds) =
    Winnings (stake * odds)

let private settle (evt: CmdArgs.SettleBackBet) bookie  =
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

let applyBackBetSettled state (evt: CmdArgs.SettleBackBet) =
    { state with Bookies = (updateBookie state.Bookies evt.Id (settle evt)) }