module CommandHandler

open System
open System.Collections.Concurrent
open Domain
open Aggregate
open CosmoStore

module Mapping = 
    open Newtonsoft.Json.Linq

    let toStoredEvent evn = 
        match evn with
        | BookieAdded args -> "BookieAdded", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | DepositMade args -> "DepositMade", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | _ -> failwith "not handled"
        // | AllTasksCleared -> "AllTasksCleared", (Newtonsoft.Json.Linq.JValue.CreateNull() :> JToken)
        // | TaskCompleted args -> "TaskCompleted", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        // | TaskDueDateChanged args -> "TaskDueDateChanged", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
    
    let toDomainEvent data =
        match data with
        | "BookieAdded", args -> args |> CosmosDb.Serialization.objectFromJToken |> BookieAdded
        | "DepositMade", args -> args |> CosmosDb.Serialization.objectFromJToken |> DepositMade
        | _ -> failwith "can't handle"
        // | "TaskRemoved", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskRemoved 
        // | "AllTasksCleared", _ -> AllTasksCleared 
        // | "TaskCompleted", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskCompleted 
        // | "TaskDueDateChanged", args -> args |> CosmosDb.Serialization.objectFromJToken |> TaskDueDateChanged 

type EventStore = {
    GetCurrentState : unit -> State
    Append : Event list -> unit
}

let inMemoryConfig: CosmoStore.InMemory.Configuration = {
    InMemoryStreams = new ConcurrentDictionary<string, Stream>()
    InMemoryEvents = new ConcurrentDictionary<Guid, EventRead>()
}

type StorageType =
    | InMemory

let createDemoStore typ =

    let store = 
        match typ with
        | InMemory -> CosmoStore.InMemory.EventStore.getEventStore inMemoryConfig
    
    let getCurrentState () =
        store.GetEvents "Bookies" EventsReadRange.AllEvents
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> List.map (fun x -> Mapping.toDomainEvent (x.Name, x.Data))
        |> List.fold aggregate.Apply State.Init

    let append evns =
        evns 
        |> List.map Mapping.toStoredEvent
        |> List.map (fun (name,data) -> { Id = Guid.NewGuid(); CorrelationId = (Some (Guid.NewGuid())); CausationId = (Some (Guid.NewGuid())); Name = name; Data = data; Metadata = None })
        |> store.AppendEvents "Bookies" ExpectedPosition.Any
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore 

    {
        GetCurrentState = getCurrentState
        Append = append
    }

let validate cmd =
    match cmd with
    | AddBookie args -> if args.Name = "" then failwith "Gimme some name!" else cmd
    // | ChangeTaskDueDate args -> if args.DueDate.IsSome && args.DueDate.Value < DateTime.Now then failwith "Are you Marty McFly?!" else cmd
    | _ -> cmd

let handleCommand (store:EventStore) command = 
    // get the latest state from store
    let currentState = store.GetCurrentState()
    // execute command to get new events
    let newEvents = command |> aggregate.Execute currentState
    // store events to event store
    newEvents |> store.Append
    // return events
    newEvents

let handle (store: EventStore) (cmd: Command) = 
    cmd 
    |> validate 
    |> handleCommand store