module PlaceFreeBetHandler

open Domain
open DomainHelpers

let handlePlaceFreeBet state (cmd: CmdArgs.PlaceFreeBet) =
    cmd
    |> onlyIfBookieExists state
    |> (fun _ -> FreeBetPlaced cmd)