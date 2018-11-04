import JsonValueEditor from './JsonValueEditor'
import TagContainer from './TagContainer'

class UpdateValueForm extends React.Component {
    constructor(props){
        super(props);
    }

    componentDidMount(){
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip();
    }

    componentWillUnmount(){
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip('remove');
    }

    _handleValueChange(event){
        this.props.dataChanged(event.target.value)
    }

    _jsonValueChanged(jsonValue){
        this.props.dataChanged(jsonValue);
    }

    _addEnvironmentTag(tag){
        this.props.appendEnvTag(tag);
    }

    _removeEnvironmentTag(tag){
        this.props.removeEnvTag(tag);
    }

    _updateValue(submitEvent){
        submitEvent.preventDefault();

        this.props.saveEditingValue();
    }

    render(){
        let valueSetter = null;

        if(this.props.valueType === 'String'){
            valueSetter = <div className="input-field col s12 m9">
                <input 
                    name="value"
                    id="value"
                    type="text"
                    className="validate"
                    value={this.props.value.data}
                    onChange={this._handleValueChange}/>
                <label htmlFor="value">Value</label>
            </div>;
        }else{ // it is JSON
            valueSetter = <div className="input-field col s12 m9">
                <JsonValueEditor elementId="update-json-editor" data={this.props.value.data} valueChanged={this._jsonValueChanged} />
            </div>;
        }

        return (
            <article className="update-value-form-container" style={this.props.shown ? { display: 'inline' } : { display: 'none' }}>
                <article className="row update-value-form">
                    <form className="col s12" onSubmit={this._updateValue}>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <p>{this.props.value.version}</p>
                            </div>
                            <div className="input-field col s12 m6">
                                <TagContainer
                                    elementId="update-eng-tags"
                                    placeholder="Environment tag"
                                    secondaryPlaceholder="+Tag"
                                    tags={this.props.value.environmentTags}
                                    addTag={this._addEnvironmentTag}
                                    removeTag={this._removeEnvironmentTag} />
                            </div>
                        </div>
                        <div className="row">
                            {valueSetter}
                        </div>
                        <div className="row">
                            <button className="btn waves-effect waves-light light-blue tooltipped"
                                    name="action"
                                    data-delay="50"
                                    data-tooltip="Save value"
                                    data-position="right">
                                <i className="material-icons">save</i>
                            </button>
                        </div>
                    </form>
                </article>
            </article>
        );
    }
}

export default UpdateValueForm;