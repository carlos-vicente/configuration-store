class ToggleResponsiveButton extends React.Component {
    constructor(props){
        super(props);

        this.state = {
            shown: false
        };

        this._toggleButton = this._toggleButton.bind(this);
        this._open = this._open.bind(this);
        this._close = this._close.bind(this);
    }

    _toggleButton(){
        this.setState((prevState, props) => {
            return { shown: !prevState.shown }
        });
    }

    _open(){
        this.props.onOpen(this._toggleButton);
    }

    _close(){
        this.props.onClose(this._toggleButton);
    }

    render() {
        var bigOpenButtonClassName = "btn waves-effect waves-light light-blue" // hide-on-small-only"
            + (this.state.shown ? " hide" : "");

        var bigCloseButtonClassName = "btn waves-effect waves-light red darken-2" // hide-on-small-only"
            + (this.state.shown ? "" : " hide");
        
        return (
            <span>
                <a className={bigOpenButtonClassName} onClick={this._open} style={{width: 170}}>
                    <i className="material-icons right">{this.props.openIcon}</i>{this.props.openButtonText}
                </a>
                <a className={bigCloseButtonClassName} onClick={this._close} style={{width: 170}}>
                    <i className="material-icons right">close</i>{this.props.closeButtonText}
                </a>
            </span>
        );
    }
}

export default ToggleResponsiveButton;