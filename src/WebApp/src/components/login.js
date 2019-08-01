import React from 'react'
import { Link, navigate } from 'gatsby'
import Auth from '../services/auth'
 
const auth = new Auth()
 
const Login = () => {
  const { isAuthenticated } = auth
 
  if (isAuthenticated()) {
    return <button 
    onClick={event => {
        event.preventDefault()
        auth.logout(() => navigate(`/`))
      }}
    
    >Logout {auth.getUserName()}</button>
  } else {
    return <button onClick={auth.login}>Login</button>
  }
}


 
export default Login
