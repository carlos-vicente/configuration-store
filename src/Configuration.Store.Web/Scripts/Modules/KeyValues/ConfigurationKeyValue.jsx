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
                <td className="config-value-actions">
                    <a className="btn-floating light-blue waves-effect waves-light modal-trigger"
                        onClick={() => this.props.edit(this.props.value)}>
                        <i className="material-icons">edit</i>
                    </a>
                    <a className="btn-floating red waves-effect waves-light modal-trigger">
                        <i className="material-icons">delete</i>
                    </a>
                </td>
            </tr>
        );
    }
}

export default ConfigurationKeyValue;