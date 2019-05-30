module SettleBackBetHandler

open Domain

let private onlyIfBookieExists state i =
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some task -> task
    | None -> failwith "Task does not exist"

let onlyIfThereIsAMatchingBackBet (betId: BetId) (bookie: Bookie) =
   if List.exists (fun (x: Bet) -> x.Id = betId) bookie.Bets then bookie else failwith "Can't find matching back bet"

let handleSettleBackBet state (settleBackBet: CmdArgs.SettleBackBet) =
    settleBackBet.Id
    |> onlyIfBookieExists state
    |> onlyIfThereIsAMatchingBackBet settleBackBet.BetId
    |> (fun _ -> BackBetSettled settleBackBet)