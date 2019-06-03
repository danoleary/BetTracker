module DepositMadeHandler

open Domain
open DomainHelpers

let applyDepositMade state (evt: CmdArgs.MakeDeposit) =
    let updateFunc =
        (fun t -> { t with Balance = addAmountToBalance t.Balance evt.Transaction.Amount })
    { state with Bookies = (updateBookie state.Bookies evt.Id updateFunc)}