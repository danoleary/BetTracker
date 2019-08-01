import React from 'react';

class Home extends React.Component {
  constructor() {
      super();
      this.state = {bookList: []};
  }

  componentDidMount() {
    fetch("/api/books", {headers: new Headers({
        "Accept": "application/json"    })})
        .then(response => response.json())
        .then(books => this.setState({bookList: books}))
        .catch(error => console.log(error))
  }

  render() {
    let bookList = this.state.bookList.map((book) =>
                               <li><i>{book.author}</i> - <h3>{book.title}</h3></li>);

    return <ul>
      {bookList}
    </ul>;
  }
}

export default Home;