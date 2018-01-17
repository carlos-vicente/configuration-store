import ConfigurationKey from '../Modules/ConfigurationKey'
import NewValueForm from '../Modules/NewValueForm'

class ConfigurationKeyPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            detail: props.detail,
            editModeOn: false,
            editingValue: null
        };

        this._saveValue = this._saveValue.bind(this);
        this._getUpdatedConfigValues = this._getUpdatedConfigValues.bind(this);
        this._enterEditMode = this._enterEditMode.bind(this);
        this._leaveEditMode = this._leaveEditMode.bind(this);
    }

    _saveValue(valueToSave) {
    //_saveValue(version, envTags, value) {
        var url = '/api/keys/' + this.state.detail.key + '/version/' + valueToSave.version + '/values';

        var body = JSON.stringify({
            tags: valueToSave.envTags,
            value: valueToSave.data
        });

        var fetchOptions = {
            method: "PUT",
            body: body,
            headers: { "Content-Type": "application/json" }
        };
        fetch(url, fetchOptions)
            .then((response) => {
                if (response.ok) {
                    Materialize.toast('Value was saved!', 4000);
                    this._getUpdatedConfigValues();
                } else {
                    Materialize.toast('Failed to save value', 4000);
                }
            }, (error) => {
                Materialize.toast('Failed to save value', 4000);
                console.log(error);
            });
    }

    _getUpdatedConfigValues(){
        console.log('get updated list....');
    }

    // _deleteKey(key, version, valueId){
    //     console.log('deleting key');

    //     var url = '/api/keys/' + name;

    //     var fetchOptions = {
    //         method: "DELETE"
    //     };
    //     fetch(url, fetchOptions)
    //         .then((response) => {
    //             if (response.ok) { this._getUpdatedConfigKeys(); }
    //         }, (error) => {
    //             {/* TODO: show error informartion */ }
    //             console.log(error);
    //         });
    // }

    _enterEditMode(valueToEdit){
        this.setState({
            editModeOn: true,
            editingValue: valueToEdit
        });
    }

    _leaveEditMode(valueToEdit){
        this.setState({
            editModeOn: false,
            editingValue: null
        });
    }

    render() {
        return (
            <div className="key-detail">
                <NewValueForm valueType={this.state.detail.type} saveValue={this._saveValue} />
                <ConfigurationKey detail={this.state.detail} enterEditMode={this._enterEditMode} />
            </div>
        );
    }
}

export default ConfigurationKeyPage;