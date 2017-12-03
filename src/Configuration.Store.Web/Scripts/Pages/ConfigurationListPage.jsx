import ConfigurationList from '../Modules/ConfigurationList'
import NewKeyForm from '../Modules/NewKeyForm'
import SearchForm from '../Modules/SearchForm'

class ConfigurationListPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            originalConfigKeys: props.configKeys,
            configKeys: props.configKeys
        };

        this._getUpdatedConfigKeys = this._getUpdatedConfigKeys.bind(this);
        this._saveKey = this._saveKey.bind(this);
        this._deleteKey = this._deleteKey.bind(this);
        this._filterKeys = this._filterKeys.bind(this);
        this._resetFilter = this._resetFilter.bind(this);
    }

    _getUpdatedConfigKeys() {
        var fetchOptions = {
            method: "GET",
            headers: { "Accept": "application/json" }
        };
        fetch('/api/keys/', fetchOptions)
            .then((response) => {
                if (response.ok) {
                    // json() will return a resolved promise to the actual value
                    // so we returns the promise to chain the async methods
                    return response.json();
                }
            }, (error) => {
                {/* TODO: show error informartion */ }
                console.log(error);
            })
            .then((data) => {
                this.setState({ configKeys: data });
            });
    }

    _saveKey(name, valueType) {
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
                if (response.ok) { this._getUpdatedConfigKeys(); }
            }, (error) => {
                {/* TODO: show error informartion */ }
                console.log(error);
            });
    }

    _deleteKey(name){
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

    _filterKeys(filter){
        var data = this.state
            .originalConfigKeys
            .filter(key => key.key.includes(filter));
        this.setState({ configKeys: data });
    }

    _resetFilter(){

    }

    render() {
        return (
            <div className="config-store">
                <NewKeyForm saveKey={this._saveKey} />
                <SearchForm onFilter={this._filterKeys} onReset={this._resetFilter} />
                <ConfigurationList configKeys={this.state.configKeys} deleteKey={this._deleteKey} />
            </div>
        );
    }
}

export default ConfigurationListPage;