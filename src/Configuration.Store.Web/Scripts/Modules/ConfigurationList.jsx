import ConfigurationListItem from './ConfigurationListItem'

class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };
    }

    _onNewKeySaved(){
        console.log("New key event");
    }

    render() {
        return (
            <div>
                {/* TODO: add a search box here to search on all configuration keys */}
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
                        {this.state.configKeys.map((configKey, index) =>
                            <ConfigurationListItem key={index} configKey={configKey} />
                        )}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ConfigurationList;