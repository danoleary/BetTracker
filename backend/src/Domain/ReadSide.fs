module ReadSide

open Domain

// create some fancy SQL command here
let fakeSqlInsert (args:CmdArgs.AddBookie) = 
    ()

let fakeSqlUpdate (args:CmdArgs.MakeDeposit) = 
    ()

let fakeSqlDelete () = 
    printfn "SQL handler says: DELETE FROM Tasks"
    ()

let handleEventToConsole =
    ()

let handleEventToSql =
    ()

let handle evn =
    ()