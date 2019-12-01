import React from "react"
import Layout from '../components/layout'
import Home from './home'

import Auth from '../services/auth'

const auth = new Auth()

export default () => {
    const { isAuthenticated } = auth
    return (
    <Layout>
        <h1 className="title is-1">Welcome to Bet Tracker</h1>
        {isAuthenticated() ? (
            <Home />
        ) : (
            <button className="button is-primary" onClick={auth.login}>
            <strong>Sign up here</strong>
            </button>
        )}
    </Layout>
    )
}
