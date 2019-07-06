module CommandHandler

open System
open System.Collections.Concurrent
open Domain
open Aggregate
open CosmoStore
open Dtos

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

let validate (cmd: CommandDto): Command =
    match cmd with
    | :? AddBookieDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.AddBookie {
            Name = dto.Name
            BookieId = BookieId cmd.AggregateId
        }
     }
    | :? MakeDepositDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.MakeDeposit {
            Amount = TransactionAmount dto.Amount
        }
     }
    | :? MakeWithdrawalDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.MakeWithdrawal {
            Amount = TransactionAmount dto.Amount
        }
     }
    | :? PlaceBackBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.PlaceBackBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
        }
     }
    | :? PlaceFreeBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.PlaceFreeBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
        }
     }
    | :? PlaceLayBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.PlaceFreeBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
        }
     }
    | :? SettleBackBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.SettleBackBet {
            BetId = BetId dto.BetId
            Result = dto.Result
        }
     }
    | :? SettleFreeBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.SettleFreeBet {
            BetId = BetId dto.BetId
            Result = dto.Result
        }
     }
    | :? SettleLayBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.SettleLayBet {
            BetId = BetId dto.BetId
            Result = dto.Result
        }
     }
    | :? CashOutBackBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.CashOutBackBet {
            BetId = BetId dto.BetId
            CashOutAmount = CashOutAmount dto.Amount
        }
     }
    | :? CreditBonusDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = Domain.CreditBonus {
            Amount = TransactionAmount dto.Amount
        }
     }
    | _ -> failwith "unknown type"

let handleCommand (store:EventStore) (command: Command): Result<Event list, CommandExecutionError> =
    // get the latest state from store
    let currentState = store.GetCurrentState command.AggregateId
    // execute command to get new events
    let newEvents: Result<Event list, CommandExecutionError> =
        command.Payload |> aggregate.Execute currentState

    match newEvents with
    | Ok events ->
        let (AggregateId aggregateId) = command.AggregateId
        // store events to event store
        store.Append (aggregateId.ToString ()) events
        // return events
        Ok events
    | Error failure -> Error failure

    

let handle (store: EventStore) (cmd: CommandDto) = 
    cmd 
    |> validate 
    |> handleCommand store

let getCurrentState (eventStore: EventStore) (aggId: Guid) =
    eventStore.GetCurrentState (AggregateId aggId)