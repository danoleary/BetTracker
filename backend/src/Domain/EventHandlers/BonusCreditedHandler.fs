module BonusCreditedHandler

open Domain
open DomainHelpers

let applyBonusCredited state (evt: CmdArgs.CreditBonus) =
    ifNotEmpty
        state
        evt
        (fun bookie evt -> Bookie { bookie with Balance = addAmountToBalance bookie.Balance evt.Amount })  