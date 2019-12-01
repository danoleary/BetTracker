namespace Domain.Api.GraphQL

type GraphQLRequest = {
        query: string
        operationName: string option
        variables: Map<string, obj> option
    }