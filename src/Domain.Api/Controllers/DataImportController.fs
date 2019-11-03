namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System.Security.Claims
open DataImport
open System
open Data.UserRepository
open Microsoft.Extensions.Configuration

[<Route("api/[controller]")>]
[<Authorize>]
[<ApiController>]
type DataImportController (configuration: IConfiguration, eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore
    let configuration = configuration
    
    [<HttpPost>]
    member this.Post() =
        let userIdClaim = this.User.Claims |> Seq.find (fun x -> x.Type = ClaimTypes.NameIdentifier)
        let userId = userIdClaim.Value.ToString()
        let connection = configuration.GetConnectionString("database")

        let aggregateIds = importCommands eventStore

        List.iter (fun x -> insertAggregate x userId connection |> ignore) (List.ofSeq aggregateIds)
        
        this.Ok() :> IActionResult

    
