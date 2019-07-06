module CreditBonusHandler

open Domain
open DomainHelpers

let handleCreditBonus state (cmd: CmdArgs.CreditBonus) =
    cmd
    |> onlyIfBookieExists state
    |> (fun _ -> BonusCredited cmd)