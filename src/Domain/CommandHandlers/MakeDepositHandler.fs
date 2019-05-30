module MakeDepositHandler

open Domain

let private onlyIfBookieExists state i =
    match state.Bookies |> List.tryFind (fun x -> x.Id = i) with
    | Some task -> task
    | None -> failwith "Task does not exist"

let handleMakeDeposit state (makeDeposit: CmdArgs.MakeDeposit) =
    makeDeposit.Id |> onlyIfBookieExists state |> (fun _ -> DepositMade makeDeposit)