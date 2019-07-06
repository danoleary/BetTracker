module PlaceLayBetHandler

open Result
open Domain
open DomainHelpers

let private onlyIfBalanceHighEnough stake odds bookie =
    let exposure = calculateExposure stake odds
    let (Balance balance) = bookie.Balance
    if balance >= exposure
        then
            Ok bookie
        else Error (BalanceNotHighEnoughError balance)

let handlePlaceLayBet state (cmd: CmdArgs.PlaceLayBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfBalanceHighEnough cmd.Stake cmd.Odds)
    |> map (fun _ -> LayBetPlaced cmd)