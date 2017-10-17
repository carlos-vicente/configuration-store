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
            <table className="highlight">
                <thead>
                    <tr>
                        <th>Configuration key</th>
                        <th>Latest version</th>
                        <th className="hide-on-med-and-down">Created at</th>
                        <th className="hide-on-small-only">Configuration type</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.configKeys.map((configKey, index) =>
                        <tr key={index}>
                            <td><a href="#!">{configKey.key}</a></td>
                            <td>{configKey.latestVersion}</td>
                            <td className="hide-on-med-and-down">{moment(configKey.createdAt).format('LLL')}</td>
                            <td className="hide-on-small-only"><div className="chip">{configKey.type}</div></td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }
}

export default ConfigurationList;