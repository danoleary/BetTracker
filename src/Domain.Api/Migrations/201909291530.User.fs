namespace Migrations
open SimpleMigrations

[<Migration(201909291530L, "Create Users")>]
type CreateUsers() =
  inherit Migration()

  override __.Up() =
    base.Execute(@"CREATE TABLE users(
      id varchar NOT NULL PRIMARY KEY);
      CREATE TABLE aggregates(id uuid NOT NULL PRIMARY KEY, user_id varchar NOT NULL REFERENCES users(id)) ")

  override __.Down() =
    base.Execute(@"DROP TABLE Users")