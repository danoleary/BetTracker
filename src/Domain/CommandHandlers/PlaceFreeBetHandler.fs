module PlaceFreeBetHandler

open Domain

let private onlyIfBookieExists state i =
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some task -> task
    | None -> failwith "Task does not exist"

let handlePlaceFreeBet state (placeFreeBet: CmdArgs.PlaceFreeBet) =
    placeFreeBet.Id
    |> onlyIfBookieExists state
    |> (fun _ -> FreeBetPlaced placeFreeBet)