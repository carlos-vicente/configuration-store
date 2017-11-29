import ConfigurationListItem from './ConfigurationListItem'

class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            keyOnModal: ''
        }

        this._setKeyOnModal = this._setKeyOnModal.bind(this);
    }

    componentDidMount() {
        jQuery('.modal').modal();
    }

    _setKeyOnModal(keyName) {
        this.setState({
            keyOnModal: keyName
        });
    }

    render() {
        return (
            <div>
                {/* TODO: add a search box here to search on all configuration keys */}
                <table className="highlight">
                    <thead>
                        <tr className="config-list-head">
                            <th>Key name</th>
                            <th className="hide-on-small-only">Created at</th>
                            <th>Value type</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.props.configKeys.map((configKey, index) =>
                            <ConfigurationListItem key={index} configKey={configKey} onDeletePress={this._setKeyOnModal} />
                        )}
                    </tbody>
                </table>

                <div id="delete-modal" className="modal">
                    <div className="modal-content">
                        <h4>Are you sure?</h4>
                        <p>By deleting <span className="modal-key">{this.state.keyOnModal}</span>, you'll be deleting all its associated values.</p>
                    </div>
                    <div className="modal-footer">
                        <a href="#!" className="modal-action modal-close waves-effect waves-green btn green">Yes</a>
                        <a href="#!" className="modal-action modal-close waves-effect waves-green btn-flat">No</a>
                    </div>
                </div>
            </div>
        );
    }
}

export default ConfigurationList;