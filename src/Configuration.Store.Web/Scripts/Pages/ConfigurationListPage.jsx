import ConfigurationList from '../Modules/ConfigurationList'
import NewKeyForm from '../Modules/NewKeyForm'

class ConfigurationListPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };

        this._getUpdatedConfigKeys = this._getUpdatedConfigKeys.bind(this);
        this._saveKey = this._saveKey.bind(this);
    }

    _getUpdatedConfigKeys() {

    }

    _saveKey(name, valueType) {
        var url = '/api/' + name;

        var body = JSON.stringify({
            type: valueType
        });

        console.log('Creating new key:');

        var fetchOptions = {
            method: "PUT",
            body: body,
            headers: { "Content-Type": "application/json" }
        };
        fetch(url, fetchOptions)
            .then((response) => {
                console.log(response);
                if (response.ok) {
                    this._getUpdatedConfigKeys();
                }
            }, (error) => {
                {/* TODO: show error informartion */ }
                console.log(error);
            });
    }

    render() {
        return (
            <div className="config-store">
                <NewKeyForm saveKey={this._saveKey} />
                <ConfigurationList configKeys={this.state.configKeys} />
            </div>
        );
    }
}

export default ConfigurationListPage;