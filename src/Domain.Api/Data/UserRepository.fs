namespace Data

open Npgsql
open Dapper


module UserRepository =
    let getUser id connectionString =
        use connection = new NpgsqlConnection(connectionString)
        let props : Dictionary<string, string> = dict ["id" => id]
        let user = connection.QuerySingleOrDefault<User>("SELECT id From users WHERE id = @id", dict ["id" => id])
        if isNull (box user) then None
        else Some user

    let getAggregatesForUser userId connectionString =
        use connection = new NpgsqlConnection(connectionString)
        connection.Query<Aggregate>("SELECT id, user_id From aggregates WHERE user_id = @UserId", (Map ["UserId", userId]))

    let insertUser userId connectionString =
        use connection = new NpgsqlConnection(connectionString)
        connection.Execute("INSERT INTO users (id) values (@UserId)", (Map ["UserId", userId]))

    let insertAggregate userId aggregateId connectionString =
        use connection = new NpgsqlConnection(connectionString)
        connection.Execute("INSERT INTO aggregates (id, user_id) values (@UserId, @AggregateId)", (Map [("UserId", userId), ("AggregateId", aggregateId)]))