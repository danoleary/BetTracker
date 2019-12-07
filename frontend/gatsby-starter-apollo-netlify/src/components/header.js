import { Link, navigate } from "gatsby"
import PropTypes from "prop-types"
import React from "react"
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
    <header
      style={{
        marginBottom: `1.45rem`,
        textAlign: `center`,
      }}
    >
      <div
        style={{
          margin: `0 auto`,
          maxWidth: 800,
          padding: `1.45rem 1.0875rem`,
        }}
      >
        <a href="https://www.gatsbyjs.org">
          <img
            alt="Gatsby"
            src="https://www.gatsbyjs.org/monogram.svg"
            width="48"
          />
        </a>
        <a href="https://www.apollographql.com/">
          <img
            alt="Apollo"
            src="https://rawgit.com/apollographql/apollo-client/master/docs/source/logo/square.png"
            width="48"
            style={{ marginLeft: `16px`, marginRight: `16px` }}
          />
        </a>
        <a href="https://www.netlify.com">
          <img
            alt="Netlify"
            src="https://www.netlify.com/img/press/logos/logomark.png"
            width="48"
          />
        </a>
        <h1 style={{ margin: 0 }}>
          <Link
            to="/"
            style={{
              textDecoration: `none`,
            }}
          >
            {siteTitle}
          </Link>
          
        </h1>
        {logInSection}
      </div>
    </header>
  )
}

Header.propTypes = {
  siteTitle: PropTypes.string,
}

Header.defaultProps = {
  siteTitle: ``,
}

export default Header
