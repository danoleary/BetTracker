module SettleLayBetHandler

open Result
open Domain
open DomainHelpers

let handleSettleLayBet state (cmd: CmdArgs.SettleLayBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfThereIsAMatchingBet cmd.BetId)
    |> map (fun _ -> LayBetSettled cmd)