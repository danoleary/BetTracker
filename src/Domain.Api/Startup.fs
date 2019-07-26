namespace Domain.Api

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open System.Collections.Concurrent
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy;
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Authorization
open Domain.Api.Auth

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2) |> ignore

        let postgresConfig: CosmoStore.Marten.Configuration = {
            Host = this.Configuration.["Db:Host"]
            Database = this.Configuration.["Db:Name"]
            Username = this.Configuration.["Db:Username"]
            Password = this.Configuration.["Db:Password"]
        }

        let usePostgres = this.Configuration.GetValue<bool>("UsePostgresEventStore")

        let eventStoreType =
            if usePostgres
            then
                CommandHandler.StorageType.Postgres postgresConfig
            else
                CommandHandler.StorageType.InMemory

        let eventStore = CommandHandler.createDemoStore eventStoreType
        services.AddScoped<CommandHandler.EventStore>(fun _ -> eventStore) |> ignore

        let domain = sprintf "https://%s/" (this.Configuration.["Auth0:Domain"])
        printfn "%A" domain

        services.AddAuthentication(
            fun options ->
                options.DefaultAuthenticateScheme <- JwtBearerDefaults.AuthenticationScheme
                options.DefaultChallengeScheme <- JwtBearerDefaults.AuthenticationScheme
            ).AddJwtBearer(
            (fun options ->
                options.Authority <- domain
                options.Audience <- this.Configuration.["Auth0:ApiIdentifier"])
        ) |> ignore
       
        services.AddAuthorization(
            fun options ->
                options.AddPolicy(
                    "write:command",
                    fun policy -> policy.Requirements.Add(HasScopeRequirement("write:command", domain)))
                options.AddPolicy(
                    "read:event",
                    fun policy -> policy.Requirements.Add(HasScopeRequirement("read:event", domain)))
                options.AddPolicy(
                    "read:state",
                    fun policy -> policy.Requirements.Add(HasScopeRequirement("read:state", domain)))
        ) |> ignore

        // register the scope authorization handler
        services.AddSingleton<IAuthorizationHandler, HasScopeHandler> () |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts() |> ignore
            //app.UseHttpsRedirection() |> ignore

        app.UseAuthentication() |> ignore

        app.UseMvc() |> ignore

    member val Configuration : IConfiguration = null with get, set
