module BookieCreatedHandlerTests

open System
open Xunit
open Domain
open Aggregate
open Domain.CmdArgs
open TestHelpers

[<Fact>]
let ``State contains one new bookie after bookie added event is applied`` () =
    let state: State = EmptyState
    let args: CmdArgs.AddBookie  ={ BookieId = createNewBookieId (); Name = "Some bookie" }
    let event = BookieAdded args

    let result = apply state event

    match result with
    | Bookie bookie ->
            Assert.Equal(Balance 0m, bookie.Balance)
            Assert.Equal(0, bookie.Bets.Length)
            Assert.Equal(args.Name, bookie.Name)
    | _ -> failwith "failed"
    