namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System.Security.Claims
open DataImport
open System

[<Route("api/[controller]")>]
[<Authorize>]
[<ApiController>]
type DataImportController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    // let addAggregate (context: BetTrackerContext) (entity: Aggregate) =
    //     async {
    //         context.Aggregates.AddAsync(entity) |> Async.AwaitTask |> ignore
    //         let! _ = context.SaveChangesAsync true |> Async.AwaitTask
    //         return entity
    //     }

    [<HttpPost>]
    member this.Post() =
        // let userIdClaim = (this.User.Claims
        //                 |> Seq.find (fun x -> x.Type = ClaimTypes.NameIdentifier))
        // let userId = userIdClaim.Value.ToString()
        // let aggregateIds = importCommands eventStore
        // let aggregates = aggregateIds |> Seq.map (fun x -> { Id = x }) |> List.ofSeq
        // let user = context.Users.Find(userId)
        // printfn "current: %A" user
        // let updatedUser = { user with Aggregates = ResizeArray<Aggregate> aggregates }
        // printfn "updatedUser: %A" updatedUser
        // context.Entry(user).CurrentValues.SetValues(updatedUser)
        // context.SaveChanges |> ignore
        
        this.Ok() :> IActionResult

    
