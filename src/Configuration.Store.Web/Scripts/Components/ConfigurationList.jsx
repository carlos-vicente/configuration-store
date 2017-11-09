import ConfigurationListItem from './ConfigurationListItem'
import NewKeyForm from './NewKeyForm'

class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };
    }

    render() {
        return (
            <div className="config-list">
                {/* TODO: add a search box here to search on all configuration keys */}
                <NewKeyForm />
                <table className="highlight">
                    <thead>
                        <tr className="config-list-head">
                            <th>Key name</th>
                            <th>Latest version</th>
                            <th className="hide-on-med-and-down">Created at</th>
                            <th className="hide-on-small-only">Value type</th>
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