module SettleFreeBetHandler

open Result
open Domain
open DomainHelpers

let handleSettleFreeBet state (cmd: CmdArgs.SettleFreeBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfThereIsAMatchingBet cmd.BetId)
    |> map (fun _ -> FreeBetSettled cmd)