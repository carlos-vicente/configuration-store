class DeleteModal extends React.Component{

    constructor(props) {
        super(props);

        this._handleDeleteClick = this._handleDeleteClick.bind(this);
    }

    componentDidMount() {
        var id = '#' + this.props.id;
        jQuery(id).modal({
            complete: () => {
                console.log('closing modal');
                this.props.onClose();
                console.log('new name: ' + this.props.name);
            }
        });
    }

    _handleDeleteClick(){
        console.log('before delete key');
        this.props.deleteKey(this.props.name);
        console.log('after delete key');
    }

    render(){
        return (
            <div id={this.props.id} className="modal">
                <div className="modal-content">
                    <h4>Are you sure?</h4>
                    <p>By deleting <span className="modal-key">{this.props.name}</span>, you'll be deleting all its associated values.</p>
                </div>
                <div className="modal-footer">
                    <a href="#!" 
                        className="modal-action modal-close waves-effect waves-green btn green"
                        onClick={this._handleDeleteClick}>Yes</a>
                    <a href="#!" className="modal-action modal-close waves-effect waves-green btn-flat">No</a>
                </div>
            </div>
        );
    }
}

export default DeleteModal;