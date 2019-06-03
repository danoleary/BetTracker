module SettleFreeBetHandler

open Domain
open DomainHelpers


let handleSettleFreeBet state (cmd: CmdArgs.SettleFreeBet) =
    cmd.Id
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBet cmd.BetId
    |> (fun _ -> FreeBetSettled cmd)