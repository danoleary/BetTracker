module MakeWithdrawalHandler

open Domain
open DomainHelpers
open System.Transactions

let handleMakeWithdrawal state (cmd: CmdArgs.MakeWithdrawal) =
    printfn "Timestamp: %A, Amount: %A" cmd.Transaction.Timestamp cmd.Transaction.Amount
    cmd
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnoughForWithdrawal cmd.Transaction.Amount
    |> (fun _ -> WithdrawalMade cmd)