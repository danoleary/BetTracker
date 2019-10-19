#!/bin/bash

dotnet ef migrations add InitCreate -p ./src/Migrations -s ./src/Domain.Api -v 

docker run -e POSTGRES_USER=bettrackerwebapp -e POSTGRES_PASSWORD=password -p 5432:5432 --name bettrackerwebappdb -d postgres
docker run -p 5050:80 -e "PGADMIN_DEFAULT_EMAIL=name@example.com" -e "PGADMIN_DEFAULT_PASSWORD=admin" -d dpage/pgadmin4