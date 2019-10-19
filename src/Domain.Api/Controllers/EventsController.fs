namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System
open Microsoft.AspNetCore.Http
open System.Security.Claims

[<Route("api/[controller]")>]
[<Authorize>]
[<ApiController>]
type EventsController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    // [<HttpGet("{id}")>]
    // member this.Get(id:Guid) =
        // let userIdClaim = (this.User.Claims
        //                 |> Seq.find (fun x -> x.Type = ClaimTypes.NameIdentifier))
        // let userId = userIdClaim.ToString()
        // let user = context.Users.Find(userId)
        // let events = user.Aggregates |> Seq.map (fun x -> CommandHandler.getEvents eventStore x.Id) |> Seq.toList
        // ActionResult<List<List<EventDtos.EventDto>>>(events)

    
