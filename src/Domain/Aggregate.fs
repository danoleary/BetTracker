module Aggregate

open Domain

type Aggregate<'state, 'command, 'event> = {
    Init : 'state
    Apply: 'state -> 'event -> 'state
    Execute: 'state -> 'command -> 'event list
}

(*
    Commands validation
*)
// let onlyIfTaskDoesNotAlreadyExist (state:State) i =
//     match state.Tasks |> List.tryFind (fun x -> x.Id = i) with
//     | Some _ -> failwith "Task already exists"
//     | None -> state

// let onlyIfTaskExists (state:State) i =
//     match state.Tasks |> List.tryFind (fun x -> x.Id = i) with
//     | Some task -> task
//     | None -> failwith "Task does not exist"

// let onlyIfNotAlreadyFinished (task:Task) =
//     match task.IsComplete with
//     | true -> failwith "Task already finished"
//     | false -> task


let execute state command = 
    match command with
    | AddBookie args -> args |> (fun _ -> BookieAdded args)
    | MakeDeposit args -> args |> (fun _ -> DepositMade args)
    | _ -> failwith "Not handled yet"
    // | AddTask args -> args.Id |> onlyIfTaskDoesNotAlreadyExist state |> (fun _ -> TaskAdded args)
    // | RemoveTask args -> args.Id |> onlyIfTaskExists state |> (fun _ -> TaskRemoved args)
    // | ClearAllTasks -> AllTasksCleared
    // | CompleteTask args -> args.Id |> onlyIfTaskExists state |> (fun _ -> TaskCompleted args)
    // | ChangeTaskDueDate args -> args.Id |> (onlyIfTaskExists state >> onlyIfNotAlreadyFinished) |> (fun _ -> TaskDueDateChanged args)

let apply state event = 
    match event with
    | BookieAdded args -> 
        let newBookie = { Id = args.Id; Name = args.Name; Deposits = []; Withdrawals = [] }
        { state with Bookies = newBookie :: state.Bookies}
    | DepositMade args -> 
        let bookie = state.Bookies |> List.find (fun x -> x.Id = args.Id) |> (fun t -> t)
        let otherBookies = state.Bookies |> List.filter (fun x -> x.Id <> args.Id)
        { state with Bookies = bookie :: otherBookies }
    | _ -> failwith "not handled yet"
    // | TaskRemoved args -> { state with Tasks = state.Tasks |> List.filter (fun x -> x.Id <> args.Id) }
    // | AllTasksCleared -> { state with Tasks = [] }
    // | TaskCompleted args -> 
    //     let task = state.Tasks |> List.find (fun x -> x.Id = args.Id) |> (fun t -> { t with IsComplete = true })
    //     let otherTasks = state.Tasks |> List.filter (fun x -> x.Id <> args.Id)
    //     { state with Tasks = task :: otherTasks }
    // | TaskDueDateChanged args -> 
    //     let task = state.Tasks |> List.find (fun x -> x.Id = args.Id) |> (fun t -> { t with DueDate = args.DueDate })
    //     let otherTasks = state.Tasks |> List.filter (fun x -> x.Id <> args.Id)
    //     { state with Tasks = task :: otherTasks }

let aggregate = {
    Init = State.Init
    Execute = fun s c -> execute s c |> List.singleton
    Apply = apply
}