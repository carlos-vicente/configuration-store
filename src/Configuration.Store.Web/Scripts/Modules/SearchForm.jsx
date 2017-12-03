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
        this._handleInputChange = this._handleInputChange.bind(this);
    }

    _openForm(callback){
        jQuery(this.searchContainer).fadeIn(this.state.speed, callback);
    }
    
    _closeForm(callback){
        var container = jQuery(this.searchContainer);
        container.fadeOut(this.state.speed, callback);
        container.find('form')[0].reset();
    }

    _search(submitEvent){
        submitEvent.preventDefault();

        console.log('search submitted ' + this.state.search);

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

                <nav className="white" style={{ display: 'none' }} ref={(sf) => {this.searchContainer = sf;}}>
                    <div className="nav-wrapper">
                        <form className="col s12" onSubmit={this._search}>
                            <div className="input-field">
                                <input id="search" name="search" type="search" onChange={this._handleInputChange}/>
                                <label className="label-icon active" htmlFor="search">
                                    <i className="material-icons">search</i>
                                </label>
                                <i className="material-icons">close</i>
                            </div>
                        </form>
                    </div>
                </nav>
            </article>
        );
    }
}

export default SearchForm;