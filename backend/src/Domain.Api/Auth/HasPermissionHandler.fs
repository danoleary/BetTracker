namespace Domain.Api.Auth

open Microsoft.AspNetCore.Authorization
open System.Threading.Tasks

type HasPermissionHandler () =
    inherit AuthorizationHandler<HasPermissionRequirement>()

        override this.HandleRequirementAsync(context: AuthorizationHandlerContext, requirement: HasPermissionRequirement): Task =
            if not (context.User.HasClaim (fun c -> c.Type = "permissions" && c.Issuer = requirement.Issuer))
                then Task.CompletedTask
                else 
                    let permissions = context.User.Claims
                                        |> Seq.filter (fun c -> c.Type = "permissions")
                                        |> Seq.map (fun c -> c.Type)

                    if Seq.exists (fun s -> s = requirement.Permission) permissions
                        then context.Succeed(requirement)
            
                    Task.CompletedTask
