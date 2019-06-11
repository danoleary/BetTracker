module DepositMadeHandler

open Domain
open DomainHelpers

let applyDepositMade state (evt: CmdArgs.MakeDeposit) =
    ifNotEmpty
        state
        evt
        (fun bookie evt -> Bookie { bookie with Balance = addAmountToBalance bookie.Balance evt.Transaction.Amount })  