module MakeWithdrawalHandler

open Result
open Domain
open DomainHelpers

let handleMakeWithdrawal state (cmd: CmdArgs.MakeWithdrawal) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfBalanceIsHighEnoughForWithdrawal cmd.Transaction)
    |> map (fun _ -> WithdrawalMade cmd)