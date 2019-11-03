namespace Domain.Api.GraphQl

type GraphQLRequest = {
        query: string
        operationName: string
        variables: obj
    }