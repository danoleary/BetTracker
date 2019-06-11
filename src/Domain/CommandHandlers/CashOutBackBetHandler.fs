module CashOutBackBetHandler

open Domain
open DomainHelpers

let handleCashOutBackBet state (cmd: CmdArgs.CashOutBackBet) =
    cmd
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBet cmd.BetId
    |> (fun _ -> BackBetCashedOut cmd)