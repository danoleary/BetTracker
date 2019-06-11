module LayBetSettledHandler

open Domain
open DomainHelpers

let private calculateWinnings (Stake stake) odds =
    let exposure = calculateExposure (Stake stake) odds
    Winnings (exposure + stake)

let applyLayBetSettled state (evt: CmdArgs.SettleLayBet) =
    ifNotEmpty
        state
        evt
        (fun bookie evt ->
            let bet: Bet = bookie.Bets
                        |> List.find (fun x -> x.Id = evt.BetId)
                        |> (fun t -> { t with State = Settled })
            let otherBets = List.filter (fun (x: Bet) -> x.Id <> bet.Id) bookie.Bets

            match evt.Result with
            | Win -> 
                Bookie { bookie with
                            Bets = bet :: otherBets;
                            Balance = addWinningsToBalance bookie.Balance (calculateWinnings bet.Stake bet.Odds) }
            | Lose -> 
                Bookie { bookie with Bets = bet :: otherBets;})      