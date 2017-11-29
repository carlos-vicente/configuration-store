import moment from 'Scripts/lib/moment/moment.min'

class ConfigurationListItem extends React.Component {
    render() {
        return (
            <tr className="config-key">
                <td><a href={this.props.configKey.links[0].link}>{this.props.configKey.key}</a></td>
                <td className="hide-on-small-only">
                    <span className="hide-on-large-only">{moment(this.props.configKey.createdAt).format('L')}</span>
                    <span className="hide-on-med-and-down">{moment(this.props.configKey.createdAt).format('lll')}</span>
                </td>
                <td>
                    <div className="chip">{this.props.configKey.type}</div>
                </td>
                <td className="config-key-acions">
                    <a className="btn-floating red">
                        <i className="material-icons">delete</i>
                    </a>
                </td>
            </tr>
        );
    }
}

export default ConfigurationListItem;