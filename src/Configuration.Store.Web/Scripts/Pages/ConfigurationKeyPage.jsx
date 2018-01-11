import ConfigurationKey from '../Modules/ConfigurationKey'
import NewValueForm from '../Modules/NewValueForm'

class ConfigurationKeyPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            detail: props.detail
        };        
    }

    _saveValue(key, version, envTags, value) {
        var url = '/api/keys/' + name;

        var body = JSON.stringify({
            type: valueType
        });

        var fetchOptions = {
            method: "PUT",
            body: body,
            headers: { "Content-Type": "application/json" }
        };
        fetch(url, fetchOptions)
            .then((response) => {
                if (response.ok) {
                    Materialize.toast('Key ' + name + ' was saved!', 4000);
                    this._getUpdatedConfigKeys();
                } else {
                    Materialize.toast('Failed to save key ' + name, 4000);
                }
            }, (error) => {
                Materialize.toast('Failed to save key ' + name, 4000);
                console.log(error);
            });
    }

    _deleteKey(key, version, valueId){
        console.log('deleting key');

        var url = '/api/keys/' + name;

        var fetchOptions = {
            method: "DELETE"
        };
        fetch(url, fetchOptions)
            .then((response) => {
                if (response.ok) { this._getUpdatedConfigKeys(); }
            }, (error) => {
                {/* TODO: show error informartion */ }
                console.log(error);
            });
    }

    render() {
        return (
            <div className="key-detail">
                <NewValueForm key={this.state.detail.key} valueType={this.state.detail.type} saveValue={this._saveValue} />
                <ConfigurationKey detail={this.state.detail} />
            </div>
        );
    }
}

export default ConfigurationKeyPage;