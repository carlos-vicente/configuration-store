import ToggleResponsiveButton from '../Modules/ToggleResponsiveButton'

class SearchForm extends React.Component{

    constructor(props){
        super(props);

        this.state = {
            speed: 100,
            shown: false,
            search: ''
        };

        this._openForm = this._openForm.bind(this);
        this._closeForm = this._closeForm.bind(this);
        this._search = this._search.bind(this);
        this._reset = this._reset.bind(this);
        this._handleInputChange = this._handleInputChange.bind(this);
    }

    componentDidMount() {
        jQuery(this.searchContainer)
            .find('.tooltipped')
            .tooltip();
    }

    componentWillUnmount() {
        jQuery(this.searchContainer)
            .find('.tooltipped')
            .tooltip('remove');
    }

    _openForm(callback){
        jQuery(this.searchContainer).fadeIn(this.state.speed, callback);
    }
    
    _closeForm(callback){
        var container = jQuery(this.searchContainer);
        container.fadeOut(this.state.speed, callback);
        this._resetForm(container);
    }

    _reset(clickEvent){
        console.log('resetting...');
        this.props.onReset();
        this._resetForm();
    }

    _resetForm(formContainer){
        var container = formContainer;
        if(container === undefined){
            container = jQuery(this.searchContainer);
        }
        container.find('form')[0].reset();
        this.setState({ search: '' });
    }
    
    _search(submitEvent){
        submitEvent.preventDefault();
        this.props.onFilter(this.state.search);
    }

    _handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
            [name]: value
        });
    }
    
    render(){
        return (
            <article className="search-form-container">
                
                <ToggleResponsiveButton
                    openButtonText="Search"
                    openIcon="search"
                    closeButtonText="Close"
                    onOpen={this._openForm}
                    onClose={this._closeForm} />

                <article className="row search-form" style={{ display: 'none' }} ref={(sf) => {this.searchContainer = sf;}}>
                    <form className="col s12" onSubmit={this._search}>
                        <div className="row">
                            <div className="input-field col s12 m8">
                                <input name="search" 
                                        id="search" 
                                        type="text" 
                                        className="validate" 
                                        onChange={this._handleInputChange}/>
                                <label htmlFor="search">Search term</label>
                            </div>
                            <div className="input-field col s12 m4">
                                <button className="btn waves-effect waves-light light-blue tooltipped"
                                        name="action"
                                        data-position="top"
                                        data-delay="50"
                                        data-tooltip="Search">
                                    <i className="material-icons">search</i>
                                </button>
                                <button type="button"
                                        className="btn waves-effect waves-light red tooltipped"
                                        name="action"
                                        onClick={this._reset}
                                        data-position="top"
                                        data-delay="50"
                                        data-tooltip="Clear search">
                                    <i className="material-icons">clear_all</i>
                                </button>
                            </div>
                        </div>
                    </form>
                </article>
            </article>
        );
    }
}

export default SearchForm;