class NewKeyForm extends React.Component {
    render() {
        return (
            <span>
                <a className="btn waves-effect waves-light light-blue hide-on-small-only">
                    <i className="material-icons right">add</i>Add key
                </a>
                <a className="btn-floating btn waves-effect waves-light light-blue hide-on-med-and-up">
                    <i className="material-icons">add</i>
                </a>
            </span>
        );
    }
}

export default NewKeyForm;