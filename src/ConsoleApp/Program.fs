open System
open Domain

let eventStore = CommandHandler.createDemoStore CommandHandler.StorageType.InMemory

let pipeline cmd =
    cmd
    |> CommandHandler.handle eventStore
    |> List.iter ReadSide.handle

let printState (desc:string) = eventStore.GetCurrentState() |> printfn "[%s] %A" (desc.ToUpper())

[<EntryPoint>]
let main argv =

    // printState "Initial"

    // AddTask { Id = 2; Name = "Give cool talk"; DueDate = None } |> pipeline
    
    // printState "After task added"

    // CompleteTask { Id = 2 } |> pipeline
    
    // printState "After task completed"

    // ClearAllTasks  |> pipeline
    
    // printState "After clear"
    0