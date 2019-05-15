module ReadSide

open Domain

// create some fancy SQL command here
let fakeSqlInsert (args:CmdArgs.AddBookie) = 
    printfn "SQL handler says: INSERT INTO Tasks VALUES (%A, '%s', false)" args.Id args.Name
    ()

let fakeSqlUpdate (args:CmdArgs.MakeDeposit) = 
    printfn "SQL handler says: UPDATE Tasks SET IsCompleted = true WHERE ID = %A" args.Id
    ()

let fakeSqlDelete () = 
    printfn "SQL handler says: DELETE FROM Tasks"
    ()

let handleEventToConsole = function
    | BookieAdded args -> printfn "Console handler says: Hurrayyyy, we have a task %s!" args.Name
    | DepositMade args -> printfn "Console handler says: Task with ID %A is completed" args.Id
    | WithdrawalMade args -> printfn "Console handler says: ...and now they are all gone"
    | _ -> ()

let handleEventToSql = function
    | BookieAdded args -> args |> fakeSqlInsert
    | DepositMade args -> args |> fakeSqlUpdate
    | WithdrawalMade args -> fakeSqlDelete()
    | _ -> ()

let handle evn =
    evn |> handleEventToConsole
    evn |> handleEventToSql