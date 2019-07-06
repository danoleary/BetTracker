module CashOutBackBetHandler

open Result
open Domain
open DomainHelpers

let handleCashOutBackBet state (cmd: CmdArgs.CashOutBackBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfThereIsAMatchingBet cmd.BetId)
    |> map (fun _ -> BackBetCashedOut cmd)