namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc

[<Route("api/[controller]")>]
[<ApiController>]
type HealthController (eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore

    [<HttpGet>]
    member this.Get () =
        ActionResult<string>("Healthy")

    
