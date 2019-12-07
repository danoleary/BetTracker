import React from 'react'

export default (props) => (
    <article className="panel is-primary" key={props.id}>
        <p className="panel-heading">
          {props.name}
        </p>
        <a className="panel-block is-active">
          <span className="panel-icon">
            <i className="fas fa-book" aria-hidden="true"></i>
          </span>
          Profit: £{props.profit.toFixed(2)}
        </a>
        <a className="panel-block">
          <span className="panel-icon">
            <i className="fas fa-book" aria-hidden="true"></i>
          </span>
          Balance: £{props.balance.toFixed(2)}
        </a>
        <a className="panel-block">
          <span className="panel-icon">
            <i className="fas fa-book" aria-hidden="true"></i>
          </span>
          Deposits: £{props.deposits.toFixed(2)}
        </a>
        <a className="panel-block">
          <span className="panel-icon">
            <i className="fas fa-book" aria-hidden="true"></i>
          </span>
          Withdrawals: £{props.withdrawals.toFixed(2)}
        </a>
      </article>
  )