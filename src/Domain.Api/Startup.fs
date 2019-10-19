namespace Domain.Api

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.AspNetCore.Authorization
open Domain.Api.Auth
open SimpleMigrations
open SimpleMigrations.DatabaseProvider
open System.Reflection
open Npgsql

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.CorsConfigName = "myorigins"

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        
        let env = services.BuildServiceProvider().GetService<IHostingEnvironment>()

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
                    fun policy -> policy.Requirements.Add(HasPermissionRequirement("write:command", domain)))
                options.AddPolicy(
                    "read:event",
                    fun policy -> policy.Requirements.Add(HasPermissionRequirement("read:event", domain)))
                options.AddPolicy(
                    "read:state",
                    fun policy -> policy.Requirements.Add(HasPermissionRequirement("read:state", domain)))
                options.AddPolicy(
                    "read:bets",
                    fun policy -> policy.Requirements.Add(HasPermissionRequirement("read:state", domain)))
        ) |> ignore

        services.AddCors(
            fun options ->
                options.AddPolicy(
                    this.CorsConfigName,
                    fun builder ->
                        builder
                            .WithOrigins("http://localhost:8000")
                            .AllowAnyHeader()
                            .AllowAnyMethod() |> ignore
                    )
        ) |> ignore

        // register the scope authorization handler
        services.AddSingleton<IAuthorizationHandler, HasPermissionHandler> () |> ignore
       

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts() |> ignore
            app.UseHttpsRedirection() |> ignore

        
        app.UseCors(this.CorsConfigName) |> ignore

        app.UseAuthentication() |> ignore

        app.UseMvc() |> ignore

        let assembly = Assembly.GetExecutingAssembly()
        use db = new NpgsqlConnection (this.Configuration.GetConnectionString("Database"))
        let provider = PostgresqlDatabaseProvider(db)
        let migrator = SimpleMigrator(assembly, provider)
        migrator.Load()
        migrator.MigrateToLatest()
    

    member val Configuration : IConfiguration = null with get, set
