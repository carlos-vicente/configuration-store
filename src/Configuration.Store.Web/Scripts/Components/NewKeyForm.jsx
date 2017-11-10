class NewKeyForm extends React.Component {

    componentDidMount() {
        // TODO: dont want to prepare all selects, just the one on this component
        console.log('About to mount all selects');
        jQuery('select').material_select();
        console.log('Just mounted all selects');
    }

    componentWillUnmount() {
        // TODO: dont want to destroy all selects, just the one on this component
        console.log('About to destroy all selects');
        jQuery('select').material_select('destroy');
        console.log('Just destroyed all selects');
    }

    handleSubmit(event){
        // handle submit of state
        console.log('Submit:' + event);
    }

    render() {
        return (
            <article>
                <a className="btn waves-effect waves-light light-blue hide-on-small-only">
                    <i className="material-icons right">add</i>Add key
                </a>
                <a className="btn-floating btn waves-effect waves-light light-blue hide-on-med-and-up">
                    <i className="material-icons">add</i>
                </a>
                <article className="row">
                    <form className="col s12" onSubmit={this.handleSubmit}>
                        <div className="row">
                            <div className="input-field col s3">
                                <input placeholder="Key name" id="key" type="text" className="validate"/>
                                <label htmlFor="key">Key name</label>
                            </div>
                            <div className="input-field col s3">
                                <input placeholder="Version" id="version" type="text" className="validate"/>
                                <label htmlFor="version">Version</label>
                            </div>
                            <div className="input-field col s3">
                                <select defaultValue="">
                                    <option value="" disabled>Choose value type</option>
                                    <option value="String">String</option>
                                    <option value="JSON">JSON</option>
                                </select>
                                <label>Value type</label>
                            </div>
                            <div className="input-field col s3">
                                {/* <a className="btn waves-effect waves-light light-blue">
                                    <i className="material-icons right">save</i>Save
                                </a> */}
                                <button className="btn waves-effect waves-light light-blue" type="submit" name="action">
                                    <i className="material-icons right">save</i>Save
                                </button>
                            </div>
                        </div>
                    </form>
                </article>
            </article>
        );
    }
}

export default NewKeyForm;