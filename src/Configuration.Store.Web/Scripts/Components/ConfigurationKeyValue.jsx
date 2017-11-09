class ConfigurationKeyValue extends React.Component {
    render() {
        return (
            <tr className="">
                <td>{this.props.value.version}</td>
                <td>{this.props.value.latestData}</td>
                <td className="hide-on-med-and-down">{this.props.value.latestSequence}</td>
                <td className="hide-on-small-only">
                    {
                        this.props
                            .value
                            .environmentTags
                            .map((tag, tagIndex) =>
                                <span key={tagIndex} className="chip">{tag}</span>)
                    }
                </td>
            </tr>
        );
    }
}

export default ConfigurationKeyValue;