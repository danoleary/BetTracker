namespace Domain.Api.GraphQL

open FSharp.Data.GraphQL
open FSharp.Data.GraphQL.Types
open FSharp.Data.GraphQL.Server.Middleware
open Domain

#nowarn "40"

type Root =
        { RequestId: string }

type Bookie =
    { Id : System.Guid
      Name : string
      Balance: float
      Deposits: float
      Withdrawals: float }

type Summary =
    { Bookies : Bookie list
      Balance : float
      Deposits: float
      Withdrawals: float
    }


module Schema =
    let executor (eventStore: CommandHandler.EventStore, aggregateIds: AggregateId list) =

        let bookieToDto (bookie: Domain.Bookie) =
            let (BookieId id) = bookie.Id
            let (Balance balance) = bookie.Balance
            let (TransactionAmount totalDeposits) = bookie.TotalDeposits
            let (TransactionAmount totalWithdrawals) = bookie.TotalWithdrawals
            { Id = id; Name = bookie.Name; Balance = float balance; Deposits = float totalDeposits; Withdrawals = float totalWithdrawals}
        
        let getBookie (aggregateId: AggregateId) =
            let state = eventStore.GetCurrentState aggregateId
            match state with
            | EmptyState -> None
            | State.Bookie b -> Some (bookieToDto b)

        let getSummary () =
            let states = aggregateIds |> List.map (fun x -> eventStore.GetCurrentState x)
            let bookies = states |> List.map (fun state ->
                                        match state with
                                        | EmptyState -> None
                                        | State.Bookie b -> Some (bookieToDto b))
            let finalBookies =
                        bookies
                        |> List.filter (fun x ->
                            match x with
                            | None -> false
                            | _-> true)
                        |> List.map (fun x -> x.Value)

            let balances: float list = List.map (fun (x: Bookie) -> x.Balance) finalBookies
            let deposits: float list = List.map (fun (x: Bookie) -> x.Deposits) finalBookies
            let withdrawals: float list = List.map (fun (x: Bookie) -> x.Withdrawals) finalBookies

            {
                Bookies = finalBookies;
                Balance = List.fold (fun x y -> x + y) 0.0 balances
                Deposits = List.fold (fun x y -> x + y) 0.0 deposits
                Withdrawals = List.fold (fun x y -> x + y) 0.0 withdrawals
            }
            
            

        let rec BookieType : ObjectDef<Bookie> =
            Define.Object<Bookie>(
                name = "Bookie",
                description = "A bookie",
                isTypeOf = (fun o -> o :? Bookie),
                fieldsFn = fun () ->
                [
                    Define.Field("id", Guid, "The id of the bookie.", (fun _ (h: Bookie) -> h.Id))
                    Define.Field("name", String, "The name of the bookie.", (fun _ (h : Bookie) -> h.Name))
                    Define.Field("balance", Float, "The balance with the bookie.", (fun _ (h : Bookie) -> h.Balance))
                    Define.Field("deposits", Float, "The deposits made with the bookie.", (fun _ (h : Bookie) -> h.Deposits))
                    Define.Field("withdrawals", Float, "The withdrawals with the bookie.", (fun _ (h : Bookie) -> h.Withdrawals))
                ])

        and SummaryType : ObjectDef<Summary> =
            Define.Object<Summary>(
                name = "Summary",
                description = "Summary of all bookies",
                isTypeOf = (fun o -> o :? Summary),
                fieldsFn = fun () ->
                [
                    Define.Field("bookies", ListOf BookieType, "The list of bookies", (fun _ (h: Summary) -> h.Bookies))
                    Define.Field("balance", Float, "The overall balance.", (fun _ (h : Summary) -> h.Balance))
                    Define.Field("deposits", Float, "The deposits made with the bookie.", (fun _ (h : Summary) -> h.Deposits))
                    Define.Field("withdrawals", Float, "The withdrawals with the bookie.", (fun _ (h : Summary) -> h.Withdrawals))
                ])

        let RootType =
            Define.Object<Root>(
                name = "Root",
                description = "The Root type to be passed to all our resolvers.",
                isTypeOf = (fun o -> o :? Root),
                fieldsFn = fun () ->
                [
                    Define.Field("requestId", String, "The ID of the client.", fun _ (r : Root) -> r.RequestId)
                ])

        let Query =
            Define.Object<Root>(
                name = "Query",
                fields = [
                    Define.Field("bookie", Nullable BookieType, "Gets bookie", [ Define.Input("id", String) ], fun ctx _ -> getBookie (ctx.Arg("id")))
                    Define.Field("summary", SummaryType, "Gets summary", fun ctx _ -> getSummary ())
                ])

        let schema : ISchema<Root> = upcast Schema(query = Query)

        let middlewares = [ Define.QueryWeightMiddleware(2.0, true) ]
        Executor(schema, middlewares)