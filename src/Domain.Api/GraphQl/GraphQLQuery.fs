namespace Domain.Api.GraphQL

open Newtonsoft.Json
open FSharp.Data.GraphQL.Types

type GraphQLQuery =
    { ExecutionPlan : ExecutionPlan
      Variables : Map<string, obj> }