namespace Domain.Api.Auth

open Microsoft.AspNetCore.Authorization

type HasScopeRequirement (scopeParam: string, issuerParam: string) =

    let mutable scope = scopeParam
    let mutable issuer = issuerParam

    member this.Scope
      with get() = scope
      and set(value) = scope <- value

    member this.Issuer
      with get() = issuer
      and set(value) = issuer <- value
    
    interface IAuthorizationRequirement


    