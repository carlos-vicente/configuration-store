import ToggleResponsiveButton from '../Modules/ToggleResponsiveButton'

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
        this._handleInputChange = this._handleInputChange.bind(this);
        this._changeInputValue = this._changeInputValue.bind(this);
        this._handleSelectChange = this._handleSelectChange.bind(this);
    }

    componentDidMount() {
        jQuery(this.formContainer)
            .find('select')
            .material_select(this._handleSelectChange);
    }

    componentWillUnmount() {
        jQuery(this.formContainer)
            .find('select')
            .material_select('destroy');
    }

    _openForm(callback){
        jQuery(this.formContainer).fadeIn(this.state.speed, callback);
    }
    
    _closeForm(callback){
        var container = jQuery(this.formContainer);
        container.fadeOut(this.state.speed, callback);
        container.find('form')[0].reset();
    }

    _saveKey(submitEvent){
        submitEvent.preventDefault();

        this.props.saveKey(
            this.state.key,
            this.state.valueType
        );
    }

    _changeInputValue(inputName, inputValue){
        this.setState({
            [inputName]: inputValue
        });
    }

    _handleSelectChange(){
        // currently this is an hack, as this can only handle valueType select
        var value = jQuery(this.formContainer).find('select#valueType').val();
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
            <article className="new-key-form-container">

                <ToggleResponsiveButton
                    openButtonText="Add key"
                    openIcon="add"
                    closeButtonText="Close"
                    onOpen={this._openForm}
                    onClose={this._closeForm} />

                <article className="row new-key-form" style={{ display: 'none' }} ref={(fc) => { this.formContainer = fc; }}>
                    <form className="col s12" onSubmit={this._saveKey}>
                        <div className="row">
                            <div className="input-field col s12 m4">
                                <input name="key" id="key" type="text" className="validate" onChange={this._handleInputChange}/>
                                <label htmlFor="key">Key name</label>
                            </div>
                            <div className="input-field col s12 m4">
                                <select defaultValue="" id="valueType">
                                    <option value="" disabled>Choose one</option>
                                    <option value="String">String</option>
                                    <option value="JSON">JSON</option>
                                </select>
                                <label htmlFor="valueType">Value type</label>
                            </div>
                            <div className="input-field col s12 m4">
                                <button className="btn waves-effect waves-light light-blue" name="action">
                                    <i className="material-icons">save</i>
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