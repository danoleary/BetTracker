module MakeDepositHandler

open Result
open Domain
open DomainHelpers

let handleMakeDeposit (state: State) (cmd: CmdArgs.MakeDeposit) =
    cmd
    |> onlyIfBookieExists state
    |> map (fun _ -> DepositMade cmd)