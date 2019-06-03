module MakeWithdrawalHandler

open Domain
open DomainHelpers

let handleMakeWithdrawal state (cmd: CmdArgs.MakeWithdrawal) =
    cmd.Id
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnoughForWithdrawal cmd.Transaction.Amount
    |> (fun _ -> WithdrawalMade cmd)