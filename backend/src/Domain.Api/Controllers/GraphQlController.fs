namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open Newtonsoft.Json
open Domain.Api.GraphQL
open FSharp.Data.GraphQL.Execution
open Microsoft.Extensions.Configuration
open Data.UserRepository
open System.Security.Claims
open Domain
open FSharp.Data.GraphQL

[<Route("graphql")>]
[<Authorize>]
[<ApiController>]
type GraphQlController (eventStore: CommandHandler.EventStore, configuration: IConfiguration) =
    inherit ControllerBase()

    let eventStore = eventStore

    let converters : JsonConverter [] = [| OptionConverter() |]
    let jsonSettings = jsonSerializerSettings converters
    let serialize d = JsonConvert.SerializeObject(d, jsonSettings)
    
    let json =
        function
        | Direct (data, _) ->
            JsonConvert.SerializeObject(data, jsonSettings)
        | Deferred (data, _, deferred) ->
            deferred |> Observable.add(fun d -> printfn "Deferred: %s" (serialize d))
            JsonConvert.SerializeObject(data, jsonSettings)
        | Stream data ->  
            data |> Observable.add(fun d -> printfn "Subscription data: %s" (serialize d))
            "{}"
    let removeWhitespacesAndLineBreaks (str : string) = str.Trim().Replace("\r\n", " ")

    [<HttpPost>]
    member this.Post ([<FromBody>] request: GraphQLRequest) =
        printfn "Received query: %s" request.query
        let query = removeWhitespacesAndLineBreaks request.query
        let userIdClaim = (this.User.Claims
                     |> Seq.find (fun x -> x.Type = ClaimTypes.NameIdentifier))
        let userId = userIdClaim.Value.ToString()
        let connection = configuration.GetConnectionString("database")
        let aggregateIds: AggregateId list =
            getAggregatesForUser userId connection |> Seq.map (fun x -> AggregateId x) |> List.ofSeq
        let executor: Executor<Root> = Schema.executor(eventStore, aggregateIds)
        let result = executor.AsyncExecute(query) |> Async.RunSynchronously
        printfn "Result metadata: %A" result.Metadata
        ActionResult<string>(json result)
    
