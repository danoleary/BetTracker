module PlaceFreeBetHandler

open Domain
open DomainHelpers

let handlePlaceFreeBet state (cmd: CmdArgs.PlaceFreeBet) =
    cmd.Id
    |> onlyIfBookieExists state
    |> (fun _ -> FreeBetPlaced cmd)