module AddBookieHandlerTests

open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``Bookie added event is raised if add bookie is handled`` () =
    let state: State = EmptyState
    let addBookie: CmdArgs.AddBookie  = { BookieId = createNewBookieId (); Name = "Some bookie" }
    let command = AddBookie addBookie

    let result = execute state command

    match result with
    | Ok (BookieAdded args) -> Assert.True(args.Name = addBookie.Name)
    | _ -> failwith "incorrect event"