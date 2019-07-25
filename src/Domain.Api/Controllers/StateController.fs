namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System

[<Route("api/[controller]")>]
[<Authorize("read:state")>]
[<ApiController>]
type StateController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    [<HttpGet("{id}")>]
    member this.Get(id:Guid) =
        let state = CommandHandler.getCurrentState eventStore id
        ActionResult<Domain.State>(state)

    
