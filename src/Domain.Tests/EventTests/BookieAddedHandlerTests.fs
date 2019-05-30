module BookieCreatedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``State contains one new bookie after bookie added event is applied`` () =
    let state: State = { Bookies = [] }
    let args: CmdArgs.AddBookie  ={ Id = createNewBookieId (); Name = "Some bookie" }
    let event = BookieAdded args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    Assert.Equal(args.Id, bookie.Id)
    Assert.Equal(args.Name, bookie.Name)
    Assert.Equal(Balance 0m, bookie.Balance)