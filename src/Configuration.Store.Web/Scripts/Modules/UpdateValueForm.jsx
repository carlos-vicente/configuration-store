import JSONEditor from 'lib/jsoneditor/jsoneditor'

class NewValueForm extends React.Component {
    constructor(props){
        super(props);
    }

    componentDidMount(){
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip();

        if(this.props.valueType === 'JSON')
        {
            var editor;
            var options = 
            {
                modes: ['code', 'view'],
                mode: 'code',
                onChange: () => {
                    var json = JSON.stringify(editor.get());
                    this.setState({
                        value: json
                    });
                    console.log(json);
                }
            };

            var container = document.getElementById('json-editor');
            editor = new JSONEditor(container, options);
        }
    }

    componentWillUnmount(){
        jQuery(this.formContainer)
            .find('.tooltipped')
            .tooltip('remove');
    }

    render(){
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
            <article className="update-value-form-container" style={this.props.show ? '' : { display: 'none' }}>
                <article className="row update-value-form">
                    <form className="col s12" onSubmit={this._saveValue}>
                        <div className="row">
                            <div className="input-field" id="env-tags-chips">
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