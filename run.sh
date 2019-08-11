#!/bin/bash

docker run -e POSTGRES_USER=bettrackerwebapp -e POSTGRES_PASSWORD=password -p 5432:5432 --name bettrackerwebappdb -d postgres