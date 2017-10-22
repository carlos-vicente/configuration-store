import moment from 'Scripts/lib/moment/moment.min'

class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };
    }

    render() {
        return (
            <div>
                {/* TODO: add a search box here to search on all configuration keys */} 
                <table className="highlight config-list">
                    <thead>
                        <tr className="config-list-head">
                            <th>Configuration key</th>
                            <th>Latest version</th>
                            <th className="hide-on-med-and-down">Created at</th>
                            <th className="hide-on-small-only">Configuration type</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.configKeys.map((configKey, index) =>
                            <tr key={index} className="config-key">
                                <td><a href={configKey.links[0].link}>{configKey.key}</a></td>
                                <td>{configKey.latestVersion}</td>
                                <td className="hide-on-med-and-down">{moment(configKey.createdAt).format('LLL')}</td>
                                <td className="hide-on-small-only"><div className="chip">{configKey.type}</div></td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ConfigurationList;