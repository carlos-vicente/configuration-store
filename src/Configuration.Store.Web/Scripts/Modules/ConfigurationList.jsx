import ConfigurationListItem from './ConfigurationListItem'
import DeleteModal from './DeleteModal'

class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            selected: ''
        }

        this._setSelectedKey = this._setSelectedKey.bind(this);
        this._clearSelectedKey = this._clearSelectedKey.bind(this);
    }

    _setSelectedKey(name) {
        this.setState({
            selected: name
        });
    }

    _clearSelectedKey(){
        this.setState({
            selected: ''
        });
    }

    render() {
        return (
            <div>
                <table className="highlight">
                    <thead>
                        <tr className="config-list-head">
                            <th>Key name</th>
                            <th className="hide-on-small-only">Created at</th>
                            <th>Value type</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.configKeys.map((configKey, index) =>
                            <ConfigurationListItem key={index} configKey={configKey} onDeletePress={this._setSelectedKey} />
                        )}
                    </tbody>
                </table>

                <DeleteModal id="delete-modal" 
                            name={this.state.selected} 
                            deleteKey={this.props.deleteKey}
                            onClose={this._clearSelectedKey} />
            </div>
        );
    }
}

export default ConfigurationList;