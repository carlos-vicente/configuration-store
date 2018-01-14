import ConfigurationKeyValue from './ConfigurationKeyValue'

class ConfigurationKey extends React.Component {

    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <span className="key-head">
                    <h2>{this.props.detail.key}</h2><span className="chip">{this.props.detail.type}</span>
                </span>

                <table className="highlight ">
                    <thead>
                        <tr className="">
                            <th>Version</th>
                            <th>Value</th>
                            <th className="hide-on-med-and-down">Sequence</th>
                            <th className="hide-on-small-only">Environment tags</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.detail.values.map((value, index) =>
                            <ConfigurationKeyValue key={index} value={value} />
                        )}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ConfigurationKey;