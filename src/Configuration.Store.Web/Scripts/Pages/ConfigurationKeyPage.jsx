import ConfigurationKey from '../Modules/KeyValues/ConfigurationKey'
import NewValueForm from '../Modules/KeyValues/NewValueForm'
import UpdateValueForm from '../Modules/KeyValues/UpdateValueForm'

class ConfigurationKeyPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            detail: props.detail,
            editModeOn: false,
            editingValue: {}
        };

        this._saveValue = this._saveValue.bind(this);
        this._getUpdatedConfigValues = this._getUpdatedConfigValues.bind(this);
        this._enterEditMode = this._enterEditMode.bind(this);
        this._leaveEditMode = this._leaveEditMode.bind(this);
        this._deleteKeyValue = this._deleteKeyValue.bind(this);
        this._updateValue = this._updateValue.bind(this);
        this._updateValueData = this._updateValueData.bind(this);
        this._addValueEnvTag = this._addValueEnvTag.bind(this);
        this._removeValueEnvTag = this._removeValueEnvTag.bind(this);
    }

    _saveValue(valueToSave) {
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

    _enterEditMode(valueToEdit){
        this.setState({
            editModeOn: true,
            editingValue: valueToEdit
        });
    }

    _updateValue(){
        console.log('update value');

        this._leaveEditMode();
    }

    _updateValueData(data){

    }

    _addValueEnvTag(tag){

    }

    _removeValueEnvTag(tag){

    }

    _leaveEditMode(){
        this.setState({
            editModeOn: false,
            editingValue: {}
        });
    }

    _deleteKeyValue(valueId){

    }

    render() {
        return (
            <div className="key-detail">
                <NewValueForm valueType={this.state.detail.type} saveValue={this._saveValue} />
                <UpdateValueForm
                    shown={this.state.editModeOn}
                    value={this.state.editingValue}
                    saveEditingValue={this._updateValue}
                    close={this._leaveEditMode}
                    dataChanged={this._updateValueData}
                    appendEnvTag={this._addValueEnvTag}
                    removeEnvTag={this._removeValueEnvTag} />
                <ConfigurationKey 
                    detail={this.state.detail}
                    enterEditMode={this._enterEditMode}
                    deleteKeyValue={this._deleteKeyValue} />
            </div>
        );
    }
}

export default ConfigurationKeyPage;