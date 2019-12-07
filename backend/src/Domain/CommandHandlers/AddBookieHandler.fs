module AddBookieHandler

open Domain

let handleAddBookie state addBookie =
    match state with
    | EmptyState -> Ok (BookieAdded addBookie)
    | _ -> Error BookieAlreadyExistsError