module SettleLayBetHandler

open Domain
open DomainHelpers

let handleSettleLayBet state (cmd: CmdArgs.SettleLayBet) =
    cmd.Id
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBet cmd.BetId
    |> (fun _ -> LayBetSettled cmd)