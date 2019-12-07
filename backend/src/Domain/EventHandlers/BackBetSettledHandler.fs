module BackBetSettledHandler

open Domain
open DomainHelpers

let private calculateWinnings (Stake stake) (Odds odds) =
    Winnings (stake * odds)

let applyBackBetSettled state (evt: CmdArgs.SettleBackBet) =
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
        