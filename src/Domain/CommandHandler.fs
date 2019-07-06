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
        | WithdrawalMade args -> "WithdrawalMade", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | BackBetPlaced args -> "BackBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | BackBetSettled args -> "BackBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | FreeBetPlaced args -> "FreeBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | FreeBetSettled args -> "FreeBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | LayBetPlaced args -> "LayBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | LayBetSettled args -> "LayBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | BackBetCashedOut args -> "BackBetCashedOut", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | BonusCredited args -> "BonusCredited", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
    
    let toDomainEvent data =
        match data with
        | "BookieAdded", args -> args |> CosmosDb.Serialization.objectFromJToken |> BookieAdded
        | "DepositMade", args -> args |> CosmosDb.Serialization.objectFromJToken |> DepositMade
        | "WithdrawalMade", args -> args |> CosmosDb.Serialization.objectFromJToken |> WithdrawalMade
        | "BackBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> BackBetPlaced
        | "BackBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> BackBetSettled
        | "FreeBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> FreeBetPlaced
        | "FreeBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> FreeBetSettled
        | "LayBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> LayBetPlaced
        | "LayBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> LayBetSettled
        | "BackBetCashedOut", args -> args |> CosmosDb.Serialization.objectFromJToken |> BackBetCashedOut
        | "BonusCredited", args -> args |> CosmosDb.Serialization.objectFromJToken |> BonusCredited
        | _ -> failwith "can't handle"

type EventStore = {
    GetCurrentState : AggregateId -> State
    Append : String -> Event list -> unit
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
    
    let getCurrentState (AggregateId aggregateId) =
        store.GetEvents (aggregateId.ToString ()) EventsReadRange.AllEvents
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> List.map (fun x -> Mapping.toDomainEvent (x.Name, x.Data))
        |> List.fold aggregate.Apply State.Init

    let append aggregateId evns =
        evns 
        |> List.map Mapping.toStoredEvent
        |> List.map (fun (name,data) -> { Id = Guid.NewGuid(); CorrelationId = (Some (Guid.NewGuid())); CausationId = (Some (Guid.NewGuid())); Name = name; Data = data; Metadata = None })
        |> store.AppendEvents aggregateId ExpectedPosition.Any
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> ignore 

    {
        GetCurrentState = getCurrentState
        Append = append
    }

let validate cmd =
    match cmd.Payload with
    | AddBookie args -> if args.Name = "" then failwith "Gimme some name!" else cmd
    // | ChangeTaskDueDate args -> if args.DueDate.IsSome && args.DueDate.Value < DateTime.Now then failwith "Are you Marty McFly?!" else cmd
    | _ -> cmd

let handleCommand (store:EventStore) command = 
    // get the latest state from store
    let currentState = store.GetCurrentState command.AggregateId
    // execute command to get new events
    let newEvents = command.Payload |> aggregate.Execute currentState

    let (AggregateId aggregateId) = command.AggregateId
    // store events to event store
    store.Append (aggregateId.ToString ()) newEvents
    // return events
    newEvents

let handle (store: EventStore) (cmd: Command) = 
    cmd 
    |> validate 
    |> handleCommand store