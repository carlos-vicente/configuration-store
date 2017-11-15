class NewKeyForm extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
            speed: 100,
            shown: false
        };

        // 'this' handler bindings
        this._openForm = this._openForm.bind(this);
        this._closeForm = this._closeForm.bind(this);
        this._saveKey = this._saveKey.bind(this);
        this._afterFade = this._afterFade.bind(this);
    }

    componentDidMount() {
        // todo: change to this.formContainer > select
        jQuery(this.valueTypeSelect).material_select();
    }

    componentWillUnmount() {
        // todo: change to this.formContainer > select
        jQuery(this.valueTypeSelect).material_select('destroy');
    }

    _afterFade(){
        this.setState((prevState, props) => {
            return {
                shown: !prevState.shown
            }
        });
    }

    _openForm(clickEvent){
        jQuery(this.formContainer).fadeIn(this.state.speed, this._afterFade);
    }
    
    _closeForm(clickEvent){
        jQuery(this.formContainer).fadeOut(this.state.speed, this._afterFade);
    }

    _saveKey(submitEvent){
        // handle submit of state
        console.log('Submit');
        submitEvent.preventDefault();

        // TODO: call this on the done promise's event
        this.props.onNewKeySaved();
    }

    render() {
        var bigOpenButtonClassName = "btn waves-effect waves-light light-blue hide-on-small-only"
            + (this.state.shown ? " hide" : "");

        var smallOpenButtonClassName = "btn-floating btn waves-effect waves-light light-blue hide-on-med-and-up"
            + (this.state.shown ? " hide" : "");

        var bigCloseButtonClassName = "btn waves-effect waves-light red darken-2 hide-on-small-only"
            + (this.state.shown ? "" : " hide");

        var smallCloseButtonClassName = "btn-floating btn waves-effect waves-light red darken-2 hide-on-med-and-up"
            + (this.state.shown ? "" : " hide");

        return (
            <article>
                <a className={bigOpenButtonClassName} onClick={this._openForm} style={{width: 170}}>
                    <i className="material-icons right">add</i>Add key
                </a>
                <a className={bigCloseButtonClassName} onClick={this._closeForm} style={{width: 170}}>
                    <i className="material-icons right">close</i>Close
                </a>
                <a className={smallOpenButtonClassName} onClick={this._openForm}>
                    <i className="material-icons">add</i>
                </a>
                <a className={smallCloseButtonClassName} onClick={this._closeForm}>
                    <i className="material-icons">close</i>
                </a>
                <article className="row" style={{ display:'none' }} ref={(fc) => { this.formContainer = fc; }}>
                    <form className="col s12" onSubmit={this._saveKey}>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <input placeholder="Key name" id="key" type="text" className="validate"/>
                                <label htmlFor="key">Key name</label>
                            </div>
                            <div className="input-field col s12 m3">
                                <input placeholder="Version" id="version" type="text" className="validate"/>
                                <label htmlFor="version">Version</label>
                            </div>
                            <div className="input-field col s12 m3">
                                <select defaultValue="" ref={(sel) => { this.valueTypeSelect = sel; }}>
                                    <option value="" disabled>Choose one</option>
                                    <option value="String">String</option>
                                    <option value="JSON">JSON</option>
                                </select>
                                <label>Value type</label>
                            </div>
                            <div className="input-field col s12 m3">
                                <button className="btn waves-effect waves-light light-blue" name="action">
                                    <i className="material-icons right">send</i>Save
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