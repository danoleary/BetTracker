import React from 'react';
import Auth from '../services/auth'

class Home extends React.Component {
  constructor() {
      super();
      this.state = {bookList: []};
      
  }

  componentDidMount() {
    const auth = new Auth();
    const accessToken = auth.getAccessToken();
    fetch("https://localhost:5003/api/values", {
        method: 'GET',
        headers: new Headers({
            "Accept": "application/json",
            "Authorization": `Bearer ${accessToken}`
        }),
        mode: 'cors'})
        .then(response => response.json())
        .then(books => this.setState({bookList: books}))
        .catch(error => console.log(error))
  }

  render() {
    let bookList = this.state.bookList.map((book) =>
                               <li><i>{book.author}</i></li>);

    return <ul>
      {bookList}
    </ul>;
  }
}

export default Home;