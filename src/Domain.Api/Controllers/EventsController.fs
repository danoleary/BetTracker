namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System

[<Route("api/[controller]")>]
[<Authorize("read:event")>]
[<ApiController>]
type EventsController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    [<HttpGet("{id}")>]
    member this.Get(id:Guid) =
        let events = CommandHandler.getEvents eventStore id
        ActionResult<List<EventDtos.EventDto>>(events)

    
