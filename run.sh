docker build -t domain-api -f Domain.Api.Dockerfile .
docker run -d -p 8080:80 -p 8443:443 --name domain-api domain-api

// set up certs
dotnet dev-certs https --trust