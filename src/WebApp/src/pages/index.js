import React from "react"
import { Link } from "gatsby"

import Layout from "../components/layout"
import Image from "../components/image"
import SEO from "../components/seo"
import Auth from '../services/auth'

const auth = new Auth()

const IndexPage = () => {
  const { isAuthenticated } = auth
  return (
  <Layout>
    <SEO title="Home" />
    <h1 className="title is-1">Welcome to Bet Tracker</h1>
    {isAuthenticated() ? (
        <p>Go to my bets</p>
      ) : (
        <button className="button is-primary" onClick={auth.login}>
          <strong>Sign up here</strong>
        </button>
      )}
  </Layout>
)}

export default IndexPage
