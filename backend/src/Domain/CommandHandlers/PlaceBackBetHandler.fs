module PlaceBackBetHandler

open Result
open Domain
open DomainHelpers

let handlePlaceBackBet state (cmd: CmdArgs.PlaceBackBet) =
    cmd
    |> onlyIfBookieExists state
    |> bind (onlyIfBalanceIsHighEnoughForStake cmd.Stake)
    |> map (fun _ -> BackBetPlaced cmd)