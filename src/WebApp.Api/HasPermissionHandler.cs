using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace WebApp.Api
{
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            // If user does not have the scope claim, get out of here
            if (!context.User.HasClaim(c => c.Type == "permissions" && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            var permissions = 
                context.User.Claims.Where(x => x.Type == "permissions").Select(x => x.Value);

            Console.WriteLine(JsonConvert.SerializeObject(permissions));

            // Split the scopes string into an array
            //var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

            // Succeed if the scope array contains the required scope
            if (permissions.Any(s => s == requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
