module CommandHandler

open System
open System.Collections.Concurrent
open Domain
open Aggregate
open CosmoStore
open CommandDtos
open EventDtos

module Mapping = 

    let toStoredEvent evn = 
        match evn with
        | Domain.BookieAdded args -> "BookieAdded", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.DepositMade args -> "DepositMade", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.WithdrawalMade args -> "WithdrawalMade", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.BackBetPlaced args -> "BackBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.BackBetSettled args -> "BackBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.FreeBetPlaced args -> "FreeBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.FreeBetSettled args -> "FreeBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.LayBetPlaced args -> "LayBetPlaced", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.LayBetSettled args -> "LayBetSettled", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.BackBetCashedOut args -> "BackBetCashedOut", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
        | Domain.BonusCredited args -> "BonusCredited", args |> CosmoStore.CosmosDb.Serialization.objectToJToken
    
    let toDomainEvent data =
        match data with
        | "BookieAdded", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.BookieAdded
        | "DepositMade", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.DepositMade
        | "WithdrawalMade", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.WithdrawalMade
        | "BackBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.BackBetPlaced
        | "BackBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.BackBetSettled
        | "FreeBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.FreeBetPlaced
        | "FreeBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.FreeBetSettled
        | "LayBetPlaced", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.LayBetPlaced
        | "LayBetSettled", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.LayBetSettled
        | "BackBetCashedOut", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.BackBetCashedOut
        | "BonusCredited", args -> args |> CosmosDb.Serialization.objectFromJToken |> Domain.BonusCredited
        | _ -> failwith "can't handle"
    
    let uwAmount (TransactionAmount amount) = amount
    let uwBetId (BetId betId) = betId
    let uwStake (Stake stake) = stake
    let uwOdds (Odds odds) = odds
    let uwDesc (EventDescription desc) = desc
    let uwCashout (CashOutAmount amt) = amt

    let toEventDto event: EventDto =
        match event with
        | Domain.BookieAdded args -> BookieAddedDto(DateTime.Now, args.Name) :> EventDto
        | Domain.DepositMade args -> DepositMadeDto(DateTime.Now, uwAmount args.Amount) :> EventDto
        | Domain.WithdrawalMade args -> WithdrawalMadeDto(DateTime.Now, uwAmount args.Amount) :> EventDto
        | Domain.BackBetPlaced args ->
            BackBetPlacedDto(DateTime.Now, uwBetId args.BetId, uwStake args.Stake,
                             uwOdds args.Odds, uwDesc args.EventDescription) :> EventDto
        | Domain.BackBetSettled args ->
            BackBetSettledDto(DateTime.Now, uwBetId args.BetId,
                             args.Result) :> EventDto
        | Domain.FreeBetPlaced args ->
            FreeBetPlacedDto(DateTime.Now, uwBetId args.BetId, uwStake args.Stake,
                             uwOdds args.Odds, uwDesc args.EventDescription) :> EventDto
        | Domain.FreeBetSettled args ->
            FreeBetSettledDto(DateTime.Now, uwBetId args.BetId,
                             args.Result) :> EventDto
        | Domain.LayBetPlaced args ->
            LayBetPlacedDto(DateTime.Now, uwBetId args.BetId, uwStake args.Stake,
                             uwOdds args.Odds, uwDesc args.EventDescription) :> EventDto
        | Domain.LayBetSettled args ->
            LayBetSettledDto(DateTime.Now, uwBetId args.BetId,
                             args.Result) :> EventDto
        | Domain.BackBetCashedOut args ->
            BackBetCashedOutDto(DateTime.Now, uwBetId args.BetId, uwCashout args.CashOutAmount) :> EventDto
        | Domain.BonusCredited args -> BonusCreditedDto(DateTime.Now, uwAmount args.Amount) :> EventDto

type EventStore = {
    GetEvents : AggregateId -> EventDto list
    GetCurrentState : AggregateId -> State
    Append : String -> Event list -> unit
}

let inMemoryConfig: CosmoStore.InMemory.Configuration = {
    InMemoryStreams = new ConcurrentDictionary<string, Stream>()
    InMemoryEvents = new ConcurrentDictionary<Guid, EventRead>()
}

type StorageType =
    | InMemory
    | Postgres of CosmoStore.Marten.Configuration

let createDemoStore typ =

    let store = 
        match typ with
        | InMemory -> CosmoStore.InMemory.EventStore.getEventStore inMemoryConfig
        | Postgres cfg -> CosmoStore.Marten.EventStore.getEventStore cfg
    
    let getEvents (AggregateId aggregateId) =
        store.GetEvents (aggregateId.ToString ()) EventsReadRange.AllEvents
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> List.map (fun x -> Mapping.toDomainEvent (x.Name, x.Data))
        |> List.map (fun x -> Mapping.toEventDto x)

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
        GetEvents = getEvents
        GetCurrentState = getCurrentState
        Append = append
    }

let validate (cmd: CommandDto): Command =
    match cmd with
    | :? AddBookieDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = AddBookie {
            Name = dto.Name
            BookieId = BookieId cmd.AggregateId
        }
     }
    | :? MakeDepositDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = MakeDeposit {
            Amount = TransactionAmount dto.Amount
        }
     }
    | :? MakeWithdrawalDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = MakeWithdrawal {
            Amount = TransactionAmount dto.Amount
        }
     }
    | :? PlaceBackBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = PlaceBackBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
            EventDescription = EventDescription dto.EventDescription
        }
     }
    | :? PlaceFreeBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = PlaceFreeBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
            EventDescription = EventDescription dto.EventDescription
        }
     }
    | :? PlaceLayBetDto as dto -> { 
        AggregateId = AggregateId dto.AggregateId;
        Timestamp = dto.Timestamp;
        Payload = PlaceFreeBet {
            BetId = BetId dto.BetId
            Stake = Stake dto.Stake
            Odds = Odds dto.Odds
            EventDescription = EventDescription dto.EventDescription
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

let getEvents (eventStore: EventStore) (aggId: Guid)  =
    eventStore.GetEvents (AggregateId aggId)