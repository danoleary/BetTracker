module BackBetCashedOutHandler

open Domain
open DomainHelpers

let private addCashOutAmountToBalance (Balance balance) (CashOutAmount cashOutAmount) =
    Balance (balance + cashOutAmount)

let applyBackBetCashedOut (state: State) (evt: CmdArgs.CashOutBackBet): State =
    ifNotEmpty
        state
        evt
        (fun bookie evt -> 
                let bet: Bet = bookie.Bets
                            |> List.find (fun x -> x.Id = evt.BetId)
                            |> (fun t -> { t with State = CashedOut })
                            
                let otherBets = List.filter (fun (x: Bet) -> x.Id <> bet.Id) bookie.Bets

                Bookie { bookie with
                            Bets = bet :: otherBets;
                            Balance = addCashOutAmountToBalance bookie.Balance evt.CashOutAmount } )