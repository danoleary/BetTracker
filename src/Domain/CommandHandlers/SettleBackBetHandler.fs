module SettleBackBetHandler

open Result
open Domain
open DomainHelpers

let handleSettleBackBet state (cmd: CmdArgs.SettleBackBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfThereIsAMatchingBet cmd.BetId)
    |> map (fun _ -> BackBetSettled cmd)