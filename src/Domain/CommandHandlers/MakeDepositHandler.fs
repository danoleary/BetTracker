module MakeDepositHandler

open Domain
open DomainHelpers

let handleMakeDeposit (state: State) (cmd: CmdArgs.MakeDeposit) =
    cmd |> onlyIfBookieExists state |> (fun _ -> DepositMade cmd)