namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Newtonsoft.Json
open Newtonsoft.Json.Linq
open FSharpx.Task
open Domain.Api.GraphQl
open FSharp.Data.GraphQL.Execution
open FSharp.Data.GraphQL.Types
open System.IO
open System.Text
open Microsoft.Extensions.Primitives

[<Route("graphql")>]
[<ApiController>]
type GraphQlController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    let graphQL body = task {
        let converters : JsonConverter [] = [| OptionConverter() |]
        let jsonSettings = jsonSerializerSettings converters
        let serialize d = JsonConvert.SerializeObject(d, jsonSettings)

        let deserialize (data : string) =
            let getMap (token : JToken) = 
                let rec mapper (name : string) (token : JToken) =
                    match name, token.Type with
                    | "variables", JTokenType.Object -> token.Children<JProperty>() |> Seq.map (fun x -> x.Name, mapper x.Name x.Value) |> Map.ofSeq |> box
                    | name, JTokenType.Array -> token |> Seq.map (fun x -> mapper name x) |> Array.ofSeq |> box
                    | _ -> (token :?> JValue).Value
                token.Children<JProperty>()
                |> Seq.map (fun x -> x.Name, mapper x.Name x.Value)
                |> Map.ofSeq
            if System.String.IsNullOrWhiteSpace(data) 
            then None
            else data |> JToken.Parse |> getMap |> Some
        
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
        
        let readStream (s : Stream) =
            use ms = new MemoryStream(4096)
            s.CopyTo(ms)
            ms.ToArray()

        let data = deserialize body
        
        let query =
            data |> Option.bind (fun data ->
                if data.ContainsKey("query")
                then
                    match data.["query"] with
                    | :? string as x -> Some x
                    | _ -> failwith "Failure deserializing repsonse. Could not read query - it is not stringified in request."
                else None)
        
        let variables =
            data |> Option.bind (fun data ->
                if data.ContainsKey("variables")
                then
                    match data.["variables"] with
                    | null -> None
                    | :? string as x -> deserialize x
                    | :? Map<string, obj> as x -> Some x
                    | _ -> failwith "Failure deserializing response. Could not read variables - it is not a object in the request."
                else None)
        
        match query, variables  with
        | Some query, Some variables ->
            printfn "Received query: %s" query
            printfn "Received variables: %A" variables
            let query = removeWhitespacesAndLineBreaks query
            let root = { RequestId = System.Guid.NewGuid().ToString() }
            let result = Schema.executor.AsyncExecute(query, root, variables) |> Async.RunSynchronously
            printfn "Result metadata: %A" result.Metadata
            return json result
        | Some query, None ->
            printfn "Received query: %s" query
            let query = removeWhitespacesAndLineBreaks query
            let result = Schema.executor.AsyncExecute(query) |> Async.RunSynchronously
            printfn "Result metadata: %A" result.Metadata
            return json result
        | None, _ ->
            return ""
    }
 

    [<HttpPost>]
    member this.Post ([<FromBody>] request: GraphQLRequest) =
        let converters : JsonConverter [] = [| OptionConverter() |]
        let jsonSettings = jsonSerializerSettings converters
        let serialize d = JsonConvert.SerializeObject(d, jsonSettings)

        let deserialize (data : string) =
            let getMap (token : JToken) = 
                let rec mapper (name : string) (token : JToken) =
                    match name, token.Type with
                    | "variables", JTokenType.Object -> token.Children<JProperty>() |> Seq.map (fun x -> x.Name, mapper x.Name x.Value) |> Map.ofSeq |> box
                    | name, JTokenType.Array -> token |> Seq.map (fun x -> mapper name x) |> Array.ofSeq |> box
                    | _ -> (token :?> JValue).Value
                token.Children<JProperty>()
                |> Seq.map (fun x -> x.Name, mapper x.Name x.Value)
                |> Map.ofSeq
            if System.String.IsNullOrWhiteSpace(data) 
            then None
            else data |> JToken.Parse |> getMap |> Some
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
        //let variables = request.variables
        let query = request.query
        //match variables with
        //| Some variables ->
        //    printfn "Received query: %s" query
        //    printfn "Received variables: %A" variables
        //    let query = removeWhitespacesAndLineBreaks query
        //    let root = { RequestId = System.Guid.NewGuid().ToString() }
        //    let result = Domain.Api.GraphQl.Schema.executor.AsyncExecute(query, root, variables) |> Async.RunSynchronously
        //    printfn "Result metadata: %A" result.Metadata
        //    ActionResult<string>(json result)
        //| None ->
            //printfn "Received query: %s" query
            //let query = removeWhitespacesAndLineBreaks query
            //let result = Domain.Api.GraphQl.Schema.executor.AsyncExecute(query) |> Async.RunSynchronously
            //printfn "Result metadata: %A" result.Metadata
            //ActionResult<string>(json result)

        printfn "Received query: %s" query
        let query = removeWhitespacesAndLineBreaks query
        let result = Domain.Api.GraphQl.Schema.executor.AsyncExecute(query) |> Async.RunSynchronously
        printfn "Result metadata: %A" result.Metadata
        //this.Response.Headers.Add("content-type", StringValues("application/json")) |> ignore
        ActionResult<string>(json result)
    
