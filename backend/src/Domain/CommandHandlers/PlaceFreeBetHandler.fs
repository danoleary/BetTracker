module PlaceFreeBetHandler

open Result
open Domain
open DomainHelpers

let handlePlaceFreeBet state (cmd: CmdArgs.PlaceFreeBet) =
    cmd
    |> onlyIfBookieExists state
    |> map (fun _ -> FreeBetPlaced cmd)