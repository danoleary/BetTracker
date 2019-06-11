module AddBookieHandler

open Domain

let handleAddBookie state addBookie =
    match state with
    | EmptyState -> BookieAdded addBookie
    | _ -> failwith "already exists"