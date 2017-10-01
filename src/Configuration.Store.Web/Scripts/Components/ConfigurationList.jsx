import ConfigurationKey from './ConfigurationKey';

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
                        <ConfigurationKey configKey={configKey.Key} />
                    )}
                </ul>
                
            </div>
        );
    }
}

export default ConfigurationList;