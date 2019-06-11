module SettleBackBetHandler

open Domain
open DomainHelpers

let handleSettleBackBet state (cmd: CmdArgs.SettleBackBet) =
    cmd
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBet cmd.BetId
    |> (fun _ -> BackBetSettled cmd)