import { Link, navigate } from "gatsby"
import PropTypes from "prop-types"
import React from "react"
import Auth from '../services/auth'

const auth = new Auth()

const Header = ({ siteTitle }) => {
  const { isAuthenticated } = auth
  const logInSection = isAuthenticated() ?
    (
      <div class="buttons">
        <a className="button is-light" onClick={event => {
          event.preventDefault()
          auth.logout(() => navigate(`/`))
        }}>
          <strong>Log out</strong>
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
    <nav class="navbar" role="navigation" aria-label="main navigation">
      <div class="navbar-brand">
        <a class="navbar-item" href="https://bulma.io">
          <img src="https://bulma.io/images/bulma-logo.png" width="112" height="28" />
        </a>

        <a role="button" class="navbar-burger burger" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>

      <div id="navbarBasicExample" class="navbar-menu">
        <div class="navbar-start">
          <a class="navbar-item">
            Home
      </a>

          <a class="navbar-item">
            Documentation
      </a>

          <div class="navbar-item has-dropdown is-hoverable">
            <a class="navbar-link">
              More
        </a>

            <div class="navbar-dropdown">
              <a class="navbar-item">
                About
          </a>
              <a class="navbar-item">
                Jobs
          </a>
              <a class="navbar-item">
                Contact
          </a>
              <hr class="navbar-divider" />
              <a class="navbar-item">
                Report an issue
          </a>
            </div>
          </div>
        </div>

        <div class="navbar-end">
          <div class="navbar-item">
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
