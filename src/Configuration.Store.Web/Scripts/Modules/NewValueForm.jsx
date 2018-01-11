import ToggleResponsiveButton from '../Modules/ToggleResponsiveButton'
// import 'lib/ace/ace'
import JSONEditor from 'lib/jsoneditor/jsoneditor'

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
    }

    componentDidMount() {
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip();

        var envTagContainer = jQuery(this.formContainer).find('#env-tags-chips');

        envTagContainer.material_chip({
            placeholder: 'Environment tag',
            secondaryPlaceholder: '+Tag',
        });

        envTagContainer.on('chip.add', function(e, chip){
            // you have the added chip here
            console.log('Added chip ' + chip.tag);
        });
            
        envTagContainer.on('chip.delete', function(e, chip){
            // you have the deleted chip here
            console.log('Removed chip ' + chip.tag);
        });

        if(this.props.valueType === 'JSON')
        {
            var options = 
            {
                modes: ['code', 'view'],
                mode: 'code'
                // set onChange (Set a callback function triggered when the contents of the JSONEditor change. Called without parameters. Will only be triggered on changes made by the user, not in case of programmatic changes via the functions set or setText.) 
            };

            var container = document.getElementById('json-editor');
            var editor = new JSONEditor(container, options);
        }
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

        this.props.saveValue(
            this.props.key,
            this.state.version,
            this.state.envTags,
            this.state.value
        );
    }

    _handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
            [name]: value
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
                <div id="json-editor"></div>
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
                    <form className="col s12" onSubmit={this._saveKey}>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <input name="version" id="version" type="text" className="validate" onChange={this._handleInputChange}/>
                                <label htmlFor="version">Version</label>
                            </div>
                            <div className="input-field col s12 m6" id="env-tags-chips">
                            </div>
                        </div>
                        <div className="row">
                            {valueSetter}
                        </div>
                        <div className="row">
                            <div className="input-field col s12 m3">
                                <button className="btn waves-effect waves-light light-blue tooltipped"
                                        name="action"
                                        data-position="top"
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