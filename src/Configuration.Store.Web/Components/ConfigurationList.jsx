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
                <ul>
                    {this.state.configKeys.map((configKey) =>
                        <li>{configKey.Key}</li>
                    )}
                </ul>
            </div>
        );
    }
}