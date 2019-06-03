module BookieAddedHandler

open Domain

let applyBookieAdded state (evt: CmdArgs.AddBookie) =
    let newBookie = { Id = evt.Id; Name = evt.Name; Balance = Balance 0m; Bets = [] }
    { state with Bookies = newBookie :: state.Bookies}