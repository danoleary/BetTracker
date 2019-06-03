module WithdrawalMadeHandler

open Domain
open DomainHelpers

let applyWithdrawlMade state (evt: CmdArgs.MakeWithdrawal) =
    let updateFunc =
        (fun t -> { t with Balance = subtractAmountFromBalance t.Balance evt.Transaction.Amount })
    { state with Bookies = (updateBookie state.Bookies evt.Id updateFunc)}