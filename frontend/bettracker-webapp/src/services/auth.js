import auth0 from 'auth0-js'
import { navigate } from 'gatsby'
 
const AUTH0_DOMAIN = process.env.AUTH0_DOMAIN
const AUTH0_CLIENT_ID = process.env.AUTH0_CLIENTID
const AUTH0_CALLBACK_URL = process.env.AUTH0_CALLBACK
const AUTH0_AUDIENCE = process.env.AUTH0_AUDIENCE

export default class Auth {
  auth0 = new auth0.WebAuth({
    domain: AUTH0_DOMAIN,
    clientID: AUTH0_CLIENT_ID,
    redirectUri: AUTH0_CALLBACK_URL,
    audience: AUTH0_AUDIENCE,
    responseType: 'token id_token',
    scope: 'openid',
  })
 
  login = () => {
    this.auth0.authorize()
  }
 
  logout = (callback) => {
    localStorage.removeItem('access_token')
    localStorage.removeItem('id_token')
    localStorage.removeItem('expires_at')
    localStorage.removeItem('user')
    localStorage.removeItem('permissions')
    callback()
  }

  hasPermission(permissions) {
    const grantedPermissions = JSON.parse(localStorage.getItem('permissions'));
    return permissions.every(permission => grantedPermissions.includes(permission));
  }
 
  handleAuthentication = () => {
    if (typeof window !== 'undefined') {
      // this must've been the trick
      this.auth0.parseHash((err, authResult) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          this.setSession(authResult)
        } else if (err) {
          console.log(err)
        }
 
        // Return to the homepage after authentication.
        navigate('/')
      })
    }
  }
 
  isAuthenticated = () => {
    const expiresAt = JSON.parse(localStorage.getItem('expires_at'))
    return new Date().getTime() < expiresAt
  }
 
  setSession = authResult => {
    
    const expiresAt = JSON.stringify(
      authResult.expiresIn * 1000 + new Date().getTime()
    )
    console.log(JSON.stringify(authResult))
    localStorage.setItem('access_token', authResult.accessToken)
    localStorage.setItem('id_token', authResult.idToken)
    localStorage.setItem('expires_at', expiresAt)
    localStorage.setItem('scopes', JSON.stringify(authResult.scope || ""));

    this.auth0.client.userInfo(authResult.accessToken, (err, user) => {
      localStorage.setItem('user', JSON.stringify(user))
      this.getuserDataFromServer(authResult.accessToken)
    })
  }
 
  getUser = () => {
    if (localStorage.getItem('user')) {
      return JSON.parse(localStorage.getItem('user'))
    }
  }
 
  getUserName = () => {
    if (this.getUser()) {
      return this.getUser().name
    }
  }

  getAccessToken() {
    const accessToken = localStorage.getItem('access_token');
    if (!accessToken) {
      throw new Error('No access token found');
    }
    return accessToken;
  }

  getuserDataFromServer(token) {
      console.log('in getuserDataFromServer')
      const url = 'https://localhost:5001/api/user/'
      fetch(url, {
          method: 'GET',
          headers: {
              'Content-Type': 'application/json',
              'Authorization': 'bearer ' + token,
          }
      });
  }
}