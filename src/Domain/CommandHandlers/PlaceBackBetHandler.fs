module PlaceBackBetHandler

open Domain

let private onlyIfBookieExists state i =
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some task -> task
    | None -> failwith "Task does not exist"

let private onlyIfBalanceIsHighEnough stake bookie  =
    let amount = match stake with | Stake args -> args
    if bookie.Balance >= Balance amount then bookie else failwith "Can't place back bet"

let handlePlaceBackBet state (placeBackBet: CmdArgs.PlaceBackBet) =
    placeBackBet.Id
    |> onlyIfBookieExists state
    |> onlyIfBalanceIsHighEnough placeBackBet.Stake
    |> (fun _ -> BackBetPlaced placeBackBet)