module AddBookieHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Bookie added event is raised if add bookie is handled`` () =
    let state: State = { Bookies = [] }
    let addBookie: CmdArgs.AddBookie  = { Id = createNewBookieId (); Name = "Some bookie" }
    let command = AddBookie addBookie

    let result: Event = execute state command

    match result with
    | BookieAdded args -> Assert.True(args.Id = addBookie.Id && args.Name = addBookie.Name)
    | _ -> failwith "incorrect event"