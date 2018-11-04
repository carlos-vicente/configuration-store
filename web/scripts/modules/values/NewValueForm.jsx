import ToggleResponsiveButton from '../ToggleResponsiveButton'
import JsonValueEditor from './JsonValueEditor'
import TagContainer from './TagContainer'

class NewValueForm extends React.Component {
    constructor(props){
        super(props);
        
        this.state = {
            speed: 100,
            shown: false,
            version: '',
            envTags: [],
            value: ''
        };

        // 'this' handler bindings
        this._openForm = this._openForm.bind(this);
        this._closeForm = this._closeForm.bind(this);
        this._saveValue = this._saveValue.bind(this);
        this._handleInputChange = this._handleInputChange.bind(this);
        this._jsonValueChanged = this._jsonValueChanged.bind(this);
        this._addEnvironmentTag = this._addEnvironmentTag.bind(this);
        this._removeEnvironmentTag = this._removeEnvironmentTag.bind(this);
    }

    componentDidMount() {
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip();
    }

    componentWillUnmount() {
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip('remove');
    }

    _openForm(callback){
        jQuery(this.formContainer).fadeIn(this.state.speed, callback);
    }
    
    _closeForm(callback){
        var container = jQuery(this.formContainer);
        container.fadeOut(this.state.speed, callback);
        container.find('form')[0].reset();

        this.setState({
            version: '',
            envTags: [],
            value: ''
        });
    }

    _saveValue(submitEvent){
        submitEvent.preventDefault();

        var valueToSave = {
            version: this.state.version,
            envTags: this.state.envTags,
            data: this.state.value
        }

        this.props.saveValue(valueToSave);
    }

    _handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
            [name]: value
        });
    }

    _jsonValueChanged(jsonValue){
        this.setState({
            value: jsonValue
        });
    }

    _addEnvironmentTag(tag){
        this.setState((previousState, props) => {
            envTags: previousState.envTags.push(tag)
        });
    }

    _removeEnvironmentTag(tag){
        this.setState((previousState, props) => {
            envTags: previousState.envTags
                .splice(previousState.envTags.indexOf(tag), 1)
        });
    }

    render() {
        let valueSetter = null;

        if(this.props.valueType === 'String'){
            valueSetter = <div className="input-field col s12 m9">
                <input name="value" id="value" type="text" className="validate" onChange={this._handleInputChange}/>
                <label htmlFor="value">Value</label>
            </div>;
        }else{ // it is JSON
            valueSetter = <div className="input-field col s12 m9">
                <JsonValueEditor elementId="new-json-editor" valueChanged={this._jsonValueChanged} />
            </div>;
        }

        return (
            <article className="new-value-form-container">

                <ToggleResponsiveButton
                    openButtonText="Add value"
                    openIcon="add"
                    closeButtonText="Close"
                    onOpen={this._openForm}
                    onClose={this._closeForm} />

                <article className="row new-value-form" style={{ display: 'none' }} ref={(fc) => { this.formContainer = fc; }}>
                    <form className="col s12" onSubmit={this._saveValue}>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <input name="version"
                                        id="version"
                                        type="text"
                                        className="validate tooltipped"
                                        data-delay="50"
                                        data-tooltip="Semantic version"
                                        data-position="top"
                                        onChange={this._handleInputChange}/>
                                <label htmlFor="version">Version</label>
                            </div>
                            <div className="input-field col s12 m6">
                                <TagContainer
                                    elementId="new-eng-tags"
                                    placeholder="Environment tag"
                                    secondaryPlaceholder="+Tag"
                                    addTag={this._addEnvironmentTag}
                                    removeTag={this._removeEnvironmentTag} />
                            </div>
                        </div>
                        <div className="row">
                            {valueSetter}
                        </div>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <button className="btn waves-effect waves-light light-blue tooltipped"
                                        name="action"
                                        data-delay="50"
                                        data-tooltip="Save value"
                                        data-position="right">
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

export default NewValueForm;