import moment from 'Scripts/lib/moment/moment.min'

class ConfigurationListItem extends React.Component {
    render() {
        return (
            <tr className="config-key">
                <td><a href={this.props.configKey.links[0].link}>{this.props.configKey.key}</a></td>
                <td>{this.props.configKey.latestVersion}</td>
                <td className="hide-on-med-and-down">
                    {moment(this.props.configKey.createdAt).format('LLL')}
                </td>
                <td className="hide-on-small-only">
                    <div className="chip">{this.props.configKey.type}</div>
                </td>
                <td className="config-key-acions">
                    <a className="btn-floating light-blue">
                        <i className="material-icons">mode_edit</i>
                    </a>
                    <a className="btn-floating light-blue hide-on-small-only">
                        <i className="material-icons">delete</i>
                    </a>
                </td>
            </tr>
        );
    }
}

export default ConfigurationListItem;