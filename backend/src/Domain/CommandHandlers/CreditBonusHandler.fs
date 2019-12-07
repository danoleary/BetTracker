module CreditBonusHandler

open Result
open Domain
open DomainHelpers

let handleCreditBonus state (cmd: CmdArgs.CreditBonus) =
    cmd
    |> onlyIfBookieExists state
    |> map (fun _ -> BonusCredited cmd)