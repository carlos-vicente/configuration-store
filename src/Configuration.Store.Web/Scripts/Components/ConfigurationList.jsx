class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            configKeys: props.configKeys
        };
    }

    render() {
        return (
            <div className="collection">
                {this.state.configKeys.map((configKey, index) =>
                    <a key={index} href="#!" className="collection-item">
                        <span className="new badge" data-badge-caption={configKey.type}></span>
                        {configKey.key} ({configKey.latestVersion})
                    </a>
                )}
            </div>
        );
    }
}

export default ConfigurationList;