module ReadModel

open System
open Domain
open Domain.CmdArgs


type BookieDto = {
    id : Guid
    name : string
    profit : decimal
}

type StateDto = {
    bookies : BookieDto list
    profit : decimal
}

let buildBookieDto state event  =
    match event with
    | Domain.BookieAdded x ->
        let (BookieId bookieId) = x.BookieId
        { state with id = bookieId; name = x.Name}
    | Domain.DepositMade x ->
        let (TransactionAmount amount) = x.Amount
        { state with profit = state.profit + amount }
    | _ -> failwith "aaagh"

let getBookieDto id events =
    let starter = {
        id = Guid.NewGuid();
        name = "";
        profit = 0.0m
    }
    List.fold (fun state evt -> buildBookieDto state evt) starter events 