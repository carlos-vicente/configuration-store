import JSONEditor from 'lib/jsoneditor/jsoneditor'

class JsonValueEditor extends React.Component {
    constructor(props){
        super(props);

        this.state = {
            editor: null
        };
    }

    componentDidMount(){
        var editor;
        var options = 
        {
            modes: ['code', 'view'],
            mode: 'code',
            onChange: () => {
                var json = JSON.stringify(editor.get());
                this.props.valueChanged(json);
            }
        };

        var container = document.getElementById(this.props.elementId);
        editor = new JSONEditor(container, options);

        this.setState({
            editor: editor
        });
    }

    componentWillReceiveProps(nextProps){
        if(this.props.data !== nextProps.data){
            this.state.editor.set(JSON.parse(nextProps.data));
        }
    }

    render(){
        return (<div id={this.props.elementId}></div>);
    }
}

export default JsonValueEditor;