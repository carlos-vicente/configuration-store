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
                <span>
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
                            <tr key={index} className="">
                                <td>{value.version}</td>
                                <td>{value.latestData}</td>
                                <td className="hide-on-med-and-down">{value.latestSequence}</td>
                                <td className="hide-on-small-only">
                                    {value.environmentTags.map((tag, tagIndex) => <span key={tagIndex} className="chip">{tag}</span>)}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    }
}

export default ConfigurationKey;