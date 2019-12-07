import React from 'react'

import BookieBlock from "../components/bookieblock"
import gql from "graphql-tag"
import { useQuery } from "@apollo/react-hooks"

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

export default () => {
    const { loading, error, data } = useQuery(APOLLO_QUERY);

    if (loading) return 'Loading...';
    if (error) return `Error! ${error.message}`;

    return (
        <div>
            <BookieBlock
                name='Overall'
                profit={data && (data.summary.withdrawals - data.summary.deposits)}
                balance={data && (data.summary.balance)}
                deposits={data && (data.summary.deposits)}
                withdrawals={data && (data.summary.withdrawals)} />
            {data.summary.bookies.map(x =>
                <BookieBlock
                    key={x.id}
                    name={x.name}
                    profit={x.withdrawals - x.deposits}
                    balance={x.balance}
                    deposits={x.deposits}
                    withdrawals={x.withdrawals}/>
            )
            }
        </div>
    );
}