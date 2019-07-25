namespace Domain.Api.Auth

open Microsoft.AspNetCore.Authorization
open System.Threading.Tasks

type HasScopeHandler () =
    inherit AuthorizationHandler<HasScopeRequirement>()

        override this.HandleRequirementAsync(context: AuthorizationHandlerContext , requirement:HasScopeRequirement ): Task =
        // If user does not have the scope claim, get out of here
            if not (context.User.HasClaim (fun c -> c.Type = "scope" && c.Issuer = requirement.Issuer))
                then Task.CompletedTask
                else 
                    // Split the scopes string into an array
                    let scopes = context.User.FindFirst(fun c -> c.Type = "scope" && c.Issuer = requirement.Issuer).Value.Split(' ')

                    // Succeed if the scope array contains the required scope
                    if Array.exists (fun s -> s = requirement.Scope) scopes
                        then context.Succeed(requirement)
            
                    Task.CompletedTask