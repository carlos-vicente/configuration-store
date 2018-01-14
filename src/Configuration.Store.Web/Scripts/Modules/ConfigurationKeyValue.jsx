class ConfigurationKeyValue extends React.Component {
    render() {
        return (
            <tr className="">
                <td>{this.props.value.version}</td>
                <td>{this.props.value.data}</td>
                <td className="hide-on-med-and-down">{this.props.value.sequence}</td>
                <td className="hide-on-small-only">
                    {
                        this.props
                            .value
                            .environmentTags
                            .map((tag, tagIndex) =>
                                <span key={tagIndex} className="chip">{tag}</span>)
                    }
                </td>
                <td classname="config-value-actions">
                
                </td>
            </tr>
        );
    }
}

export default ConfigurationKeyValue;