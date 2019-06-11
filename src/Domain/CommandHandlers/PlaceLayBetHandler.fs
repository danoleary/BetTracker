module PlaceLayBetHandler

open Domain
open DomainHelpers

let private onlyIfBalanceHighEnough stake odds bookie =
    let exposure = calculateExposure stake odds
    let (Balance balance) = bookie.Balance
    if balance >= exposure then bookie else failwith "Can't place lay bet"

let handlePlaceLayBet state (cmd: CmdArgs.PlaceLayBet) =
    cmd
    |> onlyIfBookieExists state
    |> onlyIfBalanceHighEnough cmd.Stake cmd.Odds
    |> (fun _ -> LayBetPlaced cmd)