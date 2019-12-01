namespace Data

open Npgsql
open Dapper
open System.Collections.Generic
open System


module UserRepository =

    let p name value =
            ( name, value )

    let getUser id connectionString =
        use connection = new NpgsqlConnection(connectionString)
        let p: IDictionary<string, obj> = Map.ofList [ ("id", id) ] |> Map.toSeq |> dict
        let user = connection.QuerySingleOrDefault<User>("""SELECT id From users WHERE id = @id""", p)
        if isNull (box user) then None
        else Some user

    let getAggregatesForUser userId connectionString =
        use connection = new NpgsqlConnection(connectionString)
        let p: IDictionary<string, obj> = Map.ofList [ ("user_id", userId) ] |> Map.toSeq |> dict
        connection.Query<Guid>("SELECT id From aggregates WHERE user_id = @user_id", p)

    let insertUser id connectionString =
        use connection = new NpgsqlConnection(connectionString)
        let p: IDictionary<string, obj> = Map.ofList [ ("id", id) ] |> Map.toSeq  |> dict
        connection.Execute("""insert into users (id) values (@id)""", p)

    let insertAggregate aggregateId userId connectionString =
        use connection = new NpgsqlConnection(connectionString)
        let p: IDictionary<string, obj> = Map.ofList [ ("id", aggregateId); ("user_id", userId) ] |> Map.toSeq |> dict
        connection.Execute("INSERT INTO aggregates (id, user_id) values (@id, @user_id)", p)