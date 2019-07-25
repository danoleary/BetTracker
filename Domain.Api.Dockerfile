FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# copy everything and build the project
COPY . ./
RUN dotnet restore src/Domain/Domain.fsproj
RUN dotnet restore src/Domain.Api/Domain.Api.fsproj
RUN dotnet publish src/Domain.Api/Domain.Api.fsproj -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /app/src/Domain.Api/out ./

CMD dotnet Domain.Api.dll