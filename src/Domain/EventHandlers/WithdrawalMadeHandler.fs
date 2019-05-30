module WithdrawalMadeHandler

open Domain

let private subtractAmountFromBalance balance amount =
    let x = match balance with | Balance b -> b
    let y = match amount with | TransactionAmount amount -> amount
    Balance (x - y)

let applyWithdrawlMade state (withdrawalMade: CmdArgs.MakeWithdrawal) =
    let bookie = state.Bookies
                    |> List.find (fun x -> x.Id = withdrawalMade.Id)
                    |> (fun t -> { t with Balance = subtractAmountFromBalance t.Balance withdrawalMade.Transaction.Amount })
    let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> withdrawalMade.Id)
    { state with Bookies = bookie :: otherBookies }