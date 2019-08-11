import { Link, navigate } from "gatsby"
import PropTypes from "prop-types"
import React from "react"
import 'bulma/css/bulma.css'
import Auth from '../services/auth'

const auth = new Auth()

const Header = ({ siteTitle }) => {
  const { isAuthenticated } = auth
  const logInSection = isAuthenticated() ?
    (
      <div className="buttons">
        <a className="button is-light" onClick={event => {
          event.preventDefault()
          auth.logout(() => navigate(`/`))
        }}>
          Log out
        </a>
      </div>
    ) : (<div className="buttons">
      <a className="button is-primary" onClick={auth.login}>
        <strong>Sign up</strong>
      </a>
      <a className="button is-light" onClick={auth.login}>
        Log in
    </a>
    </div>
)
return (
  <nav className="navbar is-dark" role="navigation" aria-label="main navigation">
    <div className="navbar-brand">
      <a className="navbar-item" href="https://bulma.io">
        Bet Tracker
      </a>

      <a role="button" className="navbar-burger burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
        <span aria-hidden="true"></span>
      </a>
    </div>
    <div id="navbarBasicExample" className="navbar-menu">
      <div className="navbar-end">
        <div className="navbar-item">
            {logInSection}
        </div>
      </div>
    </div>
  </nav>
)
}

Header.propTypes = {
  siteTitle: PropTypes.string,
}

Header.defaultProps = {
  siteTitle: ``,
}

export default Header
