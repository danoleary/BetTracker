import React from "react"
import Layout from "../components/layout"

export default class AddBookie extends React.Component {
    state = {
        bookieName: ""
    }

    handleInputChange = event => {
        const target = event.target
        const value = target.value
        const name = target.name
        this.setState({
            [name]: value,
        })
    }

    handleSubmit = event => {
        event.preventDefault()
    }

    render() {
        return (
            <Layout>
                <h1 className="title is-1">Add bookie</h1>
                <form>
                    <label>
                        Name
                        <input
                            type="text"
                            name="name"
                            value={this.state.name}
                            onChange={this.handleInputChange}
                        />
                    </label>
                    <button type="submit">Submit</button>
                </form>
            </Layout>
        )
    }
}
