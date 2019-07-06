module MakeWithdrawalHandler

open Domain
open DomainHelpers

let handleMakeWithdrawal state (cmd: CmdArgs.MakeWithdrawal) =
    cmd
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnoughForWithdrawal cmd.Transaction
    |> (fun _ -> WithdrawalMade cmd)