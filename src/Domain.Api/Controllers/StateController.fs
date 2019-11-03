namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System
open Data.UserRepository
open Microsoft.Extensions.Configuration
open System.Security.Claims
open Domain
open Domain.CmdArgs





[<Route("api/[controller]")>]
[<Authorize>]
[<ApiController>]
type StateController (configuration: IConfiguration, eventStore: CommandHandler.EventStore) =
    inherit ControllerBase()

    let eventStore = eventStore
    let configuration = configuration
    



    
