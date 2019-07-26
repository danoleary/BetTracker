namespace Domain.Api

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging

module Program =
    let exitCode = 0

    let CreateWebHostBuilder args =
        WebHost
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(
                fun builder -> builder.AddEnvironmentVariables() |> ignore
            )
            .UseStartup<Startup>()

    [<EntryPoint>]
    let main args =
        CreateWebHostBuilder(args).Build().Run()

        exitCode
