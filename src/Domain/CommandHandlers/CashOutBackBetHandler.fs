module CashOutBackBetHandler

open Domain
open DomainHelpers

let handleCashOutBackBet state (cmd: CmdArgs.CashOutBackBet) =
    cmd.Id
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBet cmd.BetId
    |> (fun _ -> BackBetCashedOut cmd)