module DepositMadeHandler

open Domain

let private addAmountToBalance balance amount =
    let x = match balance with | Balance b -> b
    let y = match amount with | TransactionAmount amount -> amount
    Balance (x + y)

let applyDepositMade state (depositMade: CmdArgs.MakeDeposit) =
    let bookie = state.Bookies
                    |> List.find (fun x -> x.Id = depositMade.Id)
                    |> (fun t -> { t with Balance = addAmountToBalance t.Balance depositMade.Transaction.Amount })
    let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> depositMade.Id)
    { state with Bookies = bookie :: otherBookies }