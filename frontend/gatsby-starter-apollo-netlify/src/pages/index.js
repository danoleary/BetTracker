import React from "react"
import gql from "graphql-tag"
import { useQuery } from "@apollo/react-hooks"

import Layout from "../components/layout"
import "./mystyles.scss"

const APOLLO_QUERY = gql`
  query {
    summary{
      balance
      deposits
      withdrawals
      bookies {
        id
        name
        deposits
        withdrawals
        balance
      }
    }
  }
`

const IndexPage = () => {
  const { data } = useQuery(APOLLO_QUERY)
  return (
    <Layout>
      <h1>Profit: £{data && data.summary.withdrawals - data.summary.deposits}</h1>
      <h1>Balance: £{data && data.summary.balance}</h1>
      <h1>Deposits: £{data && data.summary.deposits}</h1>
      <h1>Withdrawals: £{data && data.summary.withdrawals}</h1>
    </Layout>
  )
}

export default IndexPage
