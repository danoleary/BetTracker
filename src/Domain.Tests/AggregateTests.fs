module Tests

open System
open Xunit
open Domain
open Aggregate

[<Fact>]
let ``Bookie added event is raised if add bookie command is handled`` () =
    let state: State = { Bookies = [] }
    let args: CmdArgs.AddBookie  ={ Id = Guid.NewGuid(); Name = "Some bookie" }
    let command = AddBookie args

    let result: Event = execute state command

    match result with
    | BookieAdded args -> Assert.True(args.Id = args.Id)
    | _ -> failwith "incorrect event"

[<Fact>]
let ``State contains one new bookie after bookie added event is applied`` () =
    let state: State = { Bookies = [] }
    let args: CmdArgs.AddBookie  ={ Id = Guid.NewGuid(); Name = "Some bookie" }
    let event = BookieAdded args

    let result = apply state event

    let bookie = List.exactlyOne result.Bookies
    Assert.Equal(args.Id, bookie.Id)
    Assert.Equal(args.Name, bookie.Name)
    Assert.Equal(0, List.length bookie.Deposits)
    Assert.Equal(0, List.length bookie.Withdrawals)
