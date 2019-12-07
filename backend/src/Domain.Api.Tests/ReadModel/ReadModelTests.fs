module ReadModelTests

open Xunit
open Domain
open TestHelpers
open ReadModel

let bookieAdded bookieId =
    let args: CmdArgs.AddBookie  ={ BookieId = bookieId; Name = "Some bookie" }
    BookieAdded args

let depositMade bookieId =
    let args: CmdArgs.MakeDeposit = { Amount = TransactionAmount 100.0m }
    DepositMade args

[<Fact>]
let ``bookie name and id are set correctly`` () =
    let bookieId = createNewBookieId ()
    let bookieAddedEvent = bookieAdded bookieId

    let bookieDto = getBookieDto bookieId [ bookieAddedEvent ]

    let (BookieId bookieIdGuid)  = bookieId

    Assert.Equal("Some bookie", bookieDto.name)
    Assert.Equal(bookieIdGuid, bookieDto.id)


[<Fact>]
let ``total balance of bookie is calculated correctly`` () =
    let bookieId = createNewBookieId ()
    let bookieAddedEvent = bookieAdded bookieId
    let depositMadeEvent = depositMade bookieId

    let bookieDto = getBookieDto bookieId [ bookieAddedEvent; depositMadeEvent ]

    Assert.Equal(100.0m, bookieDto.profit)



