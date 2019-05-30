module MakeWithdrawalHandler

open Domain

let private onlyIfBookieExists state i =
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some task -> task
    | None -> failwith "Task does not exist"

let private onlyIfBalanceIsHighEnough withdrawalAmount bookie  =
    let amount = match withdrawalAmount with | TransactionAmount args -> args
    if bookie.Balance >= Balance amount then bookie else failwith "Can't make withdrawl"

let handleMakeWithdrawal state (makeWithdrawal: CmdArgs.MakeWithdrawal) =
    makeWithdrawal.Id
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnough makeWithdrawal.Transaction.Amount
    |> (fun _ -> WithdrawalMade makeWithdrawal)