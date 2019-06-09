module BackBetCashedOutHandler

open Domain
open DomainHelpers

let private addCashOutAmountToBalance (Balance balance) (CashOutAmount cashOutAmount) =
    Balance (balance + cashOutAmount)

let private settle (evt: CmdArgs.CashOutBackBet) bookie  =
    let bet: Bet = bookie.Bets
                |> List.find (fun x -> x.Id = evt.BetId)
                |> (fun t -> { t with State = CashedOut })
                
    let otherBets = List.filter (fun (x: Bet) -> x.Id <> bet.Id) bookie.Bets

    { bookie with
                Bets = bet :: otherBets;
                Balance = addCashOutAmountToBalance bookie.Balance evt.CashOutAmount }

let applyBackBetCashedOut state (evt: CmdArgs.CashOutBackBet) =
    { state with Bookies = (updateBookie state.Bookies evt.Id (settle evt)) }