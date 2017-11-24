import ConfigurationKeyValue from './ConfigurationKeyValue'

class ConfigurationKey extends React.Component {

    constructor(props) {
        super(props);

        this.state = {
            detail: props.detail
        };
    }

    render() {
        return (
            <div className="key-detail">
                <span className="key-head">
                    <h2>{this.state.detail.key}</h2><span className="chip">{this.state.detail.type}</span>
                </span>

                <table className="highlight ">
                    <thead>
                        <tr className="">
                            <th>Version</th>
                            <th>Latest value</th>
                            <th className="hide-on-med-and-down">Latest sequence</th>
                            <th className="hide-on-small-only">Environment tags</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.detail.values.map((value, index) =>
                            <ConfigurationKeyValue key={index} value={value} />
                        )}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ConfigurationKey;