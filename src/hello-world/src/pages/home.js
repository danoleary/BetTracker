import React from 'react';
import Auth from '../services/auth'
import { Link } from "gatsby"

class Home extends React.Component {
  constructor() {
      super();
      this.handleClick = this.handleClick.bind(this);
  }

  handleClick() {
    const auth = new Auth();
    const accessToken = auth.getAccessToken();
    fetch("https://localhost:5001/api/dataimport/", {
        method: 'POST',
        headers: new Headers({
            "Accept": "application/json",
            "Authorization": `Bearer ${accessToken}`
        }),
        mode: 'cors'})
        .then(console.log("Imported Data"))
        .catch(error => console.log(error))
  }

  render() {
    return (
      <div>
        <button><Link to="/addbookie">Add bookie</Link></button>
        <button onClick={this.handleClick}>Import Data</button>
      </div>
    )
  }
}

export default Home;