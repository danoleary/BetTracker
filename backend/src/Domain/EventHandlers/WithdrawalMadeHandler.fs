module WithdrawalMadeHandler

open Domain
open DomainHelpers

let applyWithdrawlMade state (evt: CmdArgs.MakeWithdrawal) =
    ifNotEmpty
        state
        evt
        (fun bookie evt ->
            Bookie { bookie with
                        Balance = subtractAmountFromBalance bookie.Balance evt.Amount;
                        TotalWithdrawals = addAmountToTotal bookie.TotalWithdrawals evt.Amount })  