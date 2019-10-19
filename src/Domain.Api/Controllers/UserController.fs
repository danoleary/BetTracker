namespace Domain.Api.Controllers

open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Authorization
open System.Security.Claims
open Data.UserRepository
open Microsoft.Extensions.Configuration

[<Route("api/[controller]")>]
[<Authorize>]
[<ApiController>]
type UserController (configuration: IConfiguration) =
    inherit ControllerBase()

    let configuration = configuration

    [<HttpGet>]
    member this.Get() =
        let userIdClaim = (this.User.Claims
                     |> Seq.find (fun x -> x.Type = ClaimTypes.NameIdentifier))
        printfn "Claims: %A" userIdClaim
        let userId = userIdClaim.Value.ToString()
        let connection = configuration.GetConnectionString("database")
        let existingUser = getUser userId connection

        match existingUser with
        | Some user ->
         printfn "User exists: %s" user.id
         this.Ok() :> IActionResult
        | None ->
         insertUser userId (configuration.GetConnectionString("database")) |> ignore
         printfn "Added new user: %s" userId
         this.Ok() :> IActionResult

        

    
