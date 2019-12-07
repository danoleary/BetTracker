import { ApolloClient } from "apollo-client"
import { InMemoryCache } from "apollo-cache-inmemory"
import { createHttpLink } from 'apollo-link-http';
import { setContext } from 'apollo-link-context';
import fetch from "isomorphic-fetch"
import Auth from '../services/auth'

const httpLink = createHttpLink({
  uri: "https://localhost:5001/graphql",
});

const auth = new Auth();

const authLink = setContext((_, { headers }) => {
  // get the authentication token from local storage if it exists
  const accessToken = auth.getAccessToken();
  // return the headers to the context so httpLink can read them
  return {
    headers: {
      ...headers,
      authorization: accessToken ? `Bearer ${accessToken}` : "",
    }
  }
});

const client = new ApolloClient({
  link: authLink.concat(httpLink),
  cache: new InMemoryCache(),
  fetch,
})

export default client
