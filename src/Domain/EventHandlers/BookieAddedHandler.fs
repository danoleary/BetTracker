module BookieAddedHandler

open Domain

let applyBookieAdded state (evt: CmdArgs.AddBookie): State =
    match state with
    | EmptyState -> Bookie { Id = evt.BookieId; Name = evt.Name; Balance = Balance 0m; Bets = [] }
    | Bookie _ -> failwith "Bookie already exists"