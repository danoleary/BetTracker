module MakeWithdrawalHandler

open Result
open Domain
open DomainHelpers

let handleMakeWithdrawal state (cmd: CmdArgs.MakeWithdrawal) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfBalanceIsHighEnoughForWithdrawal cmd.Amount)
    |> map (fun _ -> WithdrawalMade cmd)