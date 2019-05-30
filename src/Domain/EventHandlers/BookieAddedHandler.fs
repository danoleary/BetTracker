module BookieAddedHandler

open Domain

let applyBookieAdded state (bookieAdded: CmdArgs.AddBookie) =
    let newBookie = { Id = bookieAdded.Id; Name = bookieAdded.Name; Balance = Balance 0m; Bets = [] }
    { state with Bookies = newBookie :: state.Bookies}