namespace Domain.Api.Auth

open Microsoft.AspNetCore.Authorization

type HasPermissionRequirement (permissionParam: string, issuerParam: string) =

    let mutable permission = permissionParam
    let mutable issuer = issuerParam

    member this.Permission
      with get() = permission
      and set(value) = permission <- value

    member this.Issuer
      with get() = issuer
      and set(value) = issuer <- value
    
    interface IAuthorizationRequirement

