import React from "react"
import Layout from "../components/layout"
import "./mystyles.scss"
import Auth from '../services/auth'
import LoggedIn from '../components/loggedin'
import LogIn from '../components/login'
 
const auth = new Auth()

const { isAuthenticated } = auth

const IndexPage = () => 
  (<Layout>
    {isAuthenticated()
      ? <LoggedIn />
      : <LogIn />}
  </Layout>)
  

export default IndexPage
