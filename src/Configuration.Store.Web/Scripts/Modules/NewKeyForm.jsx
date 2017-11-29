﻿class NewKeyForm extends React.Component {
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
        this._toggleButton = this._toggleButton.bind(this);
        this._handleInputChange = this._handleInputChange.bind(this);
        this._changeInputValue = this._changeInputValue.bind(this);
        this._handleSelectChange = this._handleSelectChange.bind(this);
    }

    componentDidMount() {
        // todo: change to this.formContainer > select
        jQuery(this.valueTypeSelect).material_select(this._handleSelectChange);
    }

    componentWillUnmount() {
        // todo: change to this.formContainer > select
        jQuery(this.valueTypeSelect).material_select('destroy');
    }

    _toggleButton(){
        this.setState((prevState, props) => {
            return {
                shown: !prevState.shown
            }
        });
    }

    _openForm(clickEvent){
        jQuery(this.formContainer).fadeIn(this.state.speed, this._toggleButton);
    }
    
    _closeForm(clickEvent){
        jQuery(this.formContainer).fadeOut(this.state.speed, this._toggleButton);
    }

    _saveKey(submitEvent){
        submitEvent.preventDefault();

        this.props.saveKey(
            this.state.key,
            this.state.valueType,
            this._closeForm
        );
    }

    _changeInputValue(inputName, inputValue){
        this.setState({
            [inputName]: inputValue
        });
    }

    _handleSelectChange(){
        var value = jQuery(this.valueTypeSelect).val();
        this._changeInputValue('valueType', value);
    }

    _handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this._changeInputValue(name, value);        
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
                <article className="row newKeyForm" style={{ display: 'none' }} ref={(fc) => { this.formContainer = fc; }}>
                    <form className="col s12" onSubmit={this._saveKey}>
                        <div className="row">
                            <div className="input-field col s12 m4">
                                <input placeholder="Key name" name="key" id="key" type="text" className="validate" onChange={this._handleInputChange}/>
                                <label className="active" htmlFor="key">Key name</label>
                            </div>
                            <div className="input-field col s12 m4">
                                <select defaultValue="" id="valueType" ref={(sel) => { this.valueTypeSelect = sel; }}>
                                    <option value="" disabled>Choose one</option>
                                    <option value="String">String</option>
                                    <option value="JSON">JSON</option>
                                </select>
                                <label htmlFor="valueType">Value type</label>
                            </div>
                            <div className="input-field col s12 m4">
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