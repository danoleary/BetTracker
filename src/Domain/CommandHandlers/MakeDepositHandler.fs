module MakeDepositHandler

open Domain
open DomainHelpers

let handleMakeDeposit state (cmd: CmdArgs.MakeDeposit) =
    cmd.Id |> onlyIfBookieExists state |> (fun _ -> DepositMade cmd)