class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };
    }

    render() {
        return (
            <table>
                <thead>
                    <tr>
                        <th>Configuration Key</th>
                        <th>Latest version</th>
                        <th>Type</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.configKeys.map((configKey, index) =>
                        <tr key={index}>
                            <td>{configKey.key}</td>
                            <td>{configKey.latestVersion}</td>
                            <td>{configKey.type}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }
}

export default ConfigurationList;