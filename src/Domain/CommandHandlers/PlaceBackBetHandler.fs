module PlaceBackBetHandler

open Domain
open DomainHelpers

let handlePlaceBackBet state (cmd: CmdArgs.PlaceBackBet) =
    cmd.Id
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnoughForStake cmd.Stake
    |> (fun _ -> BackBetPlaced cmd)